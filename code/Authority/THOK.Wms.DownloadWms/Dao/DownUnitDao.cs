using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownUnitDao : BaseDao
    {
        /// <summary>
        /// ���ص�λ��Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfo(string unitCode)
        {
            string sql = string.Format(@"SELECT U.*,B.BRAND_N FROM V_WMS_BRAND_UNIT U
                                        LEFT JOIN  V_WMS_BRAND B ON U.BRAND_CODE =B.BRAND_CODE
                                        WHERE (B.BRAND_N <> 'NULL' OR B.BRAND_N !='') and {0}", unitCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// ���ص�λ��Ϣ ƽ��ɽ
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitInfos(string unitCode)
        {
            string sql = string.Format("SELECT * FROM IC.V_WMS_BRAND_UNIT WHERE {0}", unitCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ѯ������λϵ�б�
        /// </summary>
        /// <param name="ulistCode"></param>
        /// <returns></returns>
        public DataTable GetBrandUlistInfo(string ulistCode)
        {
            string sql = string.Format("SELECT * FROM IC.V_WMS_BRAND_ULIST WHERE {0}", ulistCode);
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// ƽ��ɽ���뵥λϵ�б�
        /// </summary>
        /// <param name="ds"></param>
        public void InsertUlist(DataTable ulistCodeTable)
        {
            BatchInsert(ulistCodeTable, "wms_unit_list");
        }

        /// <summary>
        /// ��ѯ�ִ���λϵ�б��
        /// </summary>
        /// <returns></returns>
        public DataTable GetUlistCode()
        {
            string sql = "SELECT unit_list_code FROM wms_unit_list";
            return this.ExecuteQuery(sql).Tables[0];
        }
        /// <summary>
        /// ��λ���������
        /// </summary>
        /// <param name="ds"></param>
        public void InsertUnit(DataTable unitTable)
        {
            BatchInsert(unitTable, "wms_unit");
        }

        /// <summary>
        /// ���뵥λϵ�б�
        /// </summary>
        /// <param name="ds"></param>
        public void InsertLcUnit(DataTable unitTable)
        {
            BatchInsert(unitTable, "wms_unit_list");
        }

        /// <summary>
        /// ��ѯ�ִ���λ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitCode()
        {
            string sql = "SELECT unit_code FROM WMS_UNIT";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ��ѯ��λϵ�б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnitProduct()
        {
            string sql = "SELECT * FROM WMS_UNIT_LIST";
            return this.ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// ���ݵ�λϵ�б����ѯ��λϵ�б�
        /// </summary>
        /// <param name="unitListCode"></param>
        /// <returns></returns>
        public DataTable FindUnitListCOde(string unitListCode)
        {
            string sql = string.Format("SELECT * FROM WMS_UNIT_LIST WHERE UNIT_LIST_CODE='{0}'", unitListCode);
            return this.ExecuteQuery(sql).Tables[0];
        }

    }
}
