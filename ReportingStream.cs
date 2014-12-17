using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class ReportingStream
    {
        #region Privates

        private ReportingContext reportingContext;
        private ReportProvider reportProvider;
        private Type reportType;
        private string streamIdentifier = string.Empty;
        private string progressFilePath = string.Empty;
        
        #endregion Privates

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public ReportingStream(ReportingContext context, string reportType, string streamIdentifier, string progressFilePath = null)
        {
            this.reportingContext = context;
            this.streamIdentifier = streamIdentifier;
            this.progressFilePath = progressFilePath;
            this.reportProvider = new ReportProvider(context.WebServiceUrl, context.UserName, context.Password, context.TraceLogger);
            this.reportType = Type.GetType("Microsoft.Office365.ReportingWebServiceClient.TenantReport." + reportType);

            if (!this.reportType.IsSubclassOf(typeof(ReportObject)) || !this.reportType.IsSerializable)
            {
                throw new ArgumentException(string.Format("Report Type Must be Subclass of ReportObject and Serializable: {0}", this.reportType.FullName));
            }
        }

        #endregion Constructors

        #region Private methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns>The count of data returned</returns>
        private int RetrieveData(IReportVisitor visitor, QueryFilter filter)
        {
            int totalResultCount = 0;

            //If the TopCount is 0, then it was not specified, hence we take the constant value
            if (filter.TopCount == 0)
                filter.TopCount = Constants.ResultPageSize;

            List<XmlNode> resultNodes = reportProvider.GetResponseXml(this.reportType, filter);

            if (resultNodes.Count >= Constants.ResultPageSize && filter.SkipCount == 0)
            {
                reportingContext.TraceLogger.LogInformation("Result is exceeding limit, dividing the range.");
                List<QueryRange> subRangeList = filter.QueryRange.GetDividedRanges();

                if (subRangeList != null && subRangeList.Count > 0)
                {
                    foreach (QueryRange range in subRangeList)
                    {
                        filter.QueryRange = range;
                        totalResultCount += RetrieveData(visitor, filter);
                    }
                }
                else
                {
                    VisitXmlNodes(resultNodes, visitor);
                    totalResultCount = resultNodes.Count;
                    reportingContext.TraceLogger.LogInformation(string.Format("Retrieved [{0}] rows of data...", totalResultCount));

                    reportingContext.TraceLogger.LogInformation("Divided range is null, using Skips.");
                    int subResult;
                    filter.SkipCount = 0;
                    do
                    {
                        filter.SkipCount += Constants.ResultPageSize;
                        subResult = RetrieveData(visitor, filter);
                        totalResultCount += subResult;
                    } while (subResult >= Constants.ResultPageSize);

                    filter.SkipCount = 0;
                }
            }
            else
            {
                VisitXmlNodes(resultNodes, visitor);
                totalResultCount = resultNodes.Count;
                reportingContext.TraceLogger.LogInformation(string.Format("Retrieved [{0}] rows of data...", resultNodes.Count));
            }

            StreamProgress progress = new StreamProgress(this.progressFilePath, streamIdentifier, filter.QueryRange.EndDate, false);
            progress.SaveProgress();

            return totalResultCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TReport"></typeparam>
        /// <param name="nodes"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        private List<ReportObject> VisitXmlNodes(List<XmlNode> nodes, IReportVisitor visitor)
        {
            List<ReportObject> list = new List<ReportObject>();
            DateTime lastTimeStamp = DateTime.MinValue;

            visitor.Reset();
            foreach (XmlNode node in nodes)
            {
                ReportObject report = (ReportObject)Activator.CreateInstance(this.reportType);

                report.LoadFromXml(node);

                list.Add(report);
                visitor.AddReportToBatch(report);

                lastTimeStamp = (lastTimeStamp < report.Date) ? report.Date : lastTimeStamp;
            }

            visitor.VisitBatchReport();

            StreamProgress progress = new StreamProgress(this.progressFilePath, streamIdentifier, lastTimeStamp, true);
            progress.SaveProgress();

            return list;
        }

        #endregion Private methods

        /// <summary>
        /// This is a simple method that tries to fetch 1 record of the specified report
        /// and if 0 or 1 record return then the authN & authZ to this report is validated
        /// otherwise throws an exception
        /// </summary>
        /// <returns></returns>
        public bool ValidateAccessToReport()
        {
            try
            {
                IReportVisitor visitor = new DefaultReportVisitor();
                int res = RetrieveData(visitor, new QueryFilter() { TopCount = 1 });
                if (res == 0 || res == 1)
                    return true;
                else
                    throw new ApplicationException("Tried to validate your credentials against the report specified, however the response we received from the server did not match what we expected to receive. Please try again.");
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    if (ex.InnerException is HttpRequestException)
                    {
                        //Indicates that this is an HTTP request unauthorized exception
                        if (ex.InnerException.Message.Contains("401"))
                            return false;
                        else
                            throw ex;
                    }
                    else
                        throw ex;
                }
                else
                    throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearProgress()
        {
            StreamProgress progress = new StreamProgress(this.progressFilePath, this.streamIdentifier);
            progress.ClearProgress();
        }

        /// <summary>
        /// This RetrieveData method the built-in ReportVisitor (Console)
        /// </summary>
        public void RetrieveData()
        {
            IReportVisitor visitor = new DefaultReportVisitor();
            RetrieveData(visitor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="visitor"></param>
        public void RetrieveData(IReportVisitor visitor)
        {
            reportingContext.TraceLogger.LogInformation(string.Format("Start Retrieving Data For Report {0}", this.reportType.Name));

            QueryFilter queryFilter = new QueryFilter();
            queryFilter.QueryRange.StartDate = this.reportingContext.FromDateTime;
            queryFilter.QueryRange.EndDate = this.reportingContext.ToDateTime;
            queryFilter.CustomFilter = this.reportingContext.DataFilter;

            StreamProgress progress = new StreamProgress(this.progressFilePath, this.streamIdentifier);
            progress = progress.GetProgress();

            DateTime progressTimestamp = progress.TimeStamp;

            if (queryFilter.QueryRange.StartDate < progressTimestamp)
                queryFilter.QueryRange.StartDate = progressTimestamp;

            queryFilter.ExcludeStartItem = progress.ExcludeStartItem;

            int totalCount = RetrieveData(visitor, queryFilter);

            reportingContext.TraceLogger.LogInformation(string.Format("Retrieve Data Completed. Totally [{0}] of Data Retrieved.", totalCount));
        }
    }
}