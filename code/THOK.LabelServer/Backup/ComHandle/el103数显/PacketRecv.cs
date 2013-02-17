using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el103
{
    public class PacketRecv
    {
        public string message = "";
        public RecvType recvType;
        public int address = 0;
        public string recv = "";
        public PacketRecv(RecvType recvType, int address, string recv, string message)
        {
            this.recvType = recvType;
            this.address = address;
            this.message = message;
            this.recv = recv;

        }
    }

    public enum CmdType
    {
        SendData, ResetElectronicLabel, ControlPlateID,
        OpenLight, CloseLight, LoudSpeaker, ShowOnly, KeyRaisedAck, Delete,
        ResetControlPlate
    };

    public enum RecvType
    {
        Correct, CheckError, Timeout, Others, NotExist,
        KeyEvent, EndError, SRamFull, DeleteTable, ResetCorrect, ResetError
    };

}
