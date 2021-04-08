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
    public partial class MainWin : Form
    {
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

        //关闭按钮
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int CLOSE_SIZE = 10;
        private void MainTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            try

            {
                //Graphics g = e.Graphics;
                //Pen p = new Pen(Color.Blue);
                //Font font = new Font("Arial", 10.0f);
                //SolidBrush brush = new SolidBrush(Color.Red);

                //g.DrawRectangle(p, MainTab.GetTabRect(0));
                //g.DrawString("tabPage1", font, brush, (RectangleF)MainTab.GetTabRect(0));


                Rectangle myTabRect = this.MainTab.GetTabRect(e.Index);

                // Font font
                //先添加TabPage属性  
                //e.Graphics.DrawString(this.MainTab.TabPages[e.Index].Text , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);
                // Create image.
                Image newImage = Properties.Resources.setting;//(resources.GetObject("resource"));
                // Draw image to screen.
                e.Graphics.DrawImage(newImage, 0, 0, 30, 30);
                //再画一个矩形框
                using (Pen p = new Pen(Color.Black))//自动释放资源
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }

                //画关闭符号
                using (Pen objpen = new Pen(Color.Black))
                {
                    //"/"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(objpen, p1, p2);

                    //"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    e.Graphics.DrawLine(objpen, p3, p4);
                }
                e.Graphics.Dispose();


            }

            catch (Exception)
            {
            }
        }

        private void MainTab_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;

                int CLOSE_SIZE = 10;

                //计算关闭区域  
                Rectangle myTabRect = this.MainTab.GetTabRect(this.MainTab.SelectedIndex);

                myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                myTabRect.Width = CLOSE_SIZE;
                myTabRect.Height = CLOSE_SIZE;

                //如果鼠标在区域内就关闭选项卡  
                bool isClose = x > myTabRect.X && x < myTabRect.Right
                                && y > myTabRect.Y && y < myTabRect.Bottom;

                if (isClose == true)
                {
                    this.MainTab.TabPages.Remove(this.MainTab.SelectedTab);
                }
            }

        }
    }
}
