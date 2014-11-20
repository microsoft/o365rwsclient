using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class MailboxUsageDetail : TenantReportObject
    {
        [XmlElement]
        public string WindowsLiveID
        {
            get;
            set;
        }

        [XmlElement]
        public string UserName
        {
            get;
            set;
        }

        [XmlElement]
        public Int64 MailboxSize
        {
            get;
            set;
        }

        [XmlElement]
        public Int64 CurrentMailboxSize
        {
            get;
            set;
        }

        [XmlElement]
        public Int64 PercentUsed
        {
            get;
            set;
        }

        [XmlElement]
        public string MailboxPlan
        {
            get;
            set;
        }

        [XmlElement]
        public bool IsInactive
        {
            get;
            set;
        }

        [XmlElement]
        public Int64 IssueWarningQuota
        {
            get;
            set;
        }

        [XmlElement]
        public bool IsOverWarningQuota
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            WindowsLiveID = base.TryGetValue("WindowsLiveID");
            UserName = base.TryGetValue("UserName");
            MailboxSize = StringUtil.TryParseInt64(base.TryGetValue("MailboxSize"), 0);
            CurrentMailboxSize = StringUtil.TryParseInt64(base.TryGetValue("CurrentMailboxSize"), 0);
            PercentUsed = StringUtil.TryParseInt64(base.TryGetValue("PercentUsed"), 0);
            MailboxPlan = base.TryGetValue("MailboxPlan");
            IsInactive = StringUtil.TryParseBoolean(base.TryGetValue("IsInactive"), false);
            IssueWarningQuota = StringUtil.TryParseInt64(base.TryGetValue("IssueWarningQuota"), 0);
            IsOverWarningQuota = StringUtil.TryParseBoolean(base.TryGetValue("IsOverWarningQuota"), false);
        }
    }
}