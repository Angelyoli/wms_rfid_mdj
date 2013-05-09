using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownReachBll
   {
       #region ��Ӫ��ϵͳ�����ͻ�����
       /// <summary>
       /// �����ͻ������
       /// </summary>
       /// <returns></returns>
       public bool DownReachInfo()
       {
           bool tag = true;
           DataTable reachCodeDt = this.QueryReachCode();
           string reachCodeList = UtinString.MakeString(reachCodeDt, "DIST_STA_CODE");
           reachCodeList = "DIST_STA_CODE NOT IN(" + reachCodeList + ")";
           DataTable reachDt = this.GetReachInfo(reachCodeList);
           if (reachDt.Rows.Count > 0)
               this.Insert(reachDt);
           else
               tag = false;

           return tag;
       }

       /// <summary>
       /// �����ͻ������
       /// </summary>
       /// <returns></returns>
       public bool GetDownReachInfo()
       {
           bool tag = true;
           this.Delete();//����ǰ���Ŷ�ͻ������
           DataTable reachDt = this.GetReachInfo();
           if (reachDt.Rows.Count > 0)
               this.Insert(reachDt);
           else
               tag = false;
           return tag;
       }


       /// <summary>
       /// ����ͻ������
       /// </summary>
       public void Delete()
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownReachDao dao = new DownReachDao();
               dao.Delete();
           }
       }

       /// <summary>
       /// �����ͻ�����
       /// </summary>
       /// <returns></returns>
       public DataTable GetReachInfo()
       {
           using (PersistentManager dbPm = new PersistentManager("YXConnection"))
           {
               DownReachDao dao = new DownReachDao();
               dao.SetPersistentManager(dbPm);
               return dao.GetReachInfo();
           }
       }

       /// <summary>
       /// ���ݱ�������ͻ�����
       /// </summary>
       /// <returns></returns>
       public DataTable GetReachInfo(string reachCode)
       {
           using (PersistentManager dbPm = new PersistentManager("YXConnection"))
           {
               DownReachDao dao = new DownReachDao();
               dao.SetPersistentManager(dbPm);
               return dao.GetReachInfo(reachCode);
           }
       }

       /// <summary>
       /// ��ѯ�ͻ��������
       /// </summary>
       /// <returns></returns>
       public DataTable QueryReachCode()
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownReachDao dao = new DownReachDao();
               return dao.QueryReachCode();
           }
       }

        /// <summary>
       /// ������ݵ����ݿ�
       /// </summary>
       /// <param name="reachDt"></param>
       public void Insert(DataTable reachDt)
       {
           using (PersistentManager dbPm = new PersistentManager())
           {
               DownReachDao dao = new DownReachDao();
               dao.Insert(reachDt);
           }
       }
       #endregion
   }
}
