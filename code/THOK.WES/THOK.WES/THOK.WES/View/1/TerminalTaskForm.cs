using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;
using THOK.WES.Dao;
using THOK.Util;
using System.Threading;
using THOK.WES.Interface;

namespace THOK.WES.View
{
    public partial class TerminalTaskForm : THOK.AF.View.ToolbarForm
    {
        protected string BillType = "0";
        private BillDal billDal = new BillDal();
        private TaskDal taskDal = new TaskDal();

        public TerminalTaskForm()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable table = billDal.GetRetailMaster();
                dgvMaster.DataSource = table;
                if (table.Rows.Count == 0)
                {
                    dgvDetail.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，原因：" + ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = billDal.GetBillDetail(dgvMaster.Rows[e.RowIndex].Cells["BILLMASTERID"].Value.ToString());
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            foreach (DataGridViewRow row in dgvMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedRow = row;
                    break;
                }
            }

            if (selectedRow != null)
            {
                if (selectedRow.Cells["STATENAME"].Value.ToString() == "未执行")
                {
                    billDal.SaveMasterState("5", selectedRow.Cells["BILLMASTERID"].Value.ToString());
                    taskDal.InsertRetailTask(selectedRow.Cells["BILLMASTERID"].Value.ToString());
                    if (new ConfigUtil().GetConfig("DeviceType")["Device"] != "0")
                    {
                        new SendUDP().Send();
                    }
                   
                    dgvMaster.DataSource = billDal.GetRetailMaster();
                }
                else
                    MessageBox.Show("单据状态不是‘未执行’，请重新选择单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("请选择需要进行仓库作业的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            foreach (DataGridViewRow row in dgvMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedRow = row;
                    break;
                }
            }

            if (selectedRow != null)
            {
                if (selectedRow.Cells["STATENAME"].Value.ToString() == "执行中")
                {
                    string billID = selectedRow.Cells["BILLMASTERID"].Value.ToString();
                    DataTable table = billDal.FindBillDetail(billID);
                    if (table.Select("ConfirmState = 3").Length == 0 && table.Select("ConfirmState = 2").Length > 0)
                    {
                        billDal.SaveMasterState("4", billID);
                        taskDal.GetTaskTypeDeleteTask(billID);
                        taskDal.CancelRetailTask(billID);
                        if (new ConfigUtil().GetConfig("DeviceType")["Device"] != "0")
                        {
                            new SendUDP().Send();
                        }
                        dgvMaster.DataSource = billDal.GetRetailMaster();                        
                    }
                    else
                        MessageBox.Show("不能进行取消操作，已有子项开始仓库作业。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("单据状态不为‘执行中’，请重新选择单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请选择需要取消仓库作业的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            foreach (DataGridViewRow row in dgvMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedRow = row;
                    break;
                }
            }
            if (selectedRow != null)
            {
                if (selectedRow.Cells["STATENAME"].Value.ToString() == "执行中")
                {
                    if (DialogResult.OK == MessageBox.Show("此操作将零烟柜未作业的单据确认", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        ConfirmResult result = null;
                        string masterid = dgvMaster.SelectedRows[0].Cells["BILLMASTERID"].Value.ToString();
                        DataTable BillDetailTable = billDal.GetBillDetailMasterId(masterid);

                        for (int i = 0; i < BillDetailTable.Rows.Count; i++)
                        {
                            try
                            {
                                IData dataInterface = WesContext.GetData();
                                result = dataInterface.UploadData(new BillDao().GetBill(BillDetailTable.Rows[i]["BILLMASTERID"].ToString()).Rows[0]["BILLCODE"].ToString(), BillDetailTable.Rows[i]["BILLID"].ToString(), BillDetailTable.Rows[i]["DETAILID"].ToString(), BillDetailTable.Rows[i]["OPERATECODE"].ToString(), Convert.ToInt32(BillDetailTable.Rows[i]["PIECE"].ToString()), 0, "");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("零烟柜整单确认失败! 原因：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (result.IsSuccess)
                        {
                            taskDal.GetTaskTypeDeleteTask(masterid);
                            taskDal.CancelRetailTask(masterid);
                            if (new ConfigUtil().GetConfig("DeviceType")["Device"] != "0")
                            {
                                new SendUDP().Send();
                            }
                            billDal.GetUpdateBillDetailFewColum(result.State, result.Desc, Environment.MachineName, masterid);
                            if (DialogResult.OK == MessageBox.Show("零烟柜整单确认已经完成！"))
                            {
                                DataTable table = billDal.GetRetailMaster();
                                dgvMaster.DataSource = table;
                                if (table.Rows.Count == 0)
                                    dgvDetail.DataSource = null;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("单据状态不为‘未执行’，请重新选择单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请选择需要进行整单完成的单据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

