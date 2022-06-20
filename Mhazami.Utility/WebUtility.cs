using System.Net;
using System.Text.RegularExpressions;

namespace Mhazami.Utility
{
    public class WebUtility
    {
        public static string GetServerIP()
        {
            try
            {
                var Ip = String.Empty;
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                if (ipHostInfo.AddressList.Length == 0) return Ip;
                foreach (var ipAddress in ipHostInfo.AddressList)
                {
                    var regex = new Regex(@"^(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d)$");
                    if (!regex.IsMatch(ipAddress.ToString())) continue;
                    Ip = ipAddress.ToString();
                    break;
                }
                return Ip;
            }
            catch 
            {
                return String.Empty;
            }

        }
       
      
        //public static bool IsClientFromMobile()
        //{
        //    var userAgent = HttpContext.Current.Request.UserAgent;
        //    if (userAgent != null)
        //    {
        //        return userAgent.Contains("ipod") || userAgent.Contains("iphone") || userAgent.Contains("android") ||
        //               userAgent.Contains("opera mobi") ||
        //               (userAgent.Contains("windows phone os") && userAgent.Contains("iemobile")) ||
        //               userAgent.Contains("fennec");
        //    }
        //    return false;
        //}
        //public static string GetClientBrowser()
        //{
        //    HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
        //    return browser.Browser;
        //}
        //public static string GetClientOs()
        //{
        //    var userAgent = HttpContext.Current.Request.UserAgent;
        //    if (userAgent == null) return string.Empty;
        //    if (userAgent.IndexOf("Windows NT 10.0") > 0)
        //        return "Windows 10";
        //    if (userAgent.IndexOf("Windows NT 6.3") > 0)
        //        return "Windows 8.1";
        //    if (userAgent.IndexOf("Windows NT 6.2") > 0)
        //        return "Windows 8";
        //    if (userAgent.IndexOf("Windows NT 6.1") > 0)
        //        return "Windows 7";
        //    if (userAgent.IndexOf("Windows NT 6.0") > 0)
        //        return "Windows Vista";
        //    if (userAgent.IndexOf("Windows NT 5.2") > 0)
        //        return "Windows Server 2003; Windows XP x64 Edition";
        //    if (userAgent.IndexOf("Windows NT 5.1") > 0)
        //        return "Windows XP";
        //    if (userAgent.IndexOf("Windows NT 5.01") > 0)
        //        return "Windows 2000, Service Pack 1 (SP1)";
        //    if (userAgent.IndexOf("Windows NT 5.0") > 0)
        //        return "Windows 2000";
        //    if (userAgent.IndexOf("Windows NT 4.0") > 0)
        //        return "Microsoft Windows NT 4.0";
        //    if (userAgent.IndexOf("Win 9x 4.90") > 0)
        //        return "Windows Millennium Edition (Windows Me)";
        //    if (userAgent.IndexOf("Windows 98") > 0)
        //        return "Windows 98";
        //    if (userAgent.IndexOf("Windows 95") > 0)
        //        return "Windows 95";
        //    if (userAgent.IndexOf("Windows CE") > 0)
        //        return "Windows CE";
        //    if (userAgent.Contains("iphone"))
        //        return "android";
        //    if (userAgent.Contains("iphone"))
        //        return "iphone";
        //    if (userAgent.Contains("ipod"))
        //        return "ipod";
        //    return "Others";

        //}
        //public static string GetClientIp()
        //{
        //    try
        //    {
        //        string VisitorsIPAddr = string.Empty;
        //        if (System.Web.HttpContext.Current == null) return String.Empty;
        //        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //        {
        //            VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //        }
        //        else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
        //        {
        //            VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
        //        }
        //        if (VisitorsIPAddr == "::1")
        //            VisitorsIPAddr = "127.0.0.1";
        //        return VisitorsIPAddr;
        //    }
        //    catch
        //    {
        //        return String.Empty;
        //    }
        //}
        //public static bool IsLocal(params string[] ipContains)
        //{

        //    var islocalhost = System.Web.HttpContext.Current != null &&
        //        System.Web.HttpContext.Current.Request.Url.Authority.ToLower()
        //                   .Contains("localhost");
        //    if (ipContains == null || !ipContains.Any()) return islocalhost;
        //    var isLocalIp =
        //        ipContains.Any(
        //            ipContain =>
        //                System.Web.HttpContext.Current != null &&
        //                System.Web.HttpContext.Current.Request.Url.Authority.ToLower().Contains(ipContain.ToLower()));
        //    return islocalhost || isLocalIp;

        //}
        public static string GetIP()
        {
            string localIP = "?";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

    }
}
