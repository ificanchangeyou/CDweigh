using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDweigh
{
    class weightBase
    {
        //Sclae Set
        public SerialPort Scale;
        private string Scale_ComPort="COM1";
        private int Scale_Baund=9600;
        private int Scale_Parity=(int)Parity.None;
        private int Scale_DataBits = 8;
        private int Scale_StopBits = 1;
        private int Scale_ReadTimeout = 500;
        private int Scale_WriteTimeout = 500;
        //Camera set
        private string Camera1_IP ="192.168.0.1";
        private int Camera1_Port = 8000;
        private string camera1_User = "admin";
        private string camera1_Pswd = "CDadmin123";
        private string Camera2_IP = "192.168.0.1";
        private int Camera2_Port = 8000;
        private string camera2_User = "admin";
        private string camera2_Pswd = "CDadmin123";

        public weightBase(DeviceInfo info)
        {
        
        }

    }
}
