using System;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient.TenantReport
{
    [Serializable]
    public class SPOOneDriveForBusinessFileActivity : ReportObject
    {
        /// <summary>
        /// Gets or sets the user puid
        /// </summary>
        [XmlElement]
        public string UserPuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        [XmlElement]
        public Guid DocumentId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the event name
        /// </summary>
        [XmlElement]
        public string EventName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user display name
        /// </summary>
        [XmlElement]
        public string UserDisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        [XmlElement]
        public string EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ip address
        /// </summary>
        [XmlElement]
        public string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        [XmlElement]
        public string DocumentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent folder path
        /// </summary>
        [XmlElement]
        public string ParentFolderPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client device
        /// </summary>
        [XmlElement]
        public string ClientDevice
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client OS
        /// </summary>
        [XmlElement]
        public string ClientOs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client OS
        /// </summary>
        [XmlElement]
        public string ClientApplication
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

            UserPuid = base.TryGetValue("UserPuid");
            DocumentId = Utils.TryParseGuid(base.TryGetValue("DocumentId"), Guid.Empty);
            EventName = base.TryGetValue("EventName");
            UserDisplayName = base.TryGetValue("UserDisplayName");
            EmailAddress = base.TryGetValue("EmailAddress");
            IpAddress = base.TryGetValue("IpAddress");
            DocumentName = base.TryGetValue("DocumentName");
            ParentFolderPath = base.TryGetValue("ParentFolderPath");
            ClientDevice = base.TryGetValue("ClientDevice");
            ClientOs = base.TryGetValue("ClientOs");
            ClientApplication = base.TryGetValue("ClientApplication");
        }
    }
}