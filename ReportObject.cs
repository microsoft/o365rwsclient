using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    /// <summary>
    ///  This is representing an object of the RWS Report returned
    /// </summary>
    public class ReportObject
    {
        protected Dictionary<string, string> properties = new Dictionary<string, string>();

        public DateTime Date
        {
            get;
            set;
        }

        public virtual void LoadFromXml(XmlNode node)
        {
            properties = new Dictionary<string, string>();
            foreach (XmlNode p in node)
            {
                string key = p.Name.Replace("d:", "");
                properties[key] = p.InnerText.ToString();
            }

            this.Date = Utils.TryParseDateTime(TryGetValue("Date"), DateTime.MinValue);
        }

        public string ConvertToXml()
        {
            string retval = null;
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                new XmlSerializer(this.GetType()).Serialize(writer, this);
            }
            retval = sb.ToString();

            return retval;
        }

        protected string TryGetValue(string key)
        {
            if (properties.ContainsKey(key))
            {
                return properties[key];
            }

            return null;
        }
    }
}