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
            ViewBag.hasPrint = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //POST: /ExportExcel/ExportExcel/
        public void ExportExcel()
        {
            string queryString = Request.QueryString["queryString"];
            string value = Request.QueryString["value"];
            string filename = System.DateTime.Now.ToString("yyMMdd-HHmm-ss") + ".xls";
            string path = "";
            if (System.IO.Directory.Exists(@"D:\"))
            {
                path = @"D:\";
            }
            else
            {
                path = @"C:\";
            }
            string pathFilename = path + filename;
            string titleName = "货位预设卷烟信息表";
            bool titleBold = true;
            string titleSize = "20", contentFirstSize = "10";
            string contentFontName = "Arial";

            CreateExcel(queryString, value, pathFilename, titleName, titleBold, titleSize, contentFontName, contentFirstSize);
        }

        public void CreateExcel(string queryString,string value, string pathFilename, string titleName, bool titleBold, string titleSize, string contentFontName, string contentFirstSize)
        {
            if (queryString == null)
            {
                queryString = "ProductCode";
            }
            if (value == null)
            {
                value = "";
            }
            System.Data.DataTable dt = CellService.GetProductCell(queryString, value);
            if (dt == null)
            {
                throw new Exception("数据表中无数据");
            }
            int eRowIndex = 2;
            int eColIndex = 1;
            int cols = dt.Columns.Count;
            int rows = dt.Rows.Count;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets["sheet1"];
            //xlSheet.PageSetup.CenterHorizontally = false;//页面水平居中
            //xlSheet.PageSetup.CenterVertically = false;//页面不垂直居中

            xlApp.Cells[1, 1] = titleName;
            xlApp.get_Range((object)xlApp.Cells[1, 1], (object)xlApp.Cells[1, 3]).Font.Bold = titleBold;
            xlApp.get_Range((object)xlApp.Cells[1, 1], (object)xlApp.Cells[1, 3]).Font.Size = titleSize;
            xlApp.get_Range((object)xlApp.Cells[1, 1], (object)xlApp.Cells[1, 3]).MergeCells = true;
            xlApp.get_Range((object)xlApp.Cells[1, 1], (object)xlApp.Cells[1, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            try
            {
                //列名的处理
                for (int i = 0; i < cols; i++)
                {
                    xlApp.Cells[eRowIndex, eColIndex] = dt.Columns[i].ColumnName;
                    eColIndex++;
                }

                //列内容样式
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, 3]).Font.Bold = titleBold;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, 3]).Font.Name = contentFontName;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, 3]).Font.Size = contentFirstSize;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 1], (object)xlApp.Cells[eRowIndex, 1]).ColumnWidth = 20;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 2], (object)xlApp.Cells[eRowIndex, 2]).ColumnWidth = 40;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex, 3], (object)xlApp.Cells[eRowIndex, 3]).ColumnWidth = 10;

                eRowIndex++;

                for (int i = 0; i < rows; i++)
                {
                    eColIndex = 1;
                    for (int j = 0; j < cols; j++)
                    {
                        xlApp.Cells[eRowIndex, eColIndex] = dt.Rows[i][j].ToString();
                        eColIndex++;
                    }
                    eRowIndex++;
                }
                Microsoft.Office.Interop.Excel.Range range1 = (Microsoft.Office.Interop.Excel.Range)xlApp.get_Range((object)xlApp.Cells[1, 1], (object)xlApp.Cells[eRowIndex - 1, 3]);
                range1.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                range1.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range1.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range1.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range1.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                //xlApp.Visible = true;

                xlApp.Cells[eRowIndex + 1, 1] = "附加信息：";
                xlApp.get_Range((object)xlApp.Cells[eRowIndex + 1, 1], (object)xlApp.Cells[eRowIndex + 1, 3]).MergeCells = true;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex + 1, 1], (object)xlApp.Cells[eRowIndex + 1, 3]).MergeCells = true;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex + 1, 1], (object)xlApp.Cells[eRowIndex + 1, eColIndex]).RowHeight = 50;

                xlApp.Cells[eRowIndex + 2, 1] = "确认签名：";
                xlApp.get_Range((object)xlApp.Cells[eRowIndex + 1, 1], (object)xlApp.Cells[eRowIndex + 1, 3]).MergeCells = true;
                xlApp.get_Range((object)xlApp.Cells[eRowIndex + 1, 1], (object)xlApp.Cells[eRowIndex + 1, 3]).MergeCells = true;

                //xlApp.Cells.EntireRow.AutoFit();
                //xlApp.DisplayAlerts = true;

                //xlBook.SaveCopyAs(path);
                xlSheet.SaveAs(pathFilename);

                xlApp.Workbooks.Close();
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('0000x1：'" + e.Message + ")</script>");
            }
            finally
            {
                xlApp.Quit();
                GC.Collect();   //杀掉Excel进程。
            }
        }
    }
}
