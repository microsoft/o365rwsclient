using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class ClientSoftwareBrowserSummary : TenantReportObject
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Version { get; set; }

        [XmlElement]
        public System.Int64 Count { get; set; }

        [XmlElement]
        public string Category { get; set; }

        [XmlElement]
        public int DisplayOrder { get; set; }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);

            Name = base.TryGetValue("Name");
            Version = base.TryGetValue("Version");
            Count = StringUtil.TryParseInt64(base.TryGetValue("Count"), 0);
            Category = base.TryGetValue("Category");
            DisplayOrder = StringUtil.TryParseInt(base.TryGetValue("DisplayOrder"), 0);
        }
    }
}