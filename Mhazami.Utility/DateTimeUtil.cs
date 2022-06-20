using System.Globalization;

namespace Mhazami.Utility
{
    public static class DateTimeUtil
    {
        public static List<int> GetShamsiYearsList(int from, int? to = null)
        {
            var list = new List<int>();
            if (from == 0 || from.ToString().Length != 4) return list;
            var persianCalendar = new PersianCalendar();
            if (to == null) to = persianCalendar.GetYear(DateTime.Now);
            for (int i = from; i <= to; i++)
                list.Add(i);
            return list;

        }

        public static List<int> GetYearsList(int from, int? to = null)
        {
            var list = new List<int>();
            if (from == 0 || from.ToString().Length != 4) return list;
            if (to == null) to = DateTime.Now.Year;
            for (int i = from; i <= to; i++)
                list.Add(i);
            return list;

        }

        public static int GetShamsiYearNow()
        {
            return new PersianCalendar().GetYear(DateTime.Now);
        }
        public static class PersianDate
        {

            public static CultureInfo PersianCulture()
            {
                var result = new CultureInfo("fa-IR")
                {
                    DateTimeFormat =
                    {
                        FirstDayOfWeek = DayOfWeek.Saturday,
                        DayNames =
                            new[] { Resources.Utility.Sunday, Resources.Utility.Monday, Resources.Utility.Tuesday, Resources.Utility.Wednesday, Resources.Utility.Thursday, Resources.Utility.Friday, Resources.Utility.Saturday },
                        MonthNames =
                            new[]
                                                 {
                                                     "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر",
                                                     "آبان", "آذر", "دی", "بهمن", "اسفند", ""
                                                 }
                    }
                };
                return result;
            }

            public static string FormatTime(string time)
            {
                if (string.IsNullOrEmpty(time)) return time;
                var parts = time.Split(':');
                if (parts.Length != 2)
                    throw new Exception("Time string was not in correct format.");
                return string.Format("{0:D2}:{1:D2}", parts[0].ToInt(), parts[1].ToInt());
            }
        }

        public static string GetCultureDate(this DateTime date, string culture)
        {
            switch (culture)
            {
                case "fa-IR":
                    return date.ShamsiDate();
                default:
                    return date.ToShortDateString();
            }
        }

        public static string GetCultureDate(this string date, string culture)
        {
            switch (culture)
            {
                case "fa-IR":
                    return date;
                default:
                    return ShamsiDateToGregorianDate(date).ToShortDateString();
            }
        }

        public static string GetTime(this DateTime date)
        {
            return string.Format("{0:D2}:{1:D2}", date.Hour, date.Minute);
        }

        public static string GetTimewithSecond(this DateTime date)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", date.Hour, date.Minute, date.Second);
        }

        public static string ShamsiDate(this DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0:D4}/{1:D2}/{2:D2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
        }

        public static string HijriDate(this DateTime date)
        {
            var hc = new HijriCalendar();
            return string.Format("{0:D4}/{1:D2}/{2:D2}", hc.GetYear(date), hc.GetMonth(date), hc.GetDayOfMonth(date));
        }

        public static DateTime ShamsiDateToGregorianDate(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");
            var pc = new PersianCalendar();
            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in shamsi date.");

            return pc.ToDateTime(parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt(), 0, 0, 0, 0);
        }

        public static DateTime DateToGregorianDate(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");

            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in shamsi date.");

            return new DateTime(parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt(), 0, 0, 0, 0);
        }
        public static DateTime GetStartOfWeek(DateTime input)
        {

            int dayOfWeek = (((int)input.DayOfWeek) + 6) % 7;
            return input.Date.AddDays(-dayOfWeek);
        }

        public static int GetWeeks(DateTime start, DateTime end)
        {
            start = GetStartOfWeek(start);
            end = GetStartOfWeek(end);
            int days = (int)(end - start).TotalDays;
            double value = (days / 7);
            var round = Math.Round(value, 0, MidpointRounding.ToEven);
            return (int)(round + 1);
        }

        public static int GetYears(DateTime start, DateTime end)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = end - start;
            return (zeroTime + span).Year - 1;
        }
        public static double GetMonths(DateTime start, DateTime end)
        {
            var value = end.Subtract(start).Days / (365.25 / 12);
            var months = Math.Round(value, 0, MidpointRounding.ToEven);
            return months;
        }
        public static string ShamsiDateToHijriDate(string date)
        {
            if (string.IsNullOrEmpty(date)) return date;
            return ShamsiDateToGregorianDate(date).HijriDate();
        }

        public static int ComputeBetweenDate(string StartDate, string EndDate)
        {
            if (string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
                return 0;
            var pc = new PersianCalendar();
            int year = 0;
            int month = 0;
            int day = 0;

            ParseShamsiDate(StartDate, ref year, ref month, ref day);
            DateTime sDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

            ParseShamsiDate(EndDate, ref year, ref month, ref day);
            DateTime eDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

            TimeSpan ts = eDate.Subtract(sDate);
            return ts.Days;
        }

        public static void ParseShamsiDate(string Date, ref int Year, ref int Month, ref int Day)
        {
            if (string.IsNullOrEmpty(Date)) return;
            if (Date.Length != 10)
                throw new ArgumentException("The date format is not in correct format.");
            try
            {
                Year = Date.Substring(0, 4).ToInt();
                Month = Date.Substring(5, 2).ToInt();
                Day = Date.Substring(8, 2).ToInt();
            }
            catch
            {
                throw new ArgumentException("The date format is not in correct format.");
            }
        }

        public static string AddToDate(string date, int year, int month, int day)
        {
            if (string.IsNullOrEmpty(date)) return date;
            if (date.Length == 10)
            {
                try
                {
                    var gregorianDate = ShamsiDateToGregorianDate(date);
                    var newDate = new PersianCalendar();
                    var timeSpan = newDate.AddMonths(gregorianDate, month).AddYears(year).AddDays(day);
                    return string.Format("{0:D4}/{1:D2}/{2:D2}", newDate.GetYear(timeSpan), newDate.GetMonth(timeSpan), newDate.GetDayOfMonth(timeSpan));
                }
                catch
                {
                    return "The date format is not in correct format.";
                }
            }
            return "The date format is not in correct format.";
        }

        public static string GetMonthString(short monthId)
        {
            string str = "";
            switch (monthId)
            {
                case 1:
                    str = "فروردین";
                    break;
                case 2:
                    str = "اردیبهشت";
                    break;
                case 3:
                    str = "خرداد";
                    break;
                case 4:
                    str = "تیر";
                    break;
                case 5:
                    str = "مرداد";
                    break;
                case 6:
                    str = "شهریور";
                    break;
                case 7:
                    str = "مهر";
                    break;
                case 8:
                    str = "آبان";
                    break;
                case 9:
                    str = "آذر";
                    break;
                case 10:
                    str = "دی";
                    break;
                case 11:
                    str = "بهمن";
                    break;
                case 12:
                    str = "اسفند";
                    break;
            }
            return str;
        }

        public static string ComputeBetweenDateTime(string greaterDate, string greaterTime, string lowerDate, string lowerTime)
        {
            var dateTime = GetDifferenceBetweenDateTime(greaterDate, greaterTime, lowerDate, lowerTime);
            return GetDifferenceBetweenTextValue(dateTime);


        }

        public static string GetDifferenceBetweenTextValue(TimeSpan timeSpan)
        {
            string result;
            var dayDiff = timeSpan.Days;
            var hourDiff = timeSpan.Hours;
            var minuteDiff = timeSpan.Minutes;

            if (dayDiff == 0)
            {
                if (hourDiff == 0)
                {
                    if (minuteDiff == 0) minuteDiff++;
                    result = string.Format("{0} " + "دقیقه" + " ", minuteDiff);
                }
                else
                {
                    result = string.Format("{0} " + "ساعت" + " ", hourDiff);
                    if (minuteDiff > 0)
                        result += " " + "و" + " " + minuteDiff + " " + "دقیقه" + " ";
                }
            }
            else
            {
                result = string.Format("{0} " + "روز" + " ", dayDiff);
                if (hourDiff > 0 && dayDiff < 5)
                    result += " " + "و" + " " + hourDiff + " " + "ساعت" + " ";
            }

            return (result.Contains("-") ? result.Replace("-", "") + "مانده" : result + "گذشته");
        }
        public static TimeSpan GetDifferenceBetweenDateTime(string greaterDate, string greaterTime, string lowerDate, string lowerTime)
        {

            var greater = new TimeSpan(greaterTime.Split(':')[0].ToInt(), greaterTime.Split(':')[1].ToInt(), 0);
            var lower = new TimeSpan(lowerTime.Split(':')[0].ToInt(), lowerTime.Split(':')[1].ToInt(), 0);
            var fromDate = ShamsiDateToGregorianDate(greaterDate);
            var toDate = ShamsiDateToGregorianDate(lowerDate);
            fromDate = fromDate.AddHours(greater.Hours);
            fromDate = fromDate.AddMinutes(greater.Minutes);
            toDate = toDate.AddHours(lower.Hours);
            toDate = toDate.AddMinutes(lower.Minutes);
            return fromDate.Subtract(toDate);



        }
        public static string GetDayOfWeekString(short dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return Resources.Utility.Saturday;
                case 2:
                    return Resources.Utility.Sunday;
                case 3:
                    return Resources.Utility.Monday;
                case 4:
                    return Resources.Utility.Tuesday;
                case 5:
                    return Resources.Utility.Wednesday;
                case 6:
                    return Resources.Utility.Thursday;
                case 7:
                    return Resources.Utility.Friday;
                default:
                    return string.Empty;
            }
        }
        public static string ConvertDateToPerisanMonth(this string Date)
        {
            var year = 0;
            var month = 0;
            var day = 0;
            ParseShamsiDate(Date, ref year,
                                         ref month, ref day);

            var monthString = GetMonthString((short)month);
            Date = string.Format("{0} {1} {2}", day, monthString, year);
            return Date;
        }
        public static bool ValidateParsianDate(string date)
        {
            bool status = true;

            try
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                var dateParts = date.Split(new char[] { '/' }).Select(d => int.Parse(d)).ToArray();
                var pdate = persianCalendar.ToDateTime(dateParts[0], dateParts[1], dateParts[2], 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                status = false;
            }

            return status;
        }
    }
}
