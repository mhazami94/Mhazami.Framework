using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Mhazami.Framework.DbHelper
{
    public interface IConnectionHandler
    {
        bool HandyLockConnection { get; set; }
        DBType DBType { get; set; }
        DbConnection Connection { get; set; }
        DbTransaction Transaction { get; set; }
        void StartTransaction();
        void StartTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollBack();
        void OpenConnection();
        void OpenAndLockConnection();
        void CloseConnection();
        void CloseLockedConnection();
        bool ExternalHandler { get; set; }
      
    }

    public abstract class BaseConnectionHandler : IConnectionHandler
    {
        protected DbConnection connection;

        protected BaseConnectionHandler()
        {

        }

        protected BaseConnectionHandler(IConnectionHandler connectionHandler)
        {
            this.connection = connectionHandler.Connection;
            this.Transaction = connectionHandler.Transaction;
            this.DBType = connectionHandler.DBType;
            this.ExternalHandler = true;
        }

        protected BaseConnectionHandler(IConnectionHandler connectionHandler, bool externalConnection = true)
        {
            this.connection = connectionHandler.Connection;
            this.Transaction = connectionHandler.Transaction;
            this.DBType = connectionHandler.DBType;
            this.ExternalHandler = externalConnection;

        }

        public bool ExternalHandler { get; set; }


        public bool HandyLockConnection { get; set; }

        public DBType DBType { get; set; }

        public abstract DbConnection Connection { get; set; }

        public DbTransaction Transaction
        {
            get;
            set;
        }

        public void StartTransaction()
        {
            if (this.Transaction != null) return;
            this.Transaction = this.Connection.BeginTransaction();
            this.HandyLockConnection = true;
        }

        public void StartTransaction(IsolationLevel isolationLevel)
        {
            if (this.Transaction != null) return;
            this.Transaction = this.Connection.BeginTransaction(isolationLevel);
            this.HandyLockConnection = true;
        }

        public void CommitTransaction()
        {
            if (this.Transaction != null && this.Transaction.Connection != null && !this.ExternalHandler)
                this.Transaction.Commit();
        }
     
        public void OpenAndLockConnection()
        {
            this.OpenConnection();
            this.HandyLockConnection = true;
        }
       
        public void RollBack()
        {
            if (this.Transaction != null && this.Transaction.Connection != null && !this.ExternalHandler)
                this.Transaction.Rollback();
        }

        public void OpenConnection()
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        this.connection.Open();
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        

        public void CloseConnection()
        {
            if (this.connection.State == ConnectionState.Open && !this.ExternalHandler && !this.HandyLockConnection)
                this.connection.Close();

        }

        public void CloseLockedConnection()
        {
            this.HandyLockConnection = false;
            this.CloseConnection();
        }
    }

    public enum DBType
    {
        None,
        Sql,
        Oracle,
        Access
    }
}
