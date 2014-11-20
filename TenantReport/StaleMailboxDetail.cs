using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class StaleMailboxDetail : TenantReportObject
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
        public DateTime LastLogon
        {
            get;
            set;
        }

        [XmlElement]
        public int DaysInactive
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            WindowsLiveID = base.TryGetValue("WindowsLiveID");
            UserName = base.TryGetValue("UserName");
            LastLogon = StringUtil.TryParseDateTime(base.TryGetValue("LastLogon"), DateTime.MinValue);
            DaysInactive = StringUtil.TryParseInt(base.TryGetValue("DaysInactive"), 0);
        }
    }
}