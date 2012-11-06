using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.WMS.DownloadWms.Dao;

namespace THOK.WMS.DownloadWms.Bll
{
    public class DownUnitBll
    {
        #region ��Ӫ��ϵͳ���ص�λ����

        /// <summary>
        /// ���ص�λ����
        /// </summary>
        /// <returns></returns>
        public bool DownUnitCodeInfo()
        {
            bool tag = true;
            try
            {
                DataTable unitProductDt = this.GetUnitProduct();//ȡ���м������
                string productcodeList = UtinString.MakeString(unitProductDt, "UNIT_LIST_CODE");
                productcodeList = "BRAND_N NOT IN(" + productcodeList + ")";

                DataTable Unitdt = this.GetUnitInfo(productcodeList);//��������
                if (Unitdt.Rows.Count > 0)
                {
                    DataTable codedt = this.GetUnitCode();//ȡ�õ�λ
                    DataSet ds = this.InsertTable(Unitdt);//������ת���ŵ���λ����м��
                    DataTable unitTable = ds.Tables["WMS_UNIT_INSERT"];//ȡ�õ�λ��
                    DataTable unitListTable = ds.Tables["WMS_UNIT_PRODUCT"];//ȡ�õ�λϵ�б�
                    string codeList = UtinString.MakeString(codedt, "unit_code");
                    DataRow[] unitdr = unitTable.Select("UNIT_CODE NOT IN (" + codeList + ")");//��û�����ع���ȡ����
                    DataSet unitDs = this.InsertUnit(unitdr);
                    DataSet unitlistds = this.InsertProduct(unitListTable);
                    if (unitDs.Tables["WMS_UNIT_INSERT"].Rows.Count > 0)
                    {
                        this.Insert(unitDs.Tables["WMS_UNIT_INSERT"]);
                    }
                    if (unitlistds.Tables["WMS_UNIT_PRODUCT"].Rows.Count > 0)
                    {
                        this.InsertLcUnit(unitlistds.Tables["WMS_UNIT_PRODUCT"]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("���ص�λ��ʧ�ܣ�ԭ��" + e.Message);
            }
            return tag;
        }

        /// <summary>
        /// ��ȡ��λϵͳ���ֶ�
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitProduct()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                return dao.GetUnitProduct();
            }
        }

        /// <summary>
        /// ���ص�λ����
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfo(string unitCodeList)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetUnitInfo(unitCodeList);
            }
        }
        /// <summary>
        /// ���ص�λ���� ����
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfos(string unitCodeList)
        {
            using (PersistentManager dbPm = new PersistentManager("YXConnection"))
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(dbPm);
                return dao.GetUnitInfos(unitCodeList);
            }
        }


        /// <summary>
        /// ��ѯ���ֲִ���λ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitCode()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                return dao.GetUnitCode();
            }
        }

        /// <summary>
        /// ��ת�����������ӵ������
        /// </summary>
        /// <param name="unitdr"></param>
        /// <returns></returns>
        public DataSet InsertUnit(DataRow[] unitdr)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in unitdr)
            {
                DataRow dr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                dr["unit_code"] = row["unit_code"];
                dr["unit_name"] = row["unit_name"];
                dr["count"] = row["count"];
                dr["is_active"] = row["is_active"];
                dr["update_time"] = row["update_time"];              
                ds.Tables["WMS_UNIT_INSERT"].Rows.Add(dr);
            }
            return ds;
        }

        /// <summary>
        /// ��ת�����������ӵ��м������
        /// </summary>
        /// <param name="unitdr"></param>
        /// <returns></returns>
        public DataSet InsertProduct(DataTable productdr)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in productdr.Rows)
            {
                DataRow dr = ds.Tables["WMS_UNIT_PRODUCT"].NewRow();
                dr["unit_list_code"] = row["unit_list_code"];
                dr["uniform_code"] = "1";
                dr["unit_list_name"] = row["unit_list_name"];
                dr["unit_code01"] = row["unit_code01"];
                dr["quantity01"] = Convert.ToDecimal(row["quantity01"]);
                dr["unit_code02"] = row["unit_code02"];
                dr["quantity02"] = Convert.ToDecimal(row["quantity02"]);
                dr["unit_code03"] = row["unit_code03"];
                dr["quantity03"] = Convert.ToDecimal(row["quantity03"]);
                dr["unit_code04"] = row["unit_code04"];
                dr["is_active"] = 1;
                dr["update_time"] = DateTime.Now;
                ds.Tables["WMS_UNIT_PRODUCT"].Rows.Add(dr);
            }
            return ds;
        }


        /// <summary>
        /// ��ת�����������ӵ��м������ ����
        /// </summary>
        /// <param name="unitdr"></param>
        /// <returns></returns>
        public DataSet InsertProducts(DataTable productdr)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in productdr.Rows)
            {
                DataRow dr = ds.Tables["WMS_UNIT_PRODUCT"].NewRow();
                dr["unit_list_code"] = row["BRAND_ULIST_CODE"];
                dr["uniform_code"] = "1";
                dr["unit_list_name"] = row["BRAND_ULIST_NAME"];
                dr["unit_code01"] = row["BRAND_UNIT_CODE_01"];
                dr["quantity01"] = Convert.ToDecimal(row["QTY_01"]);
                dr["unit_code02"] = row["BRAND_UNIT_CODE_02"];
                dr["quantity02"] = Convert.ToDecimal(row["QTY_02"]);
                dr["unit_code03"] = row["BRAND_UNIT_CODE_03"];
                dr["quantity03"] = Convert.ToDecimal(row["QTY_03"]);
                dr["unit_code04"] = row["BRAND_UNIT_CODE_04"];
                dr["is_active"] = 1;
                dr["update_time"] = DateTime.Now;
                ds.Tables["WMS_UNIT_PRODUCT"].Rows.Add(dr);
            }
            return ds;
        }
        /// <summary>
        /// ��λ���������
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataTable unitTable)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                dao.InsertUnit(unitTable);
            }
        }

        /// <summary>
        /// ��λϵ�б��������
        /// </summary>
        /// <param name="ds"></param>
        public void InsertLcUnit(DataTable lcUnitTable)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                dao.InsertLcUnit(lcUnitTable);
            }
        }

        /// <summary>
        /// ������ת����ӵ������
        /// </summary>
        /// <param name="unitdr"></param>
        /// <returns></returns>
        public DataSet InsertTable(DataTable unittable)
        {
            DataSet ds = this.GenerateEmptyTables();
            string zhi = "";
            //��ȡ֧��Ӧ�ľ��̱���ת������
            DataRow[] zhiDr = unittable.Select("BRAND_UNIT_CODE=01 AND BRAND_UNIT_NAME <>'null' ", "COUNT ASC");
            foreach (DataRow row in zhiDr)
            {
                if (row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString() != zhi)
                {
                    DataRow zhidr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                    zhidr["unit_code"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    zhidr["unit_name"] = row["BRAND_UNIT_NAME"];
                    zhidr["is_active"] = "1";
                    zhidr["count"] = row["COUNT"];
                    zhidr["update_time"] = DateTime.Now;
                    zhi = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    ds.Tables["WMS_UNIT_INSERT"].Rows.Add(zhidr);
                }
                //��֧�ļ�����λ�;��̱���浽�м��
                DataRow UnitzhiDr = ds.Tables["WMS_UNIT_PRODUCT"].NewRow();
                UnitzhiDr["unit_code04"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                UnitzhiDr["unit_list_code"] = row["BRAND_N"].ToString().Trim();
                UnitzhiDr["unit_list_name"] = row["BRAND_N"].ToString().Trim() + "(" + row["BRAND_UNIT_NAME"] + ")";
                UnitzhiDr["is_active"] = 1;
                UnitzhiDr["uniform_code"] = "1001";
                UnitzhiDr["update_time"] = DateTime.Now;
                ds.Tables["WMS_UNIT_PRODUCT"].Rows.Add(UnitzhiDr);
            }

            string he = "";
            //��ȡ�����ߺж�Ӧ֧��ת������
            DataRow[] heDr = unittable.Select("BRAND_UNIT_CODE=02 AND BRAND_UNIT_NAME <>'null' ", "COUNT ASC");
            foreach (DataRow row in heDr)
            {
                if (row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString() != he)
                {
                    DataRow hedr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                    hedr["unit_code"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    hedr["unit_name"] = row["BRAND_UNIT_NAME"];
                    hedr["is_active"] = "1";
                    hedr["count"] = row["COUNT"];
                    hedr["update_time"] = DateTime.Now;
                    he = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    ds.Tables["WMS_UNIT_INSERT"].Rows.Add(hedr);
                }
                DataRow[] brandhe = ds.Tables["WMS_UNIT_PRODUCT"].Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                brandhe[0]["unit_code03"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                brandhe[0]["quantity03"] =  Convert.ToDecimal(row["COUNT"]);
            }

            string tiao = "";
            //��ȡ����Ӧ֧��ת������
            DataRow[] tiaoDr = unittable.Select("BRAND_UNIT_CODE=03 AND BRAND_UNIT_NAME <>'null' ", "COUNT ASC");
            foreach (DataRow row in tiaoDr)
            {
                if (row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString() != tiao)
                {
                    DataRow tiaodr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                    tiaodr["unit_code"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    tiaodr["unit_name"] = row["BRAND_UNIT_NAME"];
                    tiaodr["is_active"] = "1";
                    tiaodr["count"] = row["COUNT"];
                    tiaodr["update_time"] = DateTime.Now;
                    tiao = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    ds.Tables["WMS_UNIT_INSERT"].Rows.Add(tiaodr);
                }
                DataRow[] tiaobybao = unittable.Select("BRAND_UNIT_CODE=02 AND BRAND_N ='" + row["BRAND_N"].ToString().Trim() + "' ");
                DataRow[] brandtiao = ds.Tables["WMS_UNIT_PRODUCT"].Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                brandtiao[0]["unit_code02"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                brandtiao[0]["quantity02"] = Convert.ToDecimal(row["COUNT"]) / Convert.ToDecimal(tiaobybao[0]["COUNT"]);
            }

            string jian = "";
            //��ȡ����Ӧ֧��ת������
            DataRow[] jianDr = unittable.Select("BRAND_UNIT_CODE=04 AND BRAND_UNIT_NAME <>'null' ", "COUNT ASC");
            foreach (DataRow row in jianDr)
            {
                if (row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString() != jian)
                {
                    DataRow jiandr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                    jiandr["unit_code"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    jiandr["unit_name"] = row["BRAND_UNIT_NAME"];
                    jiandr["is_active"] = "1";
                    jiandr["count"] = row["COUNT"];
                    jiandr["update_time"] = DateTime.Now;
                    jian = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                    ds.Tables["WMS_UNIT_INSERT"].Rows.Add(jiandr);
                }
                DataRow[] jianbytiao = unittable.Select("BRAND_UNIT_CODE=03 AND BRAND_N ='" + row["BRAND_N"].ToString().Trim() + "' ", "COUNT ASC");
                DataRow[] brandjian = ds.Tables["WMS_UNIT_PRODUCT"].Select(string.Format("unit_list_code='{0}'", row["BRAND_N"].ToString().Trim()));
                brandjian[0]["unit_code01"] = row["BRAND_UNIT_CODE"].ToString() + "_" + row["COUNT"].ToString();
                brandjian[0]["quantity01"] = Convert.ToDecimal(row["COUNT"]) / Convert.ToDecimal(jianbytiao[0]["COUNT"]);
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
            DataTable intable = ds.Tables.Add("WMS_UNIT_INSERT");
            intable.Columns.Add("unit_code");
            intable.Columns.Add("unit_name");
            intable.Columns.Add("count");
            intable.Columns.Add("is_active");
            intable.Columns.Add("update_time");
            intable.Columns.Add("row_version");

            DataTable uptable = ds.Tables.Add("WMS_UNIT_PRODUCT");
            uptable.Columns.Add("unit_list_code");
            uptable.Columns.Add("uniform_code");
            uptable.Columns.Add("unit_list_name");
            uptable.Columns.Add("unit_code01");
            uptable.Columns.Add("quantity01");
            uptable.Columns.Add("unit_code02");
            uptable.Columns.Add("quantity02");
            uptable.Columns.Add("unit_code03");
            uptable.Columns.Add("quantity03");
            uptable.Columns.Add("unit_code04");
            uptable.Columns.Add("is_active");
            uptable.Columns.Add("update_time");
            return ds;
        }

        #endregion

        #region ��Ӫ��ϵͳ���ص�λ���� ����

        /// <summary>
        /// ���ص�λ���� ����
        /// </summary>
        /// <returns></returns>
        public bool DownUnitInfo()
        {
            bool tag = true;
            //���ص�λ��Ϣ
            DataTable unitCodeTable = this.GetUnitCode();
            string unitCodeList = UtinString.StringMake(unitCodeTable, "unit_code");
            unitCodeList = UtinString.StringMake(unitCodeList);
            unitCodeList = "BRAND_UNIT_CODE NOT IN (" + unitCodeList + ")";
            DataTable brandUnitCodeTable = this.GetUnitInfos(unitCodeList);
            if (brandUnitCodeTable.Rows.Count > 0)
            {
                DataSet unitCodeDs = this.InsertUnit(brandUnitCodeTable);
                this.Insert(unitCodeDs);
            }
            else
                tag = false;

            //���ؼ�����λϵ������
            DataTable ulistCodeTable = this.GetUlistCode();
            string ulistCodeList = UtinString.StringMake(ulistCodeTable, "unit_list_code");
            ulistCodeList = UtinString.StringMake(ulistCodeList);
            ulistCodeList = "BRAND_ULIST_CODE NOT IN (" + ulistCodeList + ")";
            DataTable brandUlistCodeTable = this.GetBrandUlist(ulistCodeList);
            if (brandUlistCodeTable.Rows.Count > 0)
            {
                DataSet unitListCodeDs = this.InsertProducts(brandUlistCodeTable);
                this.InsertUlist(unitListCodeDs.Tables["WMS_UNIT_PRODUCT"]);
            }
            else
                tag = false;
            return tag;
        }

        /// <summary>
        /// ���ؼ�����λϵ�б��
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandUlist(string ulistCodeList)
        {
            using (PersistentManager pm = new PersistentManager("YXConnection"))
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(pm);
                return dao.GetBrandUlistInfo(ulistCodeList);
            }
        }

        /// <summary>
        /// ��ѯ������λϵ�б��
        /// </summary>
        /// <returns></returns>
        public DataTable GetUlistCode()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(pm);
                return dao.GetUlistCode();
            }
        }

        /// <summary>
        /// �ѵ�λ������ӵ����ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void Insert(DataSet unitCodeDs)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(pm);
                if (unitCodeDs.Tables["WMS_UNIT_INSERT"].Rows.Count > 0)
                {
                    dao.InsertUnit(unitCodeDs.Tables["WMS_UNIT_INSERT"]);
                }
            }
        }

        /// <summary>
        /// �ѵ�λ������ӵ����ݿ�
        /// </summary>
        /// <param name="ds"></param>
        public void InsertUlist(DataTable ulistCodeTable)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                DownUnitDao dao = new DownUnitDao();
                dao.SetPersistentManager(pm);
                dao.InsertUlist(ulistCodeTable);
            }
        }

        /// <summary>
        /// ��ת�����������ӵ������
        /// </summary>
        /// <param name="unitdr"></param>
        /// <returns></returns>
        public DataSet InsertUnit(DataTable unitCodeTable)
        {
            DataSet ds = this.GenerateEmptyTables();
            foreach (DataRow row in unitCodeTable.Rows)
            {
                DataRow dr = ds.Tables["WMS_UNIT_INSERT"].NewRow();
                dr["unit_code"] = row["BRAND_UNIT_CODE"];
                dr["unit_name"] = row["BRAND_UNIT_NAME"];
                dr["count"] = row["count"];
                dr["is_active"] = row["ISACTIVE"];
                dr["update_time"] = DateTime.Now.ToString();
                ds.Tables["WMS_UNIT_INSERT"].Rows.Add(dr);
            }
            return ds;
        }
        #endregion

    }
}
