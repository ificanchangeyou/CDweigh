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
using System.Threading;

namespace CDweigh
{
    public delegate void ReturnIcCode(string IC);
    public delegate void SendMsg(string msg);
    
    public partial class MainWin : Form
    {
        public event SendMsg msg; //定义发送初始化信息事件

        public MainWin()
        {
            InitializeComponent();
        }


        //FormBorderStyle.None时，支持改变窗体大小
        #region 支持改变窗体大小
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        /*
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else
                            m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else
                            m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201://鼠标左键按下的消息  拖动窗体功能
                    //m.Msg = 0x00A1;//更改消息为非客户区按下鼠标
                    //m.LParam = IntPtr.Zero; //默认值
                    //m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }*/
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

        //关闭按钮
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int CLOSE_SIZE = 10;
        private void MainTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            //try

            //{
            //    //Graphics g = e.Graphics;
            //    //Pen p = new Pen(Color.Blue);
            //    //Font font = new Font("Arial", 10.0f);
            //    //SolidBrush brush = new SolidBrush(Color.Red);

            //    //g.DrawRectangle(p, MainTab.GetTabRect(0));
            //    //g.DrawString("tabPage1", font, brush, (RectangleF)MainTab.GetTabRect(0));

            //    if (MainTab.TabPages.Count == 0) return;
            //    Rectangle myTabRect = this.MainTab.GetTabRect(e.Index);

            //    // Font font
            //    Font titleFont = new Font("宋体", 9, FontStyle.Bold);
            //    //先添加TabPage属性  
            //    e.Graphics.DrawString(this.MainTab.TabPages[e.Index].Text , titleFont, SystemBrushes.ControlText, myTabRect.X +5, myTabRect.Y + 10);
            //    //再画一个矩形框
            //    using (Pen p = new Pen(Color.Black))//自动释放资源
            //    {
            //        myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
            //        myTabRect.Width = CLOSE_SIZE;
            //        myTabRect.Height = CLOSE_SIZE;
            //        e.Graphics.DrawRectangle(p, myTabRect);
            //    }

            //    //画关闭符号
            //    using (Pen objpen = new Pen(Color.Black))
            //    {
            //        //"/"线
            //        Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
            //        Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
            //        e.Graphics.DrawLine(objpen, p1, p2);

            //        //"/"线
            //        Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
            //        Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
            //        e.Graphics.DrawLine(objpen, p3, p4);
            //    }
            //    e.Graphics.Dispose();


            //}

            //catch (Exception)
            //{
            //}
        }

        private void MainTab_MouseDown(object sender, MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left)
            //{
            //    int x = e.X, y = e.Y;

            //    int CLOSE_SIZE = 10;

            //    //计算关闭区域  
            //    Rectangle myTabRect = this.MainTab.GetTabRect(this.MainTab.SelectedIndex);

            //    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
            //    myTabRect.Width = CLOSE_SIZE;
            //    myTabRect.Height = CLOSE_SIZE;

            //    //如果鼠标在区域内就关闭选项卡  
            //    bool isClose = x > myTabRect.X && x < myTabRect.Right
            //                    && y > myTabRect.Y && y < myTabRect.Bottom;

            //    if (isClose == true)
            //    {
            //        this.MainTab.TabPages.Remove(this.MainTab.SelectedTab);
            //    }
            //}

        }
        Loading LoadFrom;
        //加载画面 
        private void MainWin_Load(object sender, EventArgs e)
        {
            LoadFrom = new Loading(this);
            LoadFrom.Show();
            Thread t = new Thread(() =>
            {
                this.Invoke(new Action(() =>
                {
                    msg("Start");
                }));
                Thread.Sleep(2000);
                this.Invoke(new Action(() =>
                {
                    msg("Init_page");
                }));
                Init_page();
                Thread.Sleep(2000);
                this.Invoke(new Action(() =>
                {
                    msg("1");
                }));

            });
            t.Start();
            //
        }



        TabPage WeighPage1; //1#地磅页面
        TabPage WeighPage2;//2#地磅页面

        TabPage ReportReservationPage; //预约订单页面
        TabPage ReportOncePage;//一次过磅订单页面
        TabPage ReportTwicePage;//二次过磅页面
        TabPage ReportOverPage; //结束过磅页面

        TabPage ManageDriverPage;//司机管理页面
        TabPage ManageVehiclePage;//车辆管理页面
        TabPage ManageCustomerPage;//客户管理页面
        TabPage ManageMaterialPage;//物料管理页面
       // weighControl weighScale1 = null;
       // weighControl weighScale2 = null;
        /// <summary>
        /// 初始化页面的参数
        /// </summary>

        public void Init_page()
        {
            WeighPage1 = new TabPage("北地磅"); 
            WeighPage2 = new TabPage("南地磅");
            ReportReservationPage = new TabPage("预约订单");
            ReportOncePage = new TabPage("一次过磅订单");
            ReportTwicePage = new TabPage("二次过磅订单");
            ReportOverPage = new TabPage("出库订单");
            ManageDriverPage = new TabPage("司机管理");
            ManageVehiclePage = new TabPage("车辆管理");
            ManageCustomerPage = new TabPage("用户管理");
            ManageMaterialPage = new TabPage("物料管理");

        }


        public void Init_Parmeter()
        {

        }



        public bool IsInTab(string text)
        {
            bool ret = false;
            foreach (TabPage page in MainTab.TabPages)
            {
                if (text.Equals(page.Text)){
                    ret = true;
                    break;
                }
            }
            return ret;
        }



        Scales Scale1;
        private void scale1Item_Click(object sender, EventArgs e)
        {

            if (WeighPage1 == null)
            {
                return;
            }
            if (!IsInTab(scale1Item.Text))
            {
                Scale1 = new Scales();
                Scale1.TopLevel = false;
                Scale1.Parent = this.WeighPage1;
                Scale1.Show();
                MainTab.TabPages.Add(WeighPage1);
            }
            MainTab.SelectedTab = WeighPage1;

        }


        Scales Scale2;
        private void scale2Item_Click(object sender, EventArgs e)
        {
            if (WeighPage2 == null)
            {
                return;
            }
            if (!IsInTab(scale2Item.Text))
            {
                Scale2 = new Scales();
                Scale2.TopLevel = false;
                Scale2.Parent = this.WeighPage2;
                Scale2.Show();
                MainTab.TabPages.Add(WeighPage2);
            }
            MainTab.SelectedTab = WeighPage2;
        }

        Report report;
        private void ReserveReportItem_Click(object sender, EventArgs e)
        {
            if (ReportReservationPage == null)
            {
                return;
                //reserverReport.Dock = DockStyle.Fill;
                //ReportReservationPage.Controls.Add(report);
            }
            if (!IsInTab(ReserveReportItem.Text))
            {
                report = new Report();
                report.TopLevel = false;
                report.Parent = this.ReportReservationPage;
                report.Show();
                MainTab.TabPages.Add(ReportReservationPage);
            }
            MainTab.SelectedTab = ReportReservationPage;


        }

        DriverInfo driverInfo;
        private void DriverItem_Click(object sender, EventArgs e)
        {
            if (ManageDriverPage == null)
            {
                return;
            }
            if (!IsInTab(ManageDriverPage.Text))
            {
                driverInfo = new DriverInfo();
                driverInfo.TopLevel = false;
                driverInfo.Parent = this.ManageDriverPage;
                driverInfo.Show();
                MainTab.TabPages.Add(ManageDriverPage);
            }
            MainTab.SelectedTab = ManageDriverPage;

        }

        private void MainWin_Shown(object sender, EventArgs e)
        {
           this.Hide();
        }
    }
}
