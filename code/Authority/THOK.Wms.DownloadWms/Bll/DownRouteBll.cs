using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownRouteBll
    {
        #region 从营销系统下载线路信息

        /// <summary>
        /// 下载线路信息
        /// </summary>
        /// <returns></returns>
        public bool DownRouteInfo()
        {
            bool tag = true;           
            DataTable RouteCodeDt = this.GetRouteCode();
            string routeCodeList = UtinString.StringMake(RouteCodeDt, "deliver_line_code");
            routeCodeList = UtinString.StringMake(routeCodeList);
            routeCodeList = "DELIVER_LINE_CODE NOT IN (" + routeCodeList + ")";

            DataTable RouteDt = this.GetRouteInfo(routeCodeList);
            if (RouteDt.Rows.Count > 0)
            {
                DataSet routeCodeDs = this.InsertRouteCode(RouteDt);
                this.Insert(routeCodeDs);
            }else
                tag = false;
            return tag;
        }

        /// <summary>
        /// 自动下载线路信息，下载前清楚线路表
        /// </summary>
        /// <returns></returns>
       public bool GetDownRouteInfo()
       {
           bool tag = true;
           this.DeleteRoute();//下载清除线路表
           DataTable RouteDt = this.GetRouteInfo();
           if (RouteDt.Rows.Count > 0)
           {
               DataSet deptDs = this.InsertRouteCode(RouteDt);
               this.Insert(deptDs);
           }
           else
               tag = false;
           return tag;
       }

       /// <summary>
       /// 清除线路表
       /// </summary>
       public void DeleteRoute()
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownRouteDao dao = new DownRouteDao();
               dao.Delete();
           }
       }

       /// <summary>
       /// 下载线路信息
       /// </summary>
       /// <returns></returns>
       public DataTable GetRouteInfo(string routeCodeList)
       {
           using (PersistentManager dbPm = new PersistentManager("YXConnection"))
           {
               DownRouteDao dao = new DownRouteDao();
               dao.SetPersistentManager(dbPm);
               return dao.GetRouteInfo(routeCodeList);
           }
       }
       /// <summary>
       /// 自动下载线路信息
       /// </summary>
       /// <returns></returns>
       public DataTable GetRouteInfo()
       {
           using (PersistentManager dbPm = new PersistentManager("YXConnection"))
           {
               DownRouteDao dao = new DownRouteDao();
               dao.SetPersistentManager(dbPm);
               return dao.GetRouteInfo();
           }
       }

       /// <summary>
       /// 查询仓库线路编号
       /// </summary>
       /// <returns></returns>
       public DataTable GetRouteCode()
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownRouteDao dao = new DownRouteDao();
               return dao.GetRouteCode();
           }
       }

       /// <summary>
       /// 把虚拟表的数据添加到数据库
       /// </summary>
       /// <param name="ds"></param>
       public void Insert(DataSet ds)
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownRouteDao dao = new DownRouteDao();
               dao.Insert(ds);
           }
       }

       /// <summary>
       /// 添加数据到虚拟表中
       /// </summary>
       /// <param name="dr"></param>
       /// <returns></returns>
       private DataSet InsertRouteCode(DataTable routeCodeTable)
       {
           DataSet ds = this.GenerateEmptyTables();
           foreach (DataRow row in routeCodeTable.Rows)
           {
               DataRow routeDr = ds.Tables["DWV_OUT_DELIVER_LINE"].NewRow();
               routeDr["deliver_line_code"] = row["DELIVER_LINE_CODE"];
               routeDr["custom_code"] = row["LINE_TYPE"];
               routeDr["deliver_line_name"] = row["DELIVER_LINE_NAME"];
               routeDr["dist_code"] = row["DIST_STA_CODE"];
               routeDr["deliver_order"] = row["DELIVER_LINE_ORDER"];
               routeDr["description"] = "";
               routeDr["is_active"] = row["ISACTIVE"];
               routeDr["update_time"] = DateTime.Now;
               ds.Tables["DWV_OUT_DELIVER_LINE"].Rows.Add(routeDr);
           }
           return ds;
       }

        /// <summary>
        /// 缓存中构建虚拟表
        /// </summary>
        /// <returns></returns>
        public DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable routeDt = ds.Tables.Add("DWV_OUT_DELIVER_LINE");
            routeDt.Columns.Add("deliver_line_code");
            routeDt.Columns.Add("custom_code");
            routeDt.Columns.Add("deliver_line_name");
            routeDt.Columns.Add("dist_code");
            routeDt.Columns.Add("deliver_order");
            routeDt.Columns.Add("description");
            routeDt.Columns.Add("is_active");
            routeDt.Columns.Add("update_time");
            return ds;
        }
        #endregion
    }
}
