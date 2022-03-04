using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Sockets;

namespace MysqlLib
{
    public class DB_mysql
    {

        public static MySqlConnection DB_Conn;// = new MySqlConnection(Config.conn);
        public static MySqlCommand DBcmd;// = new MySqlCommand(sql, DB_Conn);
        public static MySqlDataAdapter adapter;// = new MySqlDataAdapter(DBcmd); 
        public static MySqlTransaction tran;
        static public string m_strLastError = "";

        public bool dbClose()
        {
            DB_Conn.Close();
            return true;
        }

        public MySqlConnection dbOpen()
        {
            try
            {
                if (DB_Conn != null) DB_Conn.Close();
                if (Config.DBSERVER == null || Config.DBSERVER == "")
                {
                    Config.getConfig(); 
                    //string CURL = "C:/inetpub/pConfig/iljin/";
                    //string URL1 = CURL + Config.cfgFile;

                    //if (File.Exists(URL1))
                    //{
                    //    Config.URL = URL1;
                    //    Config.getConfig();
                    //}
                }
                DB_Conn = new MySqlConnection(Config.conn);
                DB_Conn.Open();
            }
            catch (Exception ex)
            {

            }

            return DB_Conn;
        }

        public bool Open()
        {
            string url = Config.URL; // + "\\" + PConfig.cfgFile;
            if (File.Exists(url))
            {
                //string connection = "Server=localhost;port=33306;Database=ibuild;Uid=root;Pwd=14687204ms!;character set=utf8;allow zero datetime=yes";
                DB_Conn = new MySqlConnection(Config.conn);
                DB_Conn.Open();
                return true;
            }
            else
            {
                // System.Windows.Forms.MessageBox.Show("DB 연결 설정을 먼저 실행하십시요");2
                return false;
            }

        }

        public DataSet GetDS(string sql)
        {
            DataSet ds = new DataSet();
            MySqlCommand DBcmd = new MySqlCommand(sql, DB_Conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(DBcmd);
            adapter.Fill(ds);
            return ds;
        }

        public static DataTable GetDT(string sql)
        {
            DataTable dt = new DataTable();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            adapter = new MySqlDataAdapter(DBcmd);
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetDTa(string sql)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt = new DataTable();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            adapter = new MySqlDataAdapter(DBcmd);
            adapter.Fill(dt);
            dbClose();
            return dt;
        }
        public DataTable tran_GetDTa(string sql)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt = new DataTable();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            adapter = new MySqlDataAdapter(DBcmd);
            adapter.Fill(dt);           
            return dt;
        }

