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
using CDweigh.Helper;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;
using System.IO.Ports;
using System.Diagnostics;
using CDweigh.Class;


namespace CDweigh
{
    public delegate void ReturnIcCode(string IC);
    public delegate void SendMsg(string msg);
    public delegate void Camera_Message(int UserID, string AlarmStr);
    public delegate bool Control_Camera(int Cmd,ref CameraInfo info);
    public delegate void Control_LED(ScaleDeviceInfo info , int cmd, string msg = null);
    public delegate void IcCode_Broadcast(string ic);
    public partial class MainWin : Form
    {
        public event SendMsg msg; //定义发送初始化信息事件
        public event Camera_Message camera_Message; //
        public event IcCode_Broadcast iccode_broadcast;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;


        SerialPort IcReader = new SerialPort();
        //登录画面
        Login login;


        //LED线程锁
        object locker = new object();
        public static int g_iGreen = 0xFF00; //绿色
        public static int g_iYellow = 0xFFFF; //黄色
        public static int g_iRed = 0x00FF; //红色


        #region Camera 摄像头部分

        //==============================================================
        //注册用户的回调委托
        public CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack = null;
        //异常消息回调委托
        private CHCNetSDK.EXCEPYIONCALLBACK m_fExceptionCB = null;
        //报警回调委托
        private CHCNetSDK.MSGCallBack_V31 m_falarmData_V31 = null;
        //老设备报警回调委托
        private CHCNetSDK.MSGCallBack m_falarmData = null;
        public delegate void UpdateTextStatusCallback(string strLogStatus, IntPtr lpDeviceInfo);
        public delegate void UpdateListBoxCallback(string strAlarmTime, string strDevIP, string strAlarmMsg);
        public delegate void UpdateListBoxCallbackException(string strAlarmTime, int lUserID, string strAlarmMsg);

        //视频流回调委托
        CHCNetSDK.REALDATACALLBACK RealData = null;

        //设备参数结构体。
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;

        CHCNetSDK.NET_VCA_TRAVERSE_PLANE m_struTraversePlane = new CHCNetSDK.NET_VCA_TRAVERSE_PLANE();
        CHCNetSDK.NET_VCA_AREA m_struVcaArea = new CHCNetSDK.NET_VCA_AREA();
        CHCNetSDK.NET_VCA_INTRUSION m_struIntrusion = new CHCNetSDK.NET_VCA_INTRUSION();
        CHCNetSDK.UNION_STATFRAME m_struStatFrame = new CHCNetSDK.UNION_STATFRAME();
        CHCNetSDK.UNION_STATTIME m_struStatTime = new CHCNetSDK.UNION_STATTIME();
        CHCNetSDK.NET_DVR_BARRIERGATE_CFG RemoteOpen = new CHCNetSDK.NET_DVR_BARRIERGATE_CFG();


        //摄像机名称与摄像机句柄字典
        Dictionary<string, int> CameraDict = new Dictionary<string, int>();
        //摄像机布防结构体
        CHCNetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
        /// <summary>
        /// 初始化摄像头
        /// </summary>
        public void init_camera()
        {
            bool m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                LogHelper.Error("初始化摄像头失败");
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            //保存SDK日志 To save the SDK log
            CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            //设置透传报警信息类型
            CHCNetSDK.NET_DVR_LOCAL_GENERAL_CFG struLocalCfg = new CHCNetSDK.NET_DVR_LOCAL_GENERAL_CFG();
            struLocalCfg.byAlarmJsonPictureSeparate = 1;//控制JSON透传报警数据和图片是否分离，0-不分离(COMM_VCA_ALARM返回)，1-分离（分离后走COMM_ISAPI_ALARM回调返回）
            Int32 nSize = Marshal.SizeOf(struLocalCfg);
            IntPtr ptrLocalCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(struLocalCfg, ptrLocalCfg, false);
            //配置本地设置
            if (!CHCNetSDK.NET_DVR_SetSDKLocalCfg(17, ptrLocalCfg))  //NET_DVR_LOCAL_CFG_TYPE_GENERAL 通用参数配置
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string strErr = "NET_DVR_SetSDKLocalCfg failed, error code= " + iLastErr;
                LogHelper.Error("[MainWindow.xmal]init_camera:" + strErr);
                MessageBox.Show(strErr);
            }
            Marshal.FreeHGlobal(ptrLocalCfg);

            //for (int i = 0; i < 200; i++)
            //{
            //    m_lAlarmHandle[i] = -1;
            //}
            //设置异常消息回调函数
            if (m_fExceptionCB == null)
            {
                m_fExceptionCB = new CHCNetSDK.EXCEPYIONCALLBACK(cbExceptionCB);
            }
            CHCNetSDK.NET_DVR_SetExceptionCallBack_V30(0, IntPtr.Zero, m_fExceptionCB, IntPtr.Zero);
            //设置报警回调函数
            if (m_falarmData_V31 == null)
            {
                m_falarmData_V31 = new CHCNetSDK.MSGCallBack_V31(MsgCallback_V31);
            }
            CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V31(m_falarmData_V31, IntPtr.Zero);

            //添加设备 
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            struAlarmParam.byLevel = 1; //0- 一级布防,1- 二级布防
            struAlarmParam.byAlarmInfoType = 1;//智能交通设备有效，新报警信息类型
            struAlarmParam.byFaceAlarmDetection = 1;//1-人脸侦测
        }

