using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace THOK.WES.Interface
{
    
    public class ReadRfid
    {
        /// <summary>
        /// 根据id读取rfid
        /// </summary>
        /// <param name="rfidId">货位rfid</param>
        /// <returns></returns>
        //[DllImport("Sense18KAPINet.dll")]        
        public bool ReadCellRfid(string rfidId)
        {
            return true;
        }

        public string ReadCellRfid()
        {
            return "";
        }

        //public bool ReadTrayRfid(string rfidId)
        //{
        //    return true;
        //}

        public string ReadTrayRfid()
        {
            return "6666666666666666666";
        }
    }
}
