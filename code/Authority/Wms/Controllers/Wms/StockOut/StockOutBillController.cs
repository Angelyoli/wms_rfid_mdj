using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.WMS.DownloadWms.Bll;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.SS;
using NPOI.DDF;
using NPOI.SS.Util;
using NPOI.SS.Formula.Eval;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI;
using THOK.Wms.Bll.Service;
using System.IO;
using System.Text;

namespace Authority.Controllers.Wms.StockOut
{
    public class StockOutBillController : Controller
    {

        [Dependency]
        public IOutBillMasterService OutBillMasterService { get; set; }
        [Dependency]
        public IOutBillDetailService OutBillDetailService { get; set; }
        //
        // GET: /StockOutBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasDownload = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasAllot = true;
            ViewBag.hasSettle = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;            
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询主单
        // GET: /StockOutBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var outBillMaster = OutBillMasterService.GetDetails(page, rows, BillNo, beginDate, endDate, OperatePersonCode, CheckPersonCode, Status, IsActive);
            return Json(outBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /StockOutBill/OutBillDetails/
        public ActionResult OutBillDetails(int page, int rows, string BillNo)
        {
            var outBillDetail = OutBillDetailService.GetDetails(page, rows, BillNo);
            return Json(outBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //生成单号
        // GET: /StockOutBill/GenInBillNo/
        public ActionResult GenInBillNo()
        {
            var outBillNo = OutBillMasterService.GenInBillNo(this.User.Identity.Name.ToString());
            return Json(outBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //新增主单
        // POST: /StockOutBill/Create/
        public ActionResult Create(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Add(outBillMaster, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改主单
        // POST: /StockOutBill/Edit/
        public ActionResult Edit(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Save(outBillMaster, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /StockOutBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Delete(BillNo, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //新增细单
        // POST: /StockOutBill/OutBillDetailCreate/
        public ActionResult OutBillDetailCreate(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Add(outBillDetail, out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除细单
        // POST: /StockOutBill/outBillDelete/
        public ActionResult outBillDelete(string BillNo, string ID)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Delete(ID, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改细单
        // POST: /StockOutBill/editOutBillDelete
        public ActionResult editOutBillDelete(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Save(outBillDetail, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /StockOutBill/outBillMasterAudit/
        public ActionResult outBillMasterAudit(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /StockOutBill/outBillMasterAntiTrial/
        public ActionResult outBillMasterAntiTrial(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.AntiTrial(BillNo, out errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单结单
        // POST: /StockOutBill/outBillMasterSettle/
        public ActionResult outBillMasterSettle(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Settle(BillNo, out errorInfo);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单结单
        // POST: /StockOutBill/DownOutBillMaster/
        public ActionResult DownOutBillMaster(string beginDate, string endDate)
        {
            string errorInfo = string.Empty;
            if (beginDate == string.Empty || endDate == string.Empty)
            {
                beginDate = DateTime.Now.ToString("yyyyMMdd");
                endDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                beginDate = Convert.ToDateTime(beginDate).ToString("yyyyMMdd");
                endDate = Convert.ToDateTime(endDate).ToString("yyyyMMdd");
            }
            DownUnitBll ubll = new DownUnitBll();
            DownProductBll pbll = new DownProductBll();
            DownOutBillBll ibll = new DownOutBillBll();
            ubll.DownUnitCodeInfo();
            pbll.DownProductInfo();            
            bool bResult = ibll.GetOutBill(beginDate, endDate, this.User.Identity.Name.ToString(), out errorInfo);

            //bool bResult = OutBillMasterService.DownOutBillMaster(beginDate, endDate, out errorInfo);
            string msg = bResult ? "下载成功" : "下载失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }
        // POST: /StockOutBill/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billNo = Request.QueryString["billNo"];

            System.Data.DataTable dt = OutBillMasterService.GetStockOut(page, rows, billNo);
            System.Data.DataTable dt2 = OutBillDetailService.GetOutBillDetail(page, rows, billNo);

            string strHeaderText = "出库信息表";
            string strColHeadText = "明细";
            #region
            string filename = strHeaderText + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            Response.ContentType = "application/ms-excel";
            string[] str = {
                               "20",        //[0]大标题字体大小
                               "700",       //[1]大标题字体粗宽
                               "10",        //[2]列标题字体大小
                               "700",       //[3]列标题字体粗宽
                               "300",       //[4]excel中有数据表格的大小
                               "微软雅黑",  //[5]大标题字体
                               "Arial",     //[6]小标题字体
                           };
            #endregion
            return new FileStreamResult(ExportDT(dt, dt2, strHeaderText, strColHeadText, str), "application/ms-excel");
        }

        #region 导出双表Excel公用方法
        /// <summary>DataTable导出到Excel的MemoryStream</summary>
        static MemoryStream ExportDT(System.Data.DataTable dt, System.Data.DataTable dt2, string strHeaderText, string strColHeadText, string[] str)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dt.Columns.Count];
            foreach (System.Data.DataColumn item in dt.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dt.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            #region dt2
            int[] arrColWidth2 = new int[dt2.Columns.Count];
            foreach (System.Data.DataColumn item in dt2.Columns)
            {
                arrColWidth2[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    int intTemp2 = Encoding.GetEncoding(936).GetBytes(dt2.Rows[i][j].ToString()).Length;
                    if (intTemp2 > arrColWidth2[j])
                    {
                        arrColWidth2[j] = intTemp2;
                    }
                }
            }
            #endregion

            int rowIndex = 0;
            foreach (System.Data.DataRow row in dt.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }
                    #region 表头及样式
                    {
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontName = str[5];                             //[5]
                        font.FontHeightInPoints = Convert.ToInt16(str[0]);  //[0]
                        font.Boldweight = Convert.ToInt16(str[1]);          //[1]
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new Region(0, 0, 0, dt.Columns.Count - 1));
                        //headerRow.Dispose();
                    }
                    #endregion
                    #region 列头及样式
                    {
                        HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontName = str[6];                             //[6]
                        font.FontHeightInPoints = Convert.ToInt16(str[2]);  //[2]
                        font.Boldweight = Convert.ToInt16(str[3]);          //[3]
                        headStyle.SetFont(font);
                        foreach (System.Data.DataColumn column in dt.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * Convert.ToInt32(str[4]));  //[4]
                        }
                        //headerRow.Dispose();
                    }
                    #endregion
                    rowIndex = 2;
                }
                #endregion
                #region 填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            string result = drValue;
                            newCell.SetCellValue(result);
                            break;
                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion
                rowIndex++;
            }

            #region dt2
            int rowIndex2 = 5;
            foreach (System.Data.DataRow row in dt2.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex2 == 5)
                {
                    if (rowIndex2 != 5)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }
                    #region 表头及样式
                    {
                        HSSFRow headerRow2 = sheet.CreateRow(5) as HSSFRow;
                        headerRow2.HeightInPoints = 25;
                        headerRow2.CreateCell(0).SetCellValue(strColHeadText);

                        HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontName = str[5];                             //[5]
                        font.FontHeightInPoints = Convert.ToInt16(str[0]);  //[0]
                        font.Boldweight = Convert.ToInt16(str[1]);          //[1]
                        headStyle2.SetFont(font);
                        headerRow2.GetCell(0).CellStyle = headStyle2;
                        sheet.AddMergedRegion(new Region(5, 0, 5, dt2.Columns.Count - 1));
                        //headerRow.Dispose();
                    }
                    #endregion
                    #region 列头及样式
                    {
                        HSSFRow headerRow2 = sheet.CreateRow(6) as HSSFRow;
                        HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontName = str[6];                             //[6]
                        font.FontHeightInPoints = Convert.ToInt16(str[2]);  //[2]
                        font.Boldweight = Convert.ToInt16(str[3]);          //[3]
                        headStyle2.SetFont(font);
                        foreach (System.Data.DataColumn column in dt2.Columns)
                        {
                            headerRow2.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow2.GetCell(column.Ordinal).CellStyle = headStyle2;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * Convert.ToInt32(str[4]));  //[4]
                        }
                        //headerRow.Dispose();
                    }
                    #endregion
                    rowIndex2 = 7;
                }
                #endregion
                #region 填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex2) as HSSFRow;
                foreach (System.Data.DataColumn column in dt2.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            string result = drValue;
                            newCell.SetCellValue(result);
                            break;
                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion
                rowIndex2++;
            }
            #endregion

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            //sheet;
            //workbook.Dispose();
            return ms;

        }
        public static bool isNumeric(String message, out double result)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
