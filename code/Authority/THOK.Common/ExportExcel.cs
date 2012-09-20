using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#region
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Web;
using System.Web.Mvc;
#endregion

namespace THOK.Common
{
    public class ExportExcel
    {
        static HSSFWorkbook workbook;

        #region 导出EXCEL单表双表
        /// <summary>导出EXCEL单表双表</summary>
        /// <param name="dt1">DataTable1</param>
        /// <param name="dt2">DataTable2 如果没有给null值</param>
        /// <param name="headText1">第一张表标题</param>
        /// <param name="headText2">第二张表标题 如果没有给null值</param>
        /// <param name="headFont">标题字体</param>
        /// <param name="headSize">标题大小</param>
        /// <param name="headColor">标题颜色</param>
        /// <param name="headBorder">标题是否显示边框</param>
        /// <param name="colHeadFont">列头字体</param>
        /// <param name="colHeadSize">列头大小</param>
        /// <param name="colHeadColor">列头颜色</param>
        /// <param name="colHeadBorder">是否显示边框(除了标题外)</param>
        /// <param name="HeaderFooter">页眉页脚:[0]左上角[1]上中间[2]右上角[3]左下角[4]下中间[5]右下角</param>
        /// <returns></returns>
        public static System.IO.MemoryStream ExportDT(DataTable dt1, DataTable dt2
                , string headText1, string headText2
                , string headFont, short headSize, short headColor, bool headBorder
                , string colHeadFont, short colHeadSize, short colHeadColor, bool colHeadBorder
                , short contentColor//暂无引用
                , string[] HeaderFooter)
        {
            #region 变量
            string exportDate = "导出时间：" + DateTime.Now.ToString("yyyy-MM-dd");
            double columnWidth = colHeadSize - 9.5;
            #endregion

            #region 浏览器下载
            BrowserLoad(headText1);
            #endregion

            #region 创建工作表
            workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet(headText1) as HSSFSheet;
            HSSFCellStyle contentDateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            sheet.PrintSetup.FitHeight = 0;
            #endregion

            #region 全局样式
            HSSFCellStyle headStyle = GetTitleStyle(headFont, headSize, headColor);
            HSSFCellStyle dateStyle = GetExportDate();
            HSSFCellStyle colHeadStyle = GetColumnStyle(colHeadFont, colHeadSize, colHeadColor, colHeadBorder);
            #endregion

            #region 建表工程
            int MaxSheetCount = dt1.Rows.Count;
            if (MaxSheetCount < 65536)  // excel中的一个sheet最大容量65536行
            {
                #region 取得列宽 表一
                int[] arrColWidth = new int[dt1.Columns.Count];
                foreach (DataColumn item in dt1.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;//936是指GB2312编码
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    for (int j = 0; j < dt1.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(dt1.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                #endregion

                #region 取得列宽 表二
                int[] arrColWidth2 = new int[0];
                if (dt2 != null && headText2 != null)
                {
                    arrColWidth2 = new int[dt2.Columns.Count];
                    foreach (DataColumn item in dt2.Columns)
                    {
                        arrColWidth2[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                    }
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt2.Columns.Count; j++)
                        {
                            int intTemp = Encoding.GetEncoding(936).GetBytes(dt2.Rows[i][j].ToString()).Length;
                            if (intTemp > arrColWidth2[j])
                            {
                                arrColWidth2[j] = intTemp;
                            }
                        }
                    }
                }
                #endregion

                HSSFCellStyle contentStyle = workbook.CreateCellStyle() as HSSFCellStyle;

                #region 建表 表一
                int rowIndex1 = 0;
                foreach (DataRow row in dt1.Rows)
                {
                    if (rowIndex1 == 0)
                    {
                        if (rowIndex1 != 0) { sheet = workbook.CreateSheet() as HSSFSheet; }
                        /*--------------------- 填充表头、样式 ---------------------*/
                        {
                            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            headerRow.HeightInPoints = Convert.ToInt16(headSize * 1.4);
                            headerRow.CreateCell(0).SetCellValue(headText1);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            CellRangeAddress region = new CellRangeAddress(0, 0, 0, dt1.Columns.Count - 1);
                            sheet.AddMergedRegion(region);
                            if (headBorder == true)
                            {
                                sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);//给合并的画线
                            }
                        }
                        /*--------------------- 导出时间、样式 ---------------------*/
                        {
                            HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                            headerRow.CreateCell(0).SetCellValue(exportDate);
                            headerRow.GetCell(0).CellStyle = dateStyle;
                            CellRangeAddress region = new CellRangeAddress(1, 1, 0, dt1.Columns.Count - 1);
                            sheet.AddMergedRegion(region);
                            if (colHeadBorder == true)
                            {
                                sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                            }
                        }
                        /*--------------------- 填充列头、样式 ---------------------*/
                        {
                            HSSFRow headerRow = sheet.CreateRow(2) as HSSFRow;
                            headerRow.HeightInPoints = Convert.ToInt16(colHeadSize * 1.4);
                            foreach (DataColumn column in dt1.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = colHeadStyle;
                                sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32((arrColWidth[column.Ordinal] + columnWidth) * 256));//设置列宽
                            }
                        }
                        rowIndex1 = 3;
                    }
                    /*---------------------- 填充内容 ------------------------*/
                    HSSFRow dataRow = sheet.CreateRow(rowIndex1) as HSSFRow;
                    foreach (DataColumn column in dt1.Columns)
                    {
                        FillContent(dataRow, column, row, contentStyle, contentDateStyle,
                            colHeadFont, colHeadSize, colHeadColor, colHeadBorder);
                    }
                    rowIndex1++;
                }
                #endregion

                #region 建表 表二
                if (dt2 != null && headText2 != null)
                {
                    int rowIndex2 = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (rowIndex2 == 0)
                        {
                            HSSFRow headerRow;
                            if (rowIndex2 != 1) { sheet = workbook.CreateSheet(headText2) as HSSFSheet; }
                            /*--------------------- 填充表头、样式 ---------------------*/
                            {
                                headerRow = sheet.CreateRow(0) as HSSFRow;
                                headerRow.HeightInPoints = Convert.ToInt16(headSize * 1.4);
                                headerRow.CreateCell(0).SetCellValue(headText2);
                                headerRow.GetCell(0).CellStyle = headStyle;
                                CellRangeAddress region = new CellRangeAddress(0, 0, 0, dt2.Columns.Count - 1);
                                sheet.AddMergedRegion(region);
                                sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                            }
                            /*--------------------- 导出时间、样式 ---------------------*/
                            {
                                headerRow = sheet.CreateRow(1) as HSSFRow;
                                headerRow.CreateCell(0).SetCellValue(exportDate);
                                headerRow.GetCell(0).CellStyle = dateStyle;
                                CellRangeAddress region = new CellRangeAddress(1, 1, 0, dt2.Columns.Count - 1);
                                sheet.AddMergedRegion(region);
                                if (colHeadBorder == true)
                                {
                                    sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                                }
                            }
                            /*--------------------- 填充列头、样式 ---------------------*/
                            {
                                headerRow = sheet.CreateRow(2) as HSSFRow;
                                foreach (DataColumn column in dt2.Columns)
                                {
                                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                    headerRow.GetCell(column.Ordinal).CellStyle = colHeadStyle;
                                    sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32((arrColWidth2[column.Ordinal] + columnWidth) * 256));
                                }
                            }
                            rowIndex2 = 3;
                        }
                        /*------------------- 填充内容 -------------------*/
                        HSSFRow dataRow = sheet.CreateRow(rowIndex2) as HSSFRow;
                        foreach (DataColumn column in dt2.Columns)
                        {
                            FillContent(dataRow, column, row, contentStyle, contentDateStyle
                                , colHeadFont, colHeadSize, colHeadColor, colHeadBorder);
                        }
                        rowIndex2++;
                    }
                }
                #endregion

