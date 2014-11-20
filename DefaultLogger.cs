using System;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class DefaultLogger : ITraceLogger
    {
        public void LogError(string message)
        {
            Console.WriteLine(message);
        }

        public void LogInformation(string message)
        {
            Console.WriteLine(message);
        }
    }
}