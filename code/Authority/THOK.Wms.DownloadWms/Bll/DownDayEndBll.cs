using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Wms.DownloadWms.Dao;

namespace THOK.Wms.DownloadWms.Bll
{
   public class DownDayEndBll
    {
        /// <summary>
        /// 下载日结信息
        /// </summary>
        /// <returns></returns>
        public bool DownDayEndInfo()
        {
            bool tag = true;
            DataTable dayEnd = this.GetDayEndInfo();
            if (dayEnd.Rows.Count > 0)
            {
            }
            return tag;
        }



        /// <summary>
        /// 读取下载日结信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDayEndInfo()
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownDayEndDao dao = new DownDayEndDao();
                dao.SetPersistentManager(dbPm);
                return dao.FindDayEnd();
            }
        }

        /// <summary>
        /// 构建一个日结虚拟表
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable inbrtable = ds.Tables.Add("V_WMS_DAILYBALANCE");
            inbrtable.Columns.Add("customer_code");
            inbrtable.Columns.Add("custom_code");
            inbrtable.Columns.Add("customer_name");
            inbrtable.Columns.Add("company_code");
            inbrtable.Columns.Add("sale_region_code");
            inbrtable.Columns.Add("uniform_code");
            inbrtable.Columns.Add("customer_type");
            inbrtable.Columns.Add("sale_scope");
            inbrtable.Columns.Add("industry_type");
            inbrtable.Columns.Add("city_or_countryside");
            inbrtable.Columns.Add("deliver_line_code");
            inbrtable.Columns.Add("deliver_order");
            inbrtable.Columns.Add("address");
            inbrtable.Columns.Add("phone");
            inbrtable.Columns.Add("license_type");
            inbrtable.Columns.Add("license_code");
            inbrtable.Columns.Add("principal_name");
            inbrtable.Columns.Add("principal_phone");
            inbrtable.Columns.Add("principal_address");
            inbrtable.Columns.Add("management_name");
            inbrtable.Columns.Add("management_phone");
            inbrtable.Columns.Add("bank");//一号工程条形码
            inbrtable.Columns.Add("bank_accounts");
            inbrtable.Columns.Add("description");
            inbrtable.Columns.Add("is_active");
            inbrtable.Columns.Add("update_time");
            return ds;
        }
    }
}
