using System;
using System.Collections.Generic;

namespace Microsoft.Office365.ReportingWebServiceClient
{
    public class QueryRange
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly int DividRangeFactor = 5;

        /// <summary>
        ///
        /// </summary>
        public QueryRange()
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        /// <summary>
        ///
        /// </summary>
        public QueryRange(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public DateTime GetRangedStartDate()
        {
            return (StartDate == DateTime.MinValue) ? DateTime.Now.AddYears(-3) : StartDate;
        }

        public DateTime GetRangedEndDate()
        {
            return (EndDate == DateTime.MinValue) ? DateTime.Now : EndDate;
        }

        /// <summary>
        /// This is a very important function in Range design.
        ///
        /// [2012-03-11, 2014-08-22]  => [2012-03-11, 2012-12-31], [2013-01-01, 2013-12-31], [2014-01-01, 2014-08-22]
        /// [2014-02-03, 2014-08-22]  => [2014-02-03, 2014-02-28], [2014-03-01, 2014-03-31], ..., [2014-08-01, 2014-08-22]
        /// [2014-08-03 2:00:00, 2014-08-03 4:00:00] => [2014-08-03 2:00:00, 2014-08-03 3:00:00], [2014-08-03 3:00:00, 2014-08-03 4:00:00]
        /// </summary>
        /// <returns></returns>
        public List<QueryRange> GetDividedRanges()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            DateTime rangedEndDate = GetRangedEndDate();

            TimeSpan span = rangedEndDate - rangedStartDate;
            if (span < new TimeSpan(0, 5, 0))
            {
                return null;
            }

            double intervalSeconds = span.TotalSeconds / DividRangeFactor;

            List<QueryRange> resultRange = new List<QueryRange>();
            QueryRange range;

            DateTime previousTimeStamp = rangedStartDate;
            DateTime lastTimeStamp;

            for (int i = 0; i < DividRangeFactor - 1; i++)
            {
                lastTimeStamp = previousTimeStamp.AddSeconds(intervalSeconds);

                range = new QueryRange(previousTimeStamp, lastTimeStamp);
                resultRange.Add(range);

                previousTimeStamp = lastTimeStamp;
            }

            range = new QueryRange(previousTimeStamp, rangedEndDate);
            resultRange.Add(range);

            return resultRange;
        }

        /// <summary>
        /// This is a very important function in Range design.
        ///
        /// [2012-03-11, 2014-08-22]  => [2012-03-11, 2012-12-31], [2013-01-01, 2013-12-31], [2014-01-01, 2014-08-22]
        /// [2014-02-03, 2014-08-22]  => [2014-02-03, 2014-02-28], [2014-03-01, 2014-03-31], ..., [2014-08-01, 2014-08-22]
        /// [2014-08-03 2:00:00, 2014-08-03 4:00:00] => [2014-08-03 2:00:00, 2014-08-03 3:00:00], [2014-08-03 3:00:00, 2014-08-03 4:00:00]
        /// </summary>
        /// <returns></returns>
        public List<QueryRange> GetDividedRanges_Old()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            DateTime rangedEndDate = GetRangedEndDate();
            DateTime endDatePrevious = rangedEndDate.AddSeconds(-1);

            List<QueryRange> resultRange = new List<QueryRange>();

            if (rangedStartDate >= rangedEndDate)
            {
                return null;
            }

            if (rangedStartDate.Year < endDatePrevious.Year)
            {
                return GetDividedRangesByYear();
            }

            if (rangedStartDate.Month < endDatePrevious.Month)
            {
                return GetDividedRangesByMonth();
            }

            if (rangedStartDate.Date < endDatePrevious.Date)
            {
                return GetDividedRangesByDay();
            }

            if (rangedStartDate.Hour < endDatePrevious.Hour)
            {
                return GetDividedRangesByHour();
            }

            // Returning NULL means the Range cannot be divided.
            return null;
        }

        private List<QueryRange> GetDividedRangesByYear()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            return GetDividedRangesByTimeslot(new DateTime(rangedStartDate.Year + 1, 1, 1), (date) => { return date.AddYears(1); });
        }

        private List<QueryRange> GetDividedRangesByMonth()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            return GetDividedRangesByTimeslot(new DateTime(rangedStartDate.Year, rangedStartDate.Month + 1, 1), (date) => { return date.AddMonths(1); });
        }

        private List<QueryRange> GetDividedRangesByDay()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            return GetDividedRangesByTimeslot(new DateTime(rangedStartDate.Year, rangedStartDate.Month, rangedStartDate.Day + 1), (date) => { return date.AddDays(1); });
        }

        private List<QueryRange> GetDividedRangesByHour()
        {
            DateTime rangedStartDate = GetRangedStartDate();
            return GetDividedRangesByTimeslot(new DateTime(rangedStartDate.Year, rangedStartDate.Month, rangedStartDate.Day, rangedStartDate.Hour + 1, 0, 0), (date) => { return date.AddHours(1); });
        }

        private List<QueryRange> GetDividedRangesByTimeslot(DateTime first, Func<DateTime, DateTime> GetNext)
        {
            List<QueryRange> resultRange = new List<QueryRange>();
            DateTime lastTimestamp = GetRangedStartDate();
            DateTime nextTimestamp = first;

            while (nextTimestamp < GetRangedEndDate())
            {
                resultRange.Add(new QueryRange(lastTimestamp, nextTimestamp));

                lastTimestamp = nextTimestamp;
                nextTimestamp = GetNext(nextTimestamp);
            }

            resultRange.Add(new QueryRange(lastTimestamp, GetRangedEndDate()));
            return resultRange;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return string.Format(@"({0}-{1})", StartDate.ToString("yyyy-MM-ddTHH:mm:ss"), EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}