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
        /// ��ȡ������
        /// </summary>
        /// <param name="billType">��������</param>
        /// <returns></returns>
        public DataTable GetBillMaster(string billType)
        {
            return dao.GetBillMaster(billType);
        }


          /// <summary>
        /// ��ȡ��ϸ����
        /// </summary>
        /// <param name="billId">�����ݺ�</param>
        /// <returns></returns>
        public DataTable GetBillDetailListByBillId(string billId)
        {
            return dao.GetBillDetailListByBillId(billId);
        }


        /// <summary>
        /// ��ȡ��ϸ����
        /// </summary>
        /// <param name="billId">������id</param>
        /// <param name="detailId">��ϸ����id</param>
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
                throw new Exception("�������ӳ���!��ʹ��usb���ӵ����ϴ�����");
            }
        }



    }
}
