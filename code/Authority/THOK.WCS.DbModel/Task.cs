
namespace THOK.WCS.DbModel
{
    public class Task
    {
        public int ID { get; set; }
        public string TaskType { get; set; }
        public int TaskLevel { get; set; }
        public int PathID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string OriginCellCode { get; set; }
        public string TargetCellCode { get; set; }
        public int OriginPositionID { get; set; }
        public int TargetPositionID { get; set; }
        public int CurrentPositionID { get; set; }
        public string CurrentPositionState { get; set; }
        public string State { get; set; }
        public string TagState { get; set; }
        public int Quantity { get; set; }
        public int TaskQuantity { get; set; }
        public int OperateQuantity { get; set; }
        public string OrderID { get; set; }
        public string OrderType { get; set; }
        public int AllotID { get; set; }
        public string DownloadState { get; set; }
        public int StorageSequence { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}