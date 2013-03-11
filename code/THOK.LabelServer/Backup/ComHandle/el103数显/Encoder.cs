using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el103
{
    class Encoder:Protocal,IEncoder 
    {
        #region IEncoder 成员

        public object Decode(string Code)
        {
            return this.Parse(Code);
        }

        public byte[] Encode(object CMD)
        {
            CmdData cmd = (CmdData)CMD;
            return this.GenerateBytes(cmd.address, cmd.cmdType, cmd.data);
        }
        #endregion
     }
}
