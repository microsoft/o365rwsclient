using System.Collections.Generic;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public abstract class IReportVisitor
    {
        protected List<ReportObject> reportObjectList = new List<ReportObject>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="report"></param>
        public void AddReportToBatch(ReportObject report)
        {
            this.reportObjectList.Add(report);
        }

        public void Reset()
        {
            this.reportObjectList.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="report"></param>
        public abstract void VisitReport(ReportObject report);

        /// <summary>
        ///
        /// </summary>
        public abstract void VisitBatchReport();
    }
}