using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Mhazami.Utility
{
    public static class Utils
    {



        public static long GenerateRandomNumber(int size)
        {
            var random = new Random(DateTime.Now.Millisecond);
            const string chars = "123456789";
            var builder = new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return builder.ToLong();
        }


        public static void FillObject(Object toObj, Object fromObj, bool fillHasValueProp = true)
        {
            if (toObj == null || fromObj == null) return;
            var propertyInfos = fromObj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {

                var property = toObj.GetType().GetProperty(propertyInfo.Name);
                if (property == null || !property.CanWrite) continue;
                var sourcevalue = property.GetValue(toObj, null);
                if (!fillHasValueProp && sourcevalue != null) continue;
                var value = propertyInfo.GetValue(fromObj, null);
                if (property.PropertyType.IsEnum && value != null && !string.IsNullOrEmpty(value.ToString().Trim()))
                    property.SetValue(toObj, Enum.Parse(property.PropertyType, value.ToString()), null);
                else
                {
                    if (property.PropertyType != propertyInfo.PropertyType) continue;
                    property.SetValue(toObj, value, null);
                }


            }
        }
        //public static T GetQueryStringValue<T>(string url, string tag)
        //{
        //    if (System.Web.HttpContext.Current == null) return default(T);
        //    var completeurl = new Uri(System.Web.HttpContext.Current.Request.Url, url);
        //    string param1 = HttpUtility.ParseQueryString(completeurl.Query).Get(tag);
        //    if (string.IsNullOrEmpty(param1)) return default(T);
        //    return SetIsValueTypeValue<T>(param1);
        //}
        private static T SetIsValueTypeValue<T>(object value)
        {
            var type = typeof(T);
            if (value == DBNull.Value) return default(T);
            if (type.IsEnum) return (T)Enum.Parse(type, value.ToString());
            try
            {
                return (T)value;
            }
            catch
            {

                return (T)System.Convert.ChangeType(value, type);
            }

        }
        public static bool IsValueType<T>()
        {

            return IsValueType(typeof(T));
        }
        public static bool IsValueType(Type type)
        {



            type = type.GetTypeValidValue();
            if (type.IsEnum)
                return true;
            if (type == typeof(Guid))
                return true;
            switch (Type.GetTypeCode(type))
            {

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }

        }
        public static T MapData<T>(NameValueCollection collection, T obj) where T : class
        {
            if (obj == null)
                obj = Activator.CreateInstance<T>();
            foreach (var property in obj.GetType().GetProperties())
            {
                var val = collection.Get(property.Name);
                if (val != null)
                {
                    property.SetValue(obj, TryCast(property, collection[property.Name]), null);
                }
            }
            return obj;
        }
        public static NameValueCollection GetQuerys(Uri uri)
        {
            var result = new NameValueCollection();
            if (!string.IsNullOrEmpty(uri.Query))
            {
                var arr = uri.Query.Substring(1).Split('&');

                foreach (var part in arr)
                {
                    if (part.Split('=').Length > 2)
                    {
                        var parts = part.Split('=');
                        result.Add(parts[0], part.Substring(part.IndexOf('=') + 1, part.Length - part.IndexOf('=') - 1));
                    }
                    else
                    {
                        var parts = part.Split('=');
                        result.Add(parts[0], parts[1]);
                    }
                }
            }
            return result;
        }
        public enum PasswordStrength
        {
            None = 0,
            // Blank Password (empty and/or space chars only)
            Blank = 1,
            // Either too short (less than 5 chars), one-case letters only or digits only
            VeryWeak = 2,
            // At least 5 characters, one strong condition met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
            Weak = 3,
            // At least 5 characters, two strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
            Medium = 4,
            // At least 8 characters, three strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
            Strong = 5,
            // At least 8 characters, all strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
            VeryStrong = 6
        }
        /// <summary>
        /// Generic method to retrieve password strength: use this for general purpose scenarios,
        /// i.e. when you don't have a strict policy to follow.
        /// </summary>
        ///  /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordStrength GetPasswordStrength(string password)
        {
            var score = 0;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
            if (!(password.Length >= 5)) return PasswordStrength.VeryWeak;
            if (!(password.Length >= 5)) score++;
            // Returns TRUE if the password has at least one uppercase letter And lowercase letter
            if (password.Any(char.IsUpper) && password.Any(char.IsLower)) score++;
            // Returns TRUE if the password has at least one digit
            if (password.Any(char.IsDigit)) score++;
            if (password.Distinct().Count() >= 5) score++;
            // Returns TRUE if the password has at least one special character
            if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1) score++;
            return (PasswordStrength)score;
        }
        public static DataTable ObjectToTable(List<object> obj)
        {
            var table = new DataTable();
            if (obj != null && obj.Count > 0)
            {
                var item = obj[0];
                foreach (var property in item.GetType().GetProperties())
                {
                    var column = new DataColumn();
                    var propertyType = property.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                        column.AllowDBNull = true;
                    }
                    column.DataType = propertyType;
                    column.ColumnName = property.Name;
                    table.Columns.Add(column);
                }

                foreach (var value in obj)
                {
                    var row = table.NewRow();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        var propertyType = property.PropertyType.GetTypeValidValue();
                        if (property.GetValue(value, null) != null)
                            row[property.Name] = System.Convert.ChangeType(property.GetValue(value, null), propertyType);
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static Type GetTypeValidValue(this Type type)
        {

            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
            return type;
        }

        private static object TryCast(PropertyInfo property, string value)
        {
            var propertyType = property.PropertyType.GetTypeValidValue();
            switch (Type.GetTypeCode(propertyType))
            {

                case TypeCode.Boolean:
                    return value.ToBool();
                case TypeCode.Char:
                    return value;
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return value.ToByte();
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return value.ToShort();
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return value.ToInt();
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return value.ToLong();
                case TypeCode.Single:
                case TypeCode.Double:
                    return value.ToDouble();
                case TypeCode.Decimal:
                    return value.ToDecimal();
                case TypeCode.DateTime:
                    return value.ToDateTime();
                case TypeCode.String:
                    return value;

            }
            return null;
        }

        public static bool In<T>(this T obj, IEnumerable<T> items)
        {
            return items.Contains(obj);
        }
        public static IEnumerable<T> Union<T>(IEnumerable<T> source, IEnumerable<T> target)
        {
            return source.Union(target);
        }
        public static bool NotIn<T>(this T obj, IEnumerable<T> items)
        {
            return !items.Contains(obj);
        }
        public static double GetObjectSize(this object obj)
        {
            double size = 0;
            using (Stream s = new MemoryStream())
            {

                var formatter = new BinaryFormatter();
                formatter.Serialize(s, obj);
                size += ((double)s.Length / 8);

            }
            return size;
        }
        public static double GetObjectSize(this IEnumerable<object> objlist)
        {
            double size = 0;
            var formatter = new BinaryFormatter();
            var cacheInfos = objlist.ToList();
            foreach (var cacheInfo in cacheInfos)
            {
                Stream s = new MemoryStream();
                formatter.Serialize(s, cacheInfo);
                size += ((double)s.Length / 8);

            }
            return size;
        }

        /// <summary>
        /// تبدیل عدد یه سایز بایت یا مگابایت و غیره ...
        /// </summary>
        /// <param name="number">عدد</param>
        /// <returns></returns>
        public static string GetNumberByteSize(this double number)
        {
            var names = Enum.GetNames(typeof(FileSizeType));
            if (number == 0) return string.Format("{0:n1} {1}", 0, names[0]);
            var mag = (int)Math.Log(number, 1024);
            double adjustedSize = number / (1L << (mag * 10));
            return string.Format("{0:n1} {1}", adjustedSize, names[mag]);
        }

        public static FileSizeType GetNumberByteSizeType(this double number)
        {
            var names = Enum.GetNames(typeof(FileSizeType));
            if (number == 0) return names[0].ToEnum<FileSizeType>();
            var mag = (int)Math.Log(number, 1024);
            return names[mag].ToEnum<FileSizeType>();
        }

        public enum FileSizeType
        {
            bytes, KB, MB, GB, TB, PB, EB, ZB, YB
        }



        public static bool isNumeric(this string val, NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle, CultureInfo.CurrentCulture, out result);
        }
        public static bool IsNumericType(this object o)
        {
            var propertyType = o.GetType().GetTypeValidValue();
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
        public static string ConvertStringToHTML(string str)
        {
            str = str.Replace("\"", "&quot;");
            str = str.Replace("'", "&#39;");
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            return str;
        }

        public static string EncodeString(string m_enc)
        {
            byte[] toEncodeAsBytes =
            System.Text.Encoding.UTF8.GetBytes(m_enc);
            string returnValue =
            System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeString(string m_enc)
        {
            byte[] encodedDataAsBytes =
            System.Convert.FromBase64String(m_enc);
            string returnValue =
            System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }


        public static string ConvertHtmlToString(string str)
        {
            if (String.IsNullOrEmpty(str)) return "";

            str = System.Net.WebUtility.HtmlDecode(str);
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&#39;", "'");
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");


            str = str.Replace("&#1740;", "ی");
            str = str.Replace("&#1728;", "ه");
            str = str.Replace("&#8204;", " ");
            str = str.Replace("&#171;", "«");
            str = str.Replace("&#187;", "»");
            str = str.Replace("&#8211;", "-");
            str = str.Replace("&#160;", " ");
            str = str.Replace("\r\n", "<br>");
            return str;
        }

        public static bool IsEmail(string inputEmail)
        {
            var regex = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            var match = regex.Match(inputEmail);
            return match.Success;

        }


        public static string GetSimpleConnectionString(string entityConnection)
        {
            const string pattern = "provider\\s.+";
            entityConnection = entityConnection.ToLower();
            var regex = new Regex(pattern);
            var match = regex.Match(entityConnection);
            if (match.Success)
            {
                var value = match.Value;
                var index = value.IndexOf('\"');
                var str = value.Substring(index + 1, value.Length - index - 1);
                str = str.Substring(0, str.IndexOf('\"'));
                return str;
            }
            return String.Empty;
        }

        #region Number

        private static readonly string[] yekan = { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
        private static readonly string[] dahgan = { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };

        private static readonly string[] dahyek =
        {
            "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده",
            "هفده", "هجده", "نوزده"
        };

        private static readonly string[] sadgan =
        {
            "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد",
            "هشتصد", "نهصد"
        };

        private static readonly string[] basex = { "", "هزار", "میلیون", "میلیارد", "تریلیون" };



        private static string GetNumberName(int num)
        {
            string s = "";
            int d12 = num % 100;
            int d3 = num / 100;
            if (d3 != 0)
                s = sadgan[d3] + " و ";
            if ((d12 >= 10) && (d12 <= 19))
            {
                s = s + dahyek[d12 - 10];
            }
            else
            {
                int d2 = d12 / 10;
                if (d2 != 0)
                    s = s + dahgan[d2] + " و ";
                int d1 = d12 % 10;
                if (d1 != 0)
                    s = s + yekan[d1] + " و ";
                s = s.Substring(0, s.Length - 3);
            }
            return s;
        }



        public static string ConvertNumberToChar(this int num)
        {
            return ConvertNumberToCharWithoutSpliter(num);
        }
        private static string ConvertNumberToCharWithoutSpliter(this double num)
        {

            bool isnegative = false;
            if (num < 0)
            {
                isnegative = true;
                num = Math.Abs(num);
            }
            var snum = num.ToString();
            string stotal = "";
            if (snum == "0")
            {
                return yekan[0];
            }
            snum = snum.PadLeft(((snum.Length - 1) / 3 + 1) * 3, '0');
            int L = snum.Length / 3 - 1;
            for (int i = 0; i <= L; i++)
            {
                int b = Int32.Parse(snum.Substring(i * 3, 3));
                if (b != 0)
                    stotal = stotal + GetNumberName(b) + " " + basex[L - i] + " و ";
            }
            stotal = stotal.Substring(0, stotal.Length - 3);
            return isnegative ? " منفی " + stotal : stotal;
        }
        public static string ConvertNumberToChar(this double num)
        {
            var snum = num.ToString().Replace("/", ".");
            if (!snum.Contains('.'))
            {
                return num.ConvertNumberToCharWithoutSpliter();
            }
            var isnegative = false;
            if (num < 0)
            {
                isnegative = true;
                snum = Math.Abs(num).ToString().Replace("/", ".");

            }
            var result = string.Empty;
            var patrs = snum.Split('.');
            result = patrs[0].ToInt().ConvertNumberToChar();
            result += "ممیز ";
            result += patrs[1].ToInt().ConvertNumberToChar();
            result += Math.Pow(10, patrs[1].Length).ConvertNumberToChar().Replace("یک", "").Trim() + "م";
            return isnegative ? (" منفی " + result) : result;
        }

        #endregion

        #region NationId

        public static bool ValidNationalID(string nationalID)
        {
            if (nationalID.Length != 10)
                //throw new Exception("کد ملی باید 10 کاراکتر باشد");
                return false;
            var nid = string.Format("{0:D10}", nationalID);

            var nidPart1 = "";
            var nidPart2 = "";
            var nidPart3 = "";
            NationalIDStep(nid, ref nidPart1, ref nidPart2, ref nidPart3);

            if (nidPart1 == "000")
                return false;

            if (nidPart2 == "000000")
                return false;
            switch (nid)
            {
                case "1111111111":
                case "2222222222":
                case "3333333333":
                case "4444444444":
                case "5555555555":
                case "6666666666":
                case "7777777777":
                case "8888888888":
                case "9999999999":
                    return false;
            }

            var total = 0;
            for (var i = 1; i < 10; i++)
                total += i * nid.Substring(i - 1, 1).ToInt();

            double reminder = total % 11;
            return reminder.ToString().Substring(0, 1) == nidPart3;
        }

        private static void NationalIDStep(string NID, ref string NationalIDPart1, ref string NationalIDPart2, ref string NationalIDPart3)
        {
            var strZero = "";
            if (NID.Length < 10 && NID.Length > 7)
            {
                for (var i = 10; i > NID.Length; i--)
                    strZero += "0";
                NID = strZero + NID;
            }
            NationalIDPart1 = NID.Substring(0, 3);
            NationalIDPart2 = NID.Substring(3, 6);
            NationalIDPart3 = NID.Substring(9, 1);
        }

        #endregion












    }




}
