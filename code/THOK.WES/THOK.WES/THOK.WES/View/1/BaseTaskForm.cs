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
using SignalR.Client;

namespace THOK.WES.View
{
    public partial class BaseTaskForm : THOK.AF.View.ToolbarForm
    {
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();
        private DataTable cellTable = null;
        ShelfInfo ShelfInfo = null;
        public delegate string TimerStateInMainThread();
        private ConfigUtil configUtil = new ConfigUtil();
        private ReadRfid rRfid = new ReadRfid();
        private string operateStorageName = "";
        private string targetStorageName = "";
        private string operateName = "";
        private string operateProductName = "";
        private int operatePieceQuantity = 0;
        private int operateBarQuantity = 0;
        private string url = @"http://59.61.87.212:8090/Task";
        private System.Media.SoundPlayer sp;

        private bool needDraw = false;
        private bool filtered = false;

        private int columns = 38;
        private int rows = 3;
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[8];
        private int left = 5;

        private string musicName = "";//音乐地址
        private string isMusicName = "";//是否使用音乐提醒

        /// <summary>
        /// 1：入库单；2：出库单；3：移库单；4：盘点单
        /// </summary>
        protected string BillTypes = "";
        private int isAppyInt = 0;

        //选择的主单；
        string billNo = string.Empty;
        BillMaster[] BillMasters = null;

        string storageName="";

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
        /// 使用Rfid  = 0：不使用；1：手动使用；2：自动使用；
        /// </summary>
        private string UseRfid = "";

        /// <summary>
        /// 读取的托盘RFID号；
        /// </summary>
        private string RfidCode = "";

        /// <summary>
        /// 错误消息；
        /// </summary>
        private string errInfo;

        /// <summary>
        /// 串口；
        /// </summary>
        private string port;

        private Connection connection = null;
        private GridUtil gridUtil = null;
        public BaseTaskForm()
        {
            InitializeComponent();
            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;
            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);

            btnChart.Enabled = false;

            gridUtil = new GridUtil(dgvMain);
            url = configUtil.GetConfig("URL")["URL"];
            OperateAreas = configUtil.GetConfig("Layers")["Number"];
            UseRfid = configUtil.GetConfig("RFID")["USEDRFID"];
            musicName = configUtil.GetConfig("MusicName")["Music"];
            isMusicName = configUtil.GetConfig("MusicName")["IsMusic"];
            connection = new Connection(url + @"/automotiveSystems");
            connection.Received += new Action<string>(connection_Received);
            connection.Closed += new Action(connection_Closed);            
            sp = new System.Media.SoundPlayer(musicName);            
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
                //this.btnBatConfirm.Visible = false;
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
            port = configUtil.GetConfig("RFID")["PORT"];            
        }

        //查询
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Stop();
                Task task = new Task(url);
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

