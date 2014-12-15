using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class SPOTenantStorageMetric : TenantReportObject
    {
        [XmlElement]
        public System.Int64 ID
        {
            get;
            set;
        }

        [XmlElement]
        public int Used
        {
            get;
            set;
        }

        [XmlElement]
        public int Allocated
        {
            get;
            set;
        }

        [XmlElement]
        public int Total
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            ID = Utils.TryParseInt64(base.TryGetValue("ID"), 0);
            Used = Utils.TryParseInt(base.TryGetValue("Used"), 0);
            Allocated = Utils.TryParseInt(base.TryGetValue("Allocated"), 0);
            Total = Utils.TryParseInt(base.TryGetValue("Total"), 0);
        }
    }
}