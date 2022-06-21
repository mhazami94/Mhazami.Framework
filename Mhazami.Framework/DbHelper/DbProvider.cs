using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Mhazami.Utility;

namespace Mhazami.Framework.DbHelper
{
    public static class DbProvider
    {
        public static DbDataAdapter GetDbAdapter(DBType DBType)
        {
            switch (DBType)
            {
                case DBType.None:
                    return null;
                case DBType.Sql:
                    return new System.Data.SqlClient.SqlDataAdapter();
                case DBType.Oracle:
                    //todo : must be refereced by OracleClient
                    return null;
                case DBType.Access:
                    //todo : must be refereced by new System.Data.OleDb.OleDbDataAdapter()
                    return null;
                default:
                    return null;
            }
        }

        public static DbCommand GetDbCommand(DBType DBType)
        {
            switch (DBType)
            {
                case DBType.None:
                    return null;
                case DBType.Sql:
                    return new System.Data.SqlClient.SqlCommand();
                case DBType.Oracle:
                    //todo : must be refereced by OracleClient
                    return null;
                case DBType.Access:
                    //todo : must be refereced by System.Data.OleDb.OleDbCommand()
                    return null ;
                default:
                    return null;
            }
        }
        public static void GetDbCommand(IConnectionHandler connectionHandler, IDbCommand command)
        {

            command.Connection = connectionHandler.Connection;
            if (connectionHandler.Transaction != null)
                command.Transaction = connectionHandler.Transaction;
            command.CommandText = command.CommandText.FixPersian();

        }
        public static DbCommand GetDbCommand(IConnectionHandler connectionHandler)
        {
            switch (connectionHandler.DBType)
            {
                case DBType.None:
                    return null;
                case DBType.Sql:
                    var sqlCommand = new System.Data.SqlClient.SqlCommand();
                    GetDbCommand(connectionHandler, sqlCommand);
                    return sqlCommand;
                case DBType.Oracle:
                    //todo : must be refereced by OracleClient
                    return null;
                case DBType.Access:
                    //todo : must be refereced by System.Data.OleDb.OleDbCommand()
                    return null;
                default:
                    return null;
            }
        }
        public static DbCommand GetDbCommand(IConnectionHandler connectionHandler, string commandText)
        {
            switch (connectionHandler.DBType)
            {
                case DBType.None:
                    return null;
                case DBType.Sql:
                    var sqlCommand = new System.Data.SqlClient.SqlCommand() { CommandText = commandText };
                    GetDbCommand(connectionHandler, sqlCommand);
                    return sqlCommand;
                case DBType.Oracle:
                    //todo : must be refereced by OracleClient
                    return null;
                case DBType.Access:
                    //todo : must be refereced by System.Data.OleDb.OleDbCommand() 
                    return null;
                default:
                    return null;
            }
        }
    }
}
