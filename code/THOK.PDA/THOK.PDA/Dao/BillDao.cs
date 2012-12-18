using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace THOK.PDA.Dao
{
    public class BillDao
    {
        /// <summary>
        /// @获取主单据
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public DataTable GetBillMaster(string billType)
        {
            string sql = @"SELECT BILLMASTERID FROM BILLMASTER 
                            WHERE STATE='5' AND BILLCODE=@BILLTYPE";
            SqlParameter[] p = new SqlParameter[] {            
               new  SqlParameter("@BILLTYPE",billType)
           };
            return DBHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// @获取明细单据
        /// </summary>
        /// <param name="billId">主单据号</param>
        /// <returns></returns>
        public DataTable GetBillDetailListByBillId(string billId)
        {
            string sql = @"SELECT A.STORAGEID,A.TOBACCONAME,C.STATENAME,B.OPERATENAME,DETAILID 
                            FROM BILLDETAIL A 
                            LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE 
                            LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE 
                            LEFT JOIN STORAGE D ON A.STORAGEID = D.STORAGEID 
                            WHERE A.BILLMASTERID=@BILLID AND CONFIRMSTATE!='3' 
                            ORDER BY A.CONFIRMSTATE, A.OPERATOR,A.STORAGEID,A.DETAILID";
            SqlParameter[] p = new SqlParameter[] { 
           
               new  SqlParameter("@BILLID",billId)
           };
            return DBHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// @获取明细单据
        /// </summary>
        /// <param name="billId">主单据id</param>
        /// <param name="detailId">明细单据id</param>
        /// <returns></returns>
        public DataTable GetBillDetailByDetailId(string billId, string detailId)
        {
            string sql = @"SELECT DETAILID,A.STORAGEID,A.TARGETSTORAGE MOVESTORAGE,
                            B.OPERATENAME,A.TOBACCONAME,OPERATEPIECE,OPERATEITEM,A.OPERATECODE,C.STATENAME,OPERATOR 
                            FROM BILLDETAIL A 
                            LEFT JOIN OPERATETYPE B ON A.OPERATECODE=B.OPERATECODE
                            LEFT JOIN STATETYPE C ON A.CONFIRMSTATE=C.STATECODE 
                            LEFT JOIN STORAGE D ON A.STORAGEID = D.STORAGEID 
                            WHERE A.BILLMASTERID = @BILLID AND DETAILID = @DETAILID ";
            SqlParameter[] p = new SqlParameter[] {            
               new  SqlParameter("@DETAILID",detailId),
                new  SqlParameter("@BILLID",billId)
           };

            return DBHelper.GetDataTable(sql, p);
        }

        //@
        public DataRow GetBillDetailCompleteInfo(string billId, string detailId)
        {
            string sql = @"SELECT * FROM BILLDETAIL 
                            WHERE BILLMASTERID = @BILLID AND DETAILID = @DETAILID ";
            SqlParameter[] p = new SqlParameter[] {            
               new  SqlParameter("@DETAILID",detailId),
                new  SqlParameter("@BILLID",billId)
           };
            return DBHelper.GetDataTable(sql, p).Rows[0];
        }

        //@
        public void UpdateDetailState(string billId, string detailId)
        {
            string sql = @"UPDATE BILLDETAIL SET CONFIRMSTATE = '2', OPERATOR = 'PDA' 
                            WHERE BILLMASTERID = @BILLID AND DETAILID = @DETAILID ";
            SqlParameter[] p = new SqlParameter[] {            
               new  SqlParameter("@DETAILID",detailId),
                new  SqlParameter("@BILLID",billId)
           };
           DBHelper.ExecuteCommand(sql, p);
        }

        //@
        public void SetTaskUpload(string billId, string detailId)
        {
            string sql = "INSERT INTO TASKUPLOAD VALUES(@BILLID,@DETAILID,0)";
            SqlParameter[] p = new SqlParameter[] {            
               new  SqlParameter("@DETAILID",detailId),
                new  SqlParameter("@BILLID",billId)
           };
           DBHelper.ExecuteCommand(sql, p);
        }

        //@
        public void ConfirmTask(string billID, string detailID, string confirmState,
                        int piece, int item, string state, string msg, string operater)
        {
            string sql = "ConfirmTask";
            SqlParameter[] p = new SqlParameter[] {           
                new  SqlParameter("@BillID", billID),
                new  SqlParameter("@DetailID", detailID),
                new  SqlParameter("@ConfirmState", confirmState),
                new  SqlParameter("@Piece", piece),
                new  SqlParameter("@Item", item),
                new  SqlParameter("@State", state),
                new  SqlParameter("@Msg", msg),
                new  SqlParameter("@Operator", operater)
           };
           DBHelper.ExecuteProcedure(sql, p);
        }
    }
}
