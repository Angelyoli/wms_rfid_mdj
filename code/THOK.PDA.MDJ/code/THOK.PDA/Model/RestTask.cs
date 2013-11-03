using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.PDA.Model
{
   public class RestTask
    {
        public int TaskID { get; set; }
        public string TaskType { get; set; }
        public string OrderID { get; set; }
        public string OrderType { get; set; }
        public string CellName { get; set; }
        public string CellCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public decimal Quantity { get; set; }
        public decimal TaskQuantity { get; set; }
        public decimal PieceQuantity { get; set; }
        public decimal BarQuantity { get; set; }
        public string Status { get; set; }
    }
}
