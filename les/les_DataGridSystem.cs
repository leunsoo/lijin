using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace les
{
    public static class les_DataGridSystem
    {
        /// <summary>
        /// DataGrid에서 DataTable 출력 조건::베타버전 
        /// </summary>
        /// <param name="grdTable">data를 가져올 DataGrid</param>
        /// <param name="fields">procedure와 동일한 filed</param>
        /// <returns></returns>
        public static DataTable Get_Dt_From_DataGrid(DataGrid grdTable,string[] fields)
        {
            DataTable dt = new DataTable();

            for(int i = 0; i<fields.Length; i++)
            {
                dt.Columns.Add(fields[i], typeof(string));
            }

            DataRow dr;

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;
            string stringValue = "";

            for (int i = 0; i < rCount; i++)
            {
                dr = dt.NewRow();
                int fieldCount = 0;

                for (int j = 0; j < cCount; j++)
                {
                    int itemCount = grdTable.Items[i].Cells[j].Controls.Count; 
                    if (itemCount > 0)
                    {
                        for (int k = 0; k < itemCount; k++)
                        {
                            stringValue = Parsing(grdTable.Items[i].Cells[j].Controls[k]);
                            if(stringValue != "isPassController")
                            {
                                dr[fields[fieldCount]] = stringValue;
                                fieldCount++;
                            }
                        }
                    }
                    else
                    {
                        dr[fields[fieldCount]] = grdTable.Items[i].Cells[j].Text;
                        fieldCount++;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 첫번째 cell에 체크박스가 있는경우
        /// Dt를 옮길때 dt컬럼 개수를 기준으로 데이터를 넣어줌 <= 그리드 cell끝에 control이 있는 경우 때문에
        /// caseNo = 0 => dt복사
        /// caseNo = 1 => 두번째 cell에 No이 있다. 그 후 dt 복사
        /// caseNo = 2 => 첫번째에 checkBox와 같이 code나 idx를 담을 hiddenfield가 있음 ,
        ///               hiddenfield 컨트롤 id는 hdn_code로 해야함 그후 case 1과 동일
        /// </summary>
        public static void Set_CheckBox_DataGrid_From_Dt(DataGrid grdTable, DataTable dt, int caseNo)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = dt.Columns.Count;


            if (caseNo == 0)
            {
                for (int i = 0; i < rCount; i++)
                {
                    for (int j = 0; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j + 1].Text = dt.Rows[i][j].ToString();
                    }
                }
            }
            else if (caseNo == 1)
            {
                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = 0; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j+2].Text = dt.Rows[i][j].ToString();
                    }
                }
            }
            else if (caseNo == 2)
            {
                for (int i = 0; i < rCount; i++)
                {
                    ((HiddenField)grdTable.Items[i].FindControl("hdn_code")).Value = dt.Rows[i][0].ToString();
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = 1; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j + 1].Text = dt.Rows[i][j].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 첫번째 cell에 체크박스가 있는경우
        /// Dt를 옮길때 dt컬럼 개수를 기준으로 데이터를 넣어줌 <= 그리드 cell끝에 control이 있는 경우 때문에
        /// caseNo = 0 => dt복사
        /// caseNo = 1 => 두번째 cell에 No이 있다. 그 후 dt 복사
        /// caseNo = 2 => 첫번째에 checkBox와 같이 code나 idx를 담을 hiddenfield가 있음 ,
        ///               hiddenfield 컨트롤 id는 hdn_code로 해야함 그후 case 1과 동일
        /// caseNo = 3 => 첫번째에 checkBox와 같이 code나 idx를 담을 hiddenfield가 있음 , no이 없음
        /// start = 기본값 1을 넣어줘야함
        /// </summary>
        public static void Set_CheckBox_DataGrid_From_Dt(DataGrid grdTable, DataTable dt, int start, int end, int caseNo)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;

            if (caseNo == 0)
            {
                if (start == 0)
                {
                    start =1;
                }

                for (int i = 0; i < rCount; i++)
                {
                    for (int j = start, k = 0; j < end; j++, k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
            else if (caseNo == 1)
            {
                if (start == 0 || start == 1)
                {
                    start = 2;
                }

                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = start, k = 0; j < end; j++, k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
            else if (caseNo == 2)
            {
                if(start == 0 || start == 1)
                {
                    start = 2;
                }

                for (int i = 0; i < rCount; i++)
                {
                    ((HiddenField)grdTable.Items[i].FindControl("hdn_code")).Value = dt.Rows[i][0].ToString();
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = start, k = 1; j < end; j++, k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
            else if (caseNo == 3)
            {
                if (start == 0 )
                {
                    start = 1;
                }

                for (int i = 0; i < rCount; i++)
                {
                    ((HiddenField)grdTable.Items[i].FindControl("hdn_code")).Value = dt.Rows[i][0].ToString();

                    for (int j = start, k = 1; j < end; j++, k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// DataTable로 DataGrid 생성 조건::아이템 컨트롤이 없어야함
        /// </summary>
        /// <param name="caseNo">0= DT그대로 복사, 1= cells[0]에 No 넣어줌, 2= cells[0]에 idx넣고 cell[1]에 No 넣어줌</param>
        public static void Set_DataGrid_From_Dt(DataGrid grdTable, DataTable dt, int caseNo)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;


            if (caseNo == 0)
            {
                for (int i = 0; i < rCount; i++)
                {
                    for (int j = 0; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j].ToString();
                    }
                }
            }
            else if(caseNo == 1)
            {
                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[0].Text = (i + 1).ToString();

                    for (int j = 1; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                    }
                }
            }
            else if(caseNo == 2)
            {
                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[0].Text = dt.Rows[i][0].ToString();
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = 2; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// DataTable로 DataGrid 생성 데이터 넣을 컬럼 범위 지정 가능 :: 마지막 범위는 Columns.Count
        /// 조건::아이템 컨트롤이 없어야함
        /// </summary>
        /// <param name="caseNo">0= DT그대로 복사, 1= cells[0]에 No 넣어줌, 2= cells[0]에 idx넣고 cell[1]에 No 넣어줌</param>
        public static void Set_DataGrid_From_Dt(DataGrid grdTable, DataTable dt,int start, int end, int caseNo)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;
            
            if (caseNo == 0)
            {
                for (int i = 0; i < rCount; i++)
                {
                    for (int j = start, k = 0; j < end; j++,k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
            else if (caseNo == 1)
            {
                if(start == 0)
                {
                    start = 1;
                }

                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[0].Text = (i + 1).ToString();

                    for (int j = start, k= 0; j < end; j++,k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
            else if (caseNo == 2)
            {
                if(start < 2)
                {
                    start = 2;
                }    

                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[0].Text = dt.Rows[i][0].ToString();
                    grdTable.Items[i].Cells[1].Text = (i + 1).ToString();

                    for (int j = start,k = 1; j < cCount; j++,k++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][k].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// DataTable로 DataGrid 생성 조건::베타버전 
        /// </summary>
        /// <param name="grdTable">생성할 DataGrid</param>
        /// <param name="dt">DataGrid에 입력할 DataTable</param>
        /// <param name="fields">DataGrid가 가지고 있는 컨트롤 names</param>
        /// <returns></returns>
        public static void Set_DataGrid_From_Dt(DataGrid grdTable,DataTable dt, string[] fields)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;
            string stringValue = "";

            for (int i = 0; i < rCount; i++)
            {
                int fieldCount = 0;
                int dtColumn = 0;

                for (int j = 0; j < cCount; j++)
                {
                    int itemCount = grdTable.Items[i].Cells[j].Controls.Count;
                    if (itemCount > 0)
                    {
                        for (int k = 0; k < itemCount; k++)
                        {
                            stringValue = Parsing(grdTable.Items[i].Cells[j].Controls[k]);
                            if (stringValue != "isPassController")
                            {
                                Control_Value_Setting(grdTable.Items[i].FindControl(fields[fieldCount]), dt.Rows[i][dtColumn].ToString());
                                fieldCount++;
                                dtColumn++;
                            }
                        }
                    }
                    else
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][dtColumn].ToString();
                        dtColumn++;
                        //fieldCount++;
                    }
                }
            }
        }

        /// <summary>
        /// DataTable로 DataGrid 생성 조건::아이템 컨트롤이 없어야함
        /// </summary>
        /// <param name="SortColumn"> DataTable에서 merge기준이 될 columnPosition </param>
        /// <param name="start"> merge 시작위치 </param>
        /// <param name="end"> merge 끝 위치 </param>
        /// <param name="caseNo">0= DT그대로 복사, 1= cells[0]에 No 넣어줌, 2= cells[0]에 idx넣고 cell[1]에 No 넣어줌</param>
        public static void Set_Merge_DataGrid_From_Dt(DataGrid grdTable, DataTable dt,int SortColumn,int start,int end, int caseNo)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;

            string mergeValue = "";
            int rowSpan = 1;
            int no = 1;

            if (caseNo == 0)
            {
                for (int i = 0; i < rCount; i++)
                {
                    mergeValue = dt.Rows[i][SortColumn].ToString();

                    if (i < rCount - 1 && mergeValue == dt.Rows[i + 1][SortColumn].ToString())
                    {
                        rowSpan++;

                        for (int j = start; j < end+1; j++)
                        {

                            grdTable.Items[i - rowSpan + 2].Cells[j].RowSpan = rowSpan;
                            grdTable.Items[i - rowSpan + 2].Cells[j].Text = dt.Rows[i][j].ToString();

                            grdTable.Items[i + 1].Cells[j].Visible = false;
                        }
                    }
                    else
                    {
                        for (int j = start; j < end +1; j++)
                        {
                            grdTable.Items[i].Cells[j].Text = dt.Rows[i][j].ToString();
                        }
                        rowSpan = 1;
                    }

                    for (int j = 0; j < start; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j].ToString();
                    }

                    for (int j = end + 1; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j].ToString();
                    }

                    if (i == rCount - 1)
                    {
                        for (int j = start; j < end + 1; j++)
                        {
                            if (grdTable.Items[i].Cells[j].Visible)
                            {
                                grdTable.Items[i].Cells[j].Text = dt.Rows[i][j].ToString();
                            }
                        }
                    }
                }
            }
            else if (caseNo == 1)
            {
                for (int i = 0; i < rCount; i++)
                {
                    mergeValue = dt.Rows[i][SortColumn].ToString();
                    grdTable.Items[i].Cells[0].Text = no.ToString();

                    if (i < rCount - 1 && mergeValue == dt.Rows[i + 1][SortColumn].ToString())
                    {
                        rowSpan++;

                        for (int j = start; j < end + 1; j++)
                        {
                            if (j == 0)
                            {
                                grdTable.Items[i].Cells[j].RowSpan = rowSpan;
                            }
                            else
                            {
                                grdTable.Items[i - rowSpan + 2].Cells[j].RowSpan = rowSpan;
                                grdTable.Items[i - rowSpan + 2].Cells[j].Text = dt.Rows[i][j -1].ToString();
                            }

                            grdTable.Items[i + 1].Cells[j].Visible = false;
                        }
                    }
                    else
                    {
                        for (int j = start; j < end + 1; j++)
                        {
                            if (j != 0)
                            {
                                grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                            }
                        }

                        rowSpan = 1;
                        no++;
                    }

                    for(int j = 1; j < start; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j-1].ToString();
                        no++;
                    }

                    for (int j = end + 1; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j-1].ToString();
                    }

                    if (i == rCount - 1)
                    {
                        for (int j = start; j < end + 1; j++)
                        {
                            if (grdTable.Items[i].Cells[j].Visible && j != 0)
                            {
                                grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                            }
                        }
                    }
                }
            }
            else if (caseNo == 2)
            {
                for (int i = 0; i < rCount; i++)
                {
                    grdTable.Items[i].Cells[0].Text = dt.Rows[i][0].ToString();
                    grdTable.Items[i].Cells[1].Text = no.ToString();

                    mergeValue = dt.Rows[i][SortColumn].ToString();

                    if (i < rCount - 1 && mergeValue == dt.Rows[i + 1][SortColumn].ToString())
                    {
                        rowSpan++;

                        for (int j = start; j < end + 1; j++)
                        {
                            if (j == 0)
                            {
                                grdTable.Items[i].Cells[0].RowSpan = rowSpan;
                            }
                            else if(j == 1)
                            {
                                grdTable.Items[i].Cells[1].RowSpan = rowSpan;
                            }
                            else
                            {
                                grdTable.Items[i - rowSpan + 2].Cells[j].RowSpan = rowSpan;
                                grdTable.Items[i - rowSpan + 2].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                            }

                            grdTable.Items[i + 1].Cells[j].Visible = false;
                        }
                    }
                    else
                    {
                        for (int j = start; j < end + 1; j++)
                        {
                            if (j != 0)
                            {
                                grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                            }
                        }

                        rowSpan = 1;
                        no++;
                    }

                    for (int j = 2; j < start; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                        no++;
                    }
                    for (int j = end + 1; j < cCount; j++)
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][j - 1].ToString();
                    }

                    if (i == rCount - 1)
                    {
                        for (int j = start; j < end + 1; j++)
                        {
                            if (grdTable.Items[i].Cells[j].Visible && j != 0)
                            {
                                grdTable.Items[i].Cells[j].Text = dt.Rows[i][j -1].ToString();
                            }
                        }
                    }
                }
            }
        }

        public static void Set_DataGrid_From_Search_Dt(DataGrid grdTable, DataTable dt, string[] fields)
        {
            grdTable.DataSource = dt;
            grdTable.DataBind();

            int rCount = grdTable.Items.Count;
            int cCount = grdTable.Columns.Count;
            string stringValue = "";

            for (int i = 0; i < rCount; i++)
            {
                int fieldCount = 0;
                int dtColumn = 0;

                for (int j = 0; j < cCount; j++)
                {
                    int itemCount = grdTable.Items[i].Cells[j].Controls.Count;
                    if (itemCount > 0)
                    {
                        for (int k = 0; k < itemCount; k++)
                        {
                            stringValue = Parsing2(grdTable.Items[i].Cells[j].Controls[k]);
                            if (stringValue != "isPassController")
                            {
                                Control_Value_Setting(grdTable.Items[i].FindControl(fields[fieldCount]), dt.Rows[i][dtColumn].ToString());
                                fieldCount++;
                                dtColumn++;
                            }
                        }
                    }
                    else
                    {
                        grdTable.Items[i].Cells[j].Text = dt.Rows[i][dtColumn].ToString();
                        dtColumn++;
                        //fieldCount++;
                    }
                }
            }
        }

        //Dt에서 값을 가져와 DataGrid.Items.Cells.Control의 저장하기 위한 함수
        private static bool Control_Value_Setting(object itemControl,string value)
        {
            if (itemControl is TableCell)
            {
                ((TableCell)itemControl).Text = value;
                return true;
            }
            else if (itemControl is Label)
            {
               ((Label)itemControl).Text = value;
                return true;
            }
            else if (itemControl is HiddenField)
            {
                ((HiddenField)itemControl).Value = value;
                return true;
            }
            else if (itemControl is TextBox)
            {
                ((TextBox)itemControl).Text = value;
                return true;
            }
            else if (itemControl is System.Windows.Forms.DateTimePicker)
            {
                //System.Windows.Forms.DateTimePicker dateTime = (System.Windows.Forms.DateTimePicker)itemControl;
                //string mindate = dateTime.MinDate.ToShortDateString();
                //string valdate = dateTime.Value.ToShortDateString();
                //if (mindate != valdate)
                //{
                //    str = dateTime.Value.ToShortDateString();
                //}
            }
            else if (itemControl is DropDownList)
            {
                DropDownList combo = (DropDownList)itemControl;
                if (combo.SelectedIndex != -1)
                    combo.SelectedValue = value;

                return true;
            }
            else if (itemControl is CheckBox)
            {
                if(value == "0")
                {
                    ((CheckBox)itemControl).Checked = false;
                }
                else
                {
                    ((CheckBox)itemControl).Checked = true;
                }

                return true;
            }

            return false;
        }

        //DataGrid 에서 Dt를 구할 때
        //DataGrid.Items.Cells.Control의 데이터 형식을 확인하고 알맞은 값을 가져온다.
        private static string Parsing(object itemControl)
        {
            string str = "isPassController";

            if (itemControl is TableCell)
            {
                str = ((TableCell)itemControl).Text;
            }
            else if(itemControl is Label)
            {
                str = ((Label)itemControl).Text;
            }
            else if (itemControl is HiddenField)
            {
                str = ((HiddenField)itemControl).Value;
            }
            else if(itemControl is TextBox)
            {
                str = ((TextBox)itemControl).Text;
            }
            else if (itemControl is System.Windows.Forms.DateTimePicker)
            {
                //System.Windows.Forms.DateTimePicker dateTime = (System.Windows.Forms.DateTimePicker)itemControl;
                //string mindate = dateTime.MinDate.ToShortDateString();
                //string valdate = dateTime.Value.ToShortDateString();
                //if (mindate != valdate)
                //{
                //    str = dateTime.Value.ToShortDateString();
                //}
            }
            else if (itemControl is DropDownList)
            {
                DropDownList combo = (DropDownList)itemControl;
                if (combo.SelectedIndex != -1)
                    str = combo.SelectedValue.ToString();
            }
            else if(itemControl is CheckBox)
            {
                str = "0";
                if (((CheckBox)itemControl).Checked == true)
                    str = "1";
            }

            return str;
        }

        private static string Parsing2(object itemControl)
        {
            string str = "isPassController";

            if (itemControl is TableCell)
            {
                str = ((TableCell)itemControl).Text;
            }
            else if (itemControl is Label)
            {
                str = ((Label)itemControl).Text;
            }
            else if (itemControl is HiddenField)
            {
                str = ((HiddenField)itemControl).Value;
            }
            else if (itemControl is TextBox)
            {
                str = ((TextBox)itemControl).Text;
            }
            else if (itemControl is DropDownList)
            {
                DropDownList combo = (DropDownList)itemControl;
                if (combo.SelectedIndex != -1)
                    str = combo.SelectedValue.ToString();
            }

            return str;
        }
    }
}
