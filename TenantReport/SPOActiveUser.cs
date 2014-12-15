using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class SPOActiveUser : TenantReportObject
    {
        [XmlElement]
        public System.Int64 ID
        {
            get;
            set;
        }

        [XmlElement]
        public int UniqueUsers
        {
            get;
            set;
        }

        [XmlElement]
        public int LicensesAssigned
        {
            get;
            set;
        }

        [XmlElement]
        public int LicensesAcquired
        {
            get;
            set;
        }

        public int TotalUsers
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            ID = Utils.TryParseInt64(base.TryGetValue("ID"), 0);
            UniqueUsers = Utils.TryParseInt(base.TryGetValue("UniqueUsers"), 0);
            LicensesAssigned = Utils.TryParseInt(base.TryGetValue("LicensesAssigned"), 0);
            LicensesAcquired = Utils.TryParseInt(base.TryGetValue("LicensesAcquired"), 0);
            TotalUsers = Utils.TryParseInt(base.TryGetValue("TotalUsers"), 0);
        }
    }
}