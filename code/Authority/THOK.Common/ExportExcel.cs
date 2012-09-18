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
        /// <summary>导出EXCEL单表双表</summary>
        /// <param name="dt1">DataTable</param>
        /// <param name="dt2">DataTable</param>
        /// <param name="headText1">标题</param>
        /// <param name="headText2">标题</param>
        /// <param name="headFont">字体</param>
        /// <param name="headSize">大小</param>
        /// <param name="colHeadFont">字体</param>
        /// <param name="colHeadSize">大小</param>
        /// <returns></returns>
        public static System.IO.MemoryStream ExportDT(DataTable dt1, DataTable dt2
                , string headText1, string headText2
                , string headFont, Int16 headSize
                , string colHeadFont, Int16 colHeadSize)
        {
            string exportDate = "导出时间：" + DateTime.Now.ToString("yyyy-MM-dd");

            #region 浏览器下载
            string filename = headText1 + DateTime.Now.ToString("yyMMdd-HHmm-ss");
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.BufferOutput = false;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeDataString(filename) + ".xls");
            response.ContentType = "application/ms-excel"; 
            #endregion

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet(headText1) as HSSFSheet;
            HSSFCellStyle cellStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            cellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");
            sheet.PrintSetup.FitHeight = 0;

            int MaxSheetCount = dt1.Rows.Count;
            if (MaxSheetCount < 65536)  /* excel中的一个sheet最大容量65536行 */
            {
                #region 取得列宽 dt1
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

                #region 取得列宽 dt2
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

                #region 建表 dt1
                int rowIndex = 0;
                foreach (DataRow row in dt1.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        //表头及样式
                        {
                            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            headerRow.HeightInPoints = Convert.ToInt16(headSize * 1.4);
                            headerRow.CreateCell(0).SetCellValue(headText1);
                            HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = HorizontalAlignment.CENTER;
                            HSSFFont font = workbook.CreateFont() as HSSFFont;
                            font.FontName = headFont;
                            font.FontHeightInPoints = headSize;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            //sheet.AddMergedRegion(new  Region(0, 0, 0, dt1.Columns.Count - 1));//过期方法，但还能用
                            CellRangeAddress region = new CellRangeAddress(0, 0, 0, dt1.Columns.Count - 1);//同上
                            sheet.AddMergedRegion(region);
                            sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);//给合并的画线
                        }
                        //导出时间
                        {
                            HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                            headerRow.CreateCell(0).SetCellValue(exportDate);
                            HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = HorizontalAlignment.CENTER;
                            headerRow.GetCell(0).CellStyle = headStyle;
                            CellRangeAddress region = new CellRangeAddress(1, 1, 0, dt1.Columns.Count - 1);
                            sheet.AddMergedRegion(region);
                            sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                        }
                        //列头及样式
                        {
                            HSSFRow headerRow = sheet.CreateRow(2) as HSSFRow;
                            headerRow.HeightInPoints = Convert.ToInt16(colHeadSize * 1.4);
                            HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = HorizontalAlignment.CENTER; //居中
                            //边框
                            headStyle.BorderBottom = BorderStyle.THIN;
                            headStyle.BorderLeft = BorderStyle.THIN;
                            headStyle.BorderRight = BorderStyle.THIN;
                            headStyle.BorderTop = BorderStyle.THIN;
                            //font
                            HSSFFont font = workbook.CreateFont() as HSSFFont;
                            font.FontName = colHeadFont;
                            font.FontHeightInPoints = colHeadSize;
                            font.Boldweight = 700;

                            headStyle.SetFont(font);
                            foreach (DataColumn column in dt1.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32((arrColWidth[column.Ordinal] + 0.5) * 256));//设置列宽
                            }
                        }
                        rowIndex = 3;
                    }
                    #endregion

                    #region 填充内容
                    HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                    dataRow.HeightInPoints = Convert.ToInt16(colHeadSize * 1.4);
                    HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as HSSFCellStyle;
                    HSSFFont contentFont = workbook.CreateFont() as HSSFFont;
                    contentFont.FontName = colHeadFont;
                    contentFont.FontHeightInPoints = colHeadSize;

                    foreach (DataColumn column in dt1.Columns)
                    {
                        HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                        #region 画边框
                        headStyle2.BorderBottom = BorderStyle.THIN;
                        headStyle2.BorderLeft = BorderStyle.THIN;
                        headStyle2.BorderRight = BorderStyle.THIN;
                        headStyle2.BorderTop = BorderStyle.THIN;
                        #endregion
                        headStyle2.SetFont(contentFont);
                        dataRow.GetCell(column.Ordinal).CellStyle = headStyle2;

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
                                newCell.CellStyle = cellStyle; //格式化显示
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
                #endregion

                #region 建表 dt2
                if (dt2 != null && headText2 != null)
                {
                    int rowIndex2 = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        #region 新建表，填充表头，填充列头，样式
                        if (rowIndex2 == 0)
                        {
                            if (rowIndex2 != 1)
                            {
                                sheet = workbook.CreateSheet(headText2) as HSSFSheet;
                            }
                            // 表头及样式
                            {
                                HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                                headerRow.HeightInPoints = Convert.ToInt16(headSize * 1.4);
                                headerRow.CreateCell(0).SetCellValue(headText2);

                                HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                                headStyle.Alignment = HorizontalAlignment.CENTER;
                                HSSFFont font = workbook.CreateFont() as HSSFFont;
                                font.FontName = headFont;
                                font.FontHeightInPoints = headSize;
                                font.Boldweight = 700;
                                headStyle.SetFont(font);
                                headerRow.GetCell(0).CellStyle = headStyle;
                                CellRangeAddress region = new CellRangeAddress(0, 0, 0, dt2.Columns.Count - 1);//同上
                                sheet.AddMergedRegion(region);
                                sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                            }
                            // 导出时间
                            {
                                HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                                headerRow.CreateCell(0).SetCellValue(exportDate);
                                HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                                headStyle.Alignment = HorizontalAlignment.CENTER;
                                headerRow.GetCell(0).CellStyle = headStyle;
                                CellRangeAddress region = new CellRangeAddress(1, 1, 0, dt2.Columns.Count - 1);
                                sheet.AddMergedRegion(region);
                                sheet.SetEnclosedBorderOfRegion(region, BorderStyle.THIN, HSSFColor.BLACK.index);
                            }

                            // 列头及样式
                            {
                                HSSFRow headerRow = sheet.CreateRow(2) as HSSFRow;
                                HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                                headStyle.Alignment = HorizontalAlignment.CENTER;
                                #region 画边框
                                headStyle.BorderBottom = BorderStyle.THIN;
                                headStyle.BorderLeft = BorderStyle.THIN;
                                headStyle.BorderRight = BorderStyle.THIN;
                                headStyle.BorderTop = BorderStyle.THIN;
                                #endregion
                                HSSFFont font = workbook.CreateFont() as HSSFFont;
                                font.FontName = colHeadFont;
                                font.FontHeightInPoints = colHeadSize;
                                font.Boldweight = 700;
                                headStyle.SetFont(font);
                                foreach (DataColumn column in dt2.Columns)
                                {
                                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                    //设置列宽
                                    sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32((arrColWidth2[column.Ordinal] + 0.5) * 256));
                                }
                            }
                            rowIndex2 = 3;
                        }
                        #endregion

                        #region 填充内容
                        HSSFRow dataRow = sheet.CreateRow(rowIndex2) as HSSFRow;
                        HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        HSSFFont contentFont = workbook.CreateFont() as HSSFFont;
                        contentFont.FontName = colHeadFont;
                        contentFont.FontHeightInPoints = colHeadSize;
                        foreach (DataColumn column in dt2.Columns)
                        {
                            HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                            #region 画边框
                            headStyle2.BorderBottom = BorderStyle.THIN;
                            headStyle2.BorderLeft = BorderStyle.THIN;
                            headStyle2.BorderRight = BorderStyle.THIN;
                            headStyle2.BorderTop = BorderStyle.THIN;
                            #endregion
                            headStyle2.SetFont(contentFont);
                            dataRow.GetCell(column.Ordinal).CellStyle = headStyle2;

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
                                    newCell.CellStyle = cellStyle; //格式化显示
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
                }
                #endregion

                #region 页眉页脚
                sheet.Header.Center = "……";
                sheet.Footer.Left = "&D";   //日期
                sheet.Footer.Right = "&P";  //页码      
                #endregion
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

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
    }
}
