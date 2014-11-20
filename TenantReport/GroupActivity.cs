using Microsoft.Office365.ReportingWebServiceClient.Utils;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    public class GroupActivity : TenantReportObject
    {
        [XmlElement]
        public int GroupCreated
        {
            get;
            set;
        }

        [XmlElement]
        public int GroupDeleted
        {
            get;
            set;
        }

        public override void LoadFromXml(XmlNode node)
        {
            base.LoadFromXml(node);
            GroupCreated = StringUtil.TryParseInt(base.TryGetValue("GroupCreated"), 0);
            GroupDeleted = StringUtil.TryParseInt(base.TryGetValue("GroupDeleted"), 0);
        }
    }
}