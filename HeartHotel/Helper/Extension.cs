using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HeartHotel
{
    public static class Extension
    {
        public static bool isImage(this string path)
        {
            var postedFileExtension = Path.GetExtension(path).Trim();
            bool img = true;
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                img = false;
            }
            return img;
        }

        public static string ToStandardURL(this string URL)
        {
            return "http://" + URL.Replace("http://", "").Replace("https://", "").Replace("www.", "");
        }

        public static string ToShortURL(this string url)
        {
            try
            {
                if (url != null)
                {
                    return url
                        .Replace("https://www.", "")
                        .Replace("http://www.", "")
                        .Replace("https://", "")
                        .Replace("http://", "")
                        .ToLower();
                }
            }
            catch { }

            return url ?? "";
        }

        public static string ToAparatLive(this string URL)
        {
            return "https://aparat.com/embed/Live/" + URL.Replace("www.", "").Replace("/live", "").Replace("aparat.com/", "");
        }

        public static int DateToInt(this string date)
        {
            return Convert.ToInt32(date.Replace("/", ""));
        }

        public static int TimeToInt(this string time)
        {
            return Convert.ToInt32(time.Replace(":", ""));
        }

        public static int DateTimeToInt(this string datetime)
        {
            return Convert.ToInt32(datetime.Replace("/", "").Replace(":", ""));
        }

        public static string ToArabic(this DateTime dt)
        {
            HijriCalendar hc = new HijriCalendar();
            int year = hc.GetYear(dt);
            int month = hc.GetMonth(dt);
            int day = hc.GetDayOfMonth(dt);

            DateTime HijriDate = new DateTime(year, month, day, new System.Globalization.HijriCalendar());

            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
        }

        public static string ToLongArabic(this DateTime dt)
        {
            HijriCalendar hc = new HijriCalendar();
            int year = hc.GetYear(dt);
            int month = hc.GetMonth(dt);
            int day = hc.GetDayOfMonth(dt);

            DateTime HijriDate = new DateTime(year, month, day, new System.Globalization.HijriCalendar());
            string[] sMonth = { "محرم", "صفر", "ربيع الأول", "ربيع الثاني", "جمادي الأول", "جمادى الثاني", "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة" };

            return day.ToString("00") + " " + sMonth[month - 1] + " " + year.ToString("0000");
        }

        public static string ToPersian(this DateTime dt)
        {
            //CultureInfo faIR = new CultureInfo("fa-IR");
            //faIR.DateTimeFormat.Calendar = new PersianCalendar();
            //CultureInfo.CurrentCulture = faIR;

            //string dateString = dt.ToStandard();

            //dateString = DateTime.ParseExact(dateString, "yyyy/MM/dd", faIR, DateTimeStyles.None).ToStandard();
            //return dateString;

            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            //DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.GregorianCalendar());
            //DateTime PersianDate = new DateTime(year, month, day);

            //return PersianDate.ToStandard();
            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
        }

        public static DateTime ToPersianDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            DateTime PersianDate = new DateTime(year, month, day);

            return PersianDate;
        }

        public static string ToPersianDateTime(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            DateTime PersianDate = new DateTime(year, month, day, new PersianCalendar());
            //DateTime PersianDate = new DateTime(year, month, day, dt.Hour, dt.Minute, dt.Second);

            //return PersianDate;
            //return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00") + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second; //+ " " + dt.ToString("tt", CultureInfo.InvariantCulture);
            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00") + " " + dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");
        }

        public static string ToLongPersian(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

            string[] sMonth = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
            string[] pd = PersianDate.ToStandard().Split('/');

            //return pd[2] + " " + sMonth[Int32.Parse(pd[1]) - 1] + " " + pd[0];
            return day.ToString("00") + " " + sMonth[month - 1] + " " + year.ToString("0000");
        }

        public static string ToStandard(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd");
        }

        public static DateTime ToGregorianDate(this DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();

            int year = gc.GetYear(dt);
            int month = gc.GetMonth(dt);
            int day = gc.GetDayOfMonth(dt);

            //DateTime GregorianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            DateTime GregorianDate = new DateTime(year, month, day, new System.Globalization.GregorianCalendar());

            return GregorianDate;

            //DateTime GregorianDate = new DateTime(year, month, day);

            //var result = GregorianDate.ToString(CultureInfo.InvariantCulture);
            //DateTime gdt = Convert.ToDateTime(result);

            //return gdt;
        }

        public static DateTime ToGregorianDateTime(this DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();

            int year = gc.GetYear(dt);
            int month = gc.GetMonth(dt);
            int day = gc.GetDayOfMonth(dt);

            int h = gc.GetHour(dt);
            int m = gc.GetMinute(dt);
            int s = gc.GetSecond(dt);

            DateTime GregorianDateTime = new DateTime(year, month, day, h, m, s, new System.Globalization.GregorianCalendar());

            return GregorianDateTime;
        }

        public static string ToGregorianDateTimeString(this DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();

            int year = gc.GetYear(dt);
            int month = gc.GetMonth(dt);
            int day = gc.GetDayOfMonth(dt);

            int h = gc.GetHour(dt);
            int m = gc.GetMinute(dt);
            int s = gc.GetSecond(dt);

            DateTime GregorianDateTime = new DateTime(year, month, day, h, m, s, new System.Globalization.GregorianCalendar());

            return GregorianDateTime.ToString("ddMMyyyy HH:mm:ss");
        }

        public static string ToGregorianDateTimeString(this DateTime dt, bool firstYear)
        {
            GregorianCalendar gc = new GregorianCalendar();

            int year = gc.GetYear(dt);
            int month = gc.GetMonth(dt);
            int day = gc.GetDayOfMonth(dt);

            int h = gc.GetHour(dt);
            int m = gc.GetMinute(dt);
            int s = gc.GetSecond(dt);

            DateTime GregorianDateTime = new DateTime(year, month, day, h, m, s, new System.Globalization.GregorianCalendar());

            return GregorianDateTime.ToString("yyyyMMdd HH:mm:ss");
        }

        public static DateTime ToGregorianDate(this string dt)
        {
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));

            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

            return PersianDate.ToGregorianDate();
        }

        public static DateTime ToGregorianDateTime(this string dt)
        {
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));
            int h = Convert.ToInt32(dt.Substring(11, 2));
            int m = Convert.ToInt32(dt.Substring(14, 2));
            int s = Convert.ToInt32(dt.Substring(17, 2));

            DateTime PersianDate = new DateTime(year, month, day, h, m, s, new System.Globalization.PersianCalendar());

            return PersianDate.ToGregorianDateTime();
        }

        public static string ToGregorianString(this string dt)
        {
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));

            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

            return PersianDate.ToGregorianDate().ToString("yyyyMMdd");
        }

        public static string ToGregorianStringDashed(this string dt)
        {
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));

            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

            return PersianDate.ToGregorianDate().ToString("yyyy-MM-dd");
        }

        public static string ToGregorian(this string dt)
        {
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));

            //DateTime gd = new DateTime(year, month, day);
            DateTime PersianDate = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

            //DateTime GregorianDate = new DateTime(year, month, day, new System.Globalization.GregorianCalendar());

            return PersianDate.ToGregorianDate().ToStandard();
        }

        public static string ToLongGregorian(this string dt)
        {
            string GDate = dt.ToGregorian();
            int year = Convert.ToInt32(GDate.Substring(0, 4));
            int month = Convert.ToInt32(GDate.Substring(5, 2));
            int day = Convert.ToInt32(GDate.Substring(8, 2));

            string[] sMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            return day.ToString("00") + " " + sMonth[month - 1] + " " + year.ToString("0000");
        }

        public static string SensitiveShow(this DateTime dt, int? Lang = 1)
        {
            string r = Lang == 1 ? dt.ToPersian() : dt.ToStandard();
            return r;
        }

        public static string ToPersianNumber(this string input)
        {
            if (input.Trim() == "") return "";

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            input = input.Replace("0", "۰");
            input = input.Replace("1", "۱");
            input = input.Replace("2", "۲");
            input = input.Replace("3", "۳");
            input = input.Replace("4", "۴");
            input = input.Replace("5", "۵");
            input = input.Replace("6", "۶");
            input = input.Replace("7", "۷");
            input = input.Replace("8", "۸");
            input = input.Replace("9", "۹");

            input = input.Replace("lkjhgfdsa@۰", "0");
            input = input.Replace("lkjhgfdsa@۱", "1");
            input = input.Replace("lkjhgfdsa@۲", "2");
            input = input.Replace("lkjhgfdsa@۳", "3");
            input = input.Replace("lkjhgfdsa@۴", "4");
            input = input.Replace("lkjhgfdsa@۵", "5");
            input = input.Replace("lkjhgfdsa@۶", "6");
            input = input.Replace("lkjhgfdsa@۷", "7");
            input = input.Replace("lkjhgfdsa@۸", "8");
            input = input.Replace("lkjhgfdsa@۹", "9");

            return input ?? string.Empty;
        }

        public static string ToEnglishNumber(this string input)
        {
            if (input.Trim() == "") return "";

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            input = input.Replace("۰", "0");
            input = input.Replace("۱", "1");
            input = input.Replace("۲", "2");
            input = input.Replace("۳", "3");
            input = input.Replace("۴", "4");
            input = input.Replace("۵", "5");
            input = input.Replace("۶", "6");
            input = input.Replace("۷", "7");
            input = input.Replace("۸", "8");
            input = input.Replace("۹", "9");

            return input;
        }

        public static string MajlesText4Bot(this string input)
        {
            input = input
                .Replace("\n", "")
                .Replace("<br>", Environment.NewLine)
                .Replace("<p>", Environment.NewLine)
                .Replace("<\\p>", "")
                .Replace("<div>", "")
                .Replace("<\\div>", "")
                ;
            return input;
        }

        public static string ToStandardChars(this string input)
        {
            try
            {
                if (input != null)
                {
                    input = input
                        .Replace("ي", "ی")
                        .Replace("ك", "ک");
                }
            }
            catch { }
            return input ?? string.Empty;
        }

        public static string ToHashtagCity(this string input)
        {
            try
            {
                var Section = input.Split("،");
                var City = Section[0].Split("-");
                Section[0] = "#" + City[1].Trim().Replace(" ", "_");
                // Section[0] = "#" + Section[0].Trim().Replace(" ", "_");
                input = String.Join("،", Section);
            }
            catch { }
            return input;
        }

        //public static string ToStandardChar(this string input)
        //{
        //    try
        //    {
        //        if (input != null)
        //        {
        //            input = input.Replace("ي", "ی").Replace("ﻙ", "ک");
        //        }
        //    }
        //    catch { }
        //    return input;
        //}

        public static string ToLinkURLs(this string input)
        {
            string output = input;
            //Regex regx = new Regex("(([\\w]+:)?//)?(([\\d\\w]|%[a-fA-f\\d]{2,2})+(:([\\d\\w]|%[a-fA-f\\d]{2,2})+)?@)?([\\d\\w][-\\d\\w]{0,253}[\\d\\w]\\.)+[\\w]{2,4}(:[\\d]+)?(/([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)*(\\?(&?([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})=?)*)?(#([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)?");
            //Regex regx = new Regex("(([\\w]+:)?//)?(([\\d\\w]|%[a-fA-f\\d]{2,2})+(:([\\d\\w]|%[a-fA-f\\d]{2,2})+)?@)?([\\d\\w]{0,253}[\\d\\w]\\.)+[\\w]{2,4}(:[\\d]+)?(/([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)*(\\?(&?([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})=?)*)?(#([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)?");
            Regex regx = new Regex("[(http(s)?):\\/\\/(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)");

            MatchCollection mactches = regx.Matches(output);

            foreach (Match match in mactches)
            {
                //output = output.Replace(match.Value, "<a href='" + match.Value + "' target='blank'>" + match.Value + "</a>");
                output = output.Replace(match.Value, "<div class=\"w-100 ltr text-start\"><a href='http://" + match.Value.Replace("http://", "").Replace("https://", "") + "' target='_blank'>" + match.Value + "</a></div>");
            }
            return output;
        }

        public static string ToStandardPoster(this string input)
        {
            string output = input;
            //Regex regx = new Regex("(([\\w]+:)?//)?(([\\d\\w]|%[a-fA-f\\d]{2,2})+(:([\\d\\w]|%[a-fA-f\\d]{2,2})+)?@)?([\\d\\w][-\\d\\w]{0,253}[\\d\\w]\\.)+[\\w]{2,4}(:[\\d]+)?(/([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)*(\\?(&?([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})=?)*)?(#([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)?");
            //Regex regx = new Regex("(([\\w]+:)?//)?(([\\d\\w]|%[a-fA-f\\d]{2,2})+(:([\\d\\w]|%[a-fA-f\\d]{2,2})+)?@)?([\\d\\w]{0,253}[\\d\\w]\\.)+[\\w]{2,4}(:[\\d]+)?(/([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)*(\\?(&?([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})=?)*)?(#([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)?");
            Regex regx = new Regex("[(http(s)?):\\/\\/(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)");

            MatchCollection mactches = regx.Matches(output);

            List<string> Urls = new List<string>();
            foreach (Match match in mactches)
            {
                Urls.Add(match.Value);
            }

            output = output.Replace(".", "");
            output = output.Replace("#", "");
            output = output.Replace("،", "");

            foreach (string url in Urls)
            {
                output = output.Replace(url.Replace(".", "").Replace("#", "").Replace("،", ""), "<div class=\"w-100 ltr text-start\">" + url + "</div>");
            }

            return output;
        }

        public static string NightTime(this DateTime dt)
        {
            string nightTime = "18:00";
            var currentDate = dt.ToPersian().Split("/");
            int mounth = Convert.ToInt32(currentDate[1]);
            int day = Convert.ToInt32(currentDate[2]);
            if ((mounth >= 1 && day >= 17) && (mounth <= 2 && day <= 21))
            {
                nightTime = "18:30";
            }
            if ((mounth >= 2 && day >= 22) && (mounth <= 5 && day <= 31))
            {
                nightTime = "19:00";
            }
            if ((mounth >= 6 && day >= 1) && (mounth <= 6 && day <= 18))
            {
                nightTime = "18:30";
            }
            if ((mounth >= 7 && day >= 8) && (mounth <= 7 && day <= 30))
            {
                nightTime = "17:30";
            }
            if ((mounth >= 8 && day >= 1) && (mounth <= 11 && day <= 9))
            {
                nightTime = "17:00";
            }
            if ((mounth >= 11 && day >= 10) && (mounth <= 11 && day <= 30))
            {
                nightTime = "17:30";
            }
            return nightTime;
        }
    }
}