using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.DbModel;
using THOK.WCS.Bll.Models;

namespace THOK.WCS.Bll.Interfaces
{
    public interface ITaskService
    {
        object GetDetails(int page, int rows, Task task);
        bool Add(Task task, out string strResult);
        bool Save(Task task, out string strResult);
        bool Delete(string taskID, out string strResult);

        bool CreateInBillTask(string billNo, out string errInfo);
        bool CreateOutBillTask(string billNo, out string errInfo);
        bool CreateMoveBillTask(string billNo,int taskLevel, out string errInfo);
        bool CreateCheckBillTask(string billNo, out string errorInfo);
        bool CreateSortWorkDispatchTask(string billNo, out string errorInfo);

        bool CreateNewTaskForEmptyPalletStack(int positionID, string positionName, out string errorInfo);
        bool CreateNewTaskForEmptyPalletSupply(int positionID, string positionName, out string errorInfo);
        bool CreateNewTaskForMoveBackRemain(int taskID, out string errorInfo);

        bool FinishTask(int taskID, out string errorInfo);    
        bool FinishTask(int taskID, string orderType, string orderID, int allotID, string originStorageCode, string targetStorageCode, out string errorInfo);

        bool ClearTask(out string errorInfo);
        bool ClearTask(string orderID, out string errorInfo);

        int FinishStockOutTask(int taskID, int stockOutQuantity, out string errorInfo);
        int FinishInventoryTask(int taskID, int realQuantity, out string errorInfo);

        bool AutoCreateMoveBill(out string errorInfo);

        void GetOutTask(string positionType, string orderType, RestReturn result);
        void FinishTask(string taskID, RestReturn result);
    }
}
