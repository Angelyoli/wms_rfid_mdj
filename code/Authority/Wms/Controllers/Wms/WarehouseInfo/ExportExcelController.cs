using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.WarehouseInfo
{
    public class ExportExcelController : Controller
    {
        [Dependency]
        public ICellService CellService { get; set; }

        //
        // GET: /ExportExcel/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //POST: /ExportExcel/ExportExcel/
        public void ExportExcel()
        {
            System.Data.DataTable dt = CellService.GetProductCell();
            string path = @"E:\" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".xls";
            CreateExcel(dt, path);
        }

        public void CreateExcel(System.Data.DataTable dt, string path)
        {
            if (dt == null)
            {
                throw new Exception("数据表中无数据");
            }
            int eRowIndex = 1;
            int eColIndex = 1;
            int cols = dt.Columns.Count;
            int rows = dt.Rows.Count;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
            try
            {
                //列名的处理
                for (int i = 0; i < cols; i++)
                {
                    xlApp.Cells[eRowIndex, eColIndex] = dt.Columns[i].ColumnName;
                    eColIndex++;
                }
                //列名加粗显示
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, cols]).Font.Bold = true;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, cols]).ColumnWidth = 10;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 2], (object)xlApp.Cells[eRowIndex, cols]).ColumnWidth = 30;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 3], (object)xlApp.Cells[eRowIndex, cols]).ColumnWidth = 10;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[rows + 1, cols]).Font.Name = "Arial";
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[rows + 1, cols]).Font.Size = "10";
                
                eRowIndex++;

                for (int i = 0; i < rows; i++)
                {
                    eColIndex = 1;
                    for (int j = 0; j < cols; j++)
                    {
                        xlApp.Cells[eRowIndex, eColIndex] = "'" + dt.Rows[i][j].ToString();
                        eColIndex++;
                    }
                    eRowIndex++;
                }
                //控制单元格中的内容。
                //xlApp.Cells.EntireColumn.AutoFit();

                xlApp.DisplayAlerts = false;
                xlBook.SaveCopyAs(path);
                xlApp.Workbooks.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                xlApp.Quit();
                GC.Collect();   //杀掉Excel进程。
            }
        }
    }
}
