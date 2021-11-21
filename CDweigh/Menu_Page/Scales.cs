using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDweigh.Menu_Page
{
    public partial class Scales : Form
    {
        /// <summary>
        /// 登录
        /// </summary>
        public const int CMD_Login = 0;
        /// <summary>
        /// 登出
        /// </summary>
        public const int CMD_Logout = 1;
        /// <summary>
        /// 开始播放
        /// </summary>
        public const int CMD_StartPlay = 2;
        /// <summary>
        /// 停止播放
        /// </summary>
        public const int CMD_StopPlay = 3;
        /// <summary>
        /// 抓拍
        /// </summary>
        public const int CMD_Snap = 4;
        /// <summary>
        /// 抬杆
        /// </summary>
        public const int CMD_Rise = 5;
        /// <summary>
        /// 落杆
        /// </summary>
        public const int CMD_Decline = 6;

        public event ReturnIcCode SendIcCode; //定义事件
        public event Control_Camera control_Camera;//操作
        public event Control_LED control_led;

        MainWin main;
        weightBase Scale1;
        Thread InitThread;
        ScaleDeviceInfo deviceInfo=null;
        Picture_Position Camera1_Video = new Picture_Position();
        public double Scale_weight_RealTime = 0.0;
        public double Scale_weight_realTime_Last = 0.0;
        public uint Scale_weight_Timeout_Number = 0;
        public uint Scale_weight_Timeout_Maxium = 4;
        public bool Infrared_Alam = false; //红外报警标志位
        public bool ScaleStable_Alam = false;//地衡稳定报警标志位


        public bool Scales_Enable = false; //地衡启用标志
        public bool Infrared_Enable =false;//红外启用标志
        public bool ICReader_Enable = false;//IC读卡器启用标志
        public bool Camera1_Enable = false;//1#摄像头启用标志
        public bool Camera2_Enable = false;//2#摄像头启用标志
        public bool LEDScreen_Enable = false;//LED大屏启用标志
        public bool IndenReader_Enable = false;//身份证读卡器启用标志

        #region 窗体事件函数

        public Scales(MainWin main,ScaleDeviceInfo info)
        {
            this.main = main;
            deviceInfo = info;
            this.main.camera_Message += Camera_Message;
            this.main.iccode_broadcast += IC_Respone;
            InitializeComponent();
        }
        

        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scales_Load(object sender, EventArgs e)
        {
            try
            {
                if (Camera1_Enable)
                {
                    //初始化现场，用于注册摄像头
                    InitThread = new Thread(Init_Scale_Device);
                    InitThread.Start();
                }


                //初始化串口，绑定数据接收触发函数
                Scale1 = new weightBase(deviceInfo);
                if (Scales_Enable)
                {
                    Scale1.Scale.DataReceived += Scale_DataReceived;
                    Scale1.Scale.Open();
                }
                if (Infrared_Enable)
                {
                    Scale1.Infrared.DataReceived += Infrared_DataReceived;
                    Scale1.Infrared_Open();
                }
                UpdateLED(Helper.LedSDK.Idle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                 
            }


           
        }

        /// <summary>
        /// 初始化现场处理函数
        /// 委托调用注册、打开监控摄像头
        /// </summary>
        public void Init_Scale_Device()
        {
            control_Camera(CMD_Login, ref deviceInfo.camera);
            this.Invoke(new Action(() => {
                deviceInfo.camera.videoPtr = Camera1_play.Handle;
            }));
            control_Camera(CMD_StartPlay, ref deviceInfo.camera);
            this.BeginInvoke(new Action(() =>
            {
                Update_Video_point();
            }));
        }



        /// <summary>
        /// 窗体绘制触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scales_Paint(object sender, PaintEventArgs e)
        {
           // picCircle(Infrared_Dis);
          //   picCircle(Vary_Dis);
        }



        /// <summary>
        /// 开卡画面按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateBill_But_Click(object sender, EventArgs e)
        {
            CreateOrder order = new CreateOrder(this);
            order.ReturnIC += Order_ReturnIC;
            order.Show();
        }


        /// <summary>
        /// 窗口关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scales_FormClosing(object sender, FormClosingEventArgs e)
        {

            //注销摄像头资源
            if (deviceInfo.camera.PlayID > -1) control_Camera(CMD_StopPlay, ref deviceInfo.camera);
            if (deviceInfo.camera.UserID > -1) control_Camera(CMD_Logout, ref deviceInfo.camera);
            //注销串口资源
            if (Scale1.Infrared.IsOpen) Scale1.Infrared_Close();
            if (Scale1.Scale.IsOpen) Scale1.Scale_Close();

        }

        #endregion

        #region 自有函数

        /// <summary>
        /// 图片控件变圆
        /// </summary>
        /// <param name="pic"></param>
        public void picCircle(PictureBox pic)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pic.ClientRectangle);
            Region region = new Region(gp);
            pic.Region = region;
            gp.Dispose();
            region.Dispose();
        }


        /// <summary>
        /// 地衡画面更新重量信息
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="alm"></param>
        public void Scale_Update(double weight, bool alm)
        {
            if (alm)
            {
                this.BeginInvoke(new Action(() => {
                    weight_Dis.ForeColor = Color.Gray;
                }));
            }
            else
            {

                this.BeginInvoke(new Action(() => {
                    //更新界面以及当前重量
                    weight_Dis.Text = weight.ToString();
                    weight_Dis.ForeColor = Color.Green;
                    Scale_weight_RealTime = weight;
                }));
            }
        }

        public void Infrared_Update(bool alm)
        {
            Infrared_Alam = alm;
            this.BeginInvoke(new Action(() =>
            {
                if(Infrared_Alam) Infrared_Dis.BackColor = Color.Red;
                else Infrared_Dis.BackColor = Color.Green;
            }));
        }

        public void ScaleStable_Update(bool alm)
        {
            ScaleStable_Alam = alm;
            this.BeginInvoke(new Action(() =>
            {
                if (ScaleStable_Alam) Vary_Dis.BackColor = Color.Red;
                else Vary_Dis.BackColor = Color.Green;
            }));
        }


        #endregion




        #region 委托事件触发函数


        /// <summary>
        /// 开票
        /// </summary>
        /// <param name="IC"></param>
        private void Order_ReturnIC(string IC)
        {
            IcCode_text.Text = IC;
        }

        public void IC_Respone(string ic)
        {
            IcCode_text.Text = ic;
        }

        /// <summary>
        /// 红外对射数据回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Infrared_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int count = sp.BytesToRead;
            if (count < 0) return;
            string indata = sp.ReadExisting();
            Infrared_Update(Scale1.Infrared_recivce(indata));
        }

        /// <summary>
        /// 地磅数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scale_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            double realdata = 0.0;
            int count = sp.BytesToRead;
            if (count < 0) return;
            string indata = sp.ReadExisting();
            if (Scale1.GetWeight(indata, ref realdata))
            {
                //超时看门狗清零
                Scale_weight_Timeout_Number = 0;
              if( Math.Abs(  Scale_weight_realTime_Last- realdata) > 20)
                {
                    ScaleStable_Update(true);
                }else
                {
                    ScaleStable_Update(false);
                }
                Scale_Update(realdata, false);
                Scale_weight_realTime_Last = realdata;
            }
            else
            {
                //没有读取到数据计数
                Scale_weight_Timeout_Number++;
                //超过次数更新界面
                if (Scale_weight_Timeout_Number > Scale_weight_Timeout_Maxium)
                {
                    Scale_Update(0, true);
                }
            }
        }


        /// <summary>
        /// 主界面的摄像头报警数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="AlarmStr"></param>
        public void Camera_Message(int UserID, string AlarmStr)
        {
            if (UserID == deviceInfo.camera.UserID)
            {
                this.BeginInvoke(new Action(() =>
                {
                    CurrentLicense.Text = AlarmStr;
                }));

                try
                {
                    control_Camera(CMD_Rise, ref deviceInfo.camera);
                    UpdateLED(Helper.LedSDK.WaitIC);
                }
                catch (Exception ex)
                {
                    //TODO: Error handle
                    throw;
                }

            }
        }

        #endregion



        #region 摄像头实时影响窗口相关函数

        /// <summary>
        /// 实时图像双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Camera1_play_DoubleClick(object sender, EventArgs e)
        {
            if(Camera1_play.Width == Camera1_Video.local_Width)
            {
                Camera1_play.SetBounds(Camera1_Video.zoom_Point.X, Camera1_Video.zoom_Point.Y, Camera1_Video.zoom_Width, Camera1_Video.zoom_Height);
                Camera1_play.BringToFront();
            }
            else
            {
                Camera1_play.SetBounds(Camera1_Video.local_Point.X, Camera1_Video.local_Point.Y, Camera1_Video.local_Width, Camera1_Video.local_Height);
                Camera1_play.SendToBack();
            }
            
        }


        /// <summary>
        /// 更新摄像头窗体位置
        /// </summary>
        public void Update_Video_point()
        {
            Camera1_Video.local_Point = new Point(Camera1_play.Bounds.X, Camera1_play.Bounds.Y);
            Camera1_Video.local_Width = Camera1_play.Bounds.Width; Camera1_Video.local_Height = Camera1_play.Bounds.Height;
        }

        #endregion


        public void UpdateLED(int cmd, string msg = null)
        {
            if (control_led != null)
            {
                control_led(deviceInfo, cmd, msg);
            }
            
        }

    }

    class Picture_Position
    {
        public Point local_Point { get; set; } = new Point(990, 47);
        public int local_Width { get; set; } = 381;
        public int local_Height { get; set; } = 328;


        public Point zoom_Point { get; set; } = new Point(500, 47);
        public int zoom_Width { get; set; } = 800;
        public int zoom_Height { get; set; } = 800;
    }
}
