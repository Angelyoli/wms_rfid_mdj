using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.PDA.Dao;

namespace THOK.PDA.Dal
{
    public class BillDal
    {

        private BillDao dao = new BillDao();
        private XMLBillDao xmlBillDao = new XMLBillDao();


        /// <summary>
        /// 获取主单据
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public DataTable GetBillMaster(string billType)
        {
            return dao.GetBillMaster(billType);
        }


          /// <summary>
        /// 获取明细单据
        /// </summary>
        /// <param name="billId">主单据号</param>
        /// <returns></returns>
        public DataTable GetBillDetailListByBillId(string billId)
        {
            return dao.GetBillDetailListByBillId(billId);
        }


        /// <summary>
        /// 获取明细单据
        /// </summary>
        /// <param name="billId">主单据id</param>
        /// <param name="detailId">明细单据id</param>
        /// <returns></returns>
        public DataRow GetBillDetailByDetailId(string billId, string detailId)
        {
            return dao.GetBillDetailByDetailId(billId, detailId).Rows[0];
        }





        public bool ApplyTask(string billId, string detailId)
        {
            dao.UpdateDetailState(billId, detailId);
            DataRow detailRow = dao.GetBillDetailCompleteInfo(billId, detailId);
            if (detailRow["ConfirmState"].ToString() != "1")
            {
                return false;
            }
            dao.UpdateDetailState(billId, detailId);
            
            return true;
        }


        public void ConfirmTask(string billID, string detailID, string confirmState,
                        int piece, int item, string state, string msg, string operater)
        {
            try
            {
                dao.ConfirmTask(billID, detailID, confirmState, piece, item, state, msg, operater);
                if (confirmState == "3")
                {
                    dao.SetTaskUpload(billID, detailID);
                }
                
            }
            catch (Exception)
            {

                xmlBillDao.SaveBill(billID, detailID);
                throw new Exception("网络连接出错!请使用usb连接电脑上传数据");
            }
        }



    }
}
