using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDweigh.Class;
using CDweigh.Helper;
using System.Runtime.InteropServices;

namespace CDweigh
{
    public partial class UsersAuthority : Form
    {

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;


        Dictionary<string, string> Field_Dic = new Dictionary<string, string> {
            {"UserName","账号" },  {"Change_password","密码修改" }, {"Delete_user","注销账号" }, {"Change_rank","权限修改" },
             {"Scales_page","地磅界面" }, {"Report_page","报表界面" }, {"DriverInfo_page","驾驶员信息" }, {"VehicleInfo_page","车辆信息" },
              {"CustomerInfo_page","客户信息" }, {"MaterialsInfo_page","物料信息" }, {"OperationLog_page","操作日志" }, {"AlarmLog_page","报警日志" },
               {"SystemSet_page","系统设置" }, {"DataAegis_page","数据维护" }, {"Camera_page","设备测试" }
        };
             
        public UsersAuthority()
        {
            InitializeComponent();
        }

        private void UsersAuthority_Load(object sender, EventArgs e)
        {
            Global.Sqldatabase = Properties.Settings.Default.SqlConnStr;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGrid.ColumnHeadersBorderStyle =DataGridViewHeaderBorderStyle.Raised;
            dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGrid.GridColor = SystemColors.ActiveBorder;
            dataGrid.RowHeadersVisible = false;
            UpdateGrid();
        }

        public void UpdateGrid()
        {

            string SqlStr = "SELECT ";
            int i = 0;
            foreach (KeyValuePair<string, string> keyvalue in Field_Dic)
            {
                if (i == 0) SqlStr += keyvalue.Key + " AS " + keyvalue.Value;
                else SqlStr += "," + keyvalue.Key + " AS " + keyvalue.Value;
                i++;
            }
            SqlStr += " FROM Rank";
            try
            {
                DataTable table = SqlHelper.QueryDataTable(Global.Sqldatabase, SqlStr);
                dataGrid.DataSource = table;
                dataGrid.Columns[0].ReadOnly = true;
            }
            catch (Exception ex)
            {
                //TODO: error log
                throw;
            }




        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ItemPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }



        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void CloseBtn_MouseDown(object sender, MouseEventArgs e)
        {
            CloseBtn.Image = CDweigh.Properties.Resources.close_White_Red;

        }

        private void CloseBtn_MouseEnter(object sender, EventArgs e)
        {
            CloseBtn.Image = CDweigh.Properties.Resources.close_White2;
        }

        private void CloseBtn_MouseLeave(object sender, EventArgs e)
        {
            CloseBtn.Image = CDweigh.Properties.Resources.close_White;
        }

        private void CloseBtn_MouseUp(object sender, MouseEventArgs e)
        {
            CloseBtn.Image = CDweigh.Properties.Resources.close_White2;
        }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string name;
            name= dataGrid.Columns[dataGrid.CurrentCell.ColumnIndex].Name;
           // MessageBox.Show(name);
        }

        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            bool Update_data = (bool)dataGrid.CurrentCell.Value; //获取修改后的数据
            string field = dataGrid.CurrentCell.OwningColumn.Name;//获取列名
            string field_S;
            string users = dataGrid.CurrentRow.Cells[0].Value.ToString();//获取当前行的用户名
            try
            {
                field_S = Find_Key(Field_Dic, field);//根据列名查找字段
                if(field_S != null)
                {
                    IDictionary<string, Object> parmater = new Dictionary<string, Object>();
                    parmater.Add("@value", Update_data);
                    parmater.Add("@name", users);
                    string SqlStr = "UPDATE Rank SET "+ field_S + " = @value WHERE UserName = @name ";
                    int line = SqlHelper.Execute(Global.Sqldatabase, SqlStr, CommandType.Text, parmater);
                    if (line != 1) MessageBox.Show(users + "的权限:" + field + "更新失败");
                    //TODO: log 
                }
            }
            catch (Exception ex)
            {
                //TODO:error log
                throw;
            }
            
        }

        private void dataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGrid.IsCurrentCellDirty)
            {
                dataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private string Find_Key(Dictionary<string ,string > dic,string value)
        {
            string ret = null;
            foreach (KeyValuePair<string,string> pair in dic)
            {
                if (pair.Value.Equals(value))
                {
                    ret = pair.Key;
                    break;
                }
            }
            return ret;
        }
    }
}
