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
using System.Reflection;

namespace CDweigh
{
    public partial class Login : Form
    {
        bool Unsee = true;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            MsgText.Text = "";
            Global.Sqldatabase = Properties.Settings.Default.SqlConnStr;
            try
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
            catch (Exception ex)
            {
                //TODO: error log
                throw;
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            //
            DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void CloseBtn_MouseEnter(object sender, EventArgs e)
        {
            CloseBtn.Image = Properties.Resources.close_White2;
        }

        private void CloseBtn_MouseLeave(object sender, EventArgs e)
        {
            CloseBtn.Image = Properties.Resources.close_White;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //检查输入
            if (username_combo.SelectedItem == null || username_combo.SelectedItem.ToString().Length < 1)
            { MsgText.Text = "请输入用户名"; return; }
            if (Password.Text.Length < 1) { MsgText.Text = "请输入密码"; return; }
            string pass = Md5Helper.Md5(Password.Text.ToString());
            try
            {
                //查询用户
                string sql = "SELECT Password,Rank From Users Where Username =@ID";
                IDictionary<string, Object> parmater = new Dictionary<string, Object>();
                parmater.Add("@ID", username_combo.SelectedItem.ToString());

                using (DataTable table = SqlHelper.QueryDataTable(Global.Sqldatabase, sql, CommandType.Text, parmater))
                {
                    if (table != null)//&& table.Rows.Count>0)
                    {
                        if (table.Rows.Count != 0)
                        {
                            string pas = table.Rows[0][0].ToString();
                            if (pass.Equals(pas)) //相等
                            {
                                Global.users.Username = username_combo.SelectedItem.ToString();
                                sql = "SELECT * FROM RANK WHERE UserName =@ID";
                                using (DataTable rankRow = SqlHelper.QueryDataTable(Global.Sqldatabase, sql, CommandType.Text, parmater))
                                {
                                    List<User> userRank = Global.TableToEntity<User>(rankRow);
                                    if (userRank.Count != 0)
                                    {
                                        Global.users = userRank[0];
                                    }
                                }
                                LogHelper.Sql_Info(Global.users.Username, "登录");
                                //Global.Username = username_combo.SelectedText.ToString();
                                //Global.Rank = Convert.ToInt32(table.Rows[0][1].ToString());
                                DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MsgText.Text = "密码错误,请检查";
                            }
                        }else
                        {
                            MsgText.Text = "为查询到此账号";
                        }

                    }
                    else
                    {
                        MsgText.Text = "账号错误,请检查";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message + " 来源:" + ex.Source);
                LogHelper.Error("登录错误", ex);
                //TODO: Log
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
