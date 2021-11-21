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

namespace CDweigh
{
    public partial class Logout : Form
    {
        public string Username;
        bool Unsee = true;
        public Logout(string username=null)
        {
            InitializeComponent();
            this.Username = username;
            Password.Enabled = (Username == null) ? false : true;
        }

        private void Logout_Load(object sender, EventArgs e)
        {
            try
            {
                if (Username == null)
                {
                    string Sqlstr = "SELECT Username FROM Users ";
                    DataTable users = SqlHelper.QueryDataTable(Global.Sqldatabase, Sqlstr);
                    if (users != null)
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
            if (Unsee)
            {
                Password.PasswordChar = '\0';
                IsSee.BackgroundImage = Properties.Resources.See;
            }
            else
            {
                Password.PasswordChar = '*';
                IsSee.BackgroundImage = Properties.Resources.UnSee;
            }
            Unsee = !Unsee;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (username_combo.SelectedItem.ToString() == null)
            {
                MsgText.Text = "请选择需要注销的用户";
                return;
            }
            if(Password.Text==null && Username == null)
            {
                MsgText.Text = "请输入密码";
            }
            string SqlStr = "";
            IDictionary<string, Object> parmater = new Dictionary<string, Object>();
            parmater.Add("@ID", username_combo.SelectedItem.ToString());
            if (Password.Enabled)
            {
                string pass = Md5Helper.Md5(Password.Text.ToString());
                SqlStr = "SELECT Password FROM Users WHERE Username = @ID";
                string  md5 = SqlHelper.QueryScalar(Global.Sqldatabase, SqlStr, CommandType.Text, parmater).ToString();
                if(!md5.Equals(pass))
                {
                    MsgText.Text = "密码不符";
                    return;
                } 
            }
            SqlStr = "DELETE FROM Users WHERE Username= @ID";
            int line= SqlHelper.Execute(Global.Sqldatabase, SqlStr, CommandType.Text, parmater);
            if (line == 0) MsgText.Text = "无法删除用户:" + username_combo.SelectedItem.ToString();
            else if (line == 1)
            {
                MsgText.Text = "成功删除用户:" + username_combo.SelectedItem.ToString();
                //TODO: log 
            }
            else
            {
                //TODO: error log
            }

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
