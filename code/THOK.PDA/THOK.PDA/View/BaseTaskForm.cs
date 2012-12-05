using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using THOK.PDA.Util;
using THOK.PDA.Dal;
using THOK.WES.Interface.Model;
using System.Net;

namespace THOK.PDA.View
{
    public partial class BaseTaskForm : Form
    {
        private BillDal dal = new BillDal();
        private DataTable detailTable = null;
        HttpDataDal httpDataDal = new HttpDataDal();
        private string billType = "";
        private string billId = "";
        public int index;

        public BaseTaskForm(string billType, string billId)
        {
            InitializeComponent();
            this.billType = billType;
            this.billId = billId;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BaseTaskForm_Load(object sender, EventArgs e)
        {
            this.label2.Text = billId;
            switch (billType)
            {
                case "1":
                    this.label2.Text += "(入库)";
                    break;
                case "2":
                    this.label2.Text += "(出库)";
                    break;
                case "3":
                    this.label2.Text += "(移库)";
                    break;
                case "4":
                    this.label2.Text += "(盘点)";
                    break;
            }
            DataTable tempTable = null;
            if (SystemCache.ConnetionType == "USB连接")
            {
                tempTable = new DataTable();
                tempTable.Columns.Add("STORAGEID");
                tempTable.Columns.Add("OPERATENAME");
                tempTable.Columns.Add("TOBACCONAME");
                tempTable.Columns.Add("STATENAME");
                tempTable.Columns.Add("DETAILID");
                DataRow[] detailRows = SystemCache.DetailTable.Select("MASTER='" + billId + "' AND ConfirmState <> '3'");
                for (int i = 0; i < detailRows.Length; i++)
                {
                    DataRow row = tempTable.NewRow();
                    row["STORAGEID"] = detailRows[i]["STORAGEID"];
                    row["OPERATENAME"] = detailRows[i]["OPERATENAME"];
                    row["TOBACCONAME"] = detailRows[i]["TOBACCONAME"];
                    row["STATENAME"] = detailRows[i]["STATENAME"];
                    row["DETAILID"] = detailRows[i]["DETAILID"];
                    tempTable.Rows.Add(row);
                }
            }
            else
            {
                BillMaster billMaster = new BillMaster();
                billMaster.BillNo = billId;
                billMaster.BillType = billType;

                tempTable = httpDataDal.SearchBillDetail(billMaster);
                detailTable = tempTable;

            }
            this.dgInfo.DataSource = tempTable;
            if (tempTable.Rows.Count == 0)
            {
                this.btnNext.Enabled = false;
            }

            DataGridTableStyle gridStyle = new DataGridTableStyle();
            gridStyle.MappingName = tempTable.TableName;
            dgInfo.TableStyles.Add(gridStyle);
            GridColumnStylesCollection columnStyles = this.dgInfo.TableStyles[0].GridColumnStyles;

            columnStyles["operateStorageName"].HeaderText = "   货  位";
            columnStyles["operateStorageName"].Width = 100;
            columnStyles["operateProductName"].HeaderText = "   烟  名";
            columnStyles["operateProductName"].Width = 120;
            columnStyles["operateName"].HeaderText = "  类  型";
            columnStyles["operateName"].Width = 50;
            columnStyles["StatusName"].HeaderText = "  状态";
            columnStyles["StatusName"].Width = 50;
            columnStyles["DetailID"].HeaderText = "单据号";
            //不显示，宽度设为0
            columnStyles["operateName"].Width = 0;
            columnStyles["DetailID"].Width = 0;
            columnStyles["operatePieceQuantity"].Width = 0;
            columnStyles["operateBarQuantity"].Width = 0;
            columnStyles["targetStorageName"].Width = 0;

            if (tempTable.Rows.Count != 0)
            {
                if (tempTable.Rows.Count <= index)
                {
                    index = tempTable.Rows.Count-1;
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
                BillMasterForm billMasterForm = new BillMasterForm(billType);
                billMasterForm.Show();
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                if (!(SystemCache.ConnetionType == "USB连接"))
                {
                    BillDetail billDetail = new BillDetail();
                    billDetail.BillNo = billId;
                    billDetail.BillType = billType;
                    billDetail.DetailID = int.Parse(this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 0].ToString());
                    billDetail.PieceQuantity = decimal.Parse(this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 5].ToString());
                    billDetail.BarQuantity = decimal.Parse(this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 6].ToString());
                    billDetail.Operator = Dns.GetHostName();
                    httpDataDal.Apply(billDetail);
                    //修改内存中对应作业的状态为：已申请
                    DataRow[] rows = detailTable.Select(string.Format("DetailID = {0}", this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 0].ToString()));
                    rows[0]["StatusName"] = "已申请";

                }

                BillDetailForm billDetailForm = new BillDetailForm(billType, this.dgInfo[this.dgInfo.CurrentCell.RowNumber, 0].ToString(), billId, detailTable);
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

        private void BaseTaskForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.btnBack_Click(null, null);
            }
            if (e.KeyCode.ToString() == "Return")
            {
                this.btnNext_Click(null, null);
            }
        }
    }
}