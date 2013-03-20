namespace THOK.Zeng.ComfixtureHandle.el100
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.IO;

    class Protocal
    {
        private byte[] btFunction = new byte[3];
        private byte[] btRowTextSize = new byte[5];
        private byte formatType = 0;
        private int keyLightFrequency = 0;
        public string strReadPrefix = "0A7B";
        public string strReadSuffix = "7D0B";
        public string strWritePrefix = "0A7B";
        public string strWriteSuffix = "7D0B";

        public Protocal()
        {
            for (int i = 0; i < this.btRowTextSize.Length; i++)
            {
                this.btRowTextSize[i] = 1;
            }
            this.btFunction[0] = 0x40;
            this.btFunction[1] = 0;
            this.btFunction[2] = 0;
            this.formatType = 1;
        }

        public string[] CommandSplit(string strData)
        {
            string[] strOperator = new string[] { this.strReadPrefix, this.strReadSuffix };
            return strData.Split(strOperator, StringSplitOptions.RemoveEmptyEntries);
        }

        private string ConvertHexString(int iValue)
        {
            return iValue.ToString("X");
        }

        private byte FCS(byte[] btData, int iStart, int iEnd)
        {
            byte btResult = 0;
            for (int i = iStart; i < iEnd; i++)
            {
                btResult = (byte) ((btResult + btData[i]) % 0x100);
            }
            return btResult;
        }

        private string FillZero(string strHexData, int iLen)
        {
            int len = strHexData.Length;
            for (int i = 0; i < (iLen - len); i++)
            {
                strHexData = "0" + strHexData;
            }
            return strHexData.Substring(strHexData.Length - iLen, iLen);
        }

        public byte[] GenerateBytes(int iAddress, CmdType cmdType, object data)
        {
            string[] strData;
            int i;
            int iTotalBytesCount = 13;
            if ((cmdType == CmdType.SendData) || (cmdType == CmdType.ShowOnly))
            {
                strData = (string[]) data;
                iTotalBytesCount = 13;
                for (i = 0; i < strData.Length; i++)
                {
                    int nCount = Encoding.Default.GetByteCount(strData[i]);
                    iTotalBytesCount = (iTotalBytesCount + nCount) + 2;
                }
                iTotalBytesCount += 8;
            }
            else
            {
                iTotalBytesCount += 2;
            }
            int iCurrentIndex = 0;
            byte[] btGenerate = new byte[iTotalBytesCount];
            byte[] btPrefix = new byte[this.strWritePrefix.Length / 2];
            this.HexString2Bin(this.strWritePrefix, btPrefix);
            btPrefix.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btPrefix.Length;
            byte[] btAddress = new byte[2];
            this.HexString2Bin(this.FillZero(this.ConvertHexString(iAddress), 4), btAddress);
            btAddress.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btAddress.Length;
            byte[] btBytesCount = new byte[] { (byte) iTotalBytesCount };
            btBytesCount.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btBytesCount.Length;
            byte[] btReserve = new byte[2];
            btReserve.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btReserve.Length;
            byte[] btBatch = new byte[] { 0 };
            btBatch.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btBatch.Length;
            byte[] btCmd = new byte[1];
            this.HexString2Bin(this.FillZero(this.GetCommandCode(cmdType), 2), btCmd);
            btCmd.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btCmd.Length;
            if ((cmdType == CmdType.SendData) || (cmdType == CmdType.ShowOnly))
            {
                strData = (string[]) data;
                byte[] btDataReserve = new byte[1];
                int nReserver = iCurrentIndex;
                btDataReserve.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex += btDataReserve.Length;
                byte[] btDataFormat = new byte[] { this.FormatType };
                btDataFormat.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex += btDataFormat.Length;
                for (i = 0; i < strData.Length; i++)
                {
                    byte[] btFont = new byte[] { this.BtRowTextSize[i] };
                    btFont.CopyTo(btGenerate, iCurrentIndex);
                    iCurrentIndex += btFont.Length;
                    byte[] btRow = new byte[Encoding.Default.GetByteCount(strData[i])];
                    btRow = Encoding.Default.GetBytes(strData[i]);
                    btRow.CopyTo(btGenerate, iCurrentIndex);
                    iCurrentIndex += btRow.Length;
                    byte[] btEnd = new byte[] { 0 };
                    btEnd.CopyTo(btGenerate, iCurrentIndex);
                    iCurrentIndex += btEnd.Length;
                }
                byte[] btKeyLight = new byte[3];
                for (i = 0; i < 3; i++)
                {
                    if (strData[i] == "")
                    {
                        btKeyLight[i] = 0;
                    }
                    else
                    {
                        btKeyLight[i] = this.btFunction[1];//(byte) (0x40 + this.KeyLightFrequency);
                        btDataReserve[0] = (byte) (btDataReserve[0] | ((byte) Math.Pow(2.0, (double) i)));
                    }
                }
                btKeyLight.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex += btKeyLight.Length;
                btDataReserve.CopyTo(btGenerate, nReserver);
                this.btFunction.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex += this.btFunction.Length;
            }
            else
            {
                byte[] btData = (byte[]) data;
                btData.CopyTo(btGenerate, iCurrentIndex);
                iCurrentIndex += btData.Length;
            }
            byte[] btFCS = new byte[2];
            btFCS[1] = this.FCS(btGenerate, 2, iCurrentIndex);
            btFCS.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btFCS.Length;
            byte[] btSuffix = new byte[this.strWriteSuffix.Length / 2];
            this.HexString2Bin(this.strWriteSuffix, btSuffix);
            btSuffix.CopyTo(btGenerate, iCurrentIndex);
            iCurrentIndex += btSuffix.Length;
            return btGenerate;
        }

        private byte Get4Bit(byte b)
        {
            if ((b >= 0x30) && (b <= 0x39))
            {
                b = (byte) (b - 0x30);
                return b;
            }
            if ((b >= 0x41) && (b <= 0x5b))
            {
                b = (byte) (b - 0x37);
                return b;
            }
            if ((b >= 0x61) && (b <= 0x7b))
            {
                b = (byte) (b - 0x57);
                return b;
            }
            b = 0;
            return b;
        }

        private string GetCommandCode(CmdType CmdType)
        {
            switch (CmdType)
            {
                case CmdType.SendData:
                    return "20";

                case CmdType.ResetElectronicLabel:
                    return "26";

                case CmdType.ControlPlateID:
                    return "27";

                case CmdType.OpenLight:
                    return "25";

                case CmdType.CloseLight:
                    return "25";

                case CmdType.LoudSpeaker:
                    return "24";

                case CmdType.ShowOnly:
                    return "22";

                case CmdType.KeyRaisedAck:
                    return "15";

                case CmdType.Delete:
                    return "12";

                case CmdType.ResetControlPlate:
                    return "10";
            }
            return "";
        }

        private void HexString2Bin(string szSrc, byte[] btDest)
        {
            int nStrLen = szSrc.Length;
            for (int i = 0; i < nStrLen; i += 2)
            {
                byte byTemp = (byte) (this.Get4Bit((byte) szSrc[i]) << 4);
                byTemp = (byte) (byTemp | this.Get4Bit((byte) szSrc[i + 1]));
                btDest[i / 2] = byTemp;
            }
        }

        public PacketRecv Parse(string recv)
        {
            PacketRecv PacketRecv0000;
            try
            {
                RecvType rt;
                int iAddress = int.Parse(recv.Substring(0, 4), NumberStyles.AllowHexSpecifier);
                
                switch (recv.Substring(12, 2).ToUpper())
                {
                    case "10":
                        rt = RecvType.Correct;
                        iAddress = int.Parse(recv.Substring(14, 2), NumberStyles.AllowHexSpecifier);
                        goto Label_0114;

                    case "11":
                        rt = RecvType.CheckError;
                        iAddress = int.Parse(recv.Substring(14, 2), NumberStyles.AllowHexSpecifier);
                        goto Label_0114;

                    case "12":
                        rt = RecvType.EndError;
                        iAddress = int.Parse(recv.Substring(14, 2), NumberStyles.AllowHexSpecifier);
                        goto Label_0114;

                    case "13":
                        rt = RecvType.SRamFull;
                        iAddress = int.Parse(recv.Substring(14, 2), NumberStyles.AllowHexSpecifier);
                        goto Label_0114;

                    case "1C":
                        rt = RecvType.DeleteTable;
                        goto Label_0114;

                    case "1D":
                        rt = RecvType.KeyEvent;
                        goto Label_0114;

                    case "18":
                        if (!(recv.Substring(14, 2).ToUpper() == "00"))
                        {
                            break;
                        }
                        rt = RecvType.ResetCorrect;
                        goto Label_0114;

                    default:
                        rt = RecvType.Others;
                        goto Label_0114;
                }
                rt = RecvType.ResetError;
            Label_0114:
                PacketRecv0000 = new PacketRecv(rt, iAddress, recv);
            }
            catch (Exception)
            {
                PacketRecv0000 = new PacketRecv(RecvType.Others, 0, recv);
            }
            return PacketRecv0000;
        }

        public byte[] BtFunction
        {
            get
            {
                return this.btFunction;
            }
            set
            {
                this.btFunction = value;
            }
        }

        public byte[] BtRowTextSize
        {
            get
            {
                return this.btRowTextSize;
            }
            set
            {
                this.btRowTextSize = value;
            }
        }

        public byte FormatType
        {
            get
            {
                return this.formatType;
            }
            set
            {
                this.formatType = value;
            }
        }

        public int KeyLightFrequency
        {
            get
            {
                return this.keyLightFrequency;
            }
            set
            {
                this.keyLightFrequency = value;
            }
        }
        public void WriteLog(String logMessage)
        {
            if (!Directory.Exists(".\\LOG\\" + DateTime.Now.ToLongDateString()))
            {
                // Create the directory it does not exist.
                Directory.CreateDirectory(".\\LOG\\" + DateTime.Now.ToLongDateString());
            }
            using (StreamWriter w = File.AppendText(".\\LOG\\" + DateTime.Now.ToLongDateString() + "\\Com" + "log.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("-------------------------------");
                // Update the underlying file.
                w.Flush();
                // Close the writer and underlying file.
                w.Close();
            }
        }
    }
}

