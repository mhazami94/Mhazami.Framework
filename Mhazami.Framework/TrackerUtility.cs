
namespace Mhazami.Framework
{
    public class TrackerUtility
    {


        public static string TrackerUserName
        {

            get { return trakerUserName; }
            set { trakerUserName = value; }

        }
        private static string trakerUserName
        {
            get
            {
                //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session["TrakerUserName"] != null)
                //    return (string)System.Web.HttpContext.Current.Session["TrakerUserName"];
                return "System";
            }

            set {  }
        }
    }
}
