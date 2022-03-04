using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using MysqlLib;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PublicLibsManagement
{
    public static class PublicLibs
    {
        public static void SetFormatCustom(DateTimePicker dtp) //DateTimePicker 공란처리
        {
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = " ";
            // dtp.Value = dtp.MinDate;
        }

        public static void SetFormatShort(DateTimePicker dtp) // DateTimePicker 공란해제
        {
            dtp.Format = DateTimePickerFormat.Short;
            // dtp.Value = DateTime.Now;
        }

        public static void controlsclear(object[] obj)  //객체 클리어
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is System.Web.UI.WebControls.TextBox)
                {
                    System.Web.UI.WebControls.TextBox txtBox = (System.Web.UI.WebControls.TextBox)obj[i];
                    txtBox.Text = "";
                }
                if (obj[i] is ComboBox)
                {
                    System.Web.UI.WebControls.DropDownList combo = (System.Web.UI.WebControls.DropDownList)obj[i];
                    combo.SelectedIndex = -1;
                }
                if (obj[i] is System.Web.UI.WebControls.CheckBox)
                {
                    System.Web.UI.WebControls.CheckBox ck = (System.Web.UI.WebControls.CheckBox)obj[i];
                    ck.Checked = false;
                }
            }
        }

        public static void DataMove(DataTable dt, object[] obj)  //테이블을 저장소로
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is System.Web.UI.WebControls.TextBox)
                {
                    System.Web.UI.WebControls.TextBox txtBox = (System.Web.UI.WebControls.TextBox)obj[i];
                    txtBox.Text = dt.Rows[0][i].ToString();
                }
                if (obj[i] is DateTimePicker)
                {
                    DateTimePicker dateTime = (DateTimePicker)obj[i];
                    if (dt.Rows[0][i].ToString() != "")
                    {
                        PublicLibs.SetFormatShort(dateTime);
                        dateTime.Value = Convert.ToDateTime(dt.Rows[0][i].ToString());
                    }
                    else
                    {
                        PublicLibs.SetFormatCustom(dateTime);
                        // dateTime.Value = DateTime.Now;
                    }
                }
                if (obj[i] is System.Web.UI.WebControls.DropDownList)
                {
                    System.Web.UI.WebControls.DropDownList combo = (System.Web.UI.WebControls.DropDownList)obj[i];
                    if (dt.Rows[0][i].ToString() != "")
                        for (int k = 0; k < combo.Items.Count; k++)
                        {
                            if (combo.Items[k].Value == dt.Rows[0][i].ToString()) combo.SelectedIndex = k;
                        }
                }
                if (obj[i] is System.Web.UI.WebControls.CheckBox)
                {
                    System.Web.UI.WebControls.CheckBox ck = (System.Web.UI.WebControls.CheckBox)obj[i];
                    ck.Checked = false;
                    if ((bool)dt.Rows[0][i] == true)
                        ck.Checked = true;
                }
            }
        }

        public static string[] DataArrayMove(object[] obj)  //자료를 데이터로 저장
        {
            string[] data = new string[obj.Length];
            string mindate = "";
            string valdate = "";

            for (int i = 0; i < obj.Length; i++)
            {
                data[i] = "";
                if (obj[i] is String)
                {
                    string txt = obj[i].ToString();
                    data[i] = txt;
                }
                if (obj[i] is HiddenField)
                {
                    HiddenField hiddenField = (HiddenField)obj[i];
                    data[i] = hiddenField.Value;
                }
                if (obj[i] is System.Web.UI.WebControls.TextBox)
                {
                    System.Web.UI.WebControls.TextBox txtBox = (System.Web.UI.WebControls.TextBox)obj[i];
                    data[i] = txtBox.Text.Replace(",", "");
                }
                if (obj[i] is System.Web.UI.WebControls.Label)
                {
                    System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)obj[i];
                    data[i] = label.Text.Replace(",", "");
                }
                if (obj[i] is DateTimePicker)
                {
                    DateTimePicker dateTime = (DateTimePicker)obj[i];
                    mindate = dateTime.MinDate.ToShortDateString();
                    valdate = dateTime.Value.ToShortDateString();
                    if (mindate != valdate)
                    {
                        data[i] = dateTime.Value.ToShortDateString();
                    }
                }
                if (obj[i] is System.Web.UI.WebControls.DropDownList)
                {
                    System.Web.UI.WebControls.DropDownList combo = (System.Web.UI.WebControls.DropDownList)obj[i];

                    if (combo.Visible == false)
                    {
                        data[i] = "";
                    }
                    else
                    {
                        if (combo.SelectedIndex != -1)
                            data[i] = combo.SelectedValue.ToString();
                    }
                }
                if (obj[i] is System.Web.UI.WebControls.CheckBox)
                {
                    System.Web.UI.WebControls.CheckBox ck = (System.Web.UI.WebControls.CheckBox)obj[i];
                    data[i] = "0";
                    if (ck.Checked == true)
                        data[i] = "1";
                }
            }
            return data;
        }

        public static string SetCode_Tran(string tableName, string colName, string code_prefix, DB_mysql km)
        {
            string sql = $"select ifnull(max({colName}),0) from {tableName} where {colName} like '" + code_prefix + DateTime.Now.ToString("yyyy") + "%';";
            DataTable dt = km.tran_GetDTa(sql);
            string code = (Convert.ToInt32(dt.Rows[0][0].ToString().Replace(code_prefix, "")) + 1).ToString();
            string temp = DateTime.Now.ToString("yyyy") + "00000";
            code = code_prefix + temp.Substring(0, (temp.Length - code.Length)) + code;
            return code;
        }

        public static string SetCode(string tableName, string colName, string code_prefix, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = $"select ifnull(max({colName}),0) from {tableName} where {colName} like '" + code_prefix + DateTime.Now.ToString("yyyy") + "%';";
            DataTable dt = km.GetDTa(sql);
            string code = (Convert.ToInt32(dt.Rows[0][0].ToString().Replace(code_prefix, "")) + 1).ToString();
            string temp = DateTime.Now.ToString("yyyy") + "00000";
            code = code_prefix + temp.Substring(0, (temp.Length - code.Length)) + code;
            return code;
        }
    }

    public static class PROCEDURE
    {
        public static DataTable SELECT(string procedureName, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = $"call {procedureName}();";

            return km.GetDTa(sql);
        }

        public static DataTable SELECT(string procedureName, string str, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = $"call {procedureName}('{str}');";

            return km.GetDTa(sql);
        }

        public static DataTable SELECT(string procedureName, object[] obj, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = "call " + procedureName + " (";

            string[] data = PublicLibs.DataArrayMove(obj);

            for (int i = 0; i < data.Length; i++)
            {
                sql += "'" + data[i] + "',";
            }

            sql = sql.Substring(0, sql.Length - 1);

            sql += ");";
            return km.GetDTa(sql);
        }

        public static DataTable SELECT_TRAN(string procedureName, DB_mysql km)
        {
            string sql = $"call {procedureName}();";

            return km.tran_GetDTa(sql);
        }

        public static DataTable SELECT_TRAN(string procedureName, string str, DB_mysql km)
        {
            string sql = $"call {procedureName}('{str}');";

            return km.tran_GetDTa(sql);
        }

        public static DataTable SELECT_TRAN(string procedureName, object[] obj, DB_mysql km)
        {
            string sql = "call " + procedureName + " (";

            string[] data = PublicLibs.DataArrayMove(obj);

            for (int i = 0; i < data.Length; i++)
            {
                sql += "'" + data[i] + "',";
            }

            sql = sql.Substring(0, sql.Length - 1);

            sql += ");";
            return km.tran_GetDTa(sql);
        }

        public static int CUD(string procedureName, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = $"call {procedureName}();";

            return km.ExSQL_Ret(sql);
        }

        public static int CUD(string procedureName, string str, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = $"call {procedureName}('{str}');";

            return km.ExSQL_Ret(sql);
        }
        public static int CUD(string procedureName, object[] obj, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string sql = "call " + procedureName + " (";

            if (obj != null)
            {
                string[] data = PublicLibs.DataArrayMove(obj);

                for (int i = 0; i < data.Length; i++)
                {
                    sql += "'" + data[i] + "',";
                }

                sql = sql.Substring(0, sql.Length - 1);
            }

            sql += ");";
            return km.ExSQL_Ret(sql);
        }

        public static int CUD_TRAN(string procedureName, string str, DB_mysql km)
        {
            string sql = $"call {procedureName}('{str}');";

            return km.tran_ExSQL_Ret(sql);
        }

        public static int CUD_TRAN(string procedureName, object[] obj, DB_mysql km)
        {
            string sql = "call " + procedureName + " (";

            if (obj != null)
            {
                string[] data = PublicLibs.DataArrayMove(obj);

                for (int i = 0; i < data.Length; i++)
                {
                    sql += "'" + data[i] + "',";
                }

                sql = sql.Substring(0, sql.Length - 1);
            }

            sql += ");";
            
            return km.tran_ExSQL_Ret(sql);
        }

        public static string CUD_ReturnID(string procedureName, object[] obj, DB_mysql km)
        {
            string sql = "call " + procedureName + " (";

            if (obj != null)
            {
                string[] data = PublicLibs.DataArrayMove(obj);

                for (int i = 0; i < data.Length; i++)
                {
                    sql += "'" + data[i] + "',";
                }

                sql = sql.Substring(0, sql.Length - 1);
            }

            sql += ");";

            if (km.tran_ExSQL_Ret(sql) > 0)
            {
                sql = "SELECT LAST_INSERT_ID();";

                try
                {
                    return km.tran_GetDTa(sql).Rows[0][0].ToString();
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

        public static void ERROR(string msg, DB_mysql km)
        {
            if (km == null) km = new DB_mysql();

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            msg = msg.Replace("'", "");
            object[] objs = { path, msg, time };

            CUD("SP_errorList_Add", objs, km);
        }

        public static void ERROR_ROLLBACK(string msg, DB_mysql km)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            km.Rollback();
            string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            msg = msg.Replace("'", "");
            object[] objs = { path, msg, time };

            km = new DB_mysql();
            CUD("SP_errorList_Add", objs, km);
        }
    }
}
