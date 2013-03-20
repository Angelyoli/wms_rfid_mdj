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
        ///byte	�����ֽڣ�1B����2λ(7��6bit)00B�����䣩01B��������10B(�ر�)11B��������6λ��0-5bit��xxxxxxB����˸Ƶ�ʣ�
        ///����3B���� 1Bָʾ 1B���� 1B��6B������ÿ�ֽڵ�ֵ
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
        /// ��������
        /// </summary>
        byte[] btKeyLightState = new byte[3];

        public byte[] BtKeyLightState
        {
            get { return btKeyLightState; }
            set { btKeyLightState = value; }
        }

        /// <summary>
        /// ����0x01(16��16)0x02(16��32)0x03(32��32)
        /// </summary>
        private byte btShowColor = 0;
        /// <summary>
        /// ��ŵ��ӱ�ǩ����ʾ����Ĵ�С,��5������ÿ�е�ֵ
        /// ����0x01(16��16)0x02(16��32)0x03(32��32)
        /// </summary>
        public byte BtShowColor
        {
            get { return btShowColor; }
            set { btShowColor = value; }
        }
        private byte formatType = 0;
        /// <summary>
        /// 0x00����������ʾ0x01���ִ�ר�ø�ʽ0x02���ּ�ר�ø�ʽ
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

        #region ֧�ֺ���
        /// <summary>
        /// ��ȡʮ�������ַ���
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        private String ConvertHexString(int iValue)
        {
            return iValue.ToString("X");
        }
        /// <summary>
        /// ��byte�������ݽ������У��
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
        /// ���ַ������Ƚ����жϣ��������ַ���֮ǰ��0
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
        /// ѹ������
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
        /// ���ַ����飬ѹ������
        /// </summary>
        /// <param name="szSrc">Դ�ַ���</param>
        /// <param name="btDest">ѹ��֮����ֽ�����</param>
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
        /// ���Э��������
        /// </summary>
        /// <param name="CmdType">��������</param>
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
        /// �Խ��յ����ַ�������Э����涨�Ŀ�ʼ����������зָ�
        /// </summary>
        /// <param name="strData">���յ����ַ���</param>
        /// <returns></returns>
        public String[] CommandSplit(String strData)
        {
            string[] strOperator = new string[2];
            strOperator[0] = strReadPrefix;
            strOperator[1] = strReadSuffix;
            return strData.Split(strOperator, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion
        #region IProtocol ��Ա
        /// <summary>
        /// ��Э�����ɷ����ֶ�
        /// </summary>
        /// <param name="iAddress">���ӱ�ǩ��ַ</param>
        /// <param name="cmdType">�������Ͱ�����λ�����ƣ���ƣ�������</param>
        /// <param name="strData">���ӱ�ǩ��Ҫ��ʾ������</param>
        /// <returns>�����ֶ�����</returns>
        public byte[] GenerateBytes(int iAddress, CmdType cmdType, object data)
        {
            #region
            //�����ܵ��ֽ���
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
            //���������ֽ�����
            byte[] btGenerate = new byte[iTotalBytesCount];
            //���ͷ
            byte[] btPrefix = new byte[strWritePrefix.Length / 2];
            HexString2Bin(strWritePrefix, btPrefix);
            btPrefix.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btPrefix.Length;
            //��ӵ�ַ�ֶ�
            byte[] btAddress = new byte[2];
            HexString2Bin(FillZero(ConvertHexString(iAddress), 4), btAddress);
            btAddress.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btAddress.Length;
            //����ֽ���
            byte[] btBytesCount = new byte[1];
            btBytesCount[0] = (byte)iTotalBytesCount;
            btBytesCount.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btBytesCount.Length;
            //��ӱ����ֶ�
            byte[] btReserve = new byte[2];
            btReserve.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btReserve.Length;
            //�������
            byte[] btBatch = new byte[1];
            btBatch[0] = 0;
            btBatch.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btBatch.Length;
            //ָ�����
            byte[] btCmd = new byte[1];
            HexString2Bin(FillZero(GetCommandCode(cmdType), 2), btCmd);
            btCmd.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btCmd.Length;
            //���data�ֶ�
            if (cmdType == CmdType.SendData || cmdType == CmdType.ShowOnly)
            {
                //��ӱ���
                string strData = (string)data;
                byte[] btDataReserve = new byte[1];
                int nReserver = iCurrentIndex;
                btDataReserve.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btDataReserve.Length;
                //��Ӹ�ʽ
                byte[] btDataFormat = new byte[1];
                btDataFormat[0] = this.FormatType;
                btDataFormat.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btDataFormat.Length;
                //�������

                //�������
                byte[] btFont = new byte[1];
                btFont[0] = btShowColor;
                btFont.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFont.Length;
                //������
                byte[] btRow = new byte[System.Text.Encoding.Default.GetByteCount(strData)];
                btRow = System.Text.Encoding.Default.GetBytes(strData);
                btRow.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btRow.Length;
                //�н���
                byte[] btEnd = new byte[1];
                btEnd[0] = (byte)0;
                btEnd.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btEnd.Length;
                //��ӹ����ֶ�3���ֽ�

                btFlashState.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFlashState.Length;

                
                //��ӹ����ֶι�6���ֽ�
                btKeyLightState.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btKeyLightState.Length;
                //�޸ı����ֶ�
                //btDataReserve.CopyTo(btGenerate, nReserver);

                this.btFunction.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btFunction.Length;
            }
            else
            {
                //��������
                byte[] btData = (byte[])data;
                //byte[] btRow = new byte[System.Text.Encoding.Default.GetByteCount(strData)];
                //btRow = System.Text.Encoding.Default.GetBytes(strData);
                btData.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex = iCurrentIndex + btData.Length;
            }
            //���У��
            byte[] btFCS = new byte[2];
            btFCS[1] = FCS(btGenerate, 2, iCurrentIndex);
            btFCS.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex = iCurrentIndex + btFCS.Length;
            //���β��
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
            //����У��
            int nLen = System.Text.Encoding.Default.GetByteCount(recv) / 2;
            byte[] btFCS = new byte[nLen];
            HexString2Bin(recv, btFCS);
            if (FCS(btFCS, 0, nLen - 2) != btFCS[nLen - 1])//У���
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
