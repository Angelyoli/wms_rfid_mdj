using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Net;
using THOK.PDA.Util;
using THOK.PDA.Service;
using THOK.PDA.Model;

namespace THOK.PDA.View
{
    public partial class TaskForm : Form
    {
        HttpDataService httpDataService = new HttpDataService();
        DataTable detailTable = null;
        string positionType = "";
        public int index;

        public TaskForm(string positionType)
        {
            InitializeComponent();
            this.positionType = positionType;
        }

        private void BaseTaskForm_Load(object sender, EventArgs e)
        {
            string method = "GetOutTask/?positionType=" + positionType;

            detailTable = httpDataService.SearchOutTask(method);

            this.dgInfo.DataSource = detailTable;
            if (detailTable.Rows.Count == 0)
            {
                this.btnNext.Enabled = false;
            }

            DataGridTableStyle gridStyle = new DataGridTableStyle();
            gridStyle.MappingName = detailTable.TableName;
            dgInfo.TableStyles.Add(gridStyle);
            GridColumnStylesCollection columnStyles = this.dgInfo.TableStyles[0].GridColumnStyles;
            columnStyles["TaskID"].HeaderText = "任务号";
            columnStyles["ProductName"].HeaderText = "卷烟";
            columnStyles["Quantity"].HeaderText = "总数量";
            columnStyles["TaskQuantity"].HeaderText = "数量(件)";
            columnStyles["PieceQuantity"].HeaderText = "数量(件)";
            columnStyles["BarQuantity"].HeaderText = "数量(条)";
            columnStyles["CellName"].HeaderText = "货位名称";
            columnStyles["OrderID"].HeaderText = "订单号";
            columnStyles["OrderType"].HeaderText = "类型";
            columnStyles["Status"].HeaderText = "状态";
            //如不显示，宽度设为0
            columnStyles["TaskID"].Width = 40;
            columnStyles["OrderType"].Width = 40;
            columnStyles["ProductName"].Width = 120;
            columnStyles["TaskQuantity"].Width = 70;
            columnStyles["Status"].Width = 40;

            columnStyles["PieceQuantity"].Width = 0;
            columnStyles["BarQuantity"].Width = 0;
            columnStyles["Quantity"].Width = 0;
            columnStyles["CellName"].Width = 0;
            columnStyles["OrderID"].Width = 0;

            if (detailTable.Rows.Count != 0)
            {
                if (detailTable.Rows.Count <= index)
                {
                    index = detailTable.Rows.Count - 1;
                }
                dgInfo.Select(index);
                dgInfo.CurrentRowIndex = index;
                dgInfo.Focus();
            }
            WaitCursor.Restore();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Close();
                WaitCursor.Restore();
            }
            catch (Exception ex)
            {
                WaitCursor.Restore();
                MessageBox.Show(ex.Message);
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                RestTask task = new RestTask();
                if (SystemCache.ConnetionType == "NetWork")
                {
                    string taskId = this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 0].ToString();
                    DataRow[] dr = detailTable.Select("TaskID=" + taskId);
                    task.TaskID = Convert.ToInt32(dr[0]["TaskID"]);
                    task.OrderID = dr[0]["OrderID"].ToString();
                    task.OrderType = dr[0]["OrderType"].ToString();
                    task.CellName = dr[0]["CellName"].ToString();
                    
                    task.PieceQuantity = Convert.ToDecimal(dr[0]["PieceQuantity"]);
                    task.BarQuantity = Convert.ToDecimal(dr[0]["BarQuantity"]);
                    task.ProductName = dr[0]["ProductName"].ToString();
                    task.Status = dr[0]["Status"].ToString();
                }
                DetailForm billDetailForm = new DetailForm(task, positionType);
                billDetailForm.Index = this.index;
                billDetailForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                WaitCursor.Restore();
                MessageBox.Show(ex.Message);
            }
        }

        private void dgInfo_CurrentCellChanged(object sender, EventArgs e)
        {
            this.dgInfo.Select(this.dgInfo.CurrentCell.RowNumber);
            this.index = this.dgInfo.CurrentCell.RowNumber;
        }     
    }
}