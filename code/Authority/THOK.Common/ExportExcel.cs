using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Common
{
    public class ExportExcel
    {
        #region 导出单表Excel
        /// <summary>DataTable导出到Excel的MemoryStream</summary>
        public static System.IO.MemoryStream ExportDT(System.Data.DataTable dt, string headText, string[] str, string exportDate)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.CreateSheet(headText) as NPOI.HSSF.UserModel.HSSFSheet;

            NPOI.HSSF.UserModel.HSSFCellStyle dateStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
            NPOI.HSSF.UserModel.HSSFDataFormat format = workbook.CreateDataFormat() as NPOI.HSSF.UserModel.HSSFDataFormat;
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
            int rowIndex = 0;
            foreach (System.Data.DataRow row in dt.Rows)
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
                        headerRow.CreateCell(0).SetCellValue(headText);
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                        font.FontName = str[5];                             //[5]
                        font.FontHeightInPoints = Convert.ToInt16(str[0]);  //[0]
                        font.Boldweight = Convert.ToInt16(str[1]);          //[1]
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt.Columns.Count - 1));
                    }
                    #endregion
                    #region 导出时间
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(1) as NPOI.HSSF.UserModel.HSSFRow;
                        headerRow.CreateCell(0).SetCellValue(exportDate);
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(1, 0, 1, dt.Columns.Count - 1));
                    }
                    #endregion
                    #region 列头及样式
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(2) as NPOI.HSSF.UserModel.HSSFRow;
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
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
                    }
                    #endregion
                    rowIndex = 3;
                }
                #endregion
                #region 填充内容
                NPOI.HSSF.UserModel.HSSFRow dataRow = sheet.CreateRow(rowIndex) as NPOI.HSSF.UserModel.HSSFRow;
                foreach (System.Data.DataColumn column in dt.Columns)
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
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }       
        #endregion

        #region 导出双表Excel
        /// <summary>DataTable导出到Excel的MemoryStream</summary>
        public static System.IO.MemoryStream ExportDT(System.Data.DataTable dt, System.Data.DataTable dt2, string headText, string headText2, string[] str, string exportDate)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.CreateSheet(headText) as NPOI.HSSF.UserModel.HSSFSheet;

            NPOI.HSSF.UserModel.HSSFCellStyle dateStyle = workbook.CreateCellStyle() as  NPOI.HSSF.UserModel.HSSFCellStyle;
            NPOI.HSSF.UserModel.HSSFDataFormat format = workbook.CreateDataFormat() as NPOI.HSSF.UserModel.HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽 dt
            //取得列宽
            int[] arrColWidth = new int[dt.Columns.Count];
            foreach (System.Data.DataColumn item in dt.Columns)
            {
                //936是指GB2312编码
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
            #endregion

            #region 取得列宽 dt2
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

            #region 建表 dt
            int rowIndex = 0;
            foreach (System.Data.DataRow row in dt.Rows)
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
                        headerRow.CreateCell(0).SetCellValue(headText);
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                        font.FontName = str[5];                             //[5]
                        font.FontHeightInPoints = Convert.ToInt16(str[0]);  //[0]
                        font.Boldweight = Convert.ToInt16(str[1]);          //[1]
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt.Columns.Count - 1));
                    }
                    #endregion
                    #region 导出时间
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(1) as NPOI.HSSF.UserModel.HSSFRow;
                        headerRow.CreateCell(0).SetCellValue(exportDate);
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(1, 0, 1, dt.Columns.Count - 1));
                    }
                    #endregion
                    #region 列头及样式
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow = sheet.CreateRow(2) as NPOI.HSSF.UserModel.HSSFRow;
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
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
                    }
                    #endregion
                    rowIndex = 3;
                }
                #endregion
                #region 填充内容
                NPOI.HSSF.UserModel.HSSFRow dataRow = sheet.CreateRow(rowIndex) as NPOI.HSSF.UserModel.HSSFRow;
                foreach (System.Data.DataColumn column in dt.Columns)
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
            #endregion

            #region 建表 dt2
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
                        NPOI.HSSF.UserModel.HSSFRow headerRow2 = sheet.CreateRow(0) as NPOI.HSSF.UserModel.HSSFRow;
                        headerRow2.HeightInPoints = 25;
                        headerRow2.CreateCell(0).SetCellValue(headText2);

                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                        font.FontName = str[5];                             //[5]
                        font.FontHeightInPoints = Convert.ToInt16(str[0]);  //[0]
                        font.Boldweight = Convert.ToInt16(str[1]);          //[1]
                        headStyle2.SetFont(font);
                        headerRow2.GetCell(0).CellStyle = headStyle2;
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt2.Columns.Count - 1));
                        //headerRow.Dispose();
                    }
                    #endregion
                    #region 导出时间
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow2 = sheet.CreateRow(1) as NPOI.HSSF.UserModel.HSSFRow;
                        headerRow2.CreateCell(0).SetCellValue(exportDate);
                        sheet.AddMergedRegion(new NPOI.SS.Util.Region(1, 0, 1, dt2.Columns.Count - 1));
                    }
                    #endregion
                    #region 列头及样式
                    {
                        NPOI.HSSF.UserModel.HSSFRow headerRow2 = sheet.CreateRow(2) as NPOI.HSSF.UserModel.HSSFRow;
                        NPOI.HSSF.UserModel.HSSFCellStyle headStyle2 = workbook.CreateCellStyle() as NPOI.HSSF.UserModel.HSSFCellStyle;
                        headStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        NPOI.HSSF.UserModel.HSSFFont font = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;
                        font.FontName = str[6];                             //[6]
                        font.FontHeightInPoints = Convert.ToInt16(str[2]);  //[2]
                        font.Boldweight = Convert.ToInt16(str[3]);          //[3]
                        headStyle2.SetFont(font);
                        foreach (System.Data.DataColumn column2 in dt2.Columns)
                        {
                            headerRow2.CreateCell(column2.Ordinal).SetCellValue(column2.ColumnName);
                            headerRow2.GetCell(column2.Ordinal).CellStyle = headStyle2;
                            //设置列宽
                            sheet.SetColumnWidth(column2.Ordinal, (arrColWidth2[column2.Ordinal] + 1) * Convert.ToInt32(str[4]));  //[4]
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

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }        
        #endregion
    }
}
