using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class ClientSoftwareOSDetail : TenantReportObject
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Version { get; set; }

        [XmlElement]
        public System.Int64 Count { get; set; }

        [XmlElement]
        public DateTime LastAccessTime { get; set; }

        public Guid ObjectId { get; set; }

        [XmlElement]
        public string UPN { get; set; }

        [XmlElement]
        public string DisplayName { get; set; }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            Name = base.TryGetValue("Name");
            Version = base.TryGetValue("Version");
            Count = Utils.TryParseInt64(base.TryGetValue("Count"), 0);
            LastAccessTime = Utils.TryParseDateTime(base.TryGetValue("LastAccessTime"), DateTime.MinValue);
            ObjectId = Utils.TryParseGuid(base.TryGetValue("ObjectId"), Guid.Empty);
            UPN = base.TryGetValue("UPN");
            DisplayName = base.TryGetValue("DisplayName");
        }
    }
}