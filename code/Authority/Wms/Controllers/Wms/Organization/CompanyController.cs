using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Wms.DbModel;
#region
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using THOK.Common; 
#endregion

namespace Authority.Controllers.Organization
{
    public class CompanyController : Controller
    {
        [Dependency]
        public ICompanyService CompanyService { get; set; }
        //
        // GET: /Company/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult SearchPage()
        {
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        }

        //
        // GET: /Company/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string CompanyCode = collection["CompanyCode"] ?? "";
            string CompanyName = collection["CompanyName"] ?? "";
            string CompanyType = collection["CompanyType"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var systems = CompanyService.GetDetails(page, rows, CompanyCode, CompanyName, CompanyType, IsActive);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/Create
        [HttpPost]
        public ActionResult Create(Company company)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Add(company, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Company/Edit/
        public ActionResult Edit(Company company)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Save(company, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/Delete/
        [HttpPost]
        public ActionResult Delete(string companyID)
        {
            string strResult = string.Empty;
            bool bResult = CompanyService.Delete(companyID, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Company/GetParentName/
        public ActionResult GetParentName(int page, int rows, string queryString, string value)
        {
            if (queryString == null)
            {
                queryString = "CompanyCode";
            }
            if (value == null)
            {
                value = "";
            }
            var company = CompanyService.GetParentName(page, rows, queryString, value);
            return Json(company, "text", JsonRequestBehavior.AllowGet);
        }

        #region /Company/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string companyCode = Request.QueryString["companyCode"] ?? "";
            string companyName = Request.QueryString["companyName"] ?? "";
            string companyType = Request.QueryString["companyType"] ?? "";
            string isActive = Request.QueryString["isActive"] ?? "";            
            DataTable dt = CompanyService.GetCompany(page, rows, companyCode, companyName, companyType, isActive);
            string headText = "公司信息";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10; Int16 colHeadWidth = 300;
           
            
            ExportExcel.GetResponse(headText);
            MemoryStream ms = ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize,
                colHeadFont, colHeadSize, colHeadWidth,null);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion

        /*----------------------------------------------------------------------------------------
                                                 Test 
         ----------------------------------------------------------------------------------------*/
        #region Test
        
        HSSFWorkbook hssfworkbook;
        public void TestExcel()
        {
            string filename = "Test-" + DateTime.Now.ToString("yyMMdd-HHmm-ss") + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            this.InitializeWorkbook();
            this.GenerateData();
            Response.BinaryWrite(this.WriteToStream().GetBuffer());
            Response.End();
        }

        void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();
            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;
        }
        void GenerateData()
        {
            ISheet sheet1 = hssfworkbook.CreateSheet("公司信息");

            #region //
            //sheet1.CreateRow(0).CreateCell(0).SetCellValue("公司信息");
            //int x = 1;
            //for (int i = 1; i <= 15; i++)
            //{
            //    IRow row = sheet1.CreateRow(i);
            //    for (int j = 0; j < 15; j++)
            //    {
            //        row.CreateCell(j).SetCellValue(x++);
            //    }
            //} 
            #endregion

            #region 填充内容
            int page = 0, rows = 0;
            string companyCode = Request.QueryString["companyCode"] ?? "";
            string companyName = Request.QueryString["companyName"] ?? "";
            string companyType = Request.QueryString["companyType"] ?? "";
            string isActive = Request.QueryString["isActive"] ?? "";
            System.Data.DataTable dt = CompanyService.GetCompany(page, rows, companyCode, companyName, companyType, isActive);

            foreach (DataRow row in dt.Rows)
            {
                HSSFRow dataRow = sheet1.CreateRow(0) as HSSFRow;
                foreach (DataColumn column in dt.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();
                    #region 字段类型处理
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
                            //newCell.CellStyle = cellStyle; //格式化显示
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
                    #endregion
                    //// 插入留余空白列
                    //int iLastCell = sheet1.GetRow(4).LastCellNum + 1 - dt.Columns.Count;
                    //for (int i = 1; i < iLastCell; i++)
                    //{
                    //    HSSFCell newNullCell = dataRow.CreateCell(newCell.ColumnIndex + i) as HSSFCell;
                    //    newNullCell.SetCellValue("");
                    //    //newNullCell.CellStyle = style;
                    //}
                }
            }
            #endregion
        }
        MemoryStream WriteToStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }
        #endregion
    }
}
