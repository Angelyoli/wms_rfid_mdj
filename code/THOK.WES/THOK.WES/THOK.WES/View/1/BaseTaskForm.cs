using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;
using THOK.WES;
using THOK.WES.Interface;
using THOK.WES.Interface.Model;
using System.Collections;

namespace THOK.WES.View
{
    public partial class BaseTaskForm : THOK.AF.View.ToolbarForm
    {
        private ConfigUtil configUtil = new ConfigUtil();
        private string operateStorageName = "";
        private string targetStorageName = "";
        private string operateName = "";
        private string operateProductName = "";
        private int operatePieceQuantity = 0;
        private int operateBarQuantity = 0;

        private string url = @"http://59.61.87.212:8090/Task";

        /// <summary>
        /// 1：入库单；2：出库单；3：移库单；4：盘点单
        /// </summary>
        protected string BillTypes = "";

        //选择的主单；
        BillMaster BillMaster = null;

        private string RfidReadProductCode = "";

        /// <summary>
        /// Real: 实时出库；NoReal: 非实时出库；
        /// </summary>
        private string OperateType = "";

        /// <summary>
        /// 操作区域 = 0：条烟柜；1～N ：货架层号；
        /// </summary>
        private string OperateAreas = "";

        /// <summary>
        /// 使用电子标签 = 0：不使用；1：使用；
        /// </summary>
        private string UseTag = "";

        /// <summary>
        /// 使用Rfid  = 0：不使用；1：使用；
        /// </summary>
        private string UseRfid = "";

