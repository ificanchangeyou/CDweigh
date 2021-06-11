using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDweigh.Class
{
    public delegate void Senddata(double data);
    class Scales
    {
        public event Senddata sendWeigh;
        SerialPort Scale;
        public Scales(string Com,int baud,Parity parity,int databits,StopBits stop )
        {
            Scale = new SerialPort(Com, baud, parity, databits, stop);
            Scale.ReadTimeout = 500;
            Scale.ReceivedBytesThreshold = 17;
            Scale.ReadBufferSize = 1024;
            Scale.DataReceived += new SerialDataReceivedEventHandler(DataReceiveHandler);
        }

        /// <summary>
        /// 地衡串口数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceiveHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if (indata.Length > 1)
            {
                string[] strNum;
                char[] chr = { ' ' };
                string[] datas = indata.Split(')');
                if (datas.Length > 1) { strNum = datas[1].Split(chr, options: StringSplitOptions.RemoveEmptyEntries); }
                else
                {
                    strNum = datas[0].Split(chr, options: StringSplitOptions.RemoveEmptyEntries);
                }
                if (strNum.Length > 2)
                {
                  //  weight = int.Parse(strNum[1]);
                }
              }
        }

        /// <summary>
        /// 发送当前的重量
        /// </summary>
        /// <param name="weigh"></param>
        public void SendWeigh(double weigh)
        {
            if (sendWeigh != null) sendWeigh(weigh);
        }
    }
}
