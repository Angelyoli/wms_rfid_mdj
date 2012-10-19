using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WMS.Upload.Dao
{
    public class UpdateUploadDao:BaseDao
    {
        /// <summary>
        /// ���ݵ��ݺźͲ�Ʒ���Ʋ�ѯ�ϱ��������ϸ����Ϣ
        /// </summary>
        /// <param name="billno"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public DataTable GetByOutInfo(string billno, string product, string tableName)
        {
            string sql = string.Format("SELECT * FROM {2} WHERE STORE_BILL_ID='{0}' AND BRAND_CODE='{1}'", billno, product, tableName);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ѯ��ǰ����������
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public decimal FindPieceQuantity(string product, string areaType)
        {
            string sql = @"SELECT ISNULL(SUM(QTY_STA),0)/(SELECT STANDARDRATE FROM WMS_UNIT U
                            LEFT JOIN WMS_PRODUCT P ON U.UNITCODE=P.TIAOCODE
                            WHERE P.PRODUCTCODE =C.CURRENTPRODUCT) AS QUANTITY FROM V_WMS_WH_CELL C
                            WHERE CURRENTPRODUCT='{0}' AND AREATYPE='{1}' GROUP BY CURRENTPRODUCT";
            sql = string.Format(sql, product, areaType);
            return Convert.ToDecimal(this.ExecuteScalar(sql));
        }

        /// <summary>
        /// ��ѯ��ǰ��������
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public decimal FindBarQuantity(string product, string areaType)
        {
            string sql = @"SELECT ISNULL(SUM(QUANTITY),0) AS QUANTITY FROM V_WMS_WH_CELL WHERE CURRENTPRODUCT='{0}'AND AREATYPE='{1}'";
            sql = string.Format(sql, product, areaType);
            return Convert.ToDecimal(this.ExecuteScalar(sql));
        }

        /// <summary>
        /// ���ݵ��ݺŲ�ѯ�ϱ������������Ϣ
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public DataTable GetByOutInfo(string billno, string tableName)
        {
            string sql = string.Format("SELECT * FROM {1} WHERE STORE_BILL_ID='{0}'", billno, tableName);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���ݻ�λ�����ѯ����
        /// </summary>
        /// <param name="cellcode"></param>
        /// <returns></returns>
        public string GetCellCodeByName(string cellcode)
        {
            string sql = string.Format("SELECT CELLNAME FROM WMS_WH_CELL WHERE CELLCODE ='{0}'", cellcode);
            return this.ExecuteScalar(sql).ToString();
        }
        /// <summary>
        /// ���ݻ�λ�����ѯ����
        /// </summary>
        /// <param name="cellcode"></param>
        /// <returns>unitcode</returns>
        public string GetCellCodeCodeByName(string cellcode)
        {
            string sql = string.Format("SELECT UNITCODE FROM WMS_WH_CELL WHERE CELLCODE ='{0}'", cellcode);
            return this.ExecuteScalar(sql).ToString();
        }
        /// <summary>
        /// ���ݴ����ı�����ѯһ���ձ�
        /// </summary>
        /// <returns></returns>
        public DataTable QueryBusiBill(string outInfoTable)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE 1=0", outInfoTable);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// �����ϴ������̵ĳ������ҵ���
        /// </summary>
        public void InsertBull(DataTable table, string outInfoTable)
        {
            BatchInsert(table, outInfoTable);
        }

        /// <summary>
        /// �������ں��û��޸ĳ�������ս���Ա���ڵȡ�
        /// </summary>
        /// <param name="uase"></param>
        /// <param name="datetime"></param>
        public void UpdateDate(string uase, string datetime)
        {
            string sql = string.Format("UPDATE DWV_IWMS_OUT_BUSI_BILL SET RECKON_STATUS='1',RECKON_DATE='201'+substring(BUSI_BILL_DETAIL_ID,1,5),UPDATE_CODE='{0}',UPDATE_DATE='{1}'  WHERE IS_IMPORT='0'", uase, datetime);
            this.ExecuteNonQuery(sql);
            sql = string.Format("UPDATE DWV_IWMS_IN_BUSI_BILL SET RECKON_STATUS='1',RECKON_DATE='201'+substring(BUSI_BILL_DETAIL_ID,1,5),UPDATE_CODE='{0}',UPDATE_DATE='{1}'  WHERE IS_IMPORT='0'", uase, datetime);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// �ֶ�������ݵ����̱�
        /// </summary>
        /// <param name="masterTable"></param>
        /// <param name="table"></param>
        public void InsertMaster(string infoTable, DataTable table)
        {
            BatchInsert(table, infoTable);
        }

        /// <summary>
        /// ִ���޸Ĳ���
        /// </summary>
        /// <param name="sql"></param>
        public void UpdateTable(string sql)
        {
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// ִ�в�ѯ����������һ��ֵ
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetDate(string sql)
        {
            return Convert.ToString(this.ExecuteScalar(sql));
        }


        /// <summary>
        /// ��ȡ��Ʒ����
        /// </summary>
        /// <param name="unitCode"></param>
        /// <returns></returns>
        public DataTable ProductRate(string productCode)
        {
            string sql = @"SELECT A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.JIANCODE=B.UNITCODE AND A.PRODUCTCODE='{0}') AS JIANRATE,
                (SELECT B.STANDARDRATE FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE A.TIAOCODE=B.UNITCODE AND A.PRODUCTCODE='{0}') AS TIAORATE 
                 FROM WMS_PRODUCT AS A,WMS_UNIT AS B WHERE  A.PRODUCTCODE='{0}' GROUP BY A.PRODUCTCODE,A.PRODUCTNAME,A.UNITCODE,A.JIANCODE,A.TIAOCODE";
            sql = string.Format(sql, productCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ȡ�������ı���
        /// </summary>
        /// <returns></returns>
        public string GetCompany()
        {
            //string sql = "SELECT DIST_CTR_CODE FROM DWV_OUT_DIST_CTR";
            //return this.ExecuteScalar(sql).ToString();
            return "0101";
        }


        public void SetData(string sql)
        {
            ExecuteNonQuery(sql);
        }
    }
}
