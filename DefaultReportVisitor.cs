using System;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class DefaultReportVisitor : IReportVisitor
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="report"></param>
        public override void VisitReport(ReportObject report)
        {
            Console.WriteLine(report.ConvertToXml());
        }

        /// <summary>
        ///
        /// </summary>
        public override void VisitBatchReport()
        {
            foreach (ReportObject report in this.reportObjectList)
            {
                Console.WriteLine(report.ConvertToXml());
            }
        }
    }
}