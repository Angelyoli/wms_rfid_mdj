using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;

namespace THOK.Wms.VehicleMounted.Service
{
    public class CellService
    {
        //[Dependency]
        //public IInBillAllotRepository InBillAllotRepository { get; set; }

        //public string WhatStatus(string status)
        //{
        //    string statusStr = "";
        //    switch (status)
        //    {
        //        case "0":
        //            statusStr = "未开始";
        //            break;
        //        case "1":
        //            statusStr = "已申请";
        //            break;
        //        case "2":
        //            statusStr = "已完成";
        //            break;
        //    }
        //    return statusStr;
        //}

        //public object Search(string billNo, int page, int rows)
        //{
        //    var allotQuery = InBillAllotRepository.GetQueryable();
        //    var query = allotQuery.Where(a => a.BillNo == billNo).OrderBy(a => a.ID).Select(i => i);

        //    return null;
        //}
    }
}
