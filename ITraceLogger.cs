namespace Microsoft.Office365.ReportingWebServiceClient
{
    public interface ITraceLogger
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void LogInformation(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
}