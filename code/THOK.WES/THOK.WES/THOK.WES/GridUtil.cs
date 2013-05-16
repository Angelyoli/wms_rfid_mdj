using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace THOK.WES
{
    class GridUtil
    {
        private DataGridView gridView = new DataGridView();
        private List<string> MergeColumnNames
        {
            get
            {
                return _mergecolumnname;
            }
            set
            {
                _mergecolumnname = value;
            }
        }
        private List<string> _mergecolumnname = new List<string>();
        private int storageColumnIndex = 0;
        private int productCodeColumnIndex = 0;
        private int billTypeNameColumnIndex = 0;
        private int targetStorageColumnIndex = 0;
        private int totalColumnIndex = 0;

        public GridUtil(DataGridView dataGridView)
        {
            gridView = dataGridView;
            MergeColumnNames.Add("Storage");
            storageColumnIndex = this.gridView.Columns["Storage"].Index;
            productCodeColumnIndex = this.gridView.Columns["ProductCode"].Index;
            billTypeNameColumnIndex = this.gridView.Columns["BillTypeName"].Index;
            targetStorageColumnIndex = this.gridView.Columns["TargetStorage"].Index;
            totalColumnIndex = this.gridView.Columns["Total"].Index;
            gridView.CellFormatting += new DataGridViewCellFormattingEventHandler(gridView_CellFormatting1);
            gridView.CellFormatting += new DataGridViewCellFormattingEventHandler(gridView_CellFormatting2);
            gridView.CellPainting += new DataGridViewCellPaintingEventHandler(gridView_CellPainting);
            gridView.CellClick += new DataGridViewCellEventHandler(gridView_CellClick);
        }

        #region"合并单元格的测试"

        private int? nextrow1 = null;
        private int? nextcol1 = null;
        private void gridView_CellFormatting1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == totalColumnIndex && e.RowIndex >= 0)
            {
                if (this.nextcol1 != null & e.ColumnIndex == this.nextcol1)
                {
                    e.CellStyle.BackColor = Color.White;
                    this.nextcol1 = null;
                }
                if (this.nextrow1 != null & e.RowIndex == nextrow1)
                {
                    e.CellStyle.BackColor = Color.LightPink;
                    this.nextrow1 = null;
                }
                if (e.RowIndex != this.gridView.RowCount - 1)
                {
                    if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() ==
                                    this.gridView.Rows[e.RowIndex + 1].Cells[storageColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[productCodeColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[e.RowIndex + 1].Cells[productCodeColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[billTypeNameColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[e.RowIndex + 1].Cells[billTypeNameColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                                && this.gridView.Rows[e.RowIndex + 1].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                        )
                    {
                        e.CellStyle.BackColor = Color.LightPink;
                        nextrow1 = e.RowIndex + 1;
                    }
                }
            }
        }

        private int? nextrow2 = null;
        private int? nextcol2 = null;
        private void gridView_CellFormatting2(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == storageColumnIndex && e.RowIndex >= 0)
            {
                if (this.nextcol2 != null & e.ColumnIndex == this.nextcol2)
                {
                    e.CellStyle.BackColor = Color.White;
                    this.nextcol2 = null;
                }
                if (this.nextrow2 != null & e.RowIndex == nextrow2)
                {
                    e.CellStyle.BackColor = Color.LightPink;
                    this.nextrow2 = null;
                }
                if (e.RowIndex != this.gridView.RowCount - 1)
                {
                    if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() ==
                            this.gridView.Rows[e.RowIndex + 1].Cells[storageColumnIndex].Value.ToString()                                
                        )
                    {
                        e.CellStyle.BackColor = Color.LightPink;
                        nextrow2 = e.RowIndex + 1;
                    }
                }
            }
        }

        private void gridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DrawTotalCell(e);
                DrawMergeCell(e);
            }
        }

        private void gridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = this.gridView.CurrentRow.Index;
            #region 获取下面的行数
            for (int i = currentRowIndex; i < this.gridView.Rows.Count; i++)
            {
                if (this.gridView.Rows[currentRowIndex].Cells[storageColumnIndex].Value.ToString() ==
                            this.gridView.Rows[i].Cells[storageColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[productCodeColumnIndex].Value.ToString() ==
                               this.gridView.Rows[i].Cells[productCodeColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[billTypeNameColumnIndex].Value.ToString() ==
                               this.gridView.Rows[i].Cells[billTypeNameColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                        && this.gridView.Rows[i].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                    )
                {

                    if (currentRowIndex != i)
                    {
                        this.gridView.Rows[i].Selected = true;
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion
            #region 获取上面的行数
            for (int i = currentRowIndex; i >= 0; i--)
            {
                if (this.gridView.Rows[currentRowIndex].Cells[storageColumnIndex].Value.ToString() ==
                            this.gridView.Rows[i].Cells[storageColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[productCodeColumnIndex].Value.ToString() ==
                               this.gridView.Rows[i].Cells[productCodeColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[billTypeNameColumnIndex].Value.ToString() ==
                               this.gridView.Rows[i].Cells[billTypeNameColumnIndex].Value.ToString()
                        && this.gridView.Rows[currentRowIndex].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                        && this.gridView.Rows[i].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                    )
                {
                    if (currentRowIndex != i)
                    {
                        this.gridView.Rows[i].Selected = true;
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion
        }

        #endregion

        #region 自定义方法

        private void DrawTotalCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            Brush gridBrush = new SolidBrush(this.gridView.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellwidth;
            //上面相同的行数
            int UpRows = 0;
            //下面相同的行数
            int DownRows = 0;
            //总行数
            int count = 0;
            if (e.ColumnIndex == totalColumnIndex && e.RowIndex >= 0)
            {
                cellwidth = e.CellBounds.Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = e.Value == null ? "" : e.Value.ToString().Trim();
                decimal value = 0;
                if (!string.IsNullOrEmpty(curValue))
                {
                    #region 获取下面的行数
                    for (int i = e.RowIndex; i < this.gridView.Rows.Count; i++)
                    {
                        if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() ==
                                    this.gridView.Rows[i].Cells[storageColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[productCodeColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[i].Cells[productCodeColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[billTypeNameColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[i].Cells[billTypeNameColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                                && this.gridView.Rows[i].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                            )
                        {
                            DownRows++;
                            value += Convert.ToDecimal(this.gridView.Rows[i].Cells[totalColumnIndex].Value);
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() ==
                                    this.gridView.Rows[i].Cells[storageColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[productCodeColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[i].Cells[productCodeColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[billTypeNameColumnIndex].Value.ToString() ==
                                       this.gridView.Rows[i].Cells[billTypeNameColumnIndex].Value.ToString()
                                && this.gridView.Rows[e.RowIndex].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                                && this.gridView.Rows[i].Cells[targetStorageColumnIndex].Value.ToString().Contains("分拣线")
                            )
                        {
                            UpRows++;                            
                            if (e.RowIndex != i)
                            {
                                value += Convert.ToDecimal(this.gridView.Rows[i].Cells[totalColumnIndex].Value);
                                cellwidth = cellwidth < this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    count = DownRows + UpRows - 1;
                    if (count < 2)
                    {
                        return;
                    }
                }

                if (this.gridView.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e,value.ToString(), cellwidth, UpRows, DownRows, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    count = 0;
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                e.Handled = true;
            }
        }
 
        private void DrawMergeCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            Brush gridBrush = new SolidBrush(this.gridView.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellwidth;
            //上面相同的行数
            int UpRows = 0;
            //下面相同的行数
            int DownRows = 0;
            //总行数
            int count = 0;
            if (this.MergeColumnNames.Contains(this.gridView.Columns[e.ColumnIndex].Name) && e.RowIndex != -1)
            {
                cellwidth = e.CellBounds.Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = e.Value == null ? "" : e.Value.ToString().Trim();
                if (!string.IsNullOrEmpty(curValue))
                {
                    #region 获取下面的行数
                    for (int i = e.RowIndex; i < this.gridView.Rows.Count; i++)
                    {
                        if (this.gridView.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            DownRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.gridView.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            UpRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.gridView.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    count = DownRows + UpRows - 1;
                    if (count < 2)
                    {
                        return;
                    }
                }
                if (this.gridView.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e,(string)e.Value,cellwidth, UpRows, DownRows, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    count = 0;
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                e.Handled = true;
            }
        }

        private void PaintingFont(DataGridViewCellPaintingEventArgs e,string value, int cellwidth, int UpRows, int DownRows, int count)
        {
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int fontheight = (int)e.Graphics.MeasureString(value.ToString(), e.CellStyle.Font).Height;
            int fontwidth = (int)e.Graphics.MeasureString(value.ToString(), e.CellStyle.Font).Width;
            int cellheight = e.CellBounds.Height;

            if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomLeft)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopCenter)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopLeft)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopRight)
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else
            {
                e.Graphics.DrawString((String)value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
        }
        
        #endregion
    }
}
