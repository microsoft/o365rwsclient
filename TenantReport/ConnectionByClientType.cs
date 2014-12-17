using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class ConnectionByClientType : TenantReportObject
    {
        private string Category;

        [XmlElement]
        public string ClientType { get; set; }

        [XmlElement]
        public System.Int64 Count { get; set; }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            ClientType = base.TryGetValue("ClientType");
            Count = Utils.TryParseInt64(base.TryGetValue("Count"), 0);
        }
    }
}