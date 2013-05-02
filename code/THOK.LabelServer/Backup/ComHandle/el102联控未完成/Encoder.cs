using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el102
{
    class Encoder : Protocal, IEncoder
    {
        #region IEncoder 成员

        public object Decode(string Code)
        {
            PacketRecv PacketRecv_ = new PacketRecv(); ;
            int iAddress;

            iAddress = (int.Parse(Code.Substring(0, 2), NumberStyles.AllowHexSpecifier) - 0x80) * 256;
            iAddress += int.Parse(Code.Substring(2, 2), NumberStyles.AllowHexSpecifier) - 200 == 13 ? 13 : int.Parse(Code.Substring(2, 2), NumberStyles.AllowHexSpecifier); ;
            PacketRecv_.address = iAddress;

            switch (Code.Substring(4, 2).ToUpper())
            {
                case "50":// Code = [@] nnnn 0x50 <OK> <FCS> * [<Return>] 发送数据指令返回
                    PacketRecv_.recvType = RecvType.Correct;
                    break;
                case "56":// Code = [@] nnnn 0x56 <KEY> <FCS> * [<Return>] 查询状态指令返回
                    PacketRecv_.recvType = RecvType.KeyEvent;
                    PacketRecv_.message = Code;
                    break;
                case "60":// Code = [@] nnnn 0x60 <OK> <FCS> * [<Return>] 复位指令返回
                    PacketRecv_.recvType = RecvType.Correct;
                    break;
                default:
                    PacketRecv_.recvType = RecvType.Others;
                    break;
            }
            return PacketRecv_;
        }

        public byte[] Encode(object CMD)
        {
            CmdData cmd = (CmdData)CMD;
            byte[] aryByte={};

            switch (cmd.cmdType)
            {
                case CmdType.SendData:
                    aryByte = getbyte_SendText(cmd);
                    break;
                case CmdType.ResetElectronicLabel:
                    aryByte = getbyte_Reset(cmd);
                    break;
                case CmdType.ControlPlateID:
                    break;
                case CmdType.OpenLight:
                    break;
                case CmdType.CloseLight:
                    break;
                case CmdType.LoudSpeaker:
                    break;
                case CmdType.ShowOnly:
                    break;
                case CmdType.KeyRaisedAck:
                    break;
                case CmdType.Delete:
                    break;
                case CmdType.ResetControlPlate:
                    break;
                case CmdType.GetKey:
                    aryByte = getbyte_GetKey(cmd);
                    break;
                default:
                    break;
            }
            return aryByte;
        }
        #endregion
        private byte[] getbyte_SendText(CmdData cmd)
        {
            IList<byte>  byteList = new List<byte>();

            int  nAddressH = cmd.address / 256 + 0x80;
            int  nAddressL = cmd.address % 256;

            if (nAddressL == 13)
                nAddressL += 200;

            byteList.Add(0x40);
            byteList.Add((byte)nAddressH);
            byteList.Add((byte)nAddressL);
            byteList.Add(0x50);

            int nPosition = 4;

            string[] aryData;
            string strData = "";

            aryData = (string[])(cmd.data);
            strData = aryData[0] + aryData[1] + aryData[2] + aryData[3];

            byte[] aryByteTemp = Encoding.Default.GetBytes(strData);
            foreach (byte data in aryByteTemp)
            {
                byteList.Add(data);
                nPosition++;
                if (nPosition == 100)
                    break;
            }

            int nVerify = byteList[0];

            for (int j = 1; j < 100; j++)
            {
                nVerify = nVerify ^ byteList[j];
            }

            byteList.Add((byte)nVerify);
            byteList.Add( 0x2a);
            byteList.Add( 0xd);

            return byteList.ToArray();
        }
        private byte[] getbyte_Reset(CmdData cmd)
        {
            int nRack = 0, nNode = 0, nVerify = 0;
            byte[] OutBuff = { 0, 0, 0, 0, 0, 0, 0 };

            nRack = cmd.address / 256 + 0x80;
            nNode = cmd.address % 256;

            if (nNode == 13)
                nNode += 200;

            nVerify = 0x40 ^ nRack ^ nNode ^ 0x60;

            OutBuff[0] = 0x40;
            OutBuff[1] = (byte)nRack;
            OutBuff[2] = (byte)nNode;
            OutBuff[3] = 0x60;
            OutBuff[4] = (byte)nVerify;
            OutBuff[5] = 0x2a;
            OutBuff[6] = 0xd;
            return OutBuff;
        }
        private byte[] getbyte_GetKey(CmdData cmd)
        {
            int nRack = 0, nNode = 0, nVerify = 0;
            byte[] OutBuff = { 0, 0, 0, 0, 0, 0, 0 };

            nRack = cmd.address  / 256 + 0x80;
            nNode = cmd.address  % 256;

            if (nNode == 13)
                nNode += 200;

            nVerify = 0x40 ^ nRack ^ nNode ^ 0x56;

            OutBuff[0] = 0x40;
            OutBuff[1] = (byte)nRack;
            OutBuff[2] = (byte)nNode;
            OutBuff[3] = 0x56;
            OutBuff[4] = (byte)nVerify;
            OutBuff[5] = 0x2a;
            OutBuff[6] = 0xd;
            return OutBuff;
        }
    }
}
