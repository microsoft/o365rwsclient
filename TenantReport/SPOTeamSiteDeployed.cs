using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class SPOTeamSiteDeployed : TenantReportObject
    {
        [XmlElement]
        public System.Int64 ID
        {
            get;
            set;
        }

        [XmlElement]
        public int Active
        {
            get;
            set;
        }

        [XmlElement]
        public int Inactive
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            ID = StringUtil.TryParseInt64(base.TryGetValue("ID"), 0);
            Active = StringUtil.TryParseInt(base.TryGetValue("Active"), 0);
            Inactive = StringUtil.TryParseInt(base.TryGetValue("Inactive"), 0);
        }
    }
}