using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDweigh.Helper;
using CDweigh.Class;
using System.Runtime.InteropServices;

namespace CDweigh
{
    public partial class ChangePaswd : Form
    {
        string Username;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public ChangePaswd(string Username=null)
        {
            InitializeComponent();
            this.Username = Username;
        }

        private void ChangePaswd_Load(object sender, EventArgs e)
        {
            try
            {
                if(Username==null)
                {
                    string Sqlstr = "SELECT Username FROM Users ";
                    DataTable users =  SqlHelper.QueryDataTable(Global.Sqldatabase, Sqlstr);
                    if(users!=null)
                    {
                        foreach (DataRow rows in users.Rows)
                        {
                            username_combo.Items.Add(rows[0].ToString());
                            username_combo.Enabled = true;
                        }
                    }
                }
                else
                {
                    username_combo.Items.Add(Username);
                    username_combo.SelectedIndex = 0;
                    username_combo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //TODO: error log
                throw;
            }
            
        }

        private void IsSee_Click(object sender, EventArgs e)
        {
            if (IsSee.BackgroundImage == Properties.Resources.UnSee)
            {
                Password_Old.PasswordChar = '\0';
                IsSee.BackgroundImage = Properties.Resources.See;
            }
            else
            {
                Password_Old.PasswordChar = '*';
                IsSee.BackgroundImage = Properties.Resources.UnSee;
            }
        }

        private void IsSee2_Click(object sender, EventArgs e)
        {
            if (IsSee2.BackgroundImage == Properties.Resources.UnSee)
            {
                Password_New.PasswordChar = '\0';
                IsSee2.BackgroundImage = Properties.Resources.See;
            }
            else
            {
                Password_New.PasswordChar = '*';
                IsSee2.BackgroundImage = Properties.Resources.UnSee;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(username_combo.SelectedText==null)
            {
                MsgText.Text = "请选择登录名";
                return;
            }
            if(Password_Old.Text==null || Password_New.Text==null)
            {
                MsgText.Text = "请填写密码";
                return;

            }
            if(Password_Old.Text!=Password_New.Text)
            {
                MsgText.Text = "两次密码不一致";
                return;

            }
            try
            {
                string pass = Md5Helper.Md5(Password_Old.Text.ToString());
                //查询用户
                string sql = "UPDATE Users SET  Password =@pass Where Username =@ID";
                IDictionary<string, Object> parmater = new Dictionary<string, Object>();
                parmater.Add("@pass", Password_Old.Text);
                parmater.Add("@ID", username_combo.SelectedText);
                int UpdateRows = SqlHelper.Execute(Global.Sqldatabase, sql, CommandType.Text, parmater);
                if (UpdateRows == 1)
                {
                    MessageBox.Show("修改成功", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else if (UpdateRows == 0)
                {
                    MessageBox.Show("修改失败", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //TODO: error log 
                }
            }
            catch (Exception ex)
            {
                //TODO: error log 
                throw;
            }

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
