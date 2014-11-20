using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class MailboxActivity : TenantReportObject
    {
        [XmlElement]
        public int TotalNumberOfActiveMailboxes
        {
            get;
            set;
        }

        [XmlElement]
        public int AccountCreated
        {
            get;
            set;
        }

        [XmlElement]
        public int AccountDeleted
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            TotalNumberOfActiveMailboxes = StringUtil.TryParseInt(base.TryGetValue("TotalNumberOfActiveMailboxes"), 0);
            AccountCreated = StringUtil.TryParseInt(base.TryGetValue("AccountCreated"), 0);
            AccountDeleted = StringUtil.TryParseInt(base.TryGetValue("AccountDeleted"), 0);
        }
    }
}