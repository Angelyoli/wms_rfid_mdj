using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace THOK.WES.Interface
{
    
    public class ReadRfid
    {

        //读写器读取标签的ID号并保存在缓冲区中，返回读到的标签数(TagCount)
        [DllImport("Sense18KAPINet.dll")]
        static extern short Sense18K_ISOListUID(string ip, byte RAddr, out byte TagCount);

        //从读写器缓冲区中取一个标签数据，同时读写器删除该组标签数据(TagData[])。
        [DllImport("Sense18KAPINet.dll")]
        static extern short Sense18K_BufGetOneAndClear(string ip, byte RAddr, out byte TagData);

      
        private byte g_RAddr = 0xFF;   //读写器地址
        private string g_IP = "";   //读写器地址
        private int errnone = 0;

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

        public List<string> ReadTrayRfid()
        {
            List<string> listRfid = new List<string>();
            byte TagCount;
            byte[] tagData =new byte[255];
            int status = Sense18K_ISOListUID(g_IP, g_RAddr, out TagCount);
            if (status == errnone)
            {
                for (int i = 0; i < TagCount; i++)
                {
                    status = Sense18K_BufGetOneAndClear(g_IP, g_RAddr,out tagData[i]);
                    if (status == errnone)
                    {
                        listRfid.Add(tagData[i + 1].ToString());
                    }
                }
            }
            return listRfid;
        }
    }
}
