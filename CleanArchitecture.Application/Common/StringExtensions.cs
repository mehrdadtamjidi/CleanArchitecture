using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Application.Common
{
    public static class StringExtensions
    {

        private static readonly string InvalidEntry =
               "(^1{1,}$)|(^2{1,10}$)|(^3{1,}$)|(^4{1,}$)|(^5{1,}$)|(^6{1,}$)|(^7{1,}$)|(^8{1,}$)|(^9{1,}$)|(^0{1,}$)";

        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value);
        }

        public static string ToNumeric(this int value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToNumeric(this decimal value)
        {
            return value.ToString("N0");
        }

        public static string ToCurrency(this int value)
        {
            //fa-IR => current culture currency symbol => ریال
            //123456 => "123,123ریال"
            return value.ToString("C0");
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }

        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
        }

        public static string Fa2En(this string str)
        {
            return str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه"); //.Replace("ئ", "ی");
        }

        public static string? CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
        }

        public static string? NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }

        public static bool CheckSheba(this string str, bool checkLenght)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            str = str.Replace(" ", "").ToLower();
            //بررسی رشته وارد شده برای اینکه در فرمت شبا باشد
            var isSheba = Regex.IsMatch(str, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}",
                   RegexOptions.Compiled);

            if (!isSheba)
                return false;
            //طول شماره شبا را چک میکند کمتر نباشد
            if (str.Length < 26)
                return false;
            str = str.ToLower();
            //بررسی اعتبار سنجی اصلی شبا
            ////ابتدا گرفتن چهار رقم اول شبا
            var get4FirstDigit = str.Substring(0, 4);
            ////جایگزین کردن عدد 18 و 27 به جای آی و آر
            var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
            ////حذف چهار رقم اول از رشته شبا
            var removedShebaFirst4Digit = str.Replace(get4FirstDigit, "");
            ////کانکت کردن شبای باقیمانده با جایگزین شده چهار رقم اول
            var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
            ////تبدیل کردن شبا به عدد  - دسیمال تا 28 رقم را نگه میدارد
            var finalLongData = Convert.ToDecimal(newSheba);
            ////تقسیم عدد نهایی به مقدار 97 - اگر باقیمانده برابر با عدد یک شود این رشته شبا صحیح خواهد بود
            var finalReminder = finalLongData % 97;
            if (finalReminder == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsIzbSheba(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            var codeBank = str.Substring(4, 3);

            if (codeBank == "069")
                return true;
            return false;
        }

        public static bool IsCorrectDateTimeRequest(this string RequestDateTime)
        {
            if (string.IsNullOrWhiteSpace(RequestDateTime))
            {
                return false;
            }

            DateTime requestDateTime = Convert.ToDateTime(RequestDateTime);
            DateTime ServerDateTime = DateTime.Now;

            DateTime TwoMinBefore = ServerDateTime.Subtract(TimeSpan.FromMinutes(2));
            DateTime TwoMinAfter = ServerDateTime.AddMinutes(2);

            if (requestDateTime.Ticks > TwoMinBefore.Ticks && requestDateTime.Ticks < TwoMinAfter.Ticks)
            {
                return true;
            }

            return false;
        }

        public static bool IsCorrectDateTimeRequest2(this string RequestDateTime)
        {
            if (string.IsNullOrWhiteSpace(RequestDateTime))
            {
                return false;
            }

            DateTime requestDateTime = Convert.ToDateTime(RequestDateTime);
            DateTime ServerDateTime = DateTime.Now;

            DateTime TwoMinBefore = ServerDateTime.Subtract(TimeSpan.FromMinutes(5));
            DateTime TwoMinAfter = ServerDateTime.AddMinutes(5);

            if (requestDateTime.Ticks > TwoMinBefore.Ticks && requestDateTime.Ticks < TwoMinAfter.Ticks)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime UnixTimeToDateTime(this long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        public static string ToMaskedPan(this string value)
        {
            if (value.Length == 16)
            {
                string maskPan = string.Format("{0}{1}{2}", value.Substring(0, 6),
                       "******", value.Substring(12, 4));
                return maskPan;
            }
            else
            {
                return "****";
            }
        }

        public static long GetTimestamp(this DateTime value)
        {
            return Convert.ToInt64(value.ToString("yyyyMMddHHmmssffff"));
        }

        public static bool IsNationalId(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) ||
                Regex.IsMatch(input: str, pattern: InvalidEntry) ||
                !Regex.IsMatch(input: str, pattern: "^[0-9]{10}$")) return false;
            var a = Convert.ToInt32(str[9].ToString());
            long b = 0;
            var coefficient = 10;
            foreach (var value in str.Substring(0, 9))
            {
                b += Convert.ToInt32(value.ToString()) * coefficient;
                coefficient -= 1;
            }

            var c = Convert.ToInt32(b % 11);
            return c == 0 & a == c | c == 1 & a == 1 | c > 1 & a == 11 - c;
        }

        public static bool IsValidMobilePhone(this string mobile)
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                    return false;
                var r = new Regex(@"^(?:0|98|\+98|\+980|0098|098|00980)?(9\d{9})$");
                return r.IsMatch(mobile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool VerifyIranianNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                return false;

            nationalCode = nationalCode.Trim();

            if (nationalCode.Length != 10 || !IsNumeric(nationalCode))
                return false;

            int[] arrayNationalCode = nationalCode.Select(x => int.Parse(x.ToString())).ToArray();

            int checkDigit = arrayNationalCode[9];
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += arrayNationalCode[i] * (10 - i);
            }

            int remainder = sum % 11;

            return remainder < 2 && checkDigit == remainder || remainder >= 2 && checkDigit == 11 - remainder;
        }

        public static bool IsNumeric(string input)
        {
            return long.TryParse(input, out _);
        }

        public static bool IsOnlyPersianCharacters(this string input)
        {
            // Persian Unicode range: \u0600 - \u06FF
            string pattern = @"^[\u0600-\u06FF\s]+$";

            return Regex.IsMatch(input, pattern);
        }

        public static bool IsOnlyEnglishCharacters(this string input)
        {
            // English Unicode range: \u0020 - \u007E
            string pattern = @"^[\u0020-\u007E]+$";

            return Regex.IsMatch(input, pattern);
        }

        public static bool IsValidBase64(this string input)
        {
            try
            {
                // Attempt to decode the input as Base64
                Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsOnlyNumbers(this string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static DateTime ToDateTimeNow(this string value)
        {
            string format = "yyyy/MM/dd";
            if (DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                date = date.Date; // Set the time to 00:00:00
                DateTime currentTime = DateTime.Now;
                date = date.AddHours(currentTime.Hour).AddMinutes(currentTime.Minute).AddSeconds(currentTime.Second);
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static long ToTimestampNow(this string value)
        {
            string format = "yyyy/MM/dd";
            if (DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                DateTime currentTime = DateTime.Now;
                date = date.Date.AddHours(currentTime.Hour).AddMinutes(currentTime.Minute).AddSeconds(currentTime.Second);

                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsed = date.ToUniversalTime() - epoch;

                return (long)elapsed.TotalSeconds;
            }
            else
            {
                return 0; // or any other default value for invalid input
            }
        }

        public static DateTime ToDateTimeAtEndOfDay(this string value)
        {
            string format = "yyyy/MM/dd";
            if (DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                date = date.Date; // Set the time to 00:00:00
                date = date.AddHours(23).AddMinutes(59).AddSeconds(59); // Set the time to the last moment of the day
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static long ToTimestampAtEndOfDay(this string value)
        {
            string format = "yyyy/MM/dd";
            if (DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                date = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // Set the time to the last moment of the day

                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsed = date.ToUniversalTime() - epoch;

                return (long)elapsed.TotalSeconds;
            }
            else
            {
                return 0; // or any other default value for invalid input
            }
        }

        public static string GetMd5Hash(this string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a StringBuilder to collect the bytes and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public static string CleanIranianMobileNumber(this string mobileNumber)
        {
            // Remove "98" if it's the first two characters
            if (mobileNumber.StartsWith("98") && mobileNumber.Length >= 10)
            {
                mobileNumber = mobileNumber.Substring(2);
            }

            // Remove "0" if it's the first character
            if (mobileNumber.StartsWith("0") && mobileNumber.Length >= 10)
            {
                mobileNumber = mobileNumber.Substring(1);
            }

            return mobileNumber;
        }

        public static string CleanUrl(this string url)
        {
            // Define a regular expression pattern to match "https://", "http://", and "www."
            string pattern = @"^(https?://)?(www\.)?";

            // Use Regex.Replace to remove the matched prefixes
            string cleanedUrl = Regex.Replace(url, pattern, "");

            return cleanedUrl;
        }

        public static int CalculateAge(this DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age))
                age--;
            return age;
        }
    }
}