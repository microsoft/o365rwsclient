using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class MailboxUsage : TenantReportObject
    {
        [XmlElement]
        public int TotalMailboxCount
        {
            get;
            set;
        }

        [XmlElement]
        public int TotalInactiveMailboxCount
        {
            get;
            set;
        }

        [XmlElement]
        public int MailboxesOverWarningSize
        {
            get;
            set;
        }

        [XmlElement]
        public int MailboxesUsedLessthan25Percent
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            TotalMailboxCount = StringUtil.TryParseInt(base.TryGetValue("TotalMailboxCount"), 0);
            TotalInactiveMailboxCount = StringUtil.TryParseInt(base.TryGetValue("TotalInactiveMailboxCount"), 0);
            MailboxesOverWarningSize = StringUtil.TryParseInt(base.TryGetValue("MailboxesOverWarningSize"), 0);
            MailboxesUsedLessthan25Percent = StringUtil.TryParseInt(base.TryGetValue("MailboxesUsedLessthan25Percent"), 0);
        }
    }
}