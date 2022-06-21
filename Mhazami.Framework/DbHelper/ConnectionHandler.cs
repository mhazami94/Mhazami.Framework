using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace Mhazami.Framework.DbHelper
{
    public class ConnectionHandler : BaseConnectionHandler
    {
        public ConnectionHandler(string connectionString)
            : this(connectionString, DBType.Sql)
        {

        }
        public ConnectionHandler(string connectionString, DBType dbType)
        {
            this.ConnectionString = connectionString;
            this.DBType = dbType;
        }

        public ConnectionHandler()
        {
            base.DBType = DBType.Sql;
        }
       
        public ConnectionHandler(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            base.DBType = DBType.Sql;
        }

        public ConnectionHandler(IConnectionHandler connectionHandler, bool externalConnection = true)
            : base(connectionHandler, externalConnection)
        {
            base.DBType = DBType.Sql;
        }
        
        protected string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(this.connectionString) && ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                    return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                return this.connectionString;
            }
            set { this.connectionString = value; }
        }

        private string connectionString;

        public override DbConnection Connection
        {
            get
            {
                if (this.connection == null)
                {
                    this.connection = new SqlConnection();
                    connection.ConnectionString = this.ConnectionString;
                }
                return connection;
            }
            set { this.connection = value; }
        }
    }
}
