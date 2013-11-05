using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WCS.Bll.Models
{
    public class RestTask
    {
        public int TaskID = 0;
        public string TaskType = string.Empty;
        public string OrderID = string.Empty;
        public string OrderType = string.Empty;
        public string CellName = string.Empty;
        public string CellCode = string.Empty;
        public string ProductCode = string.Empty;
        public string ProductName = string.Empty;

        public decimal Quantity = 0;
        public decimal TaskQuantity = 0;
        public decimal PieceQuantity = 0;
        public decimal BarQuantity = 0;
        public string Status = string.Empty;
    }
}
