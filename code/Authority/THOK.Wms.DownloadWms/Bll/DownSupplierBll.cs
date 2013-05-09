using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownSupplierBll
    {
        #region ���˳�Ӫϵͳ�����س�������

        /// <summary>
        /// ���س�������
        /// </summary>
        /// <returns></returns>
        public bool DownSupplierInfo()
        {
            bool tag = true;
            DataTable suppliercodedt = this.GetSupplierCode();
            string codeList = UtinString.MakeString(suppliercodedt, "SUPPLIERCODE");
            codeList = "FACTORY_CODE NOT IN (" + codeList + ")";
            DataTable spplierTable = this.GetSpplierInfo(codeList);
            if (spplierTable.Rows.Count > 0)
            {
                DataSet spplierds = this.Insert(spplierTable);
                this.InsertSpplier(spplierds);
            }
            else
            {
                tag = false;
            }
            return tag;
        }


        /// <summary>
        /// ���س�����Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetSpplierInfo(string spplierCode)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownSupplierDao dao = new DownSupplierDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetSupplierInfo(spplierCode);
            }
        }

        /// <summary>
        /// ��ѯ��Ӧ�̱��
        /// </summary>
        /// <returns></returns>
        public DataTable GetSupplierCode()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownSupplierDao dao = new DownSupplierDao();
                return dao.GetSupplierCode();
            }
        }
        

        /// <summary>
        /// �����ص����ݲ��뵽���ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void InsertSpplier(DataSet ds)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownSupplierDao dao = new DownSupplierDao();
                dao.Insert(ds);
            }
        }

        /// <summary>
        /// �����ص����ݴ���������
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataSet Insert(DataTable spplierTable)
        {
            DataSet ds = this.GenerateEmptyTables();

            foreach (DataRow row in spplierTable.Rows)
            {
                DataRow sudr = ds.Tables["BI_SUPPLIER_INSERT"].NewRow();
                sudr["SUPPLIERCODE"] = row["FACTORY_CODE"];
                sudr["SUPPLIERNAME"] = row["FACTORY_NAME"];
                sudr["FACTORY_N"] = row["FACTORY_N"];
                sudr["PROVINCE_NAME"]=row["PROVINCE_NAME"];
                sudr["TEL"] = "";// row["TEL"];
                sudr["FAX"] = "";//
                sudr["CONTECTPERSON"] = "";// row["CONTECTPERSON"];
                sudr["ADDRESS"] = "";//row["ADDRESS"];
                sudr["ZIP"] = "";//
                sudr["BANKACCOUNT"] = "";//
                sudr["BANKNAME"] = "";//
                sudr["TAXNO"] = "";//
                sudr["CREDITGRADE"] = "";//
                sudr["ISACTIVE"] = row["ISACTIVE"];
                sudr["MEMO"] = "";//
                ds.Tables["BI_SUPPLIER_INSERT"].Rows.Add(sudr);
            }
            return ds;
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        private DataSet GenerateEmptyTables()
        {
            DataSet ds = new DataSet();
            DataTable intable = ds.Tables.Add("BI_SUPPLIER_INSERT");
            intable.Columns.Add("SUPPLIERCODE");
            intable.Columns.Add("SUPPLIERNAME");
            intable.Columns.Add("FACTORY_N");
            intable.Columns.Add("PROVINCE_NAME");
            intable.Columns.Add("TEL");
            intable.Columns.Add("FAX");
            intable.Columns.Add("CONTECTPERSON");
            intable.Columns.Add("ADDRESS");
            intable.Columns.Add("ZIP");
            intable.Columns.Add("BANKACCOUNT");
            intable.Columns.Add("BANKNAME");
            intable.Columns.Add("TAXNO");
            intable.Columns.Add("CREDITGRADE");
            intable.Columns.Add("ISACTIVE");
            intable.Columns.Add("MEMO");

            DataTable uptable = ds.Tables.Add("BI_SUPPLIER_UPDATE");
            uptable.Columns.Add("SUPPLIERCODE");
            uptable.Columns.Add("SUPPLIERNAME");
            uptable.Columns.Add("TEL");
            uptable.Columns.Add("FAX");
            uptable.Columns.Add("CONTECTPERSON");
            uptable.Columns.Add("ADDRESS");
            uptable.Columns.Add("ZIP");
            uptable.Columns.Add("BANKACCOUNT");
            uptable.Columns.Add("BANKNAME");
            uptable.Columns.Add("TAXNO");
            uptable.Columns.Add("CREDITGRADE");
            uptable.Columns.Add("ISACTIVE");
            uptable.Columns.Add("MEMO");
            return ds;
        }

        #endregion
    }
}
