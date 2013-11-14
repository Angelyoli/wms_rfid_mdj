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
        public int index;

        HttpDataService httpDataService = new HttpDataService();
        DataTable detailTable = null;
        string positionType = "";
        string orderType = "";

        public TaskForm(string positionType,string orderType)
        {
            InitializeComponent();
            this.positionType = positionType;
            this.orderType = orderType;
        }

        private void BaseTaskForm_Load(object sender, EventArgs e)
        {
            string method = string.Format("GetOutTask/?positionType={0}&orderType={1}", positionType, orderType);

            detailTable = httpDataService.SearchOutTask(method);
            
            if (detailTable.Rows.Count == 0)
            {
                this.btnNext.Enabled = false;
            }
            
            DataGridTableStyle tableStyle = new DataGridTableStyle();

            DataGridColumnStyle columnStyle = new DataGridTextBoxColumn();
            columnStyle.MappingName = "TaskID";
            columnStyle.HeaderText = "任务号";
            columnStyle.Width = 40;
            tableStyle.GridColumnStyles.Add(columnStyle);

            columnStyle = new DataGridTextBoxColumn();
            columnStyle.MappingName = "CellName";
            columnStyle.HeaderText = "货位名称";
            columnStyle.Width = 90;
            tableStyle.GridColumnStyles.Add(columnStyle);

            columnStyle = new DataGridTextBoxColumn();
            columnStyle.MappingName = "ProductName";
            columnStyle.HeaderText = "卷烟";
            columnStyle.Width = 130;
            tableStyle.GridColumnStyles.Add(columnStyle);

            columnStyle = new DataGridTextBoxColumn();
            columnStyle.MappingName = "TaskQuantity";
            columnStyle.HeaderText = "作业数量";
            columnStyle.Width = 70;
            tableStyle.GridColumnStyles.Add(columnStyle);
            
            dgInfo.TableStyles.Add(tableStyle);

            DataView dataView = new DataView(detailTable);
            dataView.Sort = "CellName";
            dgInfo.DataSource = dataView;

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
                    task.Quantity = Convert.ToDecimal(dr[0]["Quantity"]);
                    task.PieceQuantity = Convert.ToDecimal(dr[0]["PieceQuantity"]);
                    task.BarQuantity = Convert.ToDecimal(dr[0]["BarQuantity"]);
                    task.ProductName = dr[0]["ProductName"].ToString();
                    task.Status = dr[0]["Status"].ToString();
                }
                DetailForm billDetailForm = new DetailForm(task, positionType, orderType);
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