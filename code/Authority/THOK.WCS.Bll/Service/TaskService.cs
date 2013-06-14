using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.Bll.Interfaces;

namespace THOK.WCS.Bll.Service
{
    public class TaskService:ITaskService
    {
        public bool CreateNewTaskFromInBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromOutBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromMoveBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskFromCheckBill(string billNo)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskForEmptyPalletStack(int positionID)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskForEmptyPalletSupply(int positionID)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewTaskForMoveBackRemain(int positionID)
        {
            throw new NotImplementedException();
        }

        public bool FinishTask(int taskID)
        {
            throw new NotImplementedException();
        }
    }
}