        // SQL 문을 실행합니다.
        public int ExSQL_Ret(string sql)   //return 할때
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            int result = DBcmd.ExecuteNonQuery();
            dbClose();
            return result;
        }
        public int tran_ExSQL_Ret(string sql)   //transaction일때
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            int result = DBcmd.ExecuteNonQuery();            
            return result;
        }

        public string tran_ExSQL_RetId(string sql)   //transaction일때
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            
            if(DBcmd.ExecuteNonQuery() > 0)
            {
                sql = "SELECT LAST_INSERT_ID();";

                try
                {
                    return tran_GetDTa(sql).Rows[0][0].ToString();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool Fill(string sql, DataTable tbl)
        {
            // try
            {
                //dbOpen();
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, Config.conn);
                tbl.Clear();
                adp.Fill(tbl);
                m_strLastError = "";
                return true;
            }
            //catch (Exception exm)
            //{
            //    //  m_strLastError = "[CtrlSqlCe.Fill]" + exm.Message;
            //    //throw new Exception(m_strLastError);
            //    return false;
            //}
        }

        public static DataTable UserLogin(string id, string pw)
        {
            //string sql = "select b.comCode,a.LoginId,a.UserName,b.comName from tb_user a " +
            //    "inner join nissi_master.dbo.company b on a.ComId = b.comId " +
            //    "where b.comName='" + comNM + "' and  a.LoginId = '" + id + "' and a.Password = '" + pw + "'";
            string sql = "select userCode,id,empName,authorityCode from tb_emp " +
                "where id = '" + id + "' and  password = PASSWORD('" + pw + "') AND (isUse = '1' OR userCode = '1');";

            DataTable dt = new DataTable();
            DBcmd = new MySqlCommand(sql, DB_Conn);
            adapter = new MySqlDataAdapter(DBcmd);
            adapter.Fill(dt);
            return dt;
        }

        public MySqlDataReader ExecuteReader(string sql)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DBcmd = new MySqlCommand(sql, DB_Conn);          
            return DBcmd.ExecuteReader();
        }

        public void GetCbDT_FromSql(string sql,DropDownList cb, string first_item_name)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt;
            ListItem item;
            cb.Items.Clear();
            dt = GetDT(sql);
            item = new ListItem(first_item_name,"");
            cb.Items.Add(item);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ListItem
                {
                    Text = dt.Rows[i][1].ToString(),
                    Value = dt.Rows[i][0].ToString()
                };
                cb.Items.Add(item);
            }

            cb.SelectedIndex = 0;
            dbClose();
        }

        public void GetCbDT_FromSql(string sql, DropDownList cb)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt;
            ListItem item;
            cb.Items.Clear();
            dt = GetDT(sql);

            if (dt.Rows.Count == 0)
            {
                cb.Items.Add(new ListItem("결과없음", ""));
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item = new ListItem
                    {
                        Text = dt.Rows[i][1].ToString(),
                        Value = dt.Rows[i][0].ToString()
                    };
                    cb.Items.Add(item);
                }
            }

            dbClose();
        }

        public void GetCbDT(string code, DropDownList cb, string first_item_name = "")
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt = new DataTable();
            ListItem item = new ListItem();
            cb.Items.Clear();
            string sql = string.Format("call Cb_GetParent ('{0}')", code);
            dt = GetDT(sql);
            //  cb.DataSource = dt;
            //  cb.DataBind();
            item = new ListItem();
            item.Text = first_item_name;
            item.Value = "";
            cb.Items.Add(item);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ListItem();
                item.Text = dt.Rows[i][1].ToString();
                item.Value = dt.Rows[i][0].ToString();
                cb.Items.Add(item);
            }
            cb.SelectedIndex = -1;
            dbClose();
        }

        public void GetCbDT(string code, DropDownList cb)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DataTable dt = new DataTable();
            ListItem item = new ListItem();
            cb.Items.Clear();
            string sql = string.Format("call Cb_GetParent ('{0}')", code);
            dt = GetDT(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item = new ListItem();
                    item.Text = dt.Rows[i][1].ToString();
                    item.Value = dt.Rows[i][0].ToString();
                    cb.Items.Add(item);
                }
                cb.SelectedIndex = 0;
            }
            else
            {
                cb.Items.Add(new ListItem("없음",""));
                cb.SelectedIndex = 0;
            }
            dbClose();
        }

        public DataTable GetCbDT(string code, string first_item_name = "")
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();          
      
            string sql = string.Format("call Cb_GetParent ('{0}')", code);
            DataTable dt = GetDT(sql);
            DataRow dr = dt.NewRow();
            dr["code_name"] = first_item_name;
            dt.Rows.InsertAt(dr, 0);
            dbClose();
            return dt;
        }
        public string ExecuteSQL(string sql)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            DBcmd = new MySqlCommand(sql,DB_Conn);
            DBcmd.ExecuteNonQuery();
            dbClose();
            return "OK";
        }

        public int Table_number(string sql)
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            int Ret = 1;
            MySqlDataReader hr;
            hr = ExecuteReader(sql);
            if (hr.Read())
            {
                if (hr[0].ToString() != "")
                    Ret = Int32.Parse(hr[0].ToString());
            }
            hr.Close();
            dbClose();
            return Ret;
        }

        public string Path_search(string top)
        {
            dbOpen();
            string Ret = "";
            string sql = "select path from tb_menu where MenuId='" + top + "' and UpperMenuId is null";
            MySqlDataReader hr;
            hr = ExecuteReader(sql);
            if (hr.Read())
            {
                if (hr[0].ToString() != "")
                    Ret = hr[0].ToString();
            }
            hr.Close();
            dbClose();
            return Ret;
        }


        public int DubleCheck(string sql)
        {
            dbOpen(); 
            int Ret = 0;
          MySqlDataReader hr;
            hr = ExecuteReader(sql);
            if (hr.Read())
            {
                if (hr[0].ToString() != "")
                    Ret = Int32.Parse(hr[0].ToString());
            }
            hr.Close();
            dbClose();
            return Ret;
        }

        public string Client_IP()
        {
            string clientip = "";
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        clientip = host.AddressList[i].ToString();
                    }
                }
            return clientip;
       }

        public void BeginTran()
        {
            if (DB_Conn == null || DB_Conn.State != ConnectionState.Open) dbOpen();
            tran = DB_Conn.BeginTransaction();
        }

        public void Commit()
        {
            tran.Commit();
            DB_Conn.Close();
            tran = null;
        }

        public void Rollback()
        {
            tran.Rollback();
            DB_Conn.Close();
            tran = null;
        }
    }
}