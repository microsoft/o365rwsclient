using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class ReportingStream
    {
        private ReportingContext reportingContext;
        private string streamIdentifier = string.Empty;
        private ReportProvider reportProvider;

        private Type reportType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public ReportingStream(ReportingContext context, string reportType, string streamIdentifier)
        {
            this.reportingContext = context;
            this.streamIdentifier = streamIdentifier;
            this.reportProvider = new ReportProvider(context.WebServiceUrl, context.UserName, context.Password, context.TraceLogger);
            this.reportType = Type.GetType("Microsoft.Office365.ReportingWebServiceClient.TenantReport." + reportType);

            if (!this.reportType.IsSubclassOf(typeof(ReportObject)) || !this.reportType.IsSerializable)
            {
                throw new ArgumentException(string.Format("Report Type Must be Subclass of ReportObject and Serializable: {0}", this.reportType.FullName));
            }
        }

        public void setCredential(string userName, string password)
        {
            this.reportProvider.setCredential(userName, password);
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearProgress()
        {
            StreamProgress.ClearProgress(this.streamIdentifier);
        }

        /// <summary>
        ///
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

            StreamProgress progress = StreamProgress.GetProgress(streamIdentifier);
            DateTime progressTimestamp = progress.TimeStamp;
            if (queryFilter.QueryRange.StartDate < progressTimestamp)
            {
                queryFilter.QueryRange.StartDate = progressTimestamp;
                queryFilter.ExcludeStartItem = true;
            }

            int totalCount = RetrieveData(visitor, queryFilter);

            reportingContext.TraceLogger.LogInformation(string.Format("Retrieve Data Completed. Totally [{0}] of Data Retrieved.", totalCount));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns>The count of data returned</returns>
        private int RetrieveData(IReportVisitor visitor, QueryFilter filter)
        {
            int totalResultCount = 0;

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

            StreamProgress progress = new StreamProgress(streamIdentifier, lastTimeStamp);
            progress.SaveProgress();

            return list;
        }
    }
}