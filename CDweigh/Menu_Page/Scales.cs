using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDweigh.Menu_Page
{
    public partial class Scales : Form
    {

        public event ReturnIcCode SendIcCode; //定义事件
        public Scales()
        {
            InitializeComponent();
        }

        private void Scales_Load(object sender, EventArgs e)
        {
            weightBase Scale1 = new weightBase();
            Scale1.Scale.DataReceived += Scale_DataReceived;
        }

        /// <summary>
        /// 地磅数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scale_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Scales_Paint(object sender, PaintEventArgs e)
        {
            // picCircle(Infrared_Dis);
            //picCircle(Vary_Dis);
        }


        public void picCircle(PictureBox pic)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pic.ClientRectangle);
            Region region = new Region(gp);
            pic.Region = region;
            gp.Dispose();
            region.Dispose();
        }

        private void CreateBill_But_Click(object sender, EventArgs e)
        {
            CreateOrder order = new CreateOrder(this);
            order.ReturnIC += Order_ReturnIC;
            order.Show();
        }

        private void Order_ReturnIC(string IC)
        {
            IcCode_text.Text = IC;
        }
    }
}
