using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDweigh.Menu_Page;

namespace CDweigh
{
    

    public partial class CreateOrder : Form
    {
        public event ReturnIcCode ReturnIC;
        Scales weigh;
        public CreateOrder(Scales weigh)
        {
            InitializeComponent();
            this.weigh = weigh;
            this.weigh.SendIcCode += new ReturnIcCode(Weigh_SendIcCode);
        }

        private void Weigh_SendIcCode(string IC)
        {
            
            this.IC_text.Text = IC;
        }

        private void CreateOrder_Load(object sender, EventArgs e)
        {

        }


        private void Exit_But_Click(object sender, EventArgs e)
        {
            if (ReturnIC != null) ReturnIC(IC_text.Text);
            this.Close();
        }




        #region 控件动画

        
        private void Exit_But_MouseEnter(object sender, EventArgs e)
        {
            Exit_But.BackgroundImage = Properties.Resources.Close_WR;
        }

        private void Exit_But_MouseLeave(object sender, EventArgs e)
        {
            Exit_But.BackgroundImage = Properties.Resources.Close_BT;
        }
        private void BillSave_But_MouseDown(object sender, MouseEventArgs e)
        {
            BillSave_But.BackgroundImage = Properties.Resources.SaveText_MouseDown;
        }

        private void BillSave_But_MouseEnter(object sender, EventArgs e)
        {
            BillSave_But.BackgroundImage = Properties.Resources.SaveText_Mouse;
        }

        private void BillSave_But_MouseLeave(object sender, EventArgs e)
        {
            BillSave_But.BackgroundImage = Properties.Resources.SaveText;
        }

        private void BillSave_But_MouseUp(object sender, MouseEventArgs e)
        {
            BillSave_But.BackgroundImage = Properties.Resources.SaveText_Mouse;
        }




        #endregion


        #region HeadMove 窗体标题 拖动
        private System.Drawing.Point mPoint;
        private void HeadPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Location = new System.Drawing.Point(Location.X + e.X - mPoint.X, Location.Y + e.Y - mPoint.Y);
            }
        }
        private void HeadPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new System.Drawing.Point(e.X, e.Y);
        }

        #endregion
    }
}
