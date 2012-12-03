using System;
using System.Collections.Generic;
using System.Text;
using THOK.PDA.Util;
using System.Data;
using THOK.WES.Interface.Model;
using Newtonsoft.Json;
using System.Net;

namespace THOK.PDA.Dal
{
    public class HttpDataDal
    {
        private HttpUtil util = new HttpUtil();

        /// <summary>
        /// 获取主单单号
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBillMaster(string billTypes)
        {
            string parameter = @"Parameter={'Method':'getMaster','BillTypes':" + JsonConvert.SerializeObject(billTypes.Split(',')) + "}";


            
            string msg = util.GetDataFromServer(parameter);
            Result r = JsonConvert.DeserializeObject<Result>(msg);
            
            DataTable table = GenBill();
            if (r.IsSuccess)
            {
                for (int i = 0; i < r.BillMasters.Length; i++)
                {
                    DataRow row = table.NewRow();
                    row["BILLNO"] = r.BillMasters[i].BillNo;
                    table.Rows.Add(row);
                }
                return table;
            }
            else
            {
                return table;
            }
        }

        /// <summary>
        /// 获取细单
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBillDetail(BillMaster billMaster)
        {
            string parameter = @"Parameter={'Method':'getDetail','ProductCode': '" + "" + "','OperateType':'" + billMaster.BillType + "','OperateArea':'" + "1,2,3" + "','Operator':'" + Dns.GetHostName() + "','BillMasters':" + JsonConvert.SerializeObject(new BillMaster[] { billMaster }) + "}";
            string msg = util.GetDataFromServer(parameter);
            Result r = JsonConvert.DeserializeObject<Result>(msg);

            DataTable table = GenDetailTable();
            
            
            for (int i = 0; i < r.BillDetails.Length; i++)
            {
                DataRow row = table.NewRow();

                row["DetailID"] = r.BillDetails[i].DetailID;
                row["operateStorageName"] = r.BillDetails[i].StorageName;
                row["targetStorageName"] = r.BillDetails[i].TargetStorageName;
                row["operateName"] = r.BillDetails[i].BillTypeName;
                row["operateProductName"] = r.BillDetails[i].ProductName;
                row["operatePieceQuantity"] = r.BillDetails[i].PieceQuantity;
                row["operateBarQuantity"] = r.BillDetails[i].BarQuantity;
                row["StatusName"] = r.BillDetails[i].StatusName;
                
                table.Rows.Add(row);
            }


            return table;
        }

        /// <summary>
        /// 取消作业
        /// </summary>
        /// <param name="billDetails"></param>
        /// <param name="useTag"></param>
        public void Cancel(BillDetail billDetail)
        {
            string parameter = @"Parameter={'Method':'cancel','UseTag':'" + "0" + "','BillDetails':" + JsonConvert.SerializeObject(new BillDetail[] { billDetail }) + "}";
            string msg = util.GetDataFromServer(parameter);
            //Result r = JsonConvert.DeserializeObject<Result>(msg);
        }

        /// <summary>
        /// 申请作业
        /// </summary>
        /// <param name="billDetails"></param>
        /// <param name="useTag"></param>
        public void Apply(BillDetail billDetail)
        {
            string parameter = @"Parameter={'Method':'apply','UseTag':'" + "0" + "','BillDetails':" + JsonConvert.SerializeObject(new BillDetail[] { billDetail }) + "}";
            string msg = util.GetDataFromServer(parameter);
            //Result r = JsonConvert.DeserializeObject<Result>(msg);
        }


        /// <summary>
        /// 完成作业
        /// </summary>
        /// <returns></returns>
        public void Execute(BillDetail billDetail)
        {
            string parameter = @"Parameter={'Method':'execute','UseTag':'" + "0" + "','BillDetails':" + JsonConvert.SerializeObject(new BillDetail[] { billDetail}) + "}";
            string msg = util.GetDataFromServer(parameter);
        }

        private DataTable GenBill()
        {
            DataTable table = new DataTable();
            table.TableName = "BILL";
            table.Columns.Add("BILLNO");
            return table;
        }

        private DataTable GenDetailTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("DetailID");
            table.Columns.Add("operateStorageName");
            table.Columns.Add("targetStorageName");
            table.Columns.Add("operateName");
            table.Columns.Add("operateProductName");
            table.Columns.Add("operatePieceQuantity");
            table.Columns.Add("operateBarQuantity");
            table.Columns.Add("StatusName");
            
            return table;
        }
    }
    
}
