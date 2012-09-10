using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
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
using System.Data;

namespace Authority.Controllers.Wms.StockCheckInfo
{
    public class CheckBillController : Controller
    {
        [Dependency]
        public ICheckBillMasterService CheckBillMasterService { get; set; }

        [Dependency]
        public ICheckBillDetailService CheckBillDetailService { get; set; }

        //
        // GET: /CheckBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            //ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasAudit = true;
            ViewBag.hasConfirm = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //获取盘点BIllNO
        // POST: /CheckBill/GetCheckBillNo
        public ActionResult GetCheckBillNo()
        {
            var area = CheckBillMasterService.GetCheckBillNo();
            return Json(area, "text", JsonRequestBehavior.AllowGet);
        }

        //根据货位生成盘点单主表和细表数据
        // POST: /CheckBill/CheckCellCreate/       
        public ActionResult CheckCellCreate(string wareCodes, string areaCodes, string shelfCodes, string cellCodes,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.CellAdd(wareCodes, areaCodes, shelfCodes, cellCodes,this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";            
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //根据产品生成盘点单主表和细表数据
        // POST: /CheckBill/CheckProductCreate/       
        public ActionResult CheckProductCreate(string products,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.ProductAdd(products, this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //根据产品生成盘点单主表和细表数据
        // POST: /CheckBill/CheckChangedCreate/       
        public ActionResult CheckChangedCreate(string beginDate, string endDate,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.ChangedAdd(beginDate, endDate, this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //查询货位要预览的盘点数据表
        // POST: /CheckBill/CheckCellDetails/
        public ActionResult CheckCellDetails(int page, int rows, string ware, string area, string shelf, string cell)
        {
            var storage = CheckBillMasterService.GetCellDetails(page, rows, ware, area, shelf, cell);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询产品要预览的盘点数据表
        // POST: /CheckBill/CheckProductDetails/        
        public ActionResult CheckProductDetails(int page, int rows, string products)
        {
            var storage = CheckBillMasterService.GetProductDetails(page, rows, products);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询异动要预览的盘点数据表
        // POST: /CheckBill/CheckChangedDetails/
        public ActionResult CheckChangedDetails(int page, int rows, string beginDate, string endDate)
        {
            var storage = CheckBillMasterService.GetChangedCellDetails(page, rows, beginDate, endDate);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询主单
        // GET: /CheckBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = CheckBillMasterService.GetDetails(page, rows, BillNo, beginDate, endDate, OperatePersonCode, CheckPersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /CheckBill/CheckBillDetails/
        public ActionResult CheckBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = CheckBillDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /CheckBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            bool bResult = CheckBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /CheckBill/checkBillMasterAudit/
        public ActionResult checkBillMasterAudit(string BillNo)
        {
            bool bResult = CheckBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString());
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /CheckBill/checkBillMasterAntiTrial/
        public ActionResult checkBillMasterAntiTrial(string BillNo)
        {
            bool bResult = CheckBillMasterService.AntiTrial(BillNo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单确认
        // POST: /CheckBill/checkBillMasterConfirm/
        public ActionResult checkBillMasterConfirm(string BillNo)
        {
            string errorInfo=string.Empty;
            bool bResult = CheckBillMasterService.confirmCheck(BillNo, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /MoveBillMaster/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string billNo = Request.QueryString["billNo"];

            System.Data.DataTable dt = CheckBillDetailService.GetCheckBillDetail(page, rows, billNo);

            string strHeaderText = "盘点信息明细";
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
            return new FileStreamResult(ExportDT(dt, strHeaderText, str), "application/ms-excel");
        }

        #region 导出到单表Excel公用方法
        /// <summary>DataTable导出到Excel的MemoryStream</summary>
        static MemoryStream ExportDT(DataTable dtSource, string strHeaderText, string[] str)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet(strHeaderText) as HSSFSheet;

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
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
                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
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
                        foreach (DataColumn column in dtSource.Columns)
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
                foreach (DataColumn column in dtSource.Columns)
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
