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
        public GridUtil(DataGridView dataGridView)
        {
            gridView = dataGridView;
            gridView.CellFormatting += new DataGridViewCellFormattingEventHandler(gridView_CellFormatting);
            gridView.CellPainting += new DataGridViewCellPaintingEventHandler(gridView_CellPainting);
        }

        #region"合并单元格的测试"

        private int? nextrow = null;
        private int? nextcol = null;

        private void gridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.gridView.Columns["PieceQuantity"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                if (this.nextcol != null & e.ColumnIndex == this.nextcol)
                {
                    e.CellStyle.BackColor = Color.LightBlue;
                    this.nextcol = null;
                }
                if (this.nextrow != null & e.RowIndex == nextrow)
                {
                    e.CellStyle.BackColor = Color.LightPink;
                    this.nextrow = null;
                }
                if (e.RowIndex != this.gridView.RowCount - 1)
                {
                    int storageColumnIndex = this.gridView.Columns["Storage"].Index;
                    if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() == this.gridView.Rows[e.RowIndex + 1].Cells[storageColumnIndex].Value.ToString())
                    {
                        e.CellStyle.BackColor = Color.LightPink;
                        nextrow = e.RowIndex + 1;
                    }
                }

            }
        }
        
        //绘制单元格
        private decimal sumQuantity = 0;
        private int y = 0;
        private void gridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //纵向合并            
            if (this.gridView.Columns["PieceQuantity"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                using (Brush gridBrush = new SolidBrush(this.gridView.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        //擦除原单元格背景
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        //绘制线条,这些线条是单元格相互间隔的区分线条,
                        //因为我们只对列name做处理,所以datagridview自己会处理左侧和上边缘的线条
                        int storageColumnIndex = this.gridView.Columns["Storage"].Index;
                        if (e.RowIndex != this.gridView.RowCount - 1)
                        {
                            if (this.gridView.Rows[e.RowIndex].Cells[storageColumnIndex].Value.ToString() != this.gridView.Rows[e.RowIndex + 1].Cells[storageColumnIndex].Value.ToString())
                            {
                                //下边缘的线
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                                //绘制值
                                if (e.Value != null)
                                {
                                    var cell = this.gridView.Rows[e.RowIndex].Cells[this.gridView.Columns["PieceQuantity"].Index];
                                    sumQuantity = sumQuantity + Convert.ToDecimal(e.Value);                                   
                                    if (y != 0)
                                    {
                                        y = (y + e.CellBounds.Y + 10) / 2;
                                    }
                                    else
                                    {
                                        y = e.CellBounds.Y + 2;
                                    }
                                    if (cell.Tag == null)
                                    {
                                        cell.Tag = new object [] { sumQuantity, y };
                                    }
                                    object [] tag = (object []) cell.Tag;
                                    e.Graphics.DrawString(tag[0].ToString(), e.CellStyle.Font,
                                        Brushes.Crimson, e.CellBounds.X + 2,
                                        Convert.ToInt32(tag[1]), StringFormat.GenericDefault);
                                    sumQuantity = 0;
                                    y = 0;
                                }
                            }
                            else
                            {
                                sumQuantity = sumQuantity + Convert.ToDecimal(e.Value);
                                if (y == 0)
                                {
                                    y = e.CellBounds.Y;
                                }                                
                            }
                        }
                        else
                        {
                            //下边缘的线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                            //绘制值
                            if (e.Value != null)
                            {
                                var cell = this.gridView.Rows[e.RowIndex].Cells[this.gridView.Columns["PieceQuantity"].Index];
                                sumQuantity = sumQuantity + Convert.ToDecimal(e.Value);
                                if (y != 0)
                                {
                                    y = (y + e.CellBounds.Y +10) / 2;
                                }
                                else
                                {
                                    y = e.CellBounds.Y + 2;
                                }
                                if (cell.Tag == null)
                                {
                                    cell.Tag = new object[] { sumQuantity, y };
                                }
                                object[] tag = (object[])cell.Tag;
                                e.Graphics.DrawString(tag[0].ToString(), e.CellStyle.Font,
                                    Brushes.Crimson, e.CellBounds.X + 2,
                                    Convert.ToInt32(tag[1]), StringFormat.GenericDefault);
                                sumQuantity = 0;
                                y = 0;
                            }
                        }
                        //右侧的线
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);

                        e.Handled = true;
                    }
                }
            }
        }

        #endregion
    }
}
