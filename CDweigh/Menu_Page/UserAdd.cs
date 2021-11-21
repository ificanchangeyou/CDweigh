using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDweigh.Class;
using CDweigh.Helper;
using System.Reflection;

namespace CDweigh
{
    public partial class UserAdd : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        bool issee = true;
        bool issee2 = true;

        public UserAdd(int Rank =1)
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MsgText.Text = "";
                if (Username.Text.Length < 1)
                {
                    MsgText.Text = "请输入用户名";
                    return;
                }
                if (Password.Text.Length < 1 || PasswordAgain.Text.Length < 1)
                {
                    MsgText.Text = "请输入密码";
                    return;
                }
                if (!Password.Text.Equals(PasswordAgain.Text))
                {
                    MsgText.Text = "两次密码不一致";
                    return;
                }
                string Sqlstr = "SELECT Username FROM Users WHERE Username = @ID";
                IDictionary<string, Object> parmater = new Dictionary<string, Object>();
                parmater.Add("@ID", Username.Text.ToString());
                object ret = SqlHelper.QueryScalar(Global.Sqldatabase, Sqlstr, CommandType.Text, parmater);
                if (ret != null)
                {

                    MsgText.Text = "此用户已存在";
                    return;
                }
                string md5 = Md5Helper.Md5(Password.Text.ToString());
                parmater.Add("@password", md5);
                Sqlstr = "INSERT INTO Users(Username,Password,Rank) VALUES(@ID,@password,1) ";
                int updateLine = SqlHelper.Execute(Global.Sqldatabase, Sqlstr, CommandType.Text, parmater);
                if (updateLine < 1)
                {
                    MsgText.Text = "新增用户失败";
                    return;
                }else
                {
                    parmater.Remove("@password");
                   // Insert_Rank("TEST", 0);
                    Sqlstr = "INSERT INTO Rank(Username) VALUES(@ID)";
                    updateLine = SqlHelper.Execute(Global.Sqldatabase, Sqlstr, CommandType.Text, parmater);
                    if (updateLine < 1)
                    {
                        MsgText.Text = "用户权限新增失败";
                        return;
                    }else
                    {
                        Sqlstr = "SELECT * FROM RANK WHERE UserName =@ID";
                        using (DataTable rankRow = SqlHelper.QueryDataTable(Global.Sqldatabase, Sqlstr, CommandType.Text, parmater))
                        {
                            List<User> userRank = Global.TableToEntity<User>(rankRow);
                            if (userRank.Count != 0)
                            {
                                Global.users = userRank[0];
                            }
                        }
                        //TODO: log
                        MessageBox.Show("用户 " + Username.Text.ToString() + "注册成功,请更新新用户的权限");
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Error log
                throw;
            }
        }


        private bool Insert_Rank(string username,int rank)
        {
            bool ret=false;
            string str= "INSERT INTO Rank(";
            string values = ")VALUES(";
            try
            {
                User user = new User();
                user.Username = username;
                IDictionary<string, Object> parmater = new Dictionary<string, Object>();
                Type type = user.GetType();
                PropertyInfo[] infolist = type.GetProperties();
                int i = 0;
                foreach (PropertyInfo info in infolist)
                {
                    str += (i == 0) ? (info.Name) : ("," + info.Name);
                    values += (i == 0) ? ("@" + info.Name) : ("," + "@" + info.Name);
                    i++;
                    parmater.Add("@"+info.Name, info.GetValue(user, null));
                }
                str += values + ")";
                switch (rank)
                {
                    case 0://Rank0 没有任何权限
                        //DO nothing
                        break;
                    case 1:
                        
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }



            return ret;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
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

        private void IsSee_Click(object sender, EventArgs e)
        {
            if (issee)
            {
                Password.PasswordChar = '\0';
                IsSee.BackgroundImage = Properties.Resources.See;
            }
            else
            {
                Password.PasswordChar = '*';
                IsSee.BackgroundImage = Properties.Resources.UnSee;
            }
            issee = !issee;
        }

        private void IsSee2_Click(object sender, EventArgs e)
        {
            if (issee2)
            {
                PasswordAgain.PasswordChar = '\0';
                IsSee.BackgroundImage = Properties.Resources.See;
            }
            else
            {
                PasswordAgain.PasswordChar = '*';
                IsSee.BackgroundImage = Properties.Resources.UnSee;
            }
            issee2 = !issee2;
        }
    }
}
