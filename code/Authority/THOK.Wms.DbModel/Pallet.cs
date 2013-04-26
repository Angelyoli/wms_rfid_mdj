using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Pallet
    {
        public string  PalletID { get; set; }
        public string  WmsUUID { get; set; }
        public string UUID { get; set; }
        public string TicketNo { get; set; }
        public DateTime? OperateDate { get; set; }
        public string OperateType { get; set; }
        public string BarCodeType { get; set; }
        public string RfidAntCode { get; set; }
        public string PieceCigarCode { get; set; }
        public string BoxCigarCode { get; set; }
        public string CigaretteName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime ScanTime { get; set; }
    }
}
