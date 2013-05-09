using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;


namespace THOK.WMS.DownloadWms.Bll
{
    public class DownDeptBll
    {
        #region ��Ӫ��ϵͳ���ز�����Ϣ

        /// <summary>
        /// ���ز�����Ϣ
        /// </summary>
        /// <returns></returns>
        public bool DownDeptInfo()
        {
            bool tag = true;
            DataTable deptCodeDt = this.GetDeptCode();
            string deptCodeList = UtinString.MakeString(deptCodeDt, "DEPTCODE");
            deptCodeList = "DEPT_CODE NOT IN (" + deptCodeList + ")";
            DataTable deptDt = this.GetDeptInfo(deptCodeList);
            if (deptDt.Rows.Count > 0)
            {
                DataSet deptDs = this.Insert(deptDt);
                this.Insert(deptDs);
            }
            else
                tag = false;

            return tag;
        }

        /// <summary>
        /// ��������������ӵ����ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet ds)
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownDeptDao dao = new DownDeptDao();
                dao.Insert(ds);
            }
        }

        /// <summary>
        /// ����Ӫ��ϵͳ������Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeptInfo(string deptCode)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownDeptDao dao = new DownDeptDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetDeptInfo(deptCode);
            }
        }

        /// <summary>
        /// ��ѯ���ֲִ����ű��
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeptCode()
        {
            using (PersistentManager dbPm = new PersistentManager())
            {
                DownDeptDao dao = new DownDeptDao();
                return dao.GetDeptCode();
            }
        }

        /// <summary>
        /// ������ݵ��������
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DataSet Insert(DataTable deptTable)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in deptTable.Rows)
            {
                DataRow deptDr = ds.Tables["BI_DEPARTMENT"].NewRow();
                deptDr["DEPTCODE"] = row["DEPT_CODE"].ToString().Trim();
                deptDr["DEPTNAME"] = row["DEPT_NAME"].ToString().Trim();
                deptDr["DEPTLEADER"] = "";
                deptDr["ISACTIVE"] = row["ISACTIVE"];
                deptDr["WARECODE"] = "001";
                deptDr["MEMO"] = "";
                ds.Tables["BI_DEPARTMENT"].Rows.Add(deptDr);
            }
            return ds;
        }

        /// <summary>
        /// �����й��������
        /// </summary>
        /// <returns></returns>
        public DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable deptDt = ds.Tables.Add("BI_DEPARTMENT");
            deptDt.Columns.Add("DEPTCODE");
            deptDt.Columns.Add("DEPTNAME");
            deptDt.Columns.Add("DEPTLEADER");
            deptDt.Columns.Add("ISACTIVE");
            deptDt.Columns.Add("WARECODE");
            deptDt.Columns.Add("MEMO");
            return ds;
        }
        #endregion
    }
}
