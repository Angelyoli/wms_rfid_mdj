using System;
using System.Collections.Generic;
using System.Text;
using THOK.PDA.Dao;

namespace THOK.PDA.Dal
{
    public class XMLBillDal
    {
        private XMLBillDao dao = new XMLBillDao();

        public void ReadBill()
        {
            dao.ReadBill();
        }

        public void UpdateBill(string billId, string detailId,string piece,string item)
        {
            dao.UpdateBill(billId, detailId, piece, item);
        }
    }
}
