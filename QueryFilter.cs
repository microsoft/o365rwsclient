using System;
using System.Text;
using System.Web;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class QueryFilter
    {
        public QueryRange QueryRange
        {
            get;
            set;
        }

        public int TopCount
        {
            get;
            set;
        }

        public int SkipCount
        {
            get;
            set;
        }

        public bool ExcludeStartItem
        {
            get;
            set;
        }

        public string CustomFilter
        {
            get;
            set;
        }

        public QueryFilter()
        {
            QueryRange = new QueryRange();
            TopCount = 0;
            SkipCount = 0;
            ExcludeStartItem = false;
        }

        /// <summary>
        /// Sample: $top=100&$filter=(Date%20gt%20datetime'2014-07-15T11%3A00%3A00')%20and%20(Date%20le%20datetime'2014-07-19T11%3A00%3A00')
        /// </summary>
        /// <returns></returns>
        public string ToUrlString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("?");
            if (TopCount > 0)
            {
                sb.AppendFormat(@"$top={0}&", TopCount);
            }

            if (SkipCount > 0)
            {
                sb.AppendFormat(@"$skip={0}&", SkipCount);
            }

            string startDateStr = QueryRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string endDateStr = QueryRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss");

            string startComparer = ExcludeStartItem ? "gt" : "ge";

            if (QueryRange.StartDate != DateTime.MinValue && QueryRange.EndDate != DateTime.MinValue)
            {
                sb.AppendFormat(@"$filter=((Date {0} datetime'{1}') and (Date lt datetime'{2}'))", startComparer, startDateStr, endDateStr);
            }
            else if (QueryRange.StartDate != DateTime.MinValue)
            {
                sb.AppendFormat(@"$filter=(Date {0} datetime'{1}')", startComparer, startDateStr);
            }
            else if (QueryRange.EndDate != DateTime.MinValue)
            {
                sb.AppendFormat(@"$filter=(Date lt datetime'{0}')", endDateStr);
            }

            return HttpUtility.UrlPathEncode(sb.ToString());
        }
    }
}