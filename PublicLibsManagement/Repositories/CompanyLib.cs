using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using AppConfiguration;
using MysqlLib;

namespace PublicLibsManagement
{
    public class CompanyLib: iCompanyLib
    {
        private static string procedureName;
        public IDbConnection con { get; set; }
        public SqlConnection conn { get; set; }

        private DatabaseConnection Instance;
        IDbTransaction tran = null;

        public CompanyLib(IDbConnection con = null)
        {
            Instance = DatabaseConnection.Instance();
            if (con == null)
                this.con = Instance.Connection;
            else
                this.con = con;
            if (conn == null)
                conn = new SqlConnection(Instance.ConnectionString);
        }


        public DataTable Company_GetByCode(string ComCode, IDbTransaction transaction = null)
        {
            procedureName =string.Format("exec SP_Company_GetByCode {0}", ComCode);           
            conn.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand(procedureName, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }

        public int Update(CompanyModel model, IDbTransaction transaction = null)   
        {
            procedureName = "SP_Company_Update";
            var parameters = new DynamicParameters();
            parameters.Add("@comCode", value: model.comCode, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@comName", value: model.comName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_conporate_no", value: model.com_conporate_no, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_res_no", value: model.com_res_no, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_boss_name", value: model.com_boss_name, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ComBossbirthday", value: model.ComBossbirthday, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_type", value: model.com_type, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_item", value: model.com_item, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ComstartDate", value: model.ComstartDate, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_phone", value: model.com_phone, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_phone1", value: model.com_phone1, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_fax", value: model.com_fax, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_post", value: model.com_post, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_addr", value: model.com_addr, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_mail", value: model.com_mail, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@com_memo", value: model.com_memo, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@smile_id", value: model.smile_id, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@smile_pwd", value: model.smile_pwd, dbType: DbType.String, direction: ParameterDirection.Input);
            try
            {
                return con.Execute(procedureName, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll(string ComCode, IDbTransaction transaction = null)
        {
            procedureName = string.Format("exec SP_Company_GetByCode {0}", ComCode);
            return DBOPEN.GetDT(procedureName);
        }
    }
}
