using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using THOK.PDA.Util;
using THOK.PDA.Model;
using System.Linq;

namespace THOK.PDA.Service
{
    public class HttpDataService
    {
        HttpUtil util = new HttpUtil();

        public DataTable SearchOutTask(string methodName)
        {
            string msg = util.GetDataFromServer(methodName);
            Result r = JsonConvert.DeserializeObject<Result>(msg);         

            DataTable table = BuildTaskTable();
            if (r.IsSuccess)
            {
                foreach (RestTask task in r.RestTasks)
                {
                    DataRow row = table.NewRow();
                    row["TaskID"] = task.TaskID;
                    row["CellName"] = task.CellName;
                    row["ProductName"] = task.ProductName;
                    row["OrderID"] = task.OrderID;
                    row["OrderType"] = task.OrderType;
                    row["Status"] = task.Status;
                    row["Quantity"] = task.Quantity;
                    row["TaskQuantity"] = task.TaskQuantity;
                    row["PieceQuantity"] = task.PieceQuantity;
                    row["BarQuantity"] = task.BarQuantity;
                    table.Rows.Add(row);
                }
                return table;
            }
            else
            {
                return table;
            }
        }

        public string FinishTask(string methodName)
        {
            string result = "任务已完成！";
            string msg = util.GetDataFromServer(methodName);
            Result r = JsonConvert.DeserializeObject<Result>(msg);
            if (!r.IsSuccess)
            {
                result = r.Message;
            }
            return result;
        }

        DataTable BuildTaskTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("TaskID", typeof(Int32));
            table.Columns.Add("OrderID");
            table.Columns.Add("OrderType");
            table.Columns.Add("Status");
            table.Columns.Add("CellName");
            table.Columns.Add("ProductName");
            table.Columns.Add("Quantity");
            table.Columns.Add("TaskQuantity");
            table.Columns.Add("PieceQuantity");
            table.Columns.Add("BarQuantity");
            return table;
        }
    }
}
