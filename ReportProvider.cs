using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    /// <summary>
    /// This is not used
    /// </summary>
    /// <typeparam name="TReport"></typeparam>
    public class ReportProvider
    {
        private Uri serviceEndpointUri;

        private ICredentials serviceCredential;

        private ITraceLogger logger;

        public ReportProvider(string serviceEndpoint, string userName, string password, ITraceLogger logger)
        {
            this.serviceEndpointUri = new Uri(serviceEndpoint);
            this.serviceCredential = new NetworkCredential(userName, password);
            this.logger = logger;
        }

        public ReportProvider(string serviceEndpoint)
        {
            this.serviceEndpointUri = new Uri(serviceEndpoint);
        }

        public void setCredential(string userName, string password)
        {
            serviceCredential = new NetworkCredential(userName, password);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<XmlNode> GetResponseXml(Type reportType, QueryFilter filter)
        {
            string serviceFullUrl = string.Format("{0}/{1}{2}", serviceEndpointUri.AbsoluteUri, reportType.Name, filter.ToUrlString());

            List<XmlNode> result = new List<XmlNode>();

            Stream responseContent = GetResponseContent(serviceFullUrl);
            XmlDocument doc = new XmlDocument();
            doc.Load(responseContent);

            XmlNodeList statuses = doc.DocumentElement.ChildNodes;

            foreach (XmlNode n in statuses)
            {
                if (n.Name == "entry")
                {
                    XmlNode node = n.LastChild.FirstChild;
                    result.Add(node);
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceFullUrl"></param>
        /// <returns></returns>
        private Stream GetResponseContent(string serviceFullUrl)
        {
            logger.LogInformation(string.Format("Request URL : {0}", serviceFullUrl));

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.Credentials = serviceCredential;
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = GetAsyncResult<HttpResponseMessage>(client.GetAsync(serviceFullUrl));

                        Stream responseContent = GetAsyncResult<Stream>(response.Content.ReadAsStreamAsync());

                        if (responseContent != null)
                        {
                            return responseContent;
                        }
                        else
                        {
                            throw new Exception("Response content is Null");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        private T GetAsyncResult<T>(Task<T> asyncFunction)
        {
            asyncFunction.Wait();
            return asyncFunction.Result;
        }
    }
}