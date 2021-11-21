

using System;
using CDweigh.Helper;

namespace CDweigh
{
    public class DeviceConfig
    {
        public ScaleDeviceInfo weigh1;
        public SerialInfo IcReader;
        public IdentityInfo IndenReader;
        public DeviceConfig()
        {
            weigh1 = new ScaleDeviceInfo();
            IcReader = new SerialInfo();
            IndenReader = new IdentityInfo();
        }


    }

    /// <summary>
    /// 当个地磅所需设备数据
    /// </summary>
    public class ScaleDeviceInfo
    {
        public SerialInfo Scale ;
        public SerialInfo Infrared ;
        public CameraInfo camera ;
        public LEDInfo Led;
        public ScaleDeviceInfo()
        {
            Scale = new SerialInfo();
            Infrared = new SerialInfo();
            camera = new CameraInfo();
            Led = new LEDInfo();
        }
    }

    /// <summary>
    /// 摄像机参数
    /// </summary>
    public class CameraInfo
    {
        public bool IsEnable { get; set; } = false;
        public string IP { get; set; } = "192.168.2.11";
        public string Name { get; set; } = "camer1";
        public int Port { get; set; } = 8000;
        public string User { get; set; } = "admin";
        public string Paswd { get; set; } = "CDadmin123";
        public int UserID { get; set; } = -1;
        public int AlarmID { get; set; } = -1;
        public int PlayID { get; set; } = -1;
        public IntPtr videoPtr { get; set; } = IntPtr.Zero;
    }

    /// <summary>
    /// LED屏幕数据
    /// </summary>
    public class LEDInfo
    {
        public bool IsEnable { get; set; } = false;
        public int CardNum { get; set; } = 0;
        public string FontName { get; set; } = "宋体";
        public int FontSize { get; set; } = 12;
        public int FontColor { get; set; } = LedSDK.g_iRed;

        public int Width { get; set; } = 128;
        public int Height { get; set; } = 32;

        public string Idle_prompt { get; set; } = "地衡空闲";
        public string WaitVehicle_prompt { get; set; } = "车辆请上磅";
        public string WaitIc_prompt { get; set; } = "用户请刷卡";
        public string CheckIc_prompt { get; set; } = "验证信息";
        public string WaitWeigh_prompt { get; set; } = "正在称重";
        public string WeighDown_prompt { get; set; } = "称重完成，请下磅";


    }

    /// <summary>
    /// 串口参数设置
    /// </summary>
    public class SerialInfo
    {

        public bool IsEnable { get; set; } = false;
        /// <summary>
        /// 串口名称
        /// </summary>
        public string ComPort { get; set; } = "com1";
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// 校验方法
        /// </summary>
        public int Parity { get; set; } = 0;
        /// <summary>
        /// 数据大小
        /// </summary>
        public int DataBits { get; set; } = 8;
        /// <summary>
        /// 停止位
        /// </summary>
        public int StopBits { get; set; } = 1;
        /// <summary>
        /// 读取超时
        /// </summary>
        public int ReadTimeout { get; set; } = 500;
        /// <summary>
        /// 写入超时
        /// </summary>
        public int WriteTimeout { get; set; } = 500;
        /// <summary>
        /// 返回数据触发数据量
        /// </summary>
        public int ReceivedBytesThreshold { get; set; } = 5;
        /// <summary>
        /// 读取数据缓冲大小
        /// </summary>
        public int ReadBufferSize { get; set; } = 1024;

    }



    /// <summary>
    /// 身份证读卡器信息
    /// </summary>
    public class IdentityInfo
    {
        public bool IsEnable { get; set; } = false;

        /// <summary>
        ///通讯口，支持窗口以及USB 1=Com1 1001 =USB1
        /// </summary>
        public int Connect_Port { get; set; } = 1;
    }

}