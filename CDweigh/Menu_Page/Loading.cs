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
    public partial class Loading : Form
    {
        MainWin main;
        public Loading(MainWin main)
        { 
            InitializeComponent();
            this.main = main;
            this.main.msg += Main_msg;
        }

        private void Main_msg(string msg)
        {
            if (msg == "1") { this.Close(); main.Show(); }
            MsgInfo.Text = msg;
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }
    }
}