                        List<BillMaster> listBill = new List<Interface.Model.BillMaster>();
                        int f = 0;
                        for (int i = 0; i < billMasters.Length; i++)
                        {
                            if (billNo.Contains(billMasters[i].BillNo))
                            {
                                f++;
                                listBill.Add(billMasters[i]);
                            }
                        }
                        BillMasters = new BillMaster[f];
                        listBill.CopyTo(BillMasters, 0);
                    }
                    if (!isSuccess)
                        MessageBox.Show(msg);
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
            if (BillMasters == null)
            {
                dgvMain.DataSource = null;
                return;
            }
            sslBillID.Text = "单据号：" + billNo + "                              ";
            sslOperator.Text = "操作员：" + Environment.MachineName;

            Task task = new Task(url);
            task.GetBillDetailCompleted += new Task.GetBillDetailCompletedEventHandler(delegate(bool isSuccess, string msg, BillDetail[] billDetails)
            {
                InTask = false;
               
                if (billDetails != null && billDetails.Length != 0)
                {
                    dgvMain.AutoGenerateColumns = false;
                    dgvMain.DataSource = billDetails;
                    
                    foreach (BillDetail billDetail in billDetails)
                    {
                        storageName = billDetail.StorageName;
                        targetStorageName = billDetail.TargetStorageName;
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
                    isAppyInt = 0;
                }
                ClosePlWailt();
                dgvMain.ClearSelection();
                task.Getshelf();
                task.GetShelf += new Task.GetShelfEventHandler(delegate(bool isSuccesss, string msgs, ShelfInfo[] shelfInfo)
                {
                    if (shelfInfo != null)
                    {
                        cellTable = new DataTable();
                        cellTable.Columns.Add("ShelfCode");
                        cellTable.Columns.Add("ShelfName");
                        cellTable.Columns.Add("CellCode");
                        cellTable.Columns.Add("CellName");
                        cellTable.Columns.Add("ProductCode");
                        cellTable.Columns.Add("ProductName");
                        cellTable.Columns.Add("QuantityTiao", typeof(decimal));
                        cellTable.Columns.Add("QuantityJian", typeof(decimal));
                        cellTable.Columns.Add("WareCode");
                        cellTable.Columns.Add("WareName");
                        cellTable.Columns.Add("IsActive");
                        cellTable.Columns.Add("RowNum");
                        cellTable.Columns.Add("ColNum");
                        cellTable.Columns.Add("Shelf");
                        foreach (ShelfInfo shelf in shelfInfo)
                        {
                            this.ShelfInfo = shelf;
                            DataRow dr = cellTable.NewRow();
                            dr["ShelfCode"] = shelf.ShelfCode;
                            dr["ShelfName"] = shelf.ShelfName;
                            dr["CellCode"] = shelf.CellCode;
                            dr["CellName"] = shelf.CellName;
                            dr["ProductCode"] = shelf.ProductCode;
                            dr["ProductName"] = shelf.ProductName;
                            dr["QuantityTiao"] = shelf.QuantityTiao;
                            dr["QuantityJian"] = shelf.QuantityJian;
                            dr["WareCode"] = shelf.WareCode;
                            dr["WareName"] = shelf.WareName;
                            dr["IsActive"] = shelf.IsActive;
                            dr["RowNum"] = shelf.RowNum;
                            dr["ColNum"] = shelf.ColNum;
                            dr["Shelf"] = shelf.Shelf;
                            cellTable.Rows.Add(dr);
                        }
                        if (cellTable.Rows.Count > 0)
                        {
                            btnChart.Enabled = true;
                        }
                    }
                });
            });
            task.SearchBillDetail(BillMasters, RfidReadProductCode, OperateType, OperateAreas, Environment.MachineName);           
            DisplayPlWailt();           
        }

        //申请
        private void btnApply_Click(object sender, EventArgs e)
        {
            sp.Stop();
            try
            {
                if (UseRfid == "1")
                {
                    RfidCode = getRfidCode();
                    if (RfidCode == "")
                    {
                        THOKUtil.ShowError("读取RFID数据失败！请重新申请！");
                        return;
                    }
                }

                ApplyPublicMethod();
            }
            catch (Exception ex)
            {
                THOKUtil.ShowError("申请错误：" + ex.Message);
            }
            finally
            {
                RefreshData();
            }
        }

        private string getRfidCode()
        {
            List<string> listRfid = new List<string>();
            DisplayPlWailt();
            listRfid = rRfid.ReadTrayRfid(port, 115200, out errInfo);
            Application.DoEvents();
            return listRfid[0].ToString();
        }

        //取消申请
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Stop();
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    rRfid.CloseCom();
                    IList<BillDetail> billDetails = new List<BillDetail>();
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.Operator = Environment.MachineName;
                        billDetails.Add(billDetail);
                    }
                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(url);
                    task.CancelCompleted += new Task.CancelCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        if (!isSuccess)
                            MessageBox.Show(msg);
                        RefreshData();
                    });
                    task.Cancel(tmp, UseTag);
                    RfidCode = "";
                }
                else
                    THOKUtil.ShowError("请选择要取消的仓库作业。");
            }
            catch (Exception ex)
            {
                THOKUtil.ShowError("取消失败，原因：" + ex.Message);
            }
        }

        //确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            sp.Stop();
            ConfirmPubliceMethod();
            RefreshData();
        }

        //批量确认
        private void btnBatConfirm_Click(object sender, EventArgs e)
        {
            sp.Stop();
            if (!UseRfid.Equals("0"))
            {
                THOKUtil.ShowInfo("使用RFID无法批量完成！");
                return;
            }
            if (THOKUtil.ShowQuery("当前操作将批量确认选择的已申请的所有任务！") != DialogResult.Yes)
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
                        billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.Operator = Environment.MachineName;

                        billDetail.OperatePieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                        billDetail.OperateBarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);

                        billDetails.Add(billDetail);
                    }
                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(url);
                    task.ExecuteCompleted += new Task.ExecuteCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        if (!isSuccess)
                            MessageBox.Show(msg);
                        RefreshData();
                    });
                    task.Execute(tmp, UseTag);
                    //浪潮接口
                    if (BillTypes == "1")
                    {
                        task.GetWarehouseInBillProgressFeedback(tmp);
                        task.GetWarehouseInBillFinish(tmp);
                    }
                    if (BillTypes == "2")
                    {
                        task.GetWarehouseOutBillProgressFeedback(tmp);
                        task.GetWarehouseOutBillFinish(tmp);
                    }
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
            sp.Stop();
            Exit();
        }

        private void btnOpType_Click(object sender, EventArgs e)
        {
            if (btnOpType.Text != "正常")
            {
                btnOpType.Text = "正常";
                OperateType = "NoReal";
                connection.Stop();
            }
            else
            {
                btnOpType.Text = "实时";
                OperateType = "Real";

                //todo 连接实时作业信息服务器；
                connection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("Failed to start: {0}", task.Exception.GetBaseException());
                    }
                });
            }
            RefreshData();
        }

        delegate void RefreshTask();

        void connection_Received(string data)
        {
            if (data == "TaskStart")
            {
                try
                {
                    sp.Play();
                }
                catch (Exception e)
                {
                    MessageBox.Show("播放音乐出错，原因：" + e.Message);
                }
                Application.OpenForms[0].Invoke(new RefreshTask(RefreshData));
            }
        }

        void connection_Closed()
        {
            if (OperateType == "Real")
            {
                connection.Start();
            }
        }

        private bool isBcCompose = false;
        private void btnBcCompose_Click(object sender, EventArgs e)
        {
            btnBcCompose.Enabled = false;
            if (!isBcCompose && BillTypes == "3" && BillMasters != null)
            {
                Task task = new Task(url.Replace("Task", "StockMoveBill/GeneratePalletTag"));
                task.BcComposeCompleted += new Task.BcComposeEventHandler(delegate(bool isSuccess, string msg)
                {
                    dgvMain.Columns["PalletTag"].Visible = true;
                    if (!isSuccess)
                        MessageBox.Show(msg);
                    RefreshData();
                    btnBcCompose.Enabled = true;
                    isBcCompose = true;
                });
                task.BcCompose(billNo);
            }
            else
            {
                dgvMain.Columns["PalletTag"].Visible = false;
                RefreshData();
                btnBcCompose.Enabled = true;
                isBcCompose = false;
            }
        }

        public void ApplyPublicMethod()
        {
            try
            {
                if (dgvMain.SelectedRows.Count > 1 && UseRfid.Equals("1"))
                {
                    THOKUtil.ShowInfo("当前操作只允许操作一个任务，已申请任务了，不能重新申请！");
                    return;
                }
                foreach (DataGridViewRow row in dgvMain.Rows)
                {
                    if (row.Cells["Status"].Value.ToString().Equals("1") && UseRfid.Equals("1"))
                    {
                        THOKUtil.ShowInfo("使用RFID,只能申请一条数据。");
                        return;
                    }
                }
                if (dgvMain.SelectedRows.Count != 0)
                {
                    DisplayPlWailt();
                    IList<BillDetail> billDetails = new List<BillDetail>();

                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                        billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                        billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                        billDetail.PieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                        billDetail.BarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);
                        billDetail.Operator = Environment.MachineName;
                        billDetails.Add(billDetail);
                    }

                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(url);
                    task.ApplyCompleted += new Task.ApplyCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        if (!isSuccess)
                            THOKUtil.ShowError(msg);
                        RefreshData();
                    });
                    task.Apply(tmp, UseTag);
                }
                else
                    THOKUtil.ShowError("请选择要执行的仓库作业。");
            }
            catch (Exception ex)
            {
                THOKUtil.ShowError("申请失败，原因：" + ex.Message);
                RefreshData();
            }
        }

        private void ConfirmPubliceMethod()
        {
            string errString = string.Empty;
            try
            {
                List<string> listRfid = new List<string>();
                if (UseRfid == "1")
                {
                    DisplayPlWailt();
                    //listRfid = rRfid.ReadTrayRfid(port, 115200, out errString);
                    Application.DoEvents();
                }
                if (dgvMain.SelectedRows.Count > 1)
                {
                    THOKUtil.ShowInfo("当前操作只允许操作一个任务！");
                    return;
                }

                if (dgvMain.SelectedRows.Count == 0)
                {
                    THOKUtil.ShowInfo("当前操作失败！原因：没有选择数据，请选择！");
                    return;
                }

                IList<BillDetail> billDetails = new List<BillDetail>();
                BillDetail billDetail = new BillDetail();

                if (dgvMain.SelectedRows.Count == 1)
                {
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        if (BillTypes == "3" && RfidCode != "" && listRfid.Count != 0)
                        {
                            if (!row.Cells["StorageRfid"].Value.ToString().Equals(RfidCode) || !listRfid.Contains(row.Cells["TargetStorageRfid"].Value.ToString()))//移出的库存(托盘)的rfid
                            {
                                THOKUtil.ShowError("读取RFID数据失败！请重新读取！");
                                return;
                            }
                        }
                        ConfirmMethod(row, billDetail, billDetails, RfidCode);
                    }
                }

                RfidCode = "";
            }
            catch (Exception ex)
            {
                THOKUtil.ShowError("执行失败，原因：" + ex.Message + "," + errString);
            }
        }

        private void ConfirmMethod(DataGridViewRow row, BillDetail billDetail, IList<BillDetail> billDetails, string rfidID)
        {
            if (row.Cells["StatusName"].Value.ToString() == "已申请")
            {
                billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                billDetail.Operator = Environment.MachineName;
                billDetail.StorageRfid = rfidID;//托盘rfid
                billDetail.OperatePieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                billDetail.OperateBarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);

                operateStorageName = row.Cells["Storage"].Value.ToString();
                targetStorageName = row.Cells["TargetStorage"].Value.ToString();
                operateName = row.Cells["BillTypeName"].Value.ToString();
                operateProductName = row.Cells["ProductName"].Value.ToString();
                operatePieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                operateBarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);
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

                Task task = new Task(url);
                task.ExecuteCompleted += new Task.ExecuteCompletedEventHandler(delegate(bool isSuccess, string msg)
                {
                    if (!isSuccess)
                        MessageBox.Show(msg);
                    RefreshData();
                });
                task.Execute(tmp, UseTag);
                //浪潮接口
                if (BillTypes == "1")
                {
                    task.GetWarehouseInBillProgressFeedback(tmp);
                    task.GetWarehouseInBillFinish(tmp);
                }
                if (BillTypes == "2")
                {
                    task.GetWarehouseOutBillProgressFeedback(tmp);
                    task.GetWarehouseOutBillFinish(tmp);
                }
            }
        }

        public void ReadRfidCycle()
        {
            try
            {
                if (UseRfid.Equals("2"))
                {
                    if (RfidCode.Equals(""))
                        btnApply_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CyleTimer_Tick(object sender, EventArgs e)
        {
            if (BillMasters != null)
            {
                this.ReadRfidCycle();
            }
        }

        private void Play()
        {
            bool isApply = true;
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.Cells["Status"].Value.ToString().Equals("1"))
                {
                    isApply = false;
                    return;
                }
            }
            if (this.dgvMain.Rows.Count == 0)
            {
                isAppyInt = 0;
            }
            if (this.dgvMain.Rows.Count > 0 && BillTypes.Equals("3") && isApply && OperateType.Equals("Real") && isAppyInt == 0 && isMusicName.Equals("1"))
            {
                try
                {
                    sp.PlayLooping();
                    isAppyInt++;
                }
                catch (Exception e)
                {
                    MessageBox.Show("播放音乐出错，原因：" + e.Message);
                }
            }
        }
        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos != currentPage)
            {
                currentPage = pos;
                pnlChart.Invalidate();
            }
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            if (needDraw)
            {
                Font font = new Font("宋体", 9);
                SizeF size = e.Graphics.MeasureString("第1排", font);
                float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                size = e.Graphics.MeasureString("13", font);
                float adjustWidth = (cellWidth - size.Width) / 2;
                for (int i = 0; i <= 7; i++)
                {
                    string keys = "";
                    int key = currentPage * 8 - (top.Length - (i + 1));
                    if (key < 10)
                    {
                        keys = "0" + key.ToString();
                    }
                    else
                    {
                        keys = key.ToString();
                    }
                    if (!shelf.ContainsKey(key))
                    {
                        DataRow[] rows = cellTable.Select(string.Format("Shelf= {0}", keys), "CellCode");
                        shelf.Add(key, rows);
                    }

                    DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth, e);
                    int tmpLeft = left + columns * cellWidth + 5 + cellWidth;
                    for (int j = 0; j < rows; j++)
                    {
                        string s = string.Format("第{0}排第{1}层", shelf[key][i]["ShelfName"], Convert.ToString(j + 1).PadLeft(2, '0'));
                        e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i] - 12 + (j + 1) * cellHeight + adjustHeight);//画右边的字体
                    }
                }
                if (filtered)
                {
                    int i = currentPage * top.Length;
                    for (int j = 0; j < cellTable.Rows.Count; j++)
                    {
                        int shelf = Convert.ToInt32(cellTable.Rows[j]["Shelf"]);
                        int column = Convert.ToInt32(cellTable.Rows[j]["ColNum"]) - 1;
                        int row = Convert.ToInt32(cellTable.Rows[j]["RowNum"]);
                        int quantity = Convert.ToInt32(cellTable.Rows[j]["QuantityJian"]);
                        string storagename = cellTable.Rows[j]["CellName"].ToString();
                        string storagenamein = "";
                        string storagenameout = "";
                        string billType = "";
                        foreach (DataGridViewRow gridRow in dgvMain.Rows)
                        {
                            billType = BillTypes;
                            if (storagename == ((BillDetail)(gridRow.DataBoundItem)).StorageName && (billType == "1" || billType == "4"))
                            {
                                storagenamein = ((BillDetail)(gridRow.DataBoundItem)).StorageName;
                            }
                            if (storagename == ((BillDetail)(gridRow.DataBoundItem)).StorageName && billType == "2")
                            {
                                storagenameout = ((BillDetail)(gridRow.DataBoundItem)).StorageName;
                            }
                            if (storagename == ((BillDetail)(gridRow.DataBoundItem)).TargetStorageName && billType == "3")
                            {
                                storagenamein = ((BillDetail)(gridRow.DataBoundItem)).TargetStorageName;
                            }
                            if (storagename == ((BillDetail)(gridRow.DataBoundItem)).StorageName && billType == "3")
                            {
                                storagenameout = ((BillDetail)(gridRow.DataBoundItem)).StorageName;
                            }
                        }
                        int topa = 0;
                        if (shelf <= i)
                        {
                            if (currentPage == 1)
                            {
                                topa = top[shelf - 1];
                                if (storagenamein == storagename || storagename == storagenameout)
                                {
                                    FillCell(e.Graphics, topa, row, column, quantity, storagename, storagenamein, storagenameout, billType, e);
                                }
                                else
                                {
                                    FillCell(e.Graphics, topa, row, column, quantity, storagename, e);
                                }
                            }
                            else if (currentPage == 2)
                            {
                                if (shelf >= 9)
                                {
                                    topa = top[shelf - 9];
                                    if (storagenamein == storagename || storagename == storagenameout)
                                    {
                                        FillCell(e.Graphics, topa, row, column, quantity, storagename, storagenamein, storagenameout, billType, e);
                                    }
                                    else
                                    {
                                        FillCell(e.Graphics, topa, row, column, quantity, storagename, e);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth, PaintEventArgs e)
        {
            int z = 0;
            for (int j = 0; j < columns; j++)
            {
                if (j + 1 == 19)
                {
                    z = cellWidth;//空出过道或者走廊
                }
                g.DrawString(Convert.ToString(j + 1), font, Brushes.DarkCyan, left + j * cellWidth + adjustWidth + z, top);//画上面的数字
            }
            foreach (DataRow cellRow in cellRows)
            {
                int column = Convert.ToInt32(cellRow["ColNum"]) - 1;
                int row = Convert.ToInt32(cellRow["RowNum"]);
                int quantity = Convert.ToInt32(cellRow["QuantityJian"]);
                string storagename =cellRow["CellName"].ToString();
                string storagenamein =storageName;
                string storagenameout = targetStorageName;
                string billType = BillTypes;
                int x = left + column * cellWidth;
                int y = top + row * cellHeight - 7;
                if (column >= 18)
                    x = x + cellWidth;//空出过道或者走廊
                g.DrawRectangle(Pens.Blue, new Rectangle(x, y, cellWidth, cellHeight));//画货位边框,y 这个可调整边框

                if (!filtered)
                    FillCell(g, top, row, column, quantity,storagename, storagenamein,storagenameout,billType, e);
            }
        }

        private void FillCell(Graphics g, int top, int row, int column, int quantity,string storagename, string storageNameIn,string storageNameOut,string billType, PaintEventArgs e)
        {
            int x = left + column * cellWidth;
            int y = top + row * cellHeight - 5;
            if (column >= 18 )
                x = x + cellWidth;
            if (quantity > 0 && storageNameOut == storagename && billType != "4")
                g.FillRectangle(Brushes.RoyalBlue, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//画出库货位蓝色
            else if (storageNameIn == storagename &&  billType != "4")
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//画入库货位绿色
            else if (billType == "4" && storageNameIn == storagename)
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//画盘点单货位信息，红色
            else
                g.FillRectangle(Brushes.White, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//画与单据无关的货位，白色
        }
        private void FillCell(Graphics g, int top, int row, int column, int quantity, string storAgeName, PaintEventArgs e)
        {
            int x = left + column * cellWidth;
            int y = top + row * cellHeight - 5;

            if (column >= 18)
                x = x + cellWidth;
                g.FillRectangle(Brushes.White, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//画空货位

        }
        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && currentPage + 1 <= 3)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;
        }
        private void pnlChart_Resize(object sender, EventArgs e)
        {
            cellWidth = (pnlContent.Width - 90 - sbShelf.Width - 20) / columns;
            cellHeight = ((pnlContent.Height / 8) / rows) - 7;

            top[0] = 0;
            for (int i = 1; i < top.Length; i++)
            {
                top[i] = pnlContent.Height / top.Length * i;
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
             if (cellTable != null && cellTable.Rows.Count != 0)
             {
                 if (pnlData.Visible)
                 {
                     filtered = true;
                     needDraw = true;
                     pnlData.Visible = false;
                     btnSearch.Enabled = false;
                     pnlChart.Visible = true;
                     btnChart.Text = "列表";
                     btnApply.Visible = false;
                     btnCancel.Visible = false;
                     btnConfirm.Visible = false;
                     btnBatConfirm.Visible = false;
                     btnOpType.Visible = false;
                     btnBcCompose.Visible = false;
                     button1.Visible = true;
                     button2.Visible = true;
                     button3.Visible = true;
                     button4.Visible = true;
                 }

                 else
                 {
                     needDraw = false;
                     pnlData.Visible = true;
                     btnSearch.Enabled = true;
                     pnlChart.Visible = false;
                     btnChart.Text = "图示";
                     btnApply.Visible = true;
                     btnCancel.Visible = true;
                     btnConfirm.Visible = true;
                     btnBatConfirm.Visible = true;
                     btnOpType.Visible = true;
                     btnBcCompose.Visible = true;
                     button1.Visible = false;
                     button2.Visible = false;
                     button3.Visible = false;
                     button4.Visible = false;

                 }
             }
        }
    }
}

