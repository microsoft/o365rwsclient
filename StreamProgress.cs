using System;
using System.IO;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    /// <summary>
    ///
    /// </summary>
    public class StreamProgress
    {
        public string Identifier
        {
            get;
            set;
        }

        public DateTime TimeStamp
        {
            get;
            set;
        }

        public int SkipCount
        {
            get;
            set;
        }

        public bool ExcludeStartItem { get; set; }

        public StreamProgress(string streamName)
            : this(streamName, DateTime.MinValue, 0, false)
        {
        }

        public StreamProgress(string streamName, DateTime timestamp, bool excludeStartItem)
            : this(streamName, timestamp, 0, excludeStartItem)
        {
        }

        public StreamProgress(string streamName, DateTime timestamp, int skipCount, bool excludeStartItem)
        {
            this.Identifier = streamName;
            this.TimeStamp = timestamp;
            this.SkipCount = skipCount;
            this.ExcludeStartItem = excludeStartItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        /// <param name="timestamp"></param>
        public void SaveProgress()
        {
            string fileName = GetIdenticalFileNameForStream(this.Identifier);
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine(this.TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.ffff"));
                sw.WriteLine(this.SkipCount);
                sw.WriteLine(this.ExcludeStartItem);
            }
        }

        public void ClearProgress()
        {
            string fileName = GetIdenticalFileNameForStream(this.Identifier);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        public static void ClearProgress(string streamName)
        {
            StreamProgress progress = new StreamProgress(streamName, DateTime.MinValue, false);
            progress.ClearProgress();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        /// <returns></returns>
        public static StreamProgress GetProgress(string streamName)
        {
            string fileName = GetIdenticalFileNameForStream(streamName);
            StreamProgress progress = new StreamProgress(streamName);

            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string dateStr = sr.ReadLine();
                    try
                    {
                        progress.TimeStamp = DateTime.Parse(dateStr);
                    }
                    catch
                    {
                        progress.TimeStamp = DateTime.MinValue;
                    }

                    string skipStr = sr.ReadLine();
                    try
                    {
                        progress.SkipCount = int.Parse(skipStr);
                    }
                    catch
                    {
                        progress.SkipCount = 0;
                    }

                    string exclStr = sr.ReadLine();
                    try
                    {
                        progress.ExcludeStartItem = bool.Parse(exclStr);
                    }
                    catch
                    {
                        progress.ExcludeStartItem = false;
                    }
                }
            }

            return progress;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        /// <returns></returns>
        private static string GetIdenticalFileNameForStream(string streamName)
        {
            return string.Format("{0}.progress", streamName.Replace(":", "-").Replace("/", ""));
        }
    }
}