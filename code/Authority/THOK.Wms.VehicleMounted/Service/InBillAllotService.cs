using System;
using Microsoft.Practices.Unity;
using Entities.Extensions;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.VehicleMounted.Interface;
using System.Linq;
using System.Transactions;
using THOK.Wms.SignalR.Common;
using THOK.Wms.DbModel;

namespace THOK.Wms.VehicleMounted.Service
{
    public class InBillAllotService
    {

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "0":
                    statusStr = "未开始";
                    break;
                case "1":
                    statusStr = "已申请";
                    break;
                case "2":
                    statusStr = "已完成";
                    break;
            }
            return statusStr;
        }

        
    }
}
