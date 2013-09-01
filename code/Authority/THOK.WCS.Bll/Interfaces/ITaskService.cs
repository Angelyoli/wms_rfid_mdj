using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WCS.Bll.Interfaces
{
    public interface ITaskService
    {
        bool CreateNewTaskFromInBill(string billNo);

        bool CreateNewTaskFromOutBill(string billNo);

        bool CreateNewTaskFromMoveBill(string billNo);

        bool CreateNewTaskFromCheckBill(string billNo);

        bool CreateNewTaskForEmptyPalletStack(int positionID);

        bool CreateNewTaskForEmptyPalletSupply(int positionID);

        bool CreateNewTaskForMoveBackRemain(int taskID);

        bool FinishTask(int taskID);    
        bool FinishTask(int taskID, string orderType, string orderID, int allotID,string originStorageCode, string targetStorageCode);        
    }
}
