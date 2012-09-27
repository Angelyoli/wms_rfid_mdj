using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WMS.Upload.Bll;

namespace THOK.Wms.Upload
{
    public class UploadDate
    {
        UploadBll updateBll = new UploadBll();
        public bool UploadInfoData()
        {
            bool result = false;
            try
            {
                string tag = "";
                //上报卷烟信息表
                tag = updateBll.FindProduct();
                result = true;
            }
            catch 
            {
                result = false;
            }
            return result;
        }
    }
}
