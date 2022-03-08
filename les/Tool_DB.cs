using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MysqlLib;
using System;

namespace les
{
    public static class Tool_DB
    {
        /// <summary>
        /// 채번코드 전용 함수 :: 데이터 저장 시 다시 한번 코드 확인용
        /// </summary>
        /// <returns> 중복되지 않는 새로운 코드를 가져옵니다. </returns>
        public static string SetCode_Tran(string tableName, string colName, string code_prefix, DB_mysql km)
        {
            string sql = $"select ifnull(max({colName}),0) from {tableName} where {colName} like '" + code_prefix + DateTime.Now.ToString("yyyy") + "%';";
            DataTable dt = km.tran_GetDTa(sql);
            string code = (Convert.ToInt32(dt.Rows[0][0].ToString().Replace(code_prefix, "")) + 1).ToString();
            string temp = DateTime.Now.ToString("yyyy") + "00000";
            code = code_prefix + temp.Substring(0, (temp.Length - code.Length)) + code;
            return code;
        }

        /// <summary>
        /// 채번코드 전용 함수
        /// </summary>
        /// <returns> 중복되지 않는 새로운 코드를 가져옵니다. </returns>
        public static string SetCode(string tableName, string colName, string code_prefix, DB_mysql km)
        {
            string sql = $"select ifnull(max({colName}),0) from {tableName} where {colName} like '" + code_prefix + DateTime.Now.ToString("yyyy") + "%';";
            DataTable dt = km.GetDTa(sql);
            string code = (Convert.ToInt32(dt.Rows[0][0].ToString().Replace(code_prefix, "")) + 1).ToString();
            string temp = DateTime.Now.ToString("yyyy") + "00000";
            code = code_prefix + temp.Substring(0, (temp.Length - code.Length)) + code;
            return code;
        }

    }
}
