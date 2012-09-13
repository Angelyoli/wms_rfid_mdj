using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Common
{
    public class ExportExcel
    {
        #region 导出EXCEL单表双表
        /// <summary>
        /// DataTable导出到EXCEL的MemoryStream
        /// </summary>
        /// <param name="dt1">DataTable</param>
        /// <param name="dt2">DataTable</param>
        /// <param name="headText1">标题</param>
        /// <param name="headText2">标题</param>
        /// <param name="headFont">字体</param>
        /// <param name="headSize">大小</param>
        /// <param name="colHeadFont">字体</param>
        /// <param name="colHeadSize">大小</param>
        /// <param name="colHeadWidth">宽度</param>
        /// <param name="exportDate">导出时间</param>
        /// <returns></returns>
        public static System.IO.MemoryStream ExportDT(System.Data.DataTable dt1, System.Data.DataTable dt2,
            string headText1, string headText2,
            string headFont, Int16 headSize,
            string colHeadFont, Int16 colHeadSize, Int16 colHeadWidth,
            string exportDate)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.CreateSheet(headText1) as NPOI.HSSF.UserModel.HSSFSheet;
            //sheet.CreateRow(0).Height = 1000;

            NPOI.HSSF.UserModel.HSSFCellStyle cellStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
            NPOI.HSSF.UserModel.HSSFDataFormat format = workbook.CreateDataFormat() as NPOI.HSSF.UserModel.HSSFDataFormat;
            cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽 dt1
            //取得列宽
            int[] arrColWidth = new int[dt1.Columns.Count];
            foreach (System.Data.DataColumn item in dt1.Columns)
            {
                //936是指GB2312编码
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
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
                foreach (System.Data.DataColumn item in dt2.Columns)
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

            int a = dt1.Rows.Count;
            if (a < 65536)  /* excel中的一个sheet最大容量65536行 */
            {
                #region 建表 dt1
                int rowIndex = 0;
                foreach (System.Data.DataRow row in dt1.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = workbook.CreateSheet() as NPOI.HSSF.UserModel.HSSFSheet;
                        }
                        #region 表头及样式
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(0) as NPOI.HSSF.UserModel.HSSFRow;
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(headText1);
                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                            font.FontName = headFont;
                            font.FontHeightInPoints = headSize;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            //sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt1.Columns.Count - 1));//过期方法，但还能用
                            NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt1.Columns.Count - 1);//同上
                            sheet.AddMergedRegion(region);
                            sheet.SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);//给合并的画线
                        }
                        #endregion
                        #region 导出时间
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(1) as NPOI.HSSF.UserModel.HSSFRow;
                            headerRow.CreateCell(0).SetCellValue(exportDate);
                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            headerRow.GetCell(0).CellStyle = headStyle;
                            NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, dt1.Columns.Count - 1);
                            sheet.AddMergedRegion(region);
                            sheet.SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);
                        }
                        #endregion
                        #region 列头及样式
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(2) as NPOI.HSSF.UserModel.HSSFRow;
                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                            headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                            headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                            headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                            NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                            font.FontName = colHeadFont;
                            font.FontHeightInPoints = colHeadSize;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            foreach (System.Data.DataColumn column in dt1.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                                //sheet.SetColumnWidth(0, 30 * 256);
                                //sheet.AutoSizeColumn((arrColWidth[column.Ordinal] + 1) * 256);
                            }
                        }
                        #endregion
                        rowIndex = 3;
                    }
                    #endregion
                    #region 填充内容
                    NPOI.HSSF.UserModel.HSSFRow dataRow = sheet.CreateRow(rowIndex) as NPOI.HSSF.UserModel.HSSFRow;
                    foreach (System.Data.DataColumn column in dt1.Columns)
                    {
                        NPOI.HSSF.UserModel.HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as NPOI.HSSF.UserModel.HSSFCell;
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                        headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                        headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                        headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                        dataRow.GetCell(column.Ordinal).CellStyle = headStyle;

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
            }

            #region 建表 dt2
            if (dt2 != null && headText2 != null)
            {
                int rowIndex2 = 0;
                foreach (System.Data.DataRow row in dt2.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex2 == 0)
                    {
                        if (rowIndex2 != 1)
                        {
                            sheet = workbook.CreateSheet(headText2) as NPOI.HSSF.UserModel.HSSFSheet;
                        }
                        #region 表头及样式
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(0) as NPOI.HSSF.UserModel.HSSFRow;
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(headText2);

                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                            font.FontName = headFont;
                            font.FontHeightInPoints = headSize;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt2.Columns.Count - 1));
                        }
                        #endregion
                        #region 导出时间
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(1) as NPOI.HSSF.UserModel.HSSFRow;
                            headerRow.CreateCell(0).SetCellValue(exportDate);
                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.Region(1, 0, 1, dt2.Columns.Count - 1));
                        }
                        #endregion
                        #region 列头及样式
                        {
                            NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(2) as NPOI.HSSF.UserModel.HSSFRow;
                            NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                            NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                            font.FontName = colHeadFont;
                            font.FontHeightInPoints = colHeadSize;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            foreach (System.Data.DataColumn column in dt2.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth2[column.Ordinal] + 1) * colHeadWidth);  //[4]
                            }
                        }
                        #endregion
                        rowIndex2 = 3;
                    }
                    #endregion
                    #region 填充内容
                    NPOI.HSSF.UserModel.HSSFRow dataRow = sheet.CreateRow(rowIndex2) as NPOI.HSSF.UserModel.HSSFRow;
                    foreach (System.Data.DataColumn column in dt2.Columns)
                    {
                        NPOI.HSSF.UserModel.HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as NPOI.HSSF.UserModel.HSSFCell;
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

            //sheet.SetColumnWidth(1, 100 * 256);
            sheet.PrintSetup.FitHeight = 1;
            sheet.Header.Center = "&D";    //日期
            sheet.Footer.Left = "页脚";
            sheet.Footer.Right = "&P";     //页码


            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
        #endregion
    }
}