        public BaseTaskForm()
        {
            InitializeComponent();

            url = configUtil.GetConfig("URL")["URL"];
            OperateAreas = configUtil.GetConfig("Layers")["Number"];
            UseRfid = configUtil.GetConfig("RFID")["USEDRFID"];

            if (configUtil.GetConfig("DeviceType")["Device"] == "0")
            {
                this.dgvMain.ColumnHeadersHeight = 40;
                this.dgvMain.RowTemplate.Height = 40;
                this.dgvMain.DefaultCellStyle.Font = new Font("宋体", 16);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 13);
                this.btnBatConfirm.Visible = false;
                UseTag = "0";
            }
            else if (configUtil.GetConfig("DeviceType")["Device"] == "1")
            {
                this.dgvMain.ColumnHeadersHeight = 40;
                this.dgvMain.RowTemplate.Height = 40;
                this.dgvMain.DefaultCellStyle.Font = new Font("宋体", 16);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 13);
                this.btnBatConfirm.Visible = false;
                UseTag = "1";
            }
            else
            {
                this.dgvMain.ColumnHeadersHeight = 22;
                this.dgvMain.RowTemplate.Height = 22;
                this.dgvMain.DefaultCellStyle.Font = new Font("宋体", 10);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 10);
                UseTag = "1";
            }
        }

        //查询
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string billNo = string.Empty;
                Task task = new Task(Application.OpenForms[0], url);
                task.GetBillMasterCompleted += new Task.GetBillMasterCompletedEventHandler(delegate(bool isSuccess, string msg, BillMaster[] billMasters)
                {
                    ClosePlWailt();
                    if (billMasters != null)
                    {
                        switch (billMasters.Length)
                        {
                            case 0:
                                billNo = "";
                                break;
                            case 1:
                                billNo = billMasters[0].BillNo;
                                break;
                            default:
                                SelectDialog selectDialog = new SelectDialog(billMasters);
                                if (selectDialog.ShowDialog() == DialogResult.OK)
                                {
                                    billNo = selectDialog.SelectedBillID;
                                }
                                break;
                        }
                        foreach (BillMaster billMaster in billMasters)
                        {
                            if (billNo == billMaster.BillNo)
                            {
                                this.BillMaster = billMaster;
                            }
                        }
                    }
                    RefreshData();
                });
                task.SearchBillMaster(BillTypes);
                DisplayPlWailt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //刷新数据
        private void RefreshData()
        {
            if (BillMaster == null)
            {
                dgvMain.DataSource = null;
                return;
            }
            sslBillID.Text = "单据号：" + BillMaster.BillNo + "                              ";
            sslOperator.Text = "操作员：" + Environment.MachineName;

            Task task = new Task(Application.OpenForms[0], url);
            task.GetBillDetailCompleted += new Task.GetBillDetailCompletedEventHandler(delegate(bool isSuccess, string msg, BillDetail[] billDetails)
            {
                InTask = false;
                if (billDetails != null && billDetails.Length != 0)
                {
                    dgvMain.AutoGenerateColumns = false;
                    dgvMain.DataSource = billDetails;
                    foreach (BillDetail billDetail in billDetails)
                    {
                        if (billDetail.Status == "1")
                        {
                            InTask = true;
                            btnCancel.Enabled = true;
                            btnConfirm.Enabled = true;
                            btnBatConfirm.Enabled = true;
                        }
                        else
                        {
                            btnApply.Enabled = true;
                        }
                    }
                }
                else
                {
                    dgvMain.DataSource = null;
                }
                ClosePlWailt();
            });
            task.SearchBillDetail(new BillMaster[] { this.BillMaster }, RfidReadProductCode, OperateType, OperateAreas, Environment.MachineName);
            DisplayPlWailt();
        }

        //申请
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    IList<BillDetail> billDetails = new List<BillDetail>();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = row.Cells["BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.PieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                        billDetail.BarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);
                        billDetail.Operator = Environment.MachineName;
                        billDetails.Add(billDetail);
                    }
                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(Application.OpenForms[0], url);
                    task.ApplyCompleted += new Task.ApplyCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        RefreshData();
                    });
                    task.Apply(tmp, UseTag);
                }
                else
                    MessageBox.Show("请选择要执行的仓库作业。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("申请失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //取消申请
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    IList<BillDetail> billDetails = new List<BillDetail>();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = row.Cells["BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.Operator = Environment.MachineName;
                        billDetails.Add(billDetail);
                    }
                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(Application.OpenForms[0], url);
                    task.CancelCompleted += new Task.CancelCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        RefreshData();
                    });
                    task.Cancel(tmp, UseTag);
                }
                else
                    MessageBox.Show("请选择要取消的仓库作业。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("取消失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMain.SelectedRows.Count > 1)
                {
                    MessageBox.Show("当前操作只允许操作一个任务！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (dgvMain.SelectedRows.Count == 1)
                {
                    IList<BillDetail> billDetails = new List<BillDetail>();
                    BillDetail billDetail = new BillDetail();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        if (row.Cells["StatusName"].Value.ToString() == "已申请")
                        {
                            billDetail.BillNo = row.Cells["BillNo"].Value.ToString();
                            billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                            billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                            billDetail.Operator = Environment.MachineName;

                            billDetail.OperatePieceQuantity = Convert.ToInt32(row.Cells["OperatePieceQuantity"].Value);
                            billDetail.OperateBarQuantity = Convert.ToInt32(row.Cells["OperateBarQuantity"].Value);

                            operateStorageName = row.Cells["Storage"].Value.ToString();
                            targetStorageName = row.Cells["TargetStorage"].Value.ToString();
                            operateName = row.Cells["BillTypeName"].Value.ToString();
                            operateProductName = row.Cells["ProductName"].Value.ToString();
                            operatePieceQuantity = Convert.ToInt32(row.Cells["OperatePieceQuantity"].Value);
                            operateBarQuantity = Convert.ToInt32(row.Cells["OperateBarQuantity"].Value);
                        }
                    }

                    ConfirmDialog confirmForm = new ConfirmDialog(BillTypes, operateStorageName, targetStorageName, operateName, operateProductName);
                    confirmForm.Piece = operatePieceQuantity;
                    confirmForm.Item = operateBarQuantity;

                    if (confirmForm.ShowDialog() == DialogResult.OK)
                    {
                        DisplayPlWailt();

                        if (BillTypes == "4")
                        {
                            billDetail.OperatePieceQuantity = confirmForm.Piece;
                            billDetail.OperateBarQuantity = confirmForm.Item;
                        }

                        //todo RFID 确认，及RFID 记录；

                        billDetails.Add(billDetail);
                        BillDetail[] tmp = new BillDetail[billDetails.Count];
                        billDetails.CopyTo(tmp, 0);

                        Task task = new Task(Application.OpenForms[0], url);
                        task.ExecuteCompleted += new Task.ExecuteCompletedEventHandler(delegate(bool isSuccess, string msg)
                        {
                            RefreshData();
                        });
                        task.Execute(tmp, UseTag);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //批量确认
        private void btnBatConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("当前操作将批量确认选择的已申请的所有任务！", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }
            try
            {
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    IList<BillDetail> billDetails = new List<BillDetail>();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = row.Cells["BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.Operator = Environment.MachineName;

                        billDetail.OperatePieceQuantity = Convert.ToInt32(row.Cells["OperatePieceQuantity"].Value);
                        billDetail.OperateBarQuantity = Convert.ToInt32(row.Cells["OperateBarQuantity"].Value);

                        billDetails.Add(billDetail);
                    }
                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(Application.OpenForms[0], url);
                    task.ExecuteCompleted += new Task.ExecuteCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        RefreshData();
                    });
                    task.Execute(tmp, UseTag);
                }
                else
                    MessageBox.Show("请选择要执行的仓库作业。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DisplayPlWailt()
        {
            this.plWailt.Visible = true;
            this.plWailt.Left = (this.dgvMain.Width - this.plWailt.Width) / 2;
            this.plWailt.Top = (this.dgvMain.Height - this.plWailt.Height) / 2;
            this.btnSearch.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnApply.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnConfirm.Enabled = false;
            this.btnBatConfirm.Enabled = false;
        }

        public void ClosePlWailt()
        {
            this.plWailt.Visible = false;
            this.btnSearch.Enabled = true;
            this.btnExit.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnOpType_Click(object sender, EventArgs e)
        {
            if (btnOpType.Text != "正常")
            {
                btnOpType.Text = "正常";
                OperateType = "NoReal";
            }
            else
            {
                btnOpType.Text = "实时";
                OperateType = "Real";

                //todo 连接实时作业信息服务器；
            }
        }
    }
}

