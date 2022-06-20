using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mhazami.Utility
{
    public static class StringUtils
    {
        public static string Trancate(this string txt, int lentgth, bool addContinueDot = true)
        {
            if (!string.IsNullOrEmpty(txt) && txt.Length >= lentgth)
                txt = txt.Substring(0, lentgth - 3) + (addContinueDot ? "..." : "");
            return txt;
        }
        public static string generateRandomString(int size)
        {
            var random = new Random(DateTime.Now.Millisecond);
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
            var builder= new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
          
            return builder.ToUpper();
        }
        public static string RemoveHtml(this string strHtml)
        {
            var input = Regex.Replace(strHtml, "<style>(.|\n)*?</style>", string.Empty);
            input = Regex.Replace(input, @"<xml>(.|\n)*?</xml>", string.Empty);
            return Regex.Replace(input, @"<(.|\n)*?>", string.Empty).Replace("&nbsp;", " ");

        }
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    CopyTo(msi, gs);
                return mso.ToArray();
            }
        }
        private static void CopyTo(Stream src, Stream dest)
        {
          
            byte[] bytes = new byte[16 * 1024];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
                dest.Write(bytes, 0, cnt);
        }
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    CopyTo(gs, mso);
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
        public static string ConvertNumLa2Fa(this string num)
        {
            if (string.IsNullOrEmpty(num)) return "";
            var result = string.Empty;
            foreach (var c in num)
            {
                switch (c)
                {
                    case '0':
                        result += "٠";
                        break;
                    case '1':
                        result += "١";
                        break;
                    case '2':
                        result += "٢";
                        break;
                    case '3':
                        result += "٣";
                        break;
                    case '4':
                        result += "۴";
                        break;
                    case '5':
                        result += "۵";
                        break;
                    case '6':
                        result += "۶";
                        break;
                    case '7':
                        result += "٧";
                        break;
                    case '8':
                        result += "٨";
                        break;
                    case '9':
                        result += "٩";
                        break;
                    default:
                        result += c;
                        break;
                }
            }
            return result;
        }
        public static string ConvertNumFa2La(this string num)
        {
            if (string.IsNullOrEmpty(num)) return "";
            var result = string.Empty;
            foreach (var c in num)
            {
                switch (c)
                {
                    case '٠':
                        result += "0";
                        break;
                    case '١':
                        result += "1";
                        break;
                    case '٢':
                        result += "2";
                        break;
                    case '٣':
                        result += "3";
                        break;
                    case '۴':
                        result += "4";
                        break;
                    case '۵':
                        result += "5";
                        break;
                    case '۶':
                        result += "6";
                        break;
                    case '٧':
                        result += "7";
                        break;
                    case '٨':
                        result += "8";
                        break;
                    case '٩':
                        result += "9";
                        break;
                    default:
                        result += c;
                        break;
                }
            }
            return result;
        }
        public static string InversePersianDate(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var data = str.Trim().Split('/');
            return string.Format("{0}/{1}/{2}", data[2], data[1], data[0]);
        }
        public static string InverseDraftNUmber(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.IndexOf('/') != -1)
            {
                var data = str.Trim().Split('/');
                return string.Format("{0}/{1}", data[1], data[0]);
            }
            return str.Trim();
        }
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return password;
            return System.Convert.ToBase64String(
               new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public static string Encrypt(string data)
        {
            const string key = "rdn(!)";
            var RijndaelCipher = new RijndaelManaged();
            var PlainText = Encoding.Unicode.GetBytes(data);
            var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var SecretKey = new PasswordDeriveBytes(key, Salt);
            var Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            cryptoStream.FlushFinalBlock();
            var CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return System.Convert.ToBase64String(CipherBytes);
        }
        public static string Decrypt(string TextToBeDecrypted)
        {
            if (TextToBeDecrypted.IndexOf(" ") >= 0)
            {
                TextToBeDecrypted = TextToBeDecrypted.Replace(" ", "+");
            }
            var RijndaelCipher = new RijndaelManaged();
            const string key = "rdn(!)";

            try
            {
                var EncryptedData = System.Convert.FromBase64String(TextToBeDecrypted);
                var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
                var SecretKey = new PasswordDeriveBytes(key, Salt);
                var Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
                var memoryStream = new MemoryStream(EncryptedData);
                var cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                var PlainText = new byte[EncryptedData.Length];
                var DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch
            {
                return TextToBeDecrypted;
            }
        }


        public static string FixUrlCatchall(this string txt)
        {
            txt = txt.Replace("/", " ");
            txt = txt.Replace("&", " ");
            txt = txt.Replace(":", " ");
            txt = txt.Replace(".", " ");
            txt = txt.Replace("\"", " ");
            txt = txt.Replace("+", " ");
            if (txt.StartsWith("/"))
                txt = txt.Substring(1, txt.Length - 1);
            return txt;
        }


        public static string SetDirection(this string Txt, string direction = "", string aligment = "")
        {
            if (direction == "") direction = "rtl";
            if (aligment == "") aligment = "right";
            return string.Format("<div style=\"text-align:{1};direction:{2};\" >{0}</div>", Txt, aligment, direction);
        }

        public static string ConvertFaNumToEn(string num)
        {
            var result = string.Empty;
            foreach (var c in num)
            {
                switch (c)
                {
                    case '٠':
                        result += "1";
                        break;
                    case '١':
                        result += "1";
                        break;
                    case '٢':
                        result += "2";
                        break;
                    case '٣':
                        result += "3";
                        break;
                    case '۴':
                        result += "4";
                        break;
                    case '۵':
                        result += "5";
                        break;
                    case '۶':
                        result += "6";
                        break;
                    case '٧':
                        result += "7";
                        break;
                    case '٨':
                        result += "8";
                        break;
                    case '٩':
                        result += "9";
                        break;
                    default:
                        result += c;
                        break;
                }
            }
            return result;
        }

        public static string FixPersian(this string str)
        {
            str = str.Replace('ك', 'ک');
            str = str.Replace('ي', 'ی');
            return str;
        }
         

        public static string Compress(this string instance)
        {
            if (string.IsNullOrEmpty(instance)) return string.Empty;
            var bytes = Encoding.Unicode.GetBytes(instance);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return System.Convert.ToBase64String(mso.ToArray());
            }
        }

       
        public static string Decompress(this string instance)
        {
            if (string.IsNullOrEmpty(instance)) return string.Empty;
            var bytes = System.Convert.FromBase64String(instance);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        public static bool IsCaseSensitiveEqual(this string instance, string comparing)
        {
            return string.CompareOrdinal(instance, comparing) == 0;
        }

        public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
        {
            return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }


    }
}
