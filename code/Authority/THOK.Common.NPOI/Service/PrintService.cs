using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using THOK.Common.NPOI.Models;
using System.Data;
using System.IO;

namespace THOK.Common.NPOI.Service
{
    public class PrintService
    {
        public static FileStreamResult Print(ExportParam ep)
        {
            try
            {
                MemoryStream ms = ExportExcel.ExportDT(ep);
                return new FileStreamResult(ms, ep.StreamType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
