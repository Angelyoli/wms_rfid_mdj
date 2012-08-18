using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.Wms.AutomotiveSystems.Models
{
    public class BillDetail
    {
        public string BillNo = string.Empty;
        public string BillType = string.Empty;

        public string DetailID = string.Empty;
        public string StorageName = string.Empty;
        public string TargetStorageName = string.Empty;

        public string ProductCode = string.Empty;
        public string ProductName = string.Empty;

        public int PieceQuantity = 0;
        public int BarQuantity = 0;
        public int OperatePieceQuantity = 0;
        public int OperateBarQuantity = 0;

        public string OperatorCode = string.Empty;
        public string Operator = string.Empty;
        public string Status = string.Empty;
    }
}
