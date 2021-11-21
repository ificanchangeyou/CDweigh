using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CDweigh.Helper
{
    public class CVR100SDK
    {

        public const int Init_OK = 1;
        public const int Init_Fail = 2;
        public const int Init_Fail_Unknow = -1;
        public const int Init_Fail_Dll = -2;

        public const int Close_OK = 1;
        public const int Close_Fail_Illegal = 0;
        public const int Close_Fail_IsClosed = -1;
        public const int Close_Fail_Dll = -2;

        public const int Auth_OK = 1;
        public const int Auth_Fail_Find = 2;
        public const int Auth_Fail_Select = 3;
        public const int Auth_Fail_Unlink = 4;
        public const int Auth_Fail_Dll = 0;



        /// <summary>
        /// 身份证阅读类
        ///// </summary>
        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_SetBaudRate", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_SetComBaudrate(int nBaudRate);//声明外部的标准动态库, 跟Win32API是一样的,设置波特率

        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_InitComm", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_InitComm(int Port);//声明外部的标准动态库, 跟Win32API是一样的


        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_Authenticate", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Authenticate();


        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_Read_Content", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Read_Content(int Active);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_Read_FPContent", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Read_FPContent();

        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_FindCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_FindCard();

        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_SelectCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_SelectCard();


        [DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_CloseComm", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_CloseComm();

        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetCertType", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern unsafe int GetCertType(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleName", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern unsafe int GetPeopleName(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleChineseName", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern unsafe int GetPeopleChineseName(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleNation", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetPeopleNation(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetNationCode", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetNationCode(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleBirthday", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleBirthday(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleAddress", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleAddress(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleIDCode", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleIDCode(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetDepartment", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDepartment(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetStartDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetStartDate(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetEndDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetEndDate(ref byte strTmp, ref int strLen);


        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetPeopleSex", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleSex(ref byte strTmp, ref int strLen);


        //[DllImport(@"Dll\Termb.dll", EntryPoint = "CVR_GetIDCardUID", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetIDCardUID(ref byte strTmp, int strLen);

        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetBMPData", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetBMPData(ref byte btBmp, ref int nLen);

        [DllImport(@"Dll\Termb.dll", EntryPoint = "GetJpgData", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetJpgData(ref byte btBmp, ref int nLen);

        [DllImport(@"Dll\Termb.dll", EntryPoint = "M1_MF_HL_Request", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int M1_MF_HL_Request(byte nMode, ref byte pSNR, ref byte pTagType);

        [DllImport(@"Dll\Termb.dll", EntryPoint = "M1_MF_HL_Read", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int M1_MF_HL_Read(byte nMode, uint nSNR, byte nBlock, ref byte nKey, ref byte pReadBuff, uint nBuff);

        [DllImport(@"Dll\Termb.dll", EntryPoint = "M1_MF_HL_Write", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int M1_MF_HL_Write(byte nMode, uint nSNR, byte nBlock, ref byte nKey, ref byte pWriteBuff, uint nBuff);
    }
}
