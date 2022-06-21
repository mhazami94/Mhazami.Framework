

namespace Mhazami.Framework
{
    public class KnownException : Exception
    {
        #region Constructor

        public KnownException()
            : base()
        {

        }

        public KnownException(string message)
            : base(message)
        {

        }

        public KnownException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        #endregion

        #region Property

        public string TableName { get; set; }
        public string Info { get; set; }
        public string Title { get; set; }

        #endregion
    }
}
