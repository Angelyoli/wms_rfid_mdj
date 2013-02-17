using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el103
{
    class Protocal
    {   
        public String strWritePrefix = "0A7B";
        public String strWriteSuffix = "7D0B";

        public String strReadPrefix = "0A7B";
        public String strReadSuffix = "7D0B";

        private byte[] btFunction = new byte[3];
        /// <summary>
        ///byte	功能字节（1B）高2位(7、6bit)00B（不变）01B（开启）10B(关闭)11B（—）低6位（0-5bit）xxxxxxB（闪烁频率）
        ///键灯3B背光 1B指示 1B蜂铃 1B共6B，设置每字节的值
        /// </summary>
        public byte[] BtFunction
        {
            get { return btFunction; }
            set { btFunction = value; }
        }

        private byte[] btFlashState = new byte[3];

        public byte[] BtFlashState
        {
            get { return btFlashState; }
            set { btFlashState = value; }
        }
        /// <summary>
        /// 按键功能
        /// </summary>
        byte[] btKeyLightState = new byte[3];

        public byte[] BtKeyLightState
        {
            get { return btKeyLightState; }
            set { btKeyLightState = value; }
        }

        /// <summary>
        /// 字体0x01(16×16)0x02(16×32)0x03(32×32)
        /// </summary>
        private byte btShowColor = 0;
        /// <summary>
        /// 存放电子标签行显示字体的大小,共5行设置每行的值
        /// 字体0x01(16×16)0x02(16×32)0x03(32×32)
        /// </summary>
        public byte BtShowColor
        {
            get { return btShowColor; }
            set { btShowColor = value; }
        }
        private byte formatType = 0;
        /// <summary>
        /// 0x00：纯字体显示0x01：仓储专用格式0x02：分拣专用格式
        /// </summary>
        public byte FormatType
        {
            get { return formatType; }
            set { formatType = value; }
        }

        public Protocal()
        {
            btShowColor = 0;
            btFunction[0] = 64;
            btFunction[1] = 0;
            btFunction[2] = 0;
            formatType = 1;
        }

        #region 支持函数
        /// <summary>
        /// 获取十六进制字符串
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        private String ConvertHexString(int iValue)
        {
            return iValue.ToString("X");
        }
        /// <summary>
        /// 对byte数组数据进行异或校验
        /// </summary>
        /// <param name="btData"></param>
        /// <param name="iStart"></param>
        /// <param name="iEnd"></param>
        /// <returns></returns>
        public  byte FCS(byte[] btData,int iStart,int iEnd)
        {
            byte btResult = 0;
            for (int i = iStart; i < iEnd; i++)
            {
                btResult = (byte)((btResult + btData[i]) % 256);
            }
            return btResult ;
        }
        /// <summary>
        /// 对字符串长度进行判断，不够在字符串之前补0
        /// </summary>
        /// <param name="strHexData"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        private String FillZero(String strHexData, int iLen)
        {
            int len = strHexData.Length;
            for (int i = 0;i < iLen - len; i++)
            {
                strHexData = "0" + strHexData;
            }
            return strHexData.Substring(strHexData.Length - iLen, iLen);
        }
        /// <summary>
        /// 压缩编码
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private byte Get4Bit(byte b)
        {
            if (b >= 0x30 && b <= 0x39)
                b = (byte)(b - 0x30);
            else if (b >= 0x41 && b <= 0x5B)
                b = (byte)(b - 0x37);
            else if (b >= 0x61 && b <= 0x7B)
                b = (byte)(b - 0x57);
            else
                b = 0x00;
            return b;
        }
        /// <summary>
        /// 对字符串组，压缩编码
        /// </summary>
        /// <param name="szSrc">源字符串</param>
        /// <param name="btDest">压缩之后的字节数组</param>
        public void HexString2Bin(String szSrc, byte[] btDest)
        {
            int nStrLen = szSrc.Length;
            byte byTemp;
            for (int i = 0; i < nStrLen; i += 2)
            {
                byTemp = Get4Bit((byte)szSrc[i]);
                byTemp = (byte)(byTemp << 4);
                byTemp |= Get4Bit((byte)szSrc[i + 1]);
                btDest[i / 2] = byTemp;
            }
        }

        /// <summary>
        /// 获得协议命令码
        /// </summary>
        /// <param name="CmdType">命令类型</param>
        /// <returns></returns>
        private String GetCommandCode(CmdType CmdType)
        {
            String strReturn = "";
            switch (CmdType)
            {
                case CmdType.KeyRaisedAck:
                    strReturn = "16";
                    break;
                case CmdType.SendData:
                    strReturn = "20";
                    break;
                case CmdType.ShowOnly:
                    strReturn = "22";
                    break;
                case CmdType.LoudSpeaker:
                    strReturn = "24";
                    break;
                case CmdType.OpenLight:
                    strReturn = "25";
                    break;
                case CmdType.CloseLight:
                    strReturn = "25";
                    break;
                case CmdType.ResetElectronicLabel:
                    strReturn = "26";
                    break;
                case CmdType.ControlPlateID:
                    strReturn = "27";
                    break;
                case CmdType.Delete:
                    strReturn = "12";
                    break;
                case CmdType.ResetControlPlate:
                    strReturn = "10";
                    break;
                default:
                    break;
            }
            return strReturn;
        }
        /// <summary>
        /// 对接收到的字符串按照协议里规定的开始与结束符进行分割
        /// </summary>
        /// <param name="strData">接收到的字符串</param>
        /// <returns></returns>
        public String[] CommandSplit(String strData)
        {
            string[] strOperator = new string[2];
            strOperator[0] = strReadPrefix;
            strOperator[1] = strReadSuffix;
            return strData.Split(strOperator, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion
        #region IProtocol 成员
        /// <summary>
        /// 按协议生成发送字段
        /// </summary>
        /// <param name="iAddress">电子标签地址</param>
        /// <param name="cmdType">命令类型包括复位，开灯，灭灯，声音等</param>
        /// <param name="strData">电子标签所要显示的数据</param>
        /// <returns>发送字段数组</returns>
        public byte[] GenerateBytes(int iAddress, CmdType cmdType, object data)
        {
            #region
            //计算总的字节数
            int iTotalBytesCount = 13;

            if (cmdType == CmdType.SendData || cmdType == CmdType.ShowOnly)
            {
                //string[] strData = (string[])data;

                string strData = (string)data;
                iTotalBytesCount = 13;
                int nCount = System.Text.Encoding.Default.GetByteCount(strData);
                iTotalBytesCount = iTotalBytesCount + nCount + 2;
                iTotalBytesCount = iTotalBytesCount + 3;
                iTotalBytesCount = iTotalBytesCount + 8;
            }
            else
                iTotalBytesCount = iTotalBytesCount + 2;
            int iCurrentIndex = 0;
            //创建发送字节数组
            byte[] btGenerate = new byte[iTotalBytesCount];
            //添加头
            byte[] btPrefix = new byte[strWritePrefix.Length / 2];
            HexString2Bin(strWritePrefix, btPrefix);
            btPrefix.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btPrefix.Length;
            //添加地址字段
            byte[] btAddress = new byte[2];
            HexString2Bin(FillZero(ConvertHexString(iAddress), 4), btAddress);
            btAddress.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btAddress.Length;
            //添加字节数
            byte[] btBytesCount = new byte[1];
            btBytesCount[0] = (byte)iTotalBytesCount;
            btBytesCount.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btBytesCount.Length;
            //添加保留字段
            byte[] btReserve = new byte[2];
            btReserve.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btReserve.Length;
            //添加批次
            byte[] btBatch = new byte[1];
            btBatch[0] = 0;
            btBatch.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btBatch.Length;
            //指令代码
            byte[] btCmd = new byte[1];
            HexString2Bin(FillZero(GetCommandCode(cmdType), 2), btCmd);
            btCmd.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btCmd.Length;
            //添加data字段
            if (cmdType == CmdType.SendData || cmdType == CmdType.ShowOnly)
            {
                //添加保留
                string strData = (string)data;
                byte[] btDataReserve = new byte[1];
                int nReserver = iCurrentIndex;
                btDataReserve.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btDataReserve.Length;
                //添加格式
                byte[] btDataFormat = new byte[1];
                btDataFormat[0] = this.FormatType;
                btDataFormat.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btDataFormat.Length;
                //添加数据

                //添加字体
                byte[] btFont = new byte[1];
                btFont[0] = btShowColor;
                btFont.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFont.Length;
                //行数据
                byte[] btRow = new byte[System.Text.Encoding.Default.GetByteCount(strData)];
                btRow = System.Text.Encoding.Default.GetBytes(strData);
                btRow.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btRow.Length;
                //行结束
                byte[] btEnd = new byte[1];
                btEnd[0] = (byte)0;
                btEnd.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btEnd.Length;
                //添加功能字段3个字节

                btFlashState.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFlashState.Length;

                
                //添加功能字段共6个字节
                btKeyLightState.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btKeyLightState.Length;
                //修改保留字段
                //btDataReserve.CopyTo(btGenerate, nReserver);

                this.btFunction.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFunction.Length;
            }
            else
            {
                //命令数据
                byte[] btData = (byte[])data;
                //byte[] btRow = new byte[System.Text.Encoding.Default.GetByteCount(strData)];
                //btRow = System.Text.Encoding.Default.GetBytes(strData);
                btData.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btData.Length;
            }
            //添加校验
            byte[] btFCS = new byte[2];
            btFCS[1] = FCS(btGenerate, 2, iCurrentIndex);
            btFCS.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btFCS.Length;
            //添加尾标
            byte[] btSuffix = new byte[strWriteSuffix.Length / 2];
            HexString2Bin(strWriteSuffix, btSuffix);
            btSuffix.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btSuffix.Length;

            return btGenerate;
            #endregion
        }
       
        public PacketRecv Parse(string recv)
        {
            string message = "";
            //接收校验
            int nLen = System.Text.Encoding.Default.GetByteCount(recv) / 2;
            byte[] btFCS = new byte[nLen];
            HexString2Bin(recv, btFCS);
            if (FCS(btFCS, 0, nLen - 2) != btFCS[nLen - 1])//校验错
                return null;

            try
            {
                int iAddress = int.Parse(recv.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                RecvType rt;
                
                switch (recv.Substring(12, 2).ToUpper())
                {
                    case "10":
                        rt = RecvType.Correct;
                        break;
                    case "11":
                        rt = RecvType.CheckError;
                        message = recv.Substring(14, 2).ToUpper();
                        break;
                    case "12":
                        message = recv.Substring(14, 2).ToUpper();
                        if (message == "00")
                            rt = RecvType.ResetCorrect;
                        else
                            rt = RecvType.ResetError;
                        break;
                    case "13":
                        rt = RecvType.KeyEvent;
                        break;
                    default:
                        rt = RecvType.Others;
                        break;
                }
                return new PacketRecv(rt, iAddress, recv,message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(recv);
                return new PacketRecv(RecvType.Others, 0, recv,message);
            }
        }

        #endregion
    }
}