        /// <summary>
        /// 注销摄像机
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Logout_Camera(int UserID)
        {
            bool ret = false;
            if (UserID > -1)
            {
                ret= CHCNetSDK.NET_DVR_Logout(UserID);
                if (ret == false)
                {
                    string strErr = "NET_DVR_Logout failed, error code= " + CHCNetSDK.NET_DVR_GetLastError(); 
                    LogHelper.Error("Logout_Camera:" + strErr);
                }
            }
            return ret;
        }



        public bool Registered_Camera( ref CameraInfo info)
        {
            bool ret = false;
             
            //判断是否已登录
            if (info.UserID>-1)
            {
                MessageBox.Show("摄像机" + info.Name + "已经登录!");
            }
            else
            {
                if (info.IsEnable)
                {
                    try
                    {
                        if (Login_Device(ref info))
                        {
                            //添加对应字典，用于识别
                            CameraDict.Add(info.Name, info.UserID);
                            ret = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("登录摄像机失败", ex);
                        MessageBox.Show(ex.Message, "摄像机错误");
                    }
                }
            }
            return ret;
        }

        public bool unRegistered_Camera(ref CameraInfo info)
        {
            bool ret = false;
            if (info.IsEnable && info.UserID>-1)
            {
                try
                {
                    ret = CHCNetSDK.NET_DVR_Logout(info.UserID);
                    if (!ret)
                    {
                        string strErr = "NET_DVR_Logout failed, error code= " + CHCNetSDK.NET_DVR_GetLastError();
                        LogHelper.Error("Logout_Camera:" + strErr);
                    }else
                    {
                        info.UserID = -1;
                        if (CameraDict.ContainsKey(info.Name))
                        {
                            CameraDict.Remove(info.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("注销摄像机失败", ex);
                    MessageBox.Show(ex.Message, "摄像机错误");
                }
            }
            return ret;
        }

        /// <summary>
        /// 摄像机启动预览
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool StartRealPlay_Camera(ref CameraInfo info)
        {
            bool ret = false;
            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = info.videoPtr;//预览窗口
            lpPreviewInfo.lChannel = 1;//预te览的设备通道 0-主码流 1-子码流 2-三码流 3-虚拟码流
            lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 1; //播放库播放缓冲区最大缓冲帧数
            lpPreviewInfo.byProtoType = 0;
            lpPreviewInfo.byPreviewMode = 0;


            IntPtr pUser = new IntPtr();//用户数据

            //打开预览 Start live view 
            info.PlayID = CHCNetSDK.NET_DVR_RealPlay_V40(info.UserID, ref lpPreviewInfo, null/*RealData*/, pUser);
            if (info.PlayID < 0)
            {
                string strErr = "NET_DVR_RealPlay_V40 failed, error code= " + CHCNetSDK.NET_DVR_GetLastError();
                LogHelper.Error("Logout_Camera:" + strErr);
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        public bool StopRealPlay_Camera( ref CameraInfo info)
        {
            bool ret = false;
            //停止预览 Stop live view 
            if (!CHCNetSDK.NET_DVR_StopRealPlay(info.PlayID))
            {
                string strErr = "NET_DVR_StopRealPlay failed, error code= " + CHCNetSDK.NET_DVR_GetLastError();
                LogHelper.Error("Logout_Camera:" + strErr);
            }else
            {
                info.PlayID = -1;
                ret = true;
            }
            return ret;
        }


        public bool Destroy_Camera()
        {
            bool ret = false;
            ret = CHCNetSDK.NET_DVR_Cleanup();
            if (!ret)
            {
                string strErr = "NET_DVR_Logout failed, error code= " + CHCNetSDK.NET_DVR_GetLastError();
                LogHelper.Error("Logout_Camera:" + strErr);
            }
            return ret;
        }
        
        /// <summary>
        /// 登录摄像机
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Login_Device(ref CameraInfo info)
        {
            bool ret = false;
            int UserID = -1;
            //======================================格式化参数========================================================
            CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();
            //设备IP地址或者域名
            byte[] byIP = System.Text.Encoding.Default.GetBytes(info.IP);
            struLogInfo.sDeviceAddress = new byte[129];
            byIP.CopyTo(struLogInfo.sDeviceAddress, 0);
            //设备用户名
            byte[] byUserName = System.Text.Encoding.Default.GetBytes(info.User);
            struLogInfo.sUserName = new byte[64];
            byUserName.CopyTo(struLogInfo.sUserName, 0);
            //设备密码
            byte[] byPassword = System.Text.Encoding.Default.GetBytes(info.Paswd);
            struLogInfo.sPassword = new byte[64];
            byPassword.CopyTo(struLogInfo.sPassword, 0);
            //设备服务端口号
            struLogInfo.wPort = (ushort)info.Port;
            //=========================================================================================================


            struLogInfo.cbLoginResult = LoginCallBack;
            struLogInfo.bUseAsynLogin = false; //是否异步登录：0- 否，1- 是 
            struLogInfo.byLoginMode = 0; //0-Private, 1-ISAPI, 2-自适应
            struLogInfo.byHttps = 0; //0-不适用tls，1-使用tls 2-自适应
            /*
            if ((struLogInfo.bUseAsynLogin == true) && (LoginCallBack == null))
            {
                LoginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack);//注册回调函数                    
            }
            */

            //设备信息
            CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();
            //登录设备 Login the device
            UserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
            if (UserID < 0)
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string strErr = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code
                LogHelper.Error("Login_Devices:" + strErr);
                MessageBox.Show("设备登录失败，错误代码：" + strErr, "摄像机错误");
                //TODO Alarm
            }else
            {
                info.UserID = UserID;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 布防摄像头
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Deploy_Devices(ref CameraInfo info)
        {
            bool ret = false;
            if (info.IsEnable && info.UserID > -1 && info.AlarmID==-1)
            {
                int backinfo = CHCNetSDK.NET_DVR_SetupAlarmChan_V41(info.UserID, ref struAlarmParam);
                if (backinfo < 0)
                {
                    LogHelper.Error(info + "布防失败,错误码:" + CHCNetSDK.NET_DVR_GetLastError());
                }else
                {
                    info.AlarmID = backinfo;
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
        /// 摄像头撤销布防
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool unDeploy_Camera(ref CameraInfo info)
        {
            bool ret = false;
            if (info.IsEnable && info.UserID > -1 && info.AlarmID > -1)
            {
                ret = CHCNetSDK.NET_DVR_CloseAlarmChan_V30(info.AlarmID);
                if (!ret)
                {
                    uint ErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                    LogHelper.Error("撤销布防失败，错误代码：" + ErrorCode);
                    MessageBox.Show("撤销布防失败，错误代码：" + ErrorCode, "摄像头错误");
                }
            }

            return ret;
        }

        private void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 报警消息回调函数
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        private bool MsgCallback_V31(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //camera_Message
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            AlarmMessageHandle(lCommand, ref pAlarmer, pAlarmInfo, dwBufLen, pUser);

            return true; //回调函数需要有返回，表示正常接收到数据
        }

        /// <summary>
        /// 异常消息回调函数
        /// </summary>
        /// <param name="dwType"></param>
        /// <param name="lUserID"></param>
        /// <param name="lHandle"></param>
        /// <param name="pUser"></param>
        private void cbExceptionCB(uint dwType, int lUserID, int lHandle, IntPtr pUser)
        {
            //异常消息信息类型
            string stringAlarm = "异常消息回调，信息类型：0x" + Convert.ToString(dwType, 16) + ", lUserID:" + lUserID + ", lHandle:" + lHandle;
            LogHelper.Warn(stringAlarm);
        }


        #region 摄像头报警处理
        /// <summary>
        /// 摄像头上传事件处理函数
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        public void AlarmMessageHandle(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ALARM: //(DS-8000老设备)移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    //ProcessCommAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_V30://移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    //ProcessCommAlarm_V30(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_RULE://进出区域、入侵、徘徊、人员聚集等行为分析报警信息
                    //ProcessCommAlarm_RULE(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_PLATE_RESULT://交通抓拍结果上传(老报警信息类型)
                    //ProcessCommAlarm_Plate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ITS_PLATE_RESULT://交通抓拍结果上传(新报警信息类型)
                    ProcessCommAlarm_ITSPlate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_TPS_REAL_TIME://交通抓拍结果上传(新报警信息类型)
                    ProcessCommAlarm_TPSRealInfo(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_TPS_STATISTICS://交通抓拍结果上传(新报警信息类型)
                    ProcessCommAlarm_TPSStatInfo(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_PDC://客流量统计报警信息
                    //ProcessCommAlarm_PDC(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ITS_PARK_VEHICLE://客流量统计报警信息
                    //ProcessCommAlarm_PARK(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_DIAGNOSIS_UPLOAD://VQD报警信息
                    //ProcessCommAlarm_VQD(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_FACESNAP_RESULT://人脸抓拍结果信息
                                                           // ProcessCommAlarm_FaceSnap(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_SNAP_MATCH_ALARM://人脸比对结果信息
                    //ProcessCommAlarm_FaceMatch(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_FACE_DETECTION://人脸侦测报警信息
                    //ProcessCommAlarm_FaceDetect(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARMHOST_CID_ALARM://报警主机CID报警上传
                    //ProcessCommAlarm_CIDAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_VIDEO_INTERCOM_EVENT://可视对讲事件记录信息
                    //ProcessCommAlarm_InterComEvent(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_ACS://门禁主机报警上传
                    //ProcessCommAlarm_AcsAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ID_INFO_ALARM://身份证刷卡信息上传
                    //ProcessCommAlarm_IDInfoAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_AIOP_VIDEO://设备支持AI开放平台接入，上传视频检测数据
                    //ProcessCommAlarm_AIOPVideo(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_AIOP_PICTURE://设备支持AI开放平台接入，上传图片检测数据
                    //ProcessCommAlarm_AIOPPicture(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ISAPI_ALARM://ISAPI报警信息上传
                    //ProcessCommAlarm_ISAPIAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    {
                        //报警设备IP地址
                        string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');

                        //报警信息类型
                        string stringAlarm = "报警上传，信息类型：0x" + Convert.ToString(lCommand, 16);
                        LogHelper.Warn("[IP=" + strIP + "] Alarm: " + stringAlarm);
                    }
                    break;
            }
        }

        /// <summary>
        /// 抓拍车牌回调函数
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void ProcessCommAlarm_ITSPlate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_ITS_PLATE_RESULT struITSPlateResult = new CHCNetSDK.NET_ITS_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struITSPlateResult);

            struITSPlateResult = (CHCNetSDK.NET_ITS_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_ITS_PLATE_RESULT));

            //保存抓拍图片
            /*
            for (int i = 0; i < struITSPlateResult.dwPicNum; i++)
            {
                if (struITSPlateResult.struPicInfo[i].dwDataLen != 0)
                {
                    string str = "D:\\picture\\ITS_UserID_[" + pAlarmer.lUserID + "]_Pictype_" + struITSPlateResult.struPicInfo[i].byType
                        + "_PicNum[" + (i + 1) + "]_" + iFileNumber + ".jpg";
                    FileStream fs = new FileStream(str, FileMode.Create);
                    int iLen = (int)struITSPlateResult.struPicInfo[i].dwDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(struITSPlateResult.struPicInfo[i].pBuffer, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                    iFileNumber++;
                }
            }*/
            //报警设备IP地址
            string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');

            //抓拍时间：年月日时分秒
            string strTimeYear = string.Format("{0:D4}", struITSPlateResult.struSnapFirstPicTime.wYear) +
                string.Format("{0:D2}", struITSPlateResult.struSnapFirstPicTime.byMonth) +
                string.Format("{0:D2}", struITSPlateResult.struSnapFirstPicTime.byDay) + " "
                + string.Format("{0:D2}", struITSPlateResult.struSnapFirstPicTime.byHour) + ":"
                + string.Format("{0:D2}", struITSPlateResult.struSnapFirstPicTime.byMinute) + ":"
                + string.Format("{0:D2}", struITSPlateResult.struSnapFirstPicTime.bySecond) + ":"
                + string.Format("{0:D3}", struITSPlateResult.struSnapFirstPicTime.wMilliSec);

            //上传结果
            string stringPlateLicense = System.Text.Encoding.GetEncoding("GBK").GetString(struITSPlateResult.struPlateInfo.sLicense).TrimEnd('\0');
            if (stringPlateLicense.Length > 6) { stringPlateLicense = stringPlateLicense.Substring(1); }
            string stringAlarm = "抓拍上传，" + "车牌：" + stringPlateLicense + "，车辆序号：" + struITSPlateResult.struVehicleInfo.dwIndex;
            int ID = pAlarmer.lUserID;
            LogHelper.Info("[MainWindow.xmal]ProcessCommAlarm_ITSPlate:" + stringAlarm);
            //发送车牌信息
            camera_Message(ID, stringPlateLicense);
          //  this.Dispatcher.Invoke(new Action(() => { Judge_car(stringPlateLicense, ID); }));
            //if (stringPlateLicense == "绿津ADE9061") { RemoteControl(2, 1); }


           // this.Dispatcher.BeginInvoke(new Action(() => { UpdateCameraList(DateTime.Now.ToString(), strIP, stringAlarm); }));




        }

        private void ProcessCommAlarm_TPSRealInfo(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_TPS_REAL_TIME_INFO struTPSInfo = new CHCNetSDK.NET_DVR_TPS_REAL_TIME_INFO();
            uint dwSize = (uint)Marshal.SizeOf(struTPSInfo);

            struTPSInfo = (CHCNetSDK.NET_DVR_TPS_REAL_TIME_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_TPS_REAL_TIME_INFO));

            //报警设备IP地址
            string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');

            //抓拍时间：年月日时分秒
            string strTimeYear = string.Format("{0:D4}", struTPSInfo.struTime.wYear) +
                string.Format("{0:D2}", struTPSInfo.struTime.byMonth) +
                string.Format("{0:D2}", struTPSInfo.struTime.byDay) + " "
                + string.Format("{0:D2}", struTPSInfo.struTime.byHour) + ":"
                + string.Format("{0:D2}", struTPSInfo.struTime.byMinute) + ":"
                + string.Format("{0:D2}", struTPSInfo.struTime.bySecond) + ":"
                + string.Format("{0:D3}", struTPSInfo.struTime.wMilliSec);

            //上传结果
            string stringAlarm = "TPS实时过车数据，" + "通道号：" + struTPSInfo.dwChan +
                "，设备ID：" + struTPSInfo.struTPSRealTimeInfo.wDeviceID +
                "，开始码：" + struTPSInfo.struTPSRealTimeInfo.byStart +
                "，命令号：" + struTPSInfo.struTPSRealTimeInfo.byCMD +
                "，对应车道：" + struTPSInfo.struTPSRealTimeInfo.byLane +
                "，对应车速：" + struTPSInfo.struTPSRealTimeInfo.bySpeed +
                "，byLaneState：" + struTPSInfo.struTPSRealTimeInfo.byLaneState +
                "，byQueueLen：" + struTPSInfo.struTPSRealTimeInfo.byQueueLen +
                "，wLoopState：" + struTPSInfo.struTPSRealTimeInfo.wLoopState +
                "，wStateMask：" + struTPSInfo.struTPSRealTimeInfo.wStateMask +
                "，dwDownwardFlow：" + struTPSInfo.struTPSRealTimeInfo.dwDownwardFlow +
                "，dwUpwardFlow：" + struTPSInfo.struTPSRealTimeInfo.dwUpwardFlow +
                "，byJamLevel：" + struTPSInfo.struTPSRealTimeInfo.byJamLevel;

            LogHelper.Warn("[IP=" + strIP + "] Alarm: " + stringAlarm);
        }

        private void ProcessCommAlarm_TPSStatInfo(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_TPS_STATISTICS_INFO struTPSStatInfo = new CHCNetSDK.NET_DVR_TPS_STATISTICS_INFO();
            uint dwSize = (uint)Marshal.SizeOf(struTPSStatInfo);

            struTPSStatInfo = (CHCNetSDK.NET_DVR_TPS_STATISTICS_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_TPS_STATISTICS_INFO));

            //报警设备IP地址
            string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');

            //抓拍时间：年月日时分秒
            string strTimeYear = string.Format("{0:D4}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.wYear) +
                string.Format("{0:D2}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.byMonth) +
                string.Format("{0:D2}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.byDay) + " "
                + string.Format("{0:D2}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.byHour) + ":"
                + string.Format("{0:D2}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.byMinute) + ":"
                + string.Format("{0:D2}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.bySecond) + ":"
                + string.Format("{0:D3}", struTPSStatInfo.struTPSStatisticsInfo.struStartTime.wMilliSec);

            //上传结果
            string stringAlarm = "TPS统计过车数据，" + "通道号：" + struTPSStatInfo.dwChan +
                "，开始码：" + struTPSStatInfo.struTPSStatisticsInfo.byStart +
                "，命令号：" + struTPSStatInfo.struTPSStatisticsInfo.byCMD +
                "，统计开始时间：" + strTimeYear +
                "，统计时间(秒)：" + struTPSStatInfo.struTPSStatisticsInfo.dwSamplePeriod;


            for (int i = 0; i < CHCNetSDK.MAX_TPS_RULE; i++)
            {
                stringAlarm = stringAlarm + "车道号: " + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].byLane +
                    "，车道过车平均速度:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].bySpeed +
                    "，小型车数量:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].dwLightVehicle +
                    "，中型车数量:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].dwMidVehicle +
                    "，重型车数量:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].dwHeavyVehicle +
                    "，车头时距:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].dwTimeHeadway +
                    "，车头间距:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].dwSpaceHeadway +
                    "，空间占有率:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].fSpaceOccupyRation +
                    "，时间占有率:" + struTPSStatInfo.struTPSStatisticsInfo.struLaneParam[i].fTimeOccupyRation;
            }

            LogHelper.Warn("[IP=" + strIP + "] Alarm: " + stringAlarm);

        }



        #endregion


        /// <summary>
        /// 关闭道闸
        /// </summary>
        const byte Camera_Barrier_Close=0;
        /// <summary>
        /// 打开道闸
        /// </summary>
        const byte Camera_Barrier_Open = 1;
        /// <summary>
        /// 停止道闸
        /// </summary>
        const byte Camera_Barrier_Stop = 2;
        /// <summary>
        /// 锁定道闸
        /// </summary>
        const byte Camera_Barrier_Lock = 3;
        /// <summary>
        /// 解锁道闸
        /// </summary>
        const byte Camera_Barrier_unLock = 4;
        /// <summary>
        /// 道闸控制
        /// </summary>
        /// <param name="userID"></param>摄像头地址
        /// <param name="Command">0- 关闭道闸，1- 开启道闸，2- 停止道闸，3- 锁定道闸，4- 解锁道闸 </param>控制命令
        /// <returns></returns>
        public bool RemoteControl(CameraInfo info, byte Command)
        {
            bool flage = false;
            RemoteOpen.byRes = new byte[12];
            RemoteOpen.dwChannel = 1;
            RemoteOpen.byLaneNo = 1;
            RemoteOpen.byBarrierGateCtrl = Command;
            RemoteOpen.byEntranceNo = 1;
            RemoteOpen.byUnlock = 1;
            Int32 nSize = 2 * sizeof(uint) + 16 * sizeof(byte);
            RemoteOpen.dwSize = (uint)nSize;
            nSize = nSize + sizeof(long) + sizeof(uint);
            IntPtr ptrLocalCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(RemoteOpen, ptrLocalCfg, false);
            flage = CHCNetSDK.NET_DVR_RemoteControl(info.UserID, CHCNetSDK.NET_DVR_BARRIERGATE_CTRL, ptrLocalCfg, (uint)nSize);
            LogHelper.Info("RemoteControl: " + Command.ToString());
            return flage;

        }


        public bool camera_snap(CameraInfo info)
        {
            bool ret = false;
            try
            {
                CHCNetSDK.NET_DVR_SNAPCFG CFG = new CHCNetSDK.NET_DVR_SNAPCFG();
                CFG.byRelatedDriveWay = 0;
                CFG.bySnapTimes = 1;
                CFG.wSnapWaitTime = 200;
                CFG.dwSize = (uint)Marshal.SizeOf(CFG);
                Int32 nSize = Marshal.SizeOf(CFG);
                IntPtr ptrLocalCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(CFG, ptrLocalCfg, false);
                if (!CHCNetSDK.NET_DVR_ContinuousShoot(info.UserID, ptrLocalCfg))
                {
                    LogHelper.Error("camera_snap Error:" + CHCNetSDK.NET_DVR_GetLastError());
                }
                else ret = true;

                Marshal.FreeHGlobal(ptrLocalCfg);
            }
            catch (Exception ex)
            {
                LogHelper.Error("camera_snap Error:" ,ex);

            }
            return ret;
        }


        #endregion

        //============================UI控件调用部分==========================================================================
        public MainWin()
        {

            InitializeComponent();

        }
        Loading LoadFrom;
        //加载画面 
        private void MainWin_Load(object sender, EventArgs e)
        {
            Global.Sqldatabase = Properties.Settings.Default.SqlConnStr;
            /*
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
            */
            log4net.Config.XmlConfigurator.Configure();
            string strSql = "SELECT Count(*) FROM Users";
            try
            {
                string count = SqlHelper.QueryScalar(Global.Sqldatabase, strSql).ToString();
                if(Convert.ToInt32(count)==0)
                {
                    UserAdd add = new UserAdd(10);
                    add.ShowDialog();
                    if(add.DialogResult==DialogResult.OK)
                    {
                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            login = new Login();
            login.ShowDialog();
            if (login.DialogResult != DialogResult.OK) Application.Exit();
            
            //检查配置文件同时导入配置
            if (!Censor_Config()) Application.Exit();
            //LED_Init();
            //初始化摄像头
            //init_camera();
            //IcReader_Init();
       
            //
        }
        //关闭按钮
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            if(IcReader!=null)
            {
                if (IcReader.IsOpen) IcReader.Close();
            }
            if(info.weigh1.camera.PlayID>-1)
            {
                StopRealPlay_Camera(ref info.weigh1.camera);
            }
            if(info.weigh1.camera.AlarmID>-1)
            {
                unDeploy_Camera(ref info.weigh1.camera);
            }
            if(info.weigh1.camera.UserID>-1)
            {
                Logout_Camera(info.weigh1.camera.UserID);
            }
            Destroy_Camera();
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

        private void HeadPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }



        /// <summary>
        /// 配置信息读取
        /// </summary>
        /// <returns></returns>
        private bool Censor_Config()
        {
            bool ret = false;
            string Jsonfile = Application.StartupPath + "\\config.json";
            try
            {
                //检查文件是否存在
                if (!File.Exists(Jsonfile))
                {
                    //文件不存在创建文件
                    info = new DeviceConfig();
                    string json = JsonConvert.SerializeObject(info);
                    File.WriteAllText(Jsonfile, ConvertJsonString(json), Encoding.UTF8);
                    MessageBox.Show("未找到配置文件,请修改配置文件(系统已自动创建)","配置文件错误");
                }
                else
                {
                    using (StreamReader read = File.OpenText(Jsonfile))
                    {
                        string configData = read.ReadToEnd();
                        if (configData.Length > 0)
                        {
                            info = JsonConvert.DeserializeObject<DeviceConfig>(configData);
                            ret = true;
                        }else
                        {
                            MessageBox.Show("配置文件Config.json中没有配置");
                        }
                        read.Close();
                    }
 
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("检查配置文件错误", ex);
                MessageBox.Show(ex.Message, "检查配置文件错误");
            }
            return ret;
        }

        // 需要导入Newtonsoft.Json，这里使用的版本是4.5
        private string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
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

        /// <summary>
        /// 系统消息获取
        /// </summary>
        /// <param name="m"></param>
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


        //===========================================================================================================//
        DeviceConfig info;

        //=======================================页面点击函数部分==========================================================//
        /// <summary>
        /// 用户菜单点击相应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {

                case "登录":
                    Login login = new Login();
                    login.Show();
                    break;
                case "新增":
                    UserAdd useradd = new UserAdd();
                    useradd.Show();
                    break;
                case "修改":
                    ChangePaswd paswd = new ChangePaswd();
                    paswd.Show();
                    break;
                case "删除":
                    Logout logout = new Logout();
                    logout.Show();
                    break;
                case "权限":
                    UsersAuthority auth = new UsersAuthority();
                    auth.Show();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 地磅界面点击处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scaleItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "南地磅":
                    Scales scale = new Scales(this, info.weigh1);
                    scale.TopLevel = false;
                    scale.control_Camera += Scale_control_Camera;
                    scale.control_led += Scale_control_led;
                    page.Controls.Add(scale);
                    MainTab.TabPages.Add(page);
                    scale.Show();
                    break;
                case "北地磅":
                    //Scales scale2 = new Scales(this, info.weigh2);
                    //scale2.TopLevel = false;
                    //scale2.control_Camera += Scale_control_Camera;
                    //scale2.control_led += Scale_control_led;
                    //page.Controls.Add(scale2);
                    //MainTab.TabPages.Add(page);
                    //scale2.Show();
                    //break;
                default:
                    break;
            }

        }


        /// <summary>
        /// 报表界面点击处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "预约订单":
                    Report Booking = new Report(0);
                    Booking.TopLevel = false;
                    page.Controls.Add(Booking);
                   
                    MainTab.TabPages.Add(page);
                    Booking.Show();
                    break;
                case "等待一次过磅订单":
                    Report waitOnce = new Report(2);
                    waitOnce.TopLevel = false;
                    page.Controls.Add(waitOnce);
                    MainTab.TabPages.Add(page);
                    waitOnce.Show();
                    break;
                case "等待二次过磅订单":
                    Report waittwice = new Report(3);
                    waittwice.TopLevel = false;
                    page.Controls.Add(waittwice);
                    MainTab.TabPages.Add(page);
                    waittwice.Show();
                    break;
                case "结束订单":
                    Report EndOrder = new Report(3);
                    EndOrder.TopLevel = false;
                    page.Controls.Add(EndOrder);
                    MainTab.TabPages.Add(page);
                    EndOrder.Show();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 信息管理界面点击处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoMangerItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "司机信息":
                    DriverInfo driver = new DriverInfo();
                    driver.TopLevel = false;
                    page.Controls.Add(driver);

                    MainTab.TabPages.Add(page);
                    driver.Show();
                    break;
                case "车辆管理":
                    VehicleInfo vehicle = new VehicleInfo();
                    vehicle.TopLevel = false;
                    page.Controls.Add(vehicle);
                    MainTab.TabPages.Add(page);
                    vehicle.Show();
                    break;
                case "客户管理":

                    break;
                case "物料管理":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 日志界面点击处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "操作日志":
                    OperationLog operation = new OperationLog();
                    operation.TopLevel = false;
                    page.Controls.Add(operation);
                    MainTab.TabPages.Add(page);
                    operation.Show();
                    break;
                case "报警日志":
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 功能界面点击处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void functionMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "系统设置":
                    break;
                case "数据维护":
                    break;
                case "摄像道闸":
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 帮助界面处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helperMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string pageText = e.ClickedItem.Text;//获取需要打开的页面的名称
            if (IsInTab(pageText)) return;//如果存在相同的名称的已打开页面则退出
            TabPage page = new TabPage(pageText);  //新建页面
                                                   //根据不同的页面名称绑定数据以及画面
            switch (pageText)
            {
                case "帮助文档":
                    break;
                case "错误处理":
                    break;
                case "技术支持":
                    break;
                case "关于软件":
                    break;
                default:
                    break;
            }
        }


        //===================================委托事件调用函数==========================================================//
        /// <summary>
        /// 摄像头处理函数
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool Scale_control_Camera(int Cmd, ref CameraInfo info)
        {
            bool ret = false;
            switch (Cmd)
            {
                case Scales.CMD_Login:
                    if (Registered_Camera(ref info))
                    {
                        ret = Deploy_Devices(ref info);
                    }
                    break;
                case Scales.CMD_Logout:
                    if (unDeploy_Camera(ref info))
                    {
                        ret = unRegistered_Camera(ref info);
                    }
                    break;
                case Scales.CMD_StartPlay:
                    ret = StartRealPlay_Camera(ref info);
                    break;
                case Scales.CMD_StopPlay:
                    ret = StopRealPlay_Camera(ref info);
                    break;
                case Scales.CMD_Snap:
                    ret = camera_snap(info);
                    break;
                case Scales.CMD_Rise:
                    ret = RemoteControl(info, Camera_Barrier_Open);
                    break;
                case Scales.CMD_Decline:
                    ret = RemoteControl(info, Camera_Barrier_Close);
                    break;
                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// LED大屏处理函数
        /// LED不能多线程应用，必须增加线程锁
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cmd"></param>
        /// <param name="msg"></param>
        private void Scale_control_led(ScaleDeviceInfo info, int cmd, string msg = null)
        {
           // lock (locker)
            {
                switch (cmd)
                {
                    case LedSDK.Idle:
                        LED_Idle(info);
                        break;
                    case LedSDK.WaitVehicle:
                        LED_WaitVehicle(msg, info);
                        break;
                    case LedSDK.WaitIC:
                        LED_WaitIC(info);
                        break;
                    case LedSDK.CheckIC:
                        LED_CheckIC(info);
                        break;
                    case LedSDK.WaitWeigh:
                        LED_WaitWeigh(info);
                        break;
                    case LedSDK.WeighDown:
                        LED_WeighDown(info);
                        break;
                    case LedSDK.UpdateWeigh:
                        LED_UpdateWeigh(msg, info);
                        break;
                    default:
                        break;
                }
            }

        }


        //=============================================================================================================//
        /// <summary>
        /// 页面是否打查询函数
        /// </summary>
        /// <param name="page_name"></param>
        /// <returns></returns>
        private bool IsInTab(string page_name)
        {
            bool ret = false;
            foreach (TabPage page in MainTab.TabPages)
            {
                if (page.Text.Equals(page_name)) ret = true;
            }
            return ret;
        }




        #region LED显示屏相关函数

        LedSDK.User_FontSet fontset;



        public void LED_Init()
        {
            if (info.weigh1.Led.IsEnable)
            {
                Thread ledthread = new Thread(new ThreadStart(new Action(() =>
                {
                    Debug.Print("LEDInit Start "+ DateTime.Now.ToShortTimeString());
                    string path = Application.StartupPath + @"\Dll\";
                    //LedSDK.User_ReloadIniFile(path);
                    fontset.bFontBold = false;
                    fontset.bFontItaic = false;
                    fontset.bFontUnderline = false;
                    fontset.colorFont = g_iRed;
                    fontset.iFontSize = 12;
                    fontset.strFontName = "黑体";
                    fontset.iAlignStyle = 0;
                    fontset.iVAlignerStyle = 0;
                    fontset.iRowSpace = 2;
                    //if (LedSDK.User_OpenScreen(info.weigh1.Led.CardNum))
                    {
                        LED_ClearScreen(info.weigh1.Led.CardNum);
                        LedSDK.User_AdjustTime(info.weigh1.Led.CardNum);
                        LED_SetLight(info.weigh1.Led.CardNum, 10);
                        LED_Loading(info.weigh1);
                    }

                    Debug.Print("LEDInit End  "+ DateTime.Now.ToShortTimeString());
                })));

                ledthread.Start();
                Debug.Print("LEDInit Finish  "+ DateTime.Now.ToShortTimeString());
            }
        }


        public  bool LED_SendRealText(int x, int y, int iWidth, int iHeight, string sendText, int cardnum)
        {
            bool ret = false;

            //try
            //{
                
            //    //Connect
            //    if (LedSDK.User_RealtimeConnect(cardnum))
            //    {
            //        //Clear Screen
            //        //Send Text
            //        if (LedSDK.User_RealtimeSendText(cardnum, x, y, iWidth, iHeight, sendText, ref fontset))
            //        {
            //            ret = true;
            //        }
            //        //DisConnect
            //        if (!LedSDK.User_RealtimeDisConnect(cardnum)) ret = false;
            //    }
                
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            return ret;

        }


        public bool LED_ClearScreen(int cardnum)
        {
            bool ret = false;
            try
            {

                //Connect
                if (LedSDK.User_RealtimeConnect(cardnum))
                {
                    //Clear Screen
                    //Send Text
                    if (LedSDK.User_RealtimeScreenClear(cardnum))
                    {
                        ret = true;
                    }
                    //DisConnect
                    if (!LedSDK.User_RealtimeDisConnect(cardnum)) ret = false;
                }

            }
            catch (Exception ex)
            {
                //TODO: error log
                throw;
            }
            return ret;
        }

        public bool LED_SetLight(int cardnum,int light)
        {
            bool ret = false;
            if (light > 15) light = 15;
            if (light < 0) light = 0;
            try
            {

                //Connect
                if (LedSDK.User_RealtimeConnect(cardnum))
                {
                    //Clear Screen
                    //Send Text
                    if (LedSDK.User_SetScreenLight(cardnum, light)) 
                    {
                        ret = true;
                    }
                    //DisConnect
                    if (!LedSDK.User_RealtimeDisConnect(cardnum)) ret = false;
                }

            }
            catch (Exception ex)
            {
                //TODO: error log
                throw;
            }
            return ret;
        }



        /// <summary>
        /// LED空闲状态
        /// </summary>
        /// <returns></returns>
        public  bool LED_Idle(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.Idle_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "", info.Led.CardNum);
            return ret;
        }

        /// <summary>
        /// LED等待车辆上磅状态
        /// </summary>
        /// <param name="Vehicle"></param>
        /// <returns></returns>
        public bool LED_WaitVehicle(string Vehicle, ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.WaitVehicle_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, Vehicle, info.Led.CardNum);
            return ret;
        }

        /// <summary>
        /// 等待刷卡
        /// </summary>
        /// <returns></returns>
        public bool LED_WaitIC(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.WaitIc_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "", info.Led.CardNum);
            return ret;
        }

        /// <summary>
        /// 检查IC信息
        /// </summary>
        /// <returns></returns>
        public bool LED_CheckIC(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.CheckIc_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "", info.Led.CardNum);
            return ret;
        }


        /// <summary>
        /// 等待称重
        /// </summary>
        /// <returns></returns>
        public bool LED_WaitWeigh(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.WaitWeigh_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "", info.Led.CardNum);
            return ret;
        }


        /// <summary>
        /// 称重完成
        /// </summary>
        /// <returns></returns>
        public bool LED_WeighDown(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, info.Led.WeighDown_prompt, info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "", info.Led.CardNum);
            return ret;
        }

        /// <summary>
        /// 实时更新重量
        /// </summary>
        /// <param name="weigh"></param>
        /// <returns></returns>
        public bool LED_UpdateWeigh(string weigh, ScaleDeviceInfo info)
        {
            bool ret = false;
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "实时重量(Kg):" + weigh, info.Led.CardNum);
            return ret;
        }

        /// <summary>
        /// 更新手动信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool LED_Msg(string msg, ScaleDeviceInfo info)
        {
            bool ret = false;
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, msg, info.Led.CardNum);
            return ret;
        }


        public bool LED_Loading(ScaleDeviceInfo info)
        {
            bool ret = false;
            //第一行
            ret = LED_SendRealText(0, 0, info.Led.Width, info.Led.Height / 2, " 欢 迎 使 用", info.Led.CardNum);
            //第二行
            ret = LED_SendRealText(0, info.Led.Height / 2, info.Led.Width, info.Led.Height / 2, "系统登录,请稍等", info.Led.CardNum);
            return ret;
        }

        #endregion



        #region IC读卡器相函数

        public void IcReader_Init()
        {
            try
            {
                if (info.IcReader.IsEnable)
                {
                    //参数设置
                    IcReader.PortName = info.IcReader.ComPort;
                    IcReader.BaudRate = info.IcReader.BaudRate;
                    IcReader.Parity = (Parity)info.IcReader.Parity;
                    IcReader.DataBits = info.IcReader.DataBits;
                    IcReader.StopBits = (StopBits)info.IcReader.StopBits;
                    IcReader.ReadTimeout = info.IcReader.ReadTimeout;
                    IcReader.WriteTimeout = info.IcReader.WriteTimeout;
                    IcReader.ReceivedBytesThreshold = info.IcReader.ReceivedBytesThreshold;
                    IcReader.ReadBufferSize = info.IcReader.ReadBufferSize;
                    bool isExits = false;
                    foreach (string portname in SerialPort.GetPortNames())
                    {
                        if (portname.Equals(IcReader.PortName)) { isExits = true; break; }
                    }
                    if (isExits)
                    {
                        
                        if (!IcReader.IsOpen)
                        {
                            IcReader.DataReceived += IcReader_DataReceived;
                            IcReader.Open();
                        }
                        else
                        {
                            MessageBox.Show("IC读卡器已被打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show("IC读卡器不存在\r\n请检查接线或者串口是否正确", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: error handle
                throw;
            }

        }

        private void IcReader_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int count = sp.BytesToRead;
            if (count < 4) return;
            string indata = sp.ReadExisting();
            indata=StringToHexString(indata, Encoding.ASCII);
            this.BeginInvoke(new Action(() => {
                //更新界面以及当前重量
                //MessageBox.Show(indata);
                if(iccode_broadcast!=null) iccode_broadcast(indata);

            }));
            
        }


        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        private string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }
        private string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        #endregion


    }
}
