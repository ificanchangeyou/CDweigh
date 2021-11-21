using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CDweigh.Helper;

namespace CDweigh
{
    class weightBase
    {
        //Sclae Set
        public SerialPort Scale; //地磅串口对象
        public SerialPort Infrared; //红外串口对象
        ScaleDeviceInfo deviceinfo;//设备参数
        private int _userID = 0;
        private bool[] Infrared_Sum;
        private int _infrared_Sum;
        public int UserID { get { return _userID; }  }

        public weightBase(ScaleDeviceInfo info)
        {
            
            deviceinfo = info;
            //初始化地磅串口数据
            Scale = new SerialPort();
            Scale.PortName = deviceinfo.Scale.ComPort;
            Scale.BaudRate = deviceinfo.Scale.BaudRate;
            Scale.Parity = (Parity)deviceinfo.Scale.Parity;
            Scale.DataBits = deviceinfo.Scale.DataBits;
            Scale.StopBits = (StopBits)deviceinfo.Scale.StopBits;
            Scale.ReadTimeout = deviceinfo.Scale.ReadTimeout;
            Scale.WriteTimeout = deviceinfo.Scale.WriteTimeout;
            Scale.ReceivedBytesThreshold = deviceinfo.Scale.ReceivedBytesThreshold;
            Scale.ReadBufferSize = deviceinfo.Scale.ReadBufferSize;
            //初始化红外串口数据
            Infrared = new SerialPort();
            Infrared.PortName = deviceinfo.Infrared.ComPort;
            Infrared.BaudRate = deviceinfo.Infrared.BaudRate;
            Infrared.Parity = (Parity)deviceinfo.Infrared.Parity;
            Infrared.DataBits = deviceinfo.Infrared.DataBits;
            Infrared.StopBits = (StopBits)deviceinfo.Infrared.StopBits;
            Infrared.ReadTimeout = deviceinfo.Infrared.ReadTimeout;
            Infrared.WriteTimeout = deviceinfo.Infrared.WriteTimeout;
            Infrared.ReceivedBytesThreshold = deviceinfo.Infrared.ReceivedBytesThreshold;
            Infrared.ReadBufferSize = deviceinfo.Infrared.ReadBufferSize;
            //_infrared_Sum = deviceinfo.Infrared_Sum;
            // Infrared_Sum = new bool[_infrared_Sum];

            fontset = new LedSDK.User_FontSet();
            fontset.strFontName = deviceinfo.Led.FontName;
            fontset.colorFont = deviceinfo.Led.FontColor;
            fontset.iFontSize = deviceinfo.Led.FontSize;
            
        }
        LedSDK.User_FontSet fontset;


        public bool IsExist_Comport(string comport)
        {
            foreach (string  portname in SerialPort.GetPortNames())
            {
                if (portname.Equals(comport)) return  true;
            }        
            return false;
        }

        #region Weight 地衡函数

        /// <summary>
        /// 地衡串口打开
        /// </summary>
        /// <returns></returns>
        public bool Scale_Open()
        {
            bool ret = false;
            try
            {
                if (IsExist_Comport(Scale.PortName))
                {
                    if (!Scale.IsOpen)
                    {
                        Scale.Open();
                        ret = true;
                    }
                }
                else ret = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("串口打开错误", ex);
            }
            return ret;
        }

        public bool Scale_Close()
        {
            bool ret = false;
            if (Scale.IsOpen)
            {
                Scale.Close();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public bool GetWeight(string data,ref double weight)
        {
            bool ret = false;
            string[] strNum;
            
            char[] chr = { ' ' };
            string[] datas = data.Split(')');//按照）分割字符串
            if (datas.Length > 1) { strNum = datas[1].Split(chr, options: StringSplitOptions.RemoveEmptyEntries); }
            else
            {
                strNum = datas[0].Split(chr, options: StringSplitOptions.RemoveEmptyEntries);//省略结果中包含空字符串的数组元素
            }
            if (strNum.Length > 2)
            {
                ret = true;
                weight = int.Parse(strNum[1]);
                weight = weight / 1000; //Kg=>T
            }
            return ret;
        }
        #endregion

        #region Infrared Trigger 红外触发
        /// <summary>
        /// 红外串口打开
        /// </summary>
        /// <returns></returns>
        public bool Infrared_Open()
        {
            bool ret = false;
            try
            {

                if (!Infrared.IsOpen)
                {
                    Infrared.Open();
                    ret = true;
                }
                else ret = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return ret;
        }

        /// <summary>
        /// 红外串口关闭
        /// </summary>
        /// <returns></returns>
        public bool Infrared_Close()
        {
            bool ret = false;
            if (Infrared.IsOpen)
            {
                Infrared.Close();
            }
            return ret;
        }

        /// <summary>
        /// 红外发送查询语句
        /// </summary>
        /// <returns></returns>
        public bool Infrared_Send(ref SerialPort Infrared)
        {
            bool ret = false;
            if (Infrared!=null && Infrared.IsOpen)
            {
                byte[] send = { 1, 2, 0, 0, 0, 4, 121, 201 };
                Infrared.Write(send, 0, 8);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 红外返回数据处理
        /// </summary>
        /// <param name="indata"></param>
        /// <returns></returns>
        public bool Infrared_recivce(string indata)
        {
            bool ret = false;
            byte[] data = System.Text.Encoding.Default.GetBytes(indata);
            if (data.Length > 4)
            {
                if (data[0] == 1 && data[1] == 2 && data[2] == 1)
                {
                    byte Sum = data[3];
                    for (int i = 0; i < _infrared_Sum; i++)
                    {
                       Infrared_Sum[i] = ValueBybit(Sum, i);
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// byte拆分位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ValueBybit(byte data, int index)
        {
            if (index > 7 || index < 0) return false;
            return ((data >> index) & 1) == 1;
        }
        #endregion



    }



}
