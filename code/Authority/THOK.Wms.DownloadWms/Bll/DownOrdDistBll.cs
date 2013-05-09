using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownOrdDistBll
    {
        #region ��Ӫ��ϵͳ�����䳵����Ϣ
        /// <summary>
        /// �����䳵����Ϣ
        /// </summary>
        /// <returns></returns>
        public bool DownOrgDistBillInfo()
        {
            bool tag = true;
            DataTable orgTable = this.QueryOrgDistCode();
            string distCodeList = UtinString.MakeString(orgTable, "DIST_BILL_ID");
            distCodeList = "DIST_BILL_ID NOT IN (" + distCodeList + ")";
            DataTable bistBillMasterTable = this.GetDistBillMaster(distCodeList);
            if (bistBillMasterTable.Rows.Count > 0)
                this.Insert(bistBillMasterTable);
            else
                tag = false;
            return tag;
        }

        /// <summary>
        /// �����䳵��������Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetDistBillMaster(string bistMaster)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownOrdDistDao dao = new DownOrdDistDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetDistBillMaster(bistMaster);
            }
        }

        /// <summary>
        /// �����䳵��ϸ����Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetDistBillDetail(string bistDetail)
        {
            using (PersistentManager dbpm = new PersistentManager("YXConnection"))
            {
                DownOrdDistDao dao = new DownOrdDistDao();
                dao.SetPersistentManager(dbpm);
                return dao.GetDistBillDetail(bistDetail);
            }
        }

        /// <summary>
        /// ��ѯ�����ع����䳵������
        /// </summary>
        /// <returns></returns>
        public DataTable QueryOrgDistCode()
        {
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownOrdDistDao dao = new DownOrdDistDao();
                return dao.QueryOrgDistCode();
            }
        }

        /// <summary>
        /// �����ص����ݲ������ݿ�
        /// </summary>
        /// <param name="orgDistBillTable"></param>
        public void Insert(DataTable bistBillMasterTable)
        {
            using (PersistentManager dbpm = new PersistentManager())
            {
                DownOrdDistDao dao = new DownOrdDistDao();
                dao.Insert(bistBillMasterTable);
            }
        }
        #endregion
    }
}
