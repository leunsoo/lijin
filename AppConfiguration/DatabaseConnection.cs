using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AppConfiguration
{
    public class DatabaseConnection
    {
        private static DatabaseConnection instance;
        public string ConnectionString { get; private set; }
        public MySqlConnection Connection { get; private set; } //IDbConnection Connection { get; private set; }

        protected DatabaseConnection()
        {
            MakeConnection();
        }

        private void MakeConnection()
        {
            ConfigurationMgr mgr = ConfigurationMgr.Instance();
            ConnectionString = mgr.ConnectionString;
           // if (mgr.DatabaseType == "MsSql") MakeSqlConnection();
           // else if (mgr.DatabaseType == "MySql") 
         //   MakeMySqlConnection();
           // else return;
        }

        private void MakeMySqlConnection()
        {
            throw new NotImplementedException();
        }
        //mssql 인 경우
        //private void MakeSqlConnection()
        //{
        //    if (Connection != null)
        //    {
        //        //기존 커넥션이 있으므로 Open() 상태를 체크한다.
        //        if (Connection.State != ConnectionState.Open)
        //            Connection.Open();
        //    }
        //    else
        //    {
        //        Connection = new SqlConnection(ConnectionString);
        //        Connection.Open();
        //    }
        //}

        public static DatabaseConnection Instance()
        {
            if (instance == null)
                instance = new DatabaseConnection();
            return instance;
        }
        public MySqlTransaction BeginTransaction()        //IDbTransaction BeginTransaction()
        {
            if (Connection != null)
                return Connection.BeginTransaction();
            else
                return null;
        }

    }
}
