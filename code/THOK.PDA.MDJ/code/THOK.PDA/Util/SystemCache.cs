using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.PDA.Util;
using THOK.PDA.View;

namespace THOK.PDA.Util
{
    public static class SystemCache
    {
        private static MainForm mainFrom = null;
        public static MainForm MainFrom
        {
            get { return SystemCache.mainFrom; }
            set { SystemCache.mainFrom = value; }
        }

        private static DataTable detailTable = null;
        public static DataTable DetailTable
        {
            get { return SystemCache.detailTable; }
            set { SystemCache.detailTable = value; }
        }

        private static DataTable masterTable = null;
        public static DataTable MasterTable
        {
            get { return SystemCache.masterTable; }
            set { SystemCache.masterTable = value; }
        }

        private static string connetionType = "";
        public static string ConnetionType
        {
            get
            {
                if (connetionType == "")
                {
                    SystemCache.connetionType = new ConfigUtil().GetConfig("ConnetionType")["Type"];
                }
                return SystemCache.connetionType;
            }
        }

        private static string httpConnectionStr = "";
        public static string HttpConnectionStr
        {
            get
            {
                SystemCache.httpConnectionStr = new ConfigUtil().GetConfig("HttpConnectionStr")["HttpConnStr"];
                return SystemCache.httpConnectionStr;
            }
        }
    }
}
