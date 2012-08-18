﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.AutomotiveSystems.Models;

namespace THOK.Wms.AutomotiveSystems.Interfaces
{
    public interface ITaskService 
    {
        void GetBillMaster(string[] BillTypes, Result result);

        void GetBillDetail(BillMaster[] billMaster, Result result);

        void Apply(BillDetail[] billDetail, Result result);

        void Cancel(BillDetail[] billDetail, Result result);

        void Execute(BillDetail[] billDetail, Result result);
    }
}