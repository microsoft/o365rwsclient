using System;
using System.IO;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    /// <summary>
    ///
    /// </summary>
    public class StreamProgress
    {
        #region Properties

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

        public string FilePath { get; set; }

        #endregion Properties

        #region Constructors

        public StreamProgress(string filePath, string streamName)
            : this(filePath, streamName, DateTime.MinValue, 0, false)
        {
        }

        public StreamProgress(string filePath, string streamName, DateTime timestamp, bool excludeStartItem)
            : this(filePath, streamName, timestamp, 0, excludeStartItem)
        {
        }

        public StreamProgress(string filePath, string streamName, DateTime timestamp, int skipCount, bool excludeStartItem)
        {
            this.FilePath = filePath;
            this.Identifier = streamName;
            this.TimeStamp = timestamp;
            this.SkipCount = skipCount;
            this.ExcludeStartItem = excludeStartItem;
        }

        #endregion Constructors

        #region Private methods

        /// <summary>
        /// Returns the file name only if FilePath property is null or empty
        /// or Returns file name with full path if a FilePath is specified
        /// </summary>
        /// <returns></returns>
        private string GetIdenticalFileNameAndPathForStream()
        {
            if (String.IsNullOrWhiteSpace(this.FilePath))
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.progress", this.Identifier.Replace(":", "-").Replace("/", "")));
            else
                return Path.Combine(this.FilePath, string.Format("{0}.progress", this.Identifier.Replace(":", "-").Replace("/", "")));
        }

        #endregion Private methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        /// <param name="timestamp"></param>
        public void SaveProgress()
        {
            string fileName = GetIdenticalFileNameAndPathForStream();
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine(this.TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.ffff"));
                sw.WriteLine(this.SkipCount);
                sw.WriteLine(this.ExcludeStartItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearProgress()
        {
            string fileName = GetIdenticalFileNameAndPathForStream();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="streamName"></param>
        //public static void ClearProgress(string streamName)
        //{
        //    StreamProgress progress = new StreamProgress(streamName, DateTime.MinValue, false);
        //    progress.ClearProgress();
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="streamName"></param>
        /// <returns></returns>
        public StreamProgress GetProgress()
        {
            string fileName = GetIdenticalFileNameAndPathForStream();

            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string dateStr = sr.ReadLine();
                    try
                    {
                        this.TimeStamp = DateTime.Parse(dateStr);
                    }
                    catch
                    {
                        this.TimeStamp = DateTime.MinValue;
                    }

                    string skipStr = sr.ReadLine();
                    try
                    {
                        this.SkipCount = int.Parse(skipStr);
                    }
                    catch
                    {
                        this.SkipCount = 0;
                    }

                    string exclStr = sr.ReadLine();
                    try
                    {
                        this.ExcludeStartItem = bool.Parse(exclStr);
                    }
                    catch
                    {
                        this.ExcludeStartItem = false;
                    }
                }
            }

            return this;
        }
    }
}