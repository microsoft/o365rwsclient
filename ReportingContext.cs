using System;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class ReportingContext
    {
        private static string defaultServiceEndpointUrl = "https://reports.office365.com/ecp/reportingwebservice/reporting.svc";

        public string WebServiceUrl
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public DateTime FromDateTime
        {
            get;
            set;
        }

        public DateTime ToDateTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string DataFilter
        {
            get;
            set;
        }

        private ITraceLogger logger;

        private IReportVisitor visitor;

        public ReportingContext()
            : this(defaultServiceEndpointUrl)
        {
        }

        public ReportingContext(string url)
        {
            this.WebServiceUrl = url;
            this.FromDateTime = DateTime.MinValue;
            this.ToDateTime = DateTime.MinValue;
            this.DataFilter = string.Empty;
        }

        public void SetLogger(ITraceLogger logger)
        {
            if (logger != null)
            {
                this.logger = logger;
            }
        }

        public ITraceLogger TraceLogger
        {
            get
            {
                return this.logger;
            }
        }

        public void SetReportVisitor(IReportVisitor visitor)
        {
            if (visitor != null)
            {
                this.visitor = visitor;
            }
        }

        public IReportVisitor ReportVisitor
        {
            get
            {
                return this.visitor;
            }
        }
    }
}