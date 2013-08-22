using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.AutomotiveSystems.Models;

namespace THOK.Wms.AutomotiveSystems.Interfaces
{
    public interface ITaskService 
    {
        void GetBillMaster(string[] BillTypes, Result result);

        void GetBillDetail(BillMaster[] billMaster, string productCode, string OperateType, string OperateAreas, string Operator, Result result);

        void Apply(BillDetail[] billDetail,string useTag, Result result);

        void Cancel(BillDetail[] billDetail, string useTag, Result result);

        void Execute(BillDetail[] billDetail, string useTag, Result result);

        void GetShelf(Result result);

        void SearchRfidInfo(string rfidId, Result result);

        bool ProcessSortInfo(string orderdate, string batchId, string sortingLineCode, string orderId, ref string error);

        string WarehouseInBillProgressFeedback(THOK.Wms.AutomotiveSystems.Models.BillDetail[] billDetails);
        string WarehouseInBillFinish(THOK.Wms.AutomotiveSystems.Models.BillDetail[] billDetails);
        string WarehouseOutBillProgressFeedback(THOK.Wms.AutomotiveSystems.Models.BillDetail[] billDetails);
        string WarehouseOutBillFinish(THOK.Wms.AutomotiveSystems.Models.BillDetail[] billDetails);
    }
}
