using System;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class Utils
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid TryParseGuid(string value, Guid defaultValue)
        {
            Guid result;
            if (Guid.TryParse(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime TryParseDateTime(string value, DateTime defaultValue)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int TryParseInt(string value, int defaultValue)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool TryParseBoolean(string value, bool defaultValue)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 TryParseInt64(string value, Int64 defaultValue)
        {
            Int64 result;
            if (Int64.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}