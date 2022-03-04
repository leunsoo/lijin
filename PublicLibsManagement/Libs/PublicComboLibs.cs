using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using MysqlLib;
using System.IO;
using excel = Microsoft.Office.Interop.Excel;
using System.Web;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace PublicLibsManagement
{
    public class PublicComboLibs
    {
        public static string orderNo;
        public static string commandCode;
        public static string StepNo;
        public static string balNo;
        public static string inportNo;
        public static string releaseNo;
        DB_mysql km = new DB_mysql();

        public static void customerUser(ComboBox cb)   //담당자 콤보 박스
        {
            DataTable dt = new DataTable();
            string sql = string.Format("exec SP_customerUserGetAll");
            cb.DisplayMember = "emp_name";
            cb.ValueMember = "emp_code";
            cb.DataSource = new BindingSource(km.GetDTa(sql), null);
            cb.SelectedIndex = -1;
        }

        public static void itemgroupCb(ComboBox cb)   //상품분류
        {
            DataTable dt = new DataTable();
            string sql = string.Format("exec SP_itemgroupGetAll");  //상품분류 상위
            cb.DisplayMember = "Item_GroupName";
            cb.ValueMember = "Item_GroupCode";
            cb.DataSource = new BindingSource(DBOPEN.GetDT(sql), null);
            cb.SelectedIndex = -1;
        }

        public static void itemgroupSubCb(ComboBox cb,string code)   //상품분류
        {
            DataTable dt = new DataTable();
            string sql = string.Format("exec SP_itemgroupGetParent '{0}'",code);  //상품세부분류
            cb.DisplayMember = "Item_GroupName";
            cb.ValueMember = "Item_GroupIdx";
            cb.DataSource = new BindingSource(DBOPEN.GetDT(sql), null);
            cb.SelectedIndex = -1;
        }

        public static void deliveryAddress(ComboBox cb, string tcode,string gu)   //배송지
        {
            DataTable dt = new DataTable();
            string sql = "select idx,addr from tb_delivery where tr_code ='"+ tcode +"' and deliveryGU='"+ gu +"'";  
            cb.DisplayMember = "addr";
            cb.ValueMember = "idx";
            cb.DataSource = new BindingSource(DBOPEN.GetDT(sql), null);
            cb.SelectedIndex = -1;
        }

        public static string DataTable_ExportToExcel(DataTable dt,string Fnm,string date,string title) //dt --excel
        {
            string Ret = "";
            //엑셀 저장 경로
            string path = @"C:\temp\" + Fnm;
            try
            {

                var excelApp = new excel.Application();
                excelApp.Workbooks.Add();
                excel._Worksheet workSheet = (excel._Worksheet)excelApp.ActiveSheet;
                excel.Range range;

                range = workSheet.get_Range("A1", "b1");
                range.Merge(false);
                range.HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;
                range.Font.Color= ColorTranslator.ToOle(Color.Blue);
                range.Value = date;
                range = workSheet.get_Range("c1", "f1");
                range.Merge(false);
                range.HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;
                range.Value = title;

                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    workSheet.Cells[2, i + 1] = dt.Columns[i].ColumnName;
                   //  workSheet.Columns[i+1].NumberFormat = "@";
               }

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        string data = dt.Rows[i][j].ToString().Replace("@", "'");
                        workSheet.Cells[i + 3, j + 1] = data;
                    }
                }
                workSheet.Columns.AutoFit(); 
                if (File.Exists(path)) File.Delete(path);
                workSheet.SaveAs(path);
                excelApp.Quit();
                Ret = "OK";
            }
            catch (Exception ex)
            {
                Ret = "";
            }
            return Ret;
        }
        public static void DataGridView_ExportToExcel(DataGridView Dg, string fileName)
        {
            excel.Application excelApp = new excel.Application();
            if (excelApp == null)
            {
                MessageBox.Show("엑셀이 설치되지 않았습니다");
                return;
            }
            excel.Workbook wb = excelApp.Workbooks.Add(true);
            excel._Worksheet workSheet = wb.Worksheets.get_Item(1) as excel._Worksheet;
            workSheet.Name = "C#";

            if (Dg.Rows.Count == 0)
            {
                MessageBox.Show("출력할 데이터가 없습니다");
                return;
            }

            // 헤더 출력
            for (int i = 0; i < Dg.Columns.Count - 1; i++)
            {
                workSheet.Cells[1, i + 1] = Dg.Columns[i].HeaderText;
            }
            //내용 출력
            for (int r = 0; r < Dg.Rows.Count; r++)
            {
                for (int i = 0; i < Dg.Columns.Count - 1; i++)
                {
                    workSheet.Cells[r + 2, i + 1] =Dg.Rows[r].Cells[i].Value.ToString();
                }
            }
            workSheet.Columns.AutoFit(); // 글자 크기에 맞게 셀 크기를 자동으로 조절
           // string strStyle = "<style>td { mso-number-format:\@; } </style>";
           //                                  HttpContext.Current.Response.Write(strStyle);

            // 엑셀 2003 으로만 저장이 됨
            wb.SaveAs(fileName, excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            excelApp.Quit();
            releaseObject(excelApp);
            releaseObject(workSheet);
            releaseObject(wb);
        }

        #region 메모리해제
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception e)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion
        //Dim strStyle As String = "<style>td { mso-number-format:\@; } </style>";
    }
}