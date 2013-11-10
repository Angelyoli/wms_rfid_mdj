using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using THOK.PDA.Util;
using THOK.PDA.Model;

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
                for (int i = 0; i < r.RestTasks.Length; i++)
                {
                    DataRow row = table.NewRow();
                    row["TaskID"] = r.RestTasks[i].TaskID;
                    row["CellName"] = r.RestTasks[i].CellName;
                    row["ProductName"] = r.RestTasks[i].ProductName;
                    row["OrderID"] = r.RestTasks[i].OrderID;
                    row["OrderType"] = r.RestTasks[i].OrderType;
                    row["Status"] = r.RestTasks[i].Status;
                    row["Quantity"] = r.RestTasks[i].Quantity;
                    row["TaskQuantity"] = r.RestTasks[i].TaskQuantity;
                    row["PieceQuantity"] = r.RestTasks[i].PieceQuantity;
                    row["BarQuantity"] = r.RestTasks[i].BarQuantity;
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
            string error = "任务已完成！";
            string msg = util.GetDataFromServer(methodName);
            Result r = JsonConvert.DeserializeObject<Result>(msg);
            if (!r.IsSuccess)
            {
                error = r.Message;
            }
            return error;
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
