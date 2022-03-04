using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public class Meth : Page
{

    protected void TEST(DataGrid grdTable)
    {
        DataTable dt = new DataTable();

        int grdColumnCount = grdTable.Columns.Count;
        int grdItemCount = grdTable.Items.Count;

        //테이블 그리기
        for (int i = 0; i < grdColumnCount; i++)
        {
            dt.Columns.Add();
        }
        for (int i = 0; i < grdItemCount; i++)
        {
            dt.Rows.Add();
        }

        //테이블 값넣기
        for (int i = 0; i < grdItemCount; i++)
        {
            for (int j = 0; j < grdColumnCount; j++)
            {
                if (grdTable.Items[i].Cells[j].Controls.Count > 1)
                {
                }
                else if (grdTable.Items[i].Cells[j].Controls.Count == 1)
                {
                    Parsing(dt.Rows[i][j], grdTable.Items[i].Cells[j].Controls[0]);
                }
                else
                {
                    dt.Rows[i][j] = grdTable.Items[i].Cells[j].Text;
                }
            }
        }

        DataRow dr = dt.NewRow();
        dt.Rows.InsertAt(dr, 0);


    }

    private void Parsing(object obj,object itemControl)
    {
        string value = "";

        if (itemControl is TableCell)
        {
            obj = ((TableCell)itemControl).Text;
        }
        if (itemControl is Label)
        {
            obj = ((Label)itemControl).Text;
        }
        else if (itemControl is HiddenField)
        {
            obj = ((HiddenField)itemControl).Value;
        }
        if (itemControl is TextBox)
        {
            obj = ((TextBox)itemControl).Text;
        }
        if (itemControl is System.Windows.Forms.DateTimePicker)
        {
            System.Windows.Forms.DateTimePicker dateTime = (System.Windows.Forms.DateTimePicker)itemControl;
            string mindate = dateTime.MinDate.ToShortDateString();
            string valdate = dateTime.Value.ToShortDateString();
            if (mindate != valdate)
            {
                obj = dateTime.Value.ToShortDateString();
            }
        }
        if (obj is DropDownList)
        {
            DropDownList combo = (DropDownList)itemControl;
            if (combo.SelectedIndex != -1)
                obj = combo.SelectedValue.ToString();
        }
        if (obj is CheckBox)
        {
            obj = "0";
            if (((CheckBox)itemControl).Checked == true)
                obj = "1";
        }
    }

    /*
    //팝업 호출 함수
    //조건  1.호출하려는 팝업 페이지가 현재 페이지가 있는 폴더경로와 같은위치에 있는 popUp이라는 폴더 안에 있어야한다.
    //      2.넘겨줄 string[] 파라미터가 없을 경우 null값을 넣어야한다.
    protected string PopUp(string pageName, string[] parameter, int sizeX, int sizeY)
    {
        string[] path = Request.Url.AbsolutePath.Split('/'); //현재 페이지의 폴더경로를 가져온다

        string popUpPath = $"/{path[1]}/{path[2]}/popUp/{pageName}.aspx"; //팝업창 경로
        string paramStr = ""; //넘겨줄 매개변수 목록
        string option = "";
        int posX = (Screen.PrimaryScreen.Bounds.Width / 2) - (sizeX / 2);
        int posY = (Screen.PrimaryScreen.Bounds.Height / 2) - (sizeY / 2);


        //매개변수 목록 생성
        if (parameter != null)
        {
            paramStr += "?";

            for (int i = 0; i < parameter.Length; i++)
            {
                paramStr += ("param" + i.ToString() + "=" + parameter[i] + "&");
            }

            paramStr.Substring(0, paramStr.Length - 1);
        }

        //화면옵션설정
        option = $"status=no,width={sizeX.ToString()},height={sizeY.ToString()},left={posX.ToString()},top={posY.ToString()}";

        //전체 구문 window.open(파일경로,위치,옵션)
        string totalStr = $"window.open('{popUpPath + paramStr}','_blank','{option}'); return false;";

        return totalStr;
    }
    */
}