using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WES.Interface.Model
{
    public class ShelfInfo
    {
        private string shelfCode = string.Empty;
        public string ShelfCode
        {
            get { return shelfCode; }
            set { shelfCode = value; }
        }
        private string shelfName = string.Empty;
        public string ShelfName
        {
            get { return shelfName; }
            set { shelfName = value; }
        }
        private string cellCode = string.Empty;
        public string CellCode
        {
            get { return cellCode; }
            set { cellCode = value; }
        }
        private string cellName = string.Empty;
        public string CellName
        {
            get { return cellName; }
            set { cellName = value; }
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
        private decimal quantityTiao = 0;
        public decimal QuantityTiao
        {
            get { return quantityTiao; }
            set { quantityTiao = value; }
        }
        private decimal quantityJian =0;
        public decimal QuantityJian
        {
            get { return quantityJian; }
            set { quantityJian = value; }
        }
        private string wareCode = string.Empty;
        public string WareCode
        {
            get { return wareCode; }
            set { wareCode = value; }
        }
        private string wareName = string.Empty;
        public string WareName
        {
            get { return wareName; }
            set { wareName = value; }
        }
        private string isActive = string.Empty;
        public string IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private int colNum = 0;
        public int ColNum
        {
            get { return colNum; }
            set { colNum = value; }
        }
        private int rowNum = 0;
        public int RowNum
        {
            get { return rowNum; }
            set { rowNum = value; }
        }
        private string shelf = "";
        public string Shelf
        {
            get { return shelf; }
            set { shelf = value; }
        }
        //public DateTime UpdateDate = DateTime.Now;
    }
}