                #region 页眉 页脚
                sheet.Header.Left = HeaderFooter[0].ToString();
                sheet.Header.Center = HeaderFooter[1].ToString();
                sheet.Header.Right = HeaderFooter[2].ToString();
                sheet.Footer.Left = HeaderFooter[3].ToString();
                sheet.Footer.Center = HeaderFooter[4].ToString();
                sheet.Footer.Right = HeaderFooter[5].ToString();
                #endregion
            }
            #endregion

            #region 返回内存流
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
            #endregion
        }
        #endregion

        #region 填充内容
        /// <summary>填充内容</summary>
        static void FillContent(HSSFRow dataRow, DataColumn column, DataRow row, HSSFCellStyle contentStyle, HSSFCellStyle contentDateStyle, string colHeadFont, short colHeadSize, short colHeadColor, bool contentBorder)
        {
            HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
            HSSFCellStyle fillContentDateStyle = GetContentDateStyle(contentDateStyle);
            HSSFCellStyle fillContentStyle = GetContentStyle(contentStyle, contentBorder);

            dataRow.GetCell(column.Ordinal).CellStyle = fillContentStyle;

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
                    newCell.CellStyle = fillContentDateStyle; //格式化显示
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

        #region 标题样式
        /// <summary>标题样式</summary>
        static HSSFCellStyle GetTitleStyle(string headFont, short headSize, short headColor)
        {
            HSSFCellStyle cellStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            cellStyle.Alignment = HorizontalAlignment.CENTER;
            HSSFFont font = workbook.CreateFont() as HSSFFont;
            font.FontName = headFont;
            font.FontHeightInPoints = headSize;
            font.Color = headColor;
            font.Boldweight = 700;
            cellStyle.SetFont(font);
            return cellStyle;
        }
        #endregion

        #region 列头样式
        /// <summary>列头样式</summary>
        static HSSFCellStyle GetColumnStyle(string colHeadFont, short colHeadSize, short colHeadColor, bool colHeadBorder)
        {
            HSSFCellStyle cellStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            cellStyle.Alignment = HorizontalAlignment.CENTER; //居中
            if (colHeadBorder == true)
            {
                //边框
                cellStyle.BorderBottom = BorderStyle.THIN;
                cellStyle.BorderLeft = BorderStyle.THIN;
                cellStyle.BorderRight = BorderStyle.THIN;
                cellStyle.BorderTop = BorderStyle.THIN;
            }
            //font
            HSSFFont font = workbook.CreateFont() as HSSFFont;
            font.FontName = colHeadFont;
            font.FontHeightInPoints = colHeadSize;
            font.Color = colHeadColor;
            font.Boldweight = 700;

            cellStyle.SetFont(font);
            return cellStyle;
        }
        #endregion

        #region 内容样式
        /// <summary>内容样式</summary>
        static HSSFCellStyle GetContentStyle(HSSFCellStyle cellStyle, bool contentBoder)
        {
            //画边框
            if (contentBoder == true)
            {
                cellStyle.BorderBottom = BorderStyle.THIN;
                cellStyle.BorderLeft = BorderStyle.THIN;
                cellStyle.BorderRight = BorderStyle.THIN;
                cellStyle.BorderTop = BorderStyle.THIN;
            }
            return cellStyle;
        }
        #endregion

        #region 导出时间样式
        /// <summary>导出时间样式</summary>
        static HSSFCellStyle GetExportDate()
        {
            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            dateStyle.Alignment = HorizontalAlignment.CENTER;
            return dateStyle;
        }
        #endregion

        #region 内容日期样式
        /// <summary>内容日期样式</summary>
        static HSSFCellStyle GetContentDateStyle(HSSFCellStyle cellDateStyle)
        {
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            cellDateStyle.DataFormat = format.GetFormat("yyyy-MM-dd");
            return cellDateStyle;
        }
        #endregion

        #region 浏览器下载
        /// <summary>浏览器下载</summary>
        static void BrowserLoad(string headText1)
        {
            string filename = headText1 + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.BufferOutput = false;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            response.ContentType = "application/ms-excel";
        }
        #endregion

        #region 单元格数据生成通用类
        public static int CreateDataMethod
            (
                  ISheet sheet1
                , ICellStyle style
                , DataTable dt
                , int rowIndex
                , HSSFCellStyle dateStyle
            )
        {
            foreach (DataRow row in dt.Rows)
            {
                #region 填充内容
                HSSFRow dataRow = sheet1.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dt.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();
                    #region 字段类型处理
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            newCell.SetCellValue(drValue);
                            newCell.CellStyle = style;
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
                            newCell.CellStyle = style;
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            newCell.CellStyle = style;
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            newCell.CellStyle = style;
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            newCell.CellStyle = style;
                            break;
                        default:
                            newCell.SetCellValue(drValue);
                            newCell.CellStyle = style;
                            break;
                    }
                    #endregion
                    #region 插入留余空白列
                    int iLastCell = sheet1.GetRow(4).LastCellNum + 1 - dt.Columns.Count;
                    for (int i = 1; i < iLastCell; i++)
                    {
                        HSSFCell newNullCell = dataRow.CreateCell(newCell.ColumnIndex + i) as HSSFCell;
                        newNullCell.SetCellValue("");
                        newNullCell.CellStyle = style;
                    }
                    #endregion
                }
                #endregion
                rowIndex++;
            }
            return rowIndex;
        }
        #endregion


        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="_Request"></param>
        /// <param name="_Response"></param>
        /// <param name="_fileName"></param>
        /// <param name="_fullPath"></param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;
                    double pack = 10240;
                    //10K bytes                
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒     
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;
                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            System.Threading.Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
