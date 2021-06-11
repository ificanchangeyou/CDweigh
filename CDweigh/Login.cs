using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDweigh
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            MsgText.Text = "";
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
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
            if (UserName.Text == null) { MsgText.Text = "请输入用户名"; return; }
            if (Password.Text == null) MsgText.Text = "请输入密码"; return;

        }
    }
}
