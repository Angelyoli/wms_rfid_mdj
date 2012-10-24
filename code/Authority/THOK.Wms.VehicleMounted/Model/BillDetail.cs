using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.VehicleMounted.Model
{
    public class BillDetail
    {
        private string billNo = string.Empty;

        public string BillNo
        {
            get { return billNo; }
            set { billNo = value; }
        }

        private string billType = string.Empty;

        public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }

        public string BillTypeName
        {
            get
            {
                switch (billType)
                {
                    case "1":
                        return "入库";
                        break;
                    case "2":
                        return "出库";
                        break;
                    case "3":
                        return "移库";
                        break;
                    case "4":
                        return "盘点";
                        break;
                    default:
                        return "";
                        break;
                }
            }

            set
            {
                billType = value;
            }
        }

        private int detailID = 0;

        public int DetailID
        {
            get { return detailID; }
            set { detailID = value; }
        }

        private string storageName = string.Empty;

        public string StorageName
        {
            get { return storageName; }
            set { storageName = value; }
        }

        private string storageRfid = string.Empty;

        public string StorageRfid
        {
            get { return storageRfid; }
            set { storageRfid = value; }
        }

        private string targetStorageName = string.Empty;

        public string TargetStorageName
        {
            get { return targetStorageName; }
            set { targetStorageName = value; }
        }

        private string targetStorageRfid = string.Empty;

        public string TargetStorageRfid
        {
            get { return targetStorageRfid; }
            set { targetStorageRfid = value; }
        }

        private string productCode = string.Empty;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private string productName = string.Empty;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private decimal pieceQuantity = 0;

        public decimal PieceQuantity
        {
            get { return pieceQuantity; }
            set { pieceQuantity = value; }
        }

        private decimal barQuantity = 0;

        public decimal BarQuantity
        {
            get { return barQuantity; }
            set { barQuantity = value; }
        }

        private decimal operatePieceQuantity = 0;

        public decimal OperatePieceQuantity
        {
            get { return operatePieceQuantity; }
            set { operatePieceQuantity = value; }
        }

        private decimal operateBarQuantity = 0;

        public decimal OperateBarQuantity
        {
            get { return operateBarQuantity; }
            set { operateBarQuantity = value; }
        }

        private string operatorCode = string.Empty;

        public string OperatorCode
        {
            //get { return operatorCode; }
            set { operatorCode = value; }
        }

        private string @operator = string.Empty;

        public string Operator
        {
            get { return @operator; }
            set { @operator = value; }
        }

        private string status = string.Empty;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string StatusName
        {
            get
            {
                switch (status)
                {
                    case "0":
                        return "未开始";
                        break;
                    case "1":
                        return "已申请";
                        break;
                    case "2":
                        return "已完成";
                        break;
                    default:
                        return "";
                        break;
                }
            }
            set
            {
                status = value;
            }
        }

        private int palletTag = 0;

        public int PalletTag
        {
            get { return palletTag; }
            set { palletTag = value; }
        }
    }
}
