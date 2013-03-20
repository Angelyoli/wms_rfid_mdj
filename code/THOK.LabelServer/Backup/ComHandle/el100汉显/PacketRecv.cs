using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el100
{
    class PacketRecv
    {
        public int address = 0;
        public string message = "";
        public RecvType recvType;

        public PacketRecv(RecvType recvType, int address, string message)
        {
            this.recvType = recvType;
            this.address = address;
            this.message = message;
        }
    }
}
