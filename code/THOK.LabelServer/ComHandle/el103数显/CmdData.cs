using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el103
{
    class CmdData
    {
        public int address = 0;
        public CmdType cmdType = CmdType.SendData;
        public object data = null;
    }
}
