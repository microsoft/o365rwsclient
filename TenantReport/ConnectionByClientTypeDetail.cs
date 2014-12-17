using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class ConnectionByClientTypeDetail : TenantReportObject
    {
        [XmlElement]
        public string WindowsLiveID { get; set; }

        [XmlElement]
        public string UserName { get; set; }

        [XmlElement]
        public string ClientType { get; set; }

        [XmlElement]
        public System.Int64 Count { get; set; }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            WindowsLiveID = base.TryGetValue("WindowsLiveID");
            UserName = base.TryGetValue("UserName");
            ClientType = base.TryGetValue("ClientType");
            Count = Utils.TryParseInt64(base.TryGetValue("Count"), 0);
        }
    }
}