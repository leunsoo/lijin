using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MysqlLib;
using System;

namespace les
{
    public static class les_Tool
    {
        public static string Get_Date_Now()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string Get_Date_MM_01()
        {
            return DateTime.Now.ToString("yyyy-MM-01");
        }

        public static string Get_Date_01_01()
        {
            return DateTime.Now.ToString("yyyy-01-01");
        }

        public static void Set_TextBox_Date_MM_dd(TextBox tb)
        {
            tb.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static void Set_TextBox_Date_MM_01(TextBox tb)
        {
            tb.Text = DateTime.Now.ToString("yyyy-MM-01");
        }

        public static void Set_TextBox_Date_01_01(TextBox tb)
        {
            tb.Text = DateTime.Now.ToString("yyyy-01-01");
        }

        public static void Set_TextBoxes_Period_MM_01_To_Now(TextBox start,TextBox end)
        {
            start.Text = DateTime.Now.ToString("yyyy-MM-01");
            end.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static void Set_TextBoxes_Period_01_01_To_Now(TextBox start, TextBox end)
        {
            start.Text = DateTime.Now.ToString("yyyy-01-01");
            end.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static void Set_TextBox_Period_AddMonth_MM_01_To_Now(TextBox start, TextBox end)
        {
            start.Text = DateTime.Now.AddMonths(-4).ToString("yyyy-MM-01");
            end.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
