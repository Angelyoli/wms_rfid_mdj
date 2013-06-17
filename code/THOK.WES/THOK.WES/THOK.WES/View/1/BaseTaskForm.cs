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

        private string musicName = "";//���ֵ�ַ
        private string isMusicName = "";//�Ƿ�ʹ����������

        /// <summary>
        /// 1����ⵥ��2�����ⵥ��3���ƿⵥ��4���̵㵥
        /// </summary>
        protected string BillTypes = "";
        private int isAppyInt = 0;

        //ѡ���������
        string billNo = string.Empty;
        BillMaster[] BillMasters = null;

        string storageName="";

        private string RfidReadProductCode = "";

        /// <summary>
        /// Real: ʵʱ���⣻NoReal: ��ʵʱ���⣻
        /// </summary>
        private string OperateType = "";

        /// <summary>
        /// �������� = 0�����̹�1��N �����ܲ�ţ�
        /// </summary>
        private string OperateAreas = "";

        /// <summary>
        /// ʹ�õ��ӱ�ǩ = 0����ʹ�ã�1��ʹ�ã�
        /// </summary>
        private string UseTag = "";

        /// <summary>
        /// ʹ��Rfid  = 0����ʹ�ã�1���ֶ�ʹ�ã�2���Զ�ʹ�ã�
        /// </summary>
        private string UseRfid = "";

        /// <summary>
        /// ��ȡ������RFID�ţ�
        /// </summary>
        private string RfidCode = "";

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        private string errInfo;

        /// <summary>
        /// ���ڣ�
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
                this.dgvMain.DefaultCellStyle.Font = new Font("����", 16);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("����", 13);
                this.btnBatConfirm.Visible = false;
                UseTag = "0";
            }
            else if (configUtil.GetConfig("DeviceType")["Device"] == "1")
            {
                this.dgvMain.ColumnHeadersHeight = 40;
                this.dgvMain.RowTemplate.Height = 40;
                this.dgvMain.DefaultCellStyle.Font = new Font("����", 16);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("����", 13);
                //this.btnBatConfirm.Visible = false;
                UseTag = "1";
            }
            else
            {
                this.dgvMain.ColumnHeadersHeight = 22;
                this.dgvMain.RowTemplate.Height = 22;
                this.dgvMain.DefaultCellStyle.Font = new Font("����", 10);
                this.dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("����", 10);
                UseTag = "1";
            }
            port = configUtil.GetConfig("RFID")["PORT"];            
        }

        //��ѯ
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
                MessageBox.Show("��ȡ����ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //ˢ������
        private void RefreshData()
        {
            if (BillMasters == null)
            {
                dgvMain.DataSource = null;
                return;
            }
            sslBillID.Text = "���ݺţ�" + billNo + "                              ";
            sslOperator.Text = "����Ա��" + Environment.MachineName;

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
                    }
                });
            });
            task.SearchBillDetail(BillMasters, RfidReadProductCode, OperateType, OperateAreas, Environment.MachineName);           
            DisplayPlWailt();           
        }

        //����
        private void btnApply_Click(object sender, EventArgs e)
        {
            sp.Stop();
            string errString = string.Empty;
            List<string> listRfid = new List<string>();
            string productRfid = "";
            decimal quantityRfid = 0;
            try
            {
                if (UseRfid == "0")
                {
                    ApplyPublicMethod();
                }
                else
                {
                    if (BillTypes == "1")
                    {
                        DateTime now = DateTime.Now;
                        while (listRfid.Count == 0 || listRfid == null)
                        {
                            DisplayPlWailt();
                            listRfid = rRfid.ReadTrayRfid(port, 115200, out errString);
                            Application.DoEvents();
                            DateTime newTiem =DateTime.Now;
                            if (((TimeSpan)(DateTime.Now - now)).TotalSeconds < 10000 && (listRfid.Count==0 ||listRfid==null))
                            {
                                MessageBox.Show("����ʧ�ܣ���ȡ����RFID���ݣ�" );
                                RefreshData();
                                return;
                            }
                        }
                        RfidCode = RfidCode = listRfid[0].ToString();
                        Task task = new Task(url);
                        task.SearchRfidInfo(RfidCode);
                        task.GetRfidInfoCompleted += new Task.GetRfidInfoCompletedEventHandler(delegate(bool isSuccess, string msg, BillDetail[] billDetails)
                        {
                            if (billDetails != null && billDetails.Length != 0)
                            {
                                productRfid = billDetails[0].ProductCode;
                                quantityRfid = billDetails[0].PieceQuantity;
                            }
                            ApplyPublicMethod(UseRfid, RfidCode, productRfid, quantityRfid);
                        });
                    }
                    else
                    {
                        ApplyPublicMethod();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������" + ex.Message + " ,����:" + errString);
                RefreshData();
            }
        }

        //ȡ������
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
                    MessageBox.Show("��ѡ��Ҫȡ���Ĳֿ���ҵ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ȡ��ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //ȷ��
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            sp.Stop();
            ConfirmPubliceMethod();
            RefreshData();
        }

        //����ȷ��
        private void btnBatConfirm_Click(object sender, EventArgs e)
        {
            sp.Stop();
            if (!UseRfid.Equals("0"))
            {
                MessageBox.Show("ʹ��RFID�޷�������ɣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("��ǰ����������ȷ��ѡ������������������", "��ʾ",
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
                }
                else
                    MessageBox.Show("��ѡ��Ҫִ�еĲֿ���ҵ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ִ��ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (btnOpType.Text != "����")
            {
                btnOpType.Text = "����";
                OperateType = "NoReal";
                connection.Stop();
            }
            else
            {
                btnOpType.Text = "ʵʱ";
                OperateType = "Real";

                //todo ����ʵʱ��ҵ��Ϣ��������
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
                    MessageBox.Show("�������ֳ���ԭ��" + e.Message);
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

        public void ApplyPublicMethod(string uRfid, string rfidId, string rfidProductCode, decimal rfidQuantity)
        {
            try
            {
                errInfo = "";
                bool isRfid = true;
                RfidCode = rfidId;
                decimal rfidQty = Convert.ToInt32(rfidQuantity);
                if (dgvMain.SelectedRows.Count != 0)
                {
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        if (row.Cells["Status"].Value.ToString().Equals("1") && !uRfid.Equals("0"))
                        {
                            MessageBox.Show("ʹ��RFID,ֻ������һ�����ݡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    DisplayPlWailt();
                    IList<BillDetail> billDetails = new List<BillDetail>();

                    switch (uRfid)
                    {
                        case "0":
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
                            isRfid = false;
                            break;
                        case "1":
                            foreach (DataGridViewRow row in dgvMain.SelectedRows)
                            {
                                if (rfidProductCode.Equals(row.Cells["ProductCode"].Value.ToString())
                                && rfidQty == Convert.ToInt32(row.Cells["PieceQuantity"].Value)
                                && row.Cells["Status"].Value.ToString().Equals("0"))
                                {
                                    BillDetail billDetail = new BillDetail();
                                    billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                                    billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                                    billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                                    billDetail.PieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                                    billDetail.BarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);
                                    billDetail.Operator = Environment.MachineName;
                                    billDetails.Add(billDetail);
                                    isRfid = false;
                                    break;
                                }
                            }
                            break;
                        case "2":
                            foreach (DataGridViewRow row in dgvMain.Rows)
                            {
                                //�жϾ��̺��������ȡ���Ƿ�һ��������״̬�ų��Ѿ�����Ļ�λ��
                                if (rfidProductCode.Equals(row.Cells["ProductCode"].Value.ToString())
                                    && rfidQty == Convert.ToInt32(row.Cells["PieceQuantity"].Value)
                                    && row.Cells["Status"].Value.ToString().Equals("0"))
                                {
                                    BillDetail billDetail = new BillDetail();
                                    billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                                    billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                                    billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                                    billDetail.PieceQuantity = Convert.ToInt32(row.Cells["PieceQuantity"].Value);
                                    billDetail.BarQuantity = Convert.ToInt32(row.Cells["BarQuantity"].Value);
                                    billDetail.Operator = Environment.MachineName;
                                    billDetails.Add(billDetail);
                                    isRfid = false;
                                    break;
                                }
                            }
                            break;
                        default:
                            errInfo = "��鿴ʹ��RFID���ò����Ƿ���ȷ��";
                            break;
                    }

                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(url);
                    task.ApplyCompleted += new Task.ApplyCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        if (!isSuccess)
                            errInfo += "  " + msg;
                        RefreshData();
                    });
                    task.Apply(tmp, UseTag);

                    if (isRfid)
                        MessageBox.Show("����ʧ�ܣ�ԭ�򣺵�ǰ���̾��̺���������ҵ���ݲ�ƥ����� ��������:  " + errInfo, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("��ѡ��Ҫִ�еĲֿ���ҵ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ApplyPublicMethod()
        {
            try
            {
                string storageRfide = "";
                if (dgvMain.SelectedRows.Count > 1 && !UseRfid.Equals("0"))
                {
                    MessageBox.Show("��ǰ����ֻ�������һ������", "��ʾ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (DataGridViewRow row in dgvMain.Rows)
                {
                    if (row.Cells["Status"].Value.ToString().Equals("1") && !UseRfid.Equals("0"))
                    {
                        MessageBox.Show("ʹ��RFID,ֻ������һ�����ݡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (!UseRfid.Equals("0"))
                        {
                            storageRfide = row.Cells["StorageRfid"].Value.ToString();
                        }
                    }

                    BillDetail[] tmp = new BillDetail[billDetails.Count];
                    billDetails.CopyTo(tmp, 0);

                    Task task = new Task(url);
                    task.ApplyCompleted += new Task.ApplyCompletedEventHandler(delegate(bool isSuccess, string msg)
                    {
                        if (!isSuccess)
                            MessageBox.Show(msg);
                        RefreshData();
                    });
                    task.Apply(tmp, UseTag);
                }
                else
                    MessageBox.Show("��ѡ��Ҫִ�еĲֿ���ҵ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //if (!RfidCode.Equals(storageRfide))
                //    MessageBox.Show("��ȡ��rfid��Ϣ�������������Ϣ��һ�£�����������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ�ԭ��" + ex.Message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
        }

        private void ConfirmPubliceMethod()
        {
            string errString = string.Empty;
            try
            {
                bool isRfid = true;
                decimal quantity = 0;
                List<string> listRfid = new List<string>();
                if (UseRfid != "0")
                {
                    foreach (DataGridViewRow row in dgvMain.SelectedRows)
                    {
                        quantity = Convert.ToDecimal(row.Cells["PieceQuantity"].Value);
                    }
                    if (quantity == 30)
                    {
                        while (listRfid.Count == 0 || listRfid == null)
                        {
                            DisplayPlWailt();
                            listRfid = rRfid.ReadTrayRfid(port, 115200, out errString);
                            Application.DoEvents();
                            DateTime now = DateTime.Now;
                            if (((TimeSpan)(DateTime.Now - now)).TotalSeconds < 10000 && (listRfid.Count == 0 || listRfid == null))
                            {
                                MessageBox.Show("����ʧ�ܣ���ȡ����RFID���ݣ�");
                                RefreshData();
                                return;
                            }
                        }
                    }
                }
                if (dgvMain.SelectedRows.Count > 1)
                {
                    MessageBox.Show("��ǰ����ֻ�������һ������", "��ʾ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (listRfid.Count == 0 && !UseRfid.Equals("0") && quantity==30)
                {
                    MessageBox.Show("��ȡRFID��Ϣʧ�ܣ���ȡ�������������룡", "��ʾ",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }

                if (dgvMain.SelectedRows.Count == 0)
                {
                    MessageBox.Show("��ǰ����ʧ�ܣ�ԭ��û��ѡ�����ݣ���ѡ��", "��ʾ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                IList<BillDetail> billDetails = new List<BillDetail>();
                BillDetail billDetail = new BillDetail();
                switch (UseRfid)
                {
                    case "0":
                        if (dgvMain.SelectedRows.Count == 1)
                        {
                            foreach (DataGridViewRow row in dgvMain.SelectedRows)
                            {
                                ConfirmMethod(row, billDetail, billDetails, RfidCode);
                                isRfid = false;
                            }
                        }                        
                        break;
                    case "1":
                        if (dgvMain.SelectedRows.Count == 1)
                        {
                            foreach (DataGridViewRow row in dgvMain.SelectedRows)
                            {
                                string cellRfid = row.Cells["CellRfid"].Value.ToString();
                                if (BillTypes == "3")
                                {
                                    if (!listRfid.Contains(row.Cells["StorageRfid"].Value.ToString())&& quantity==30)//�Ƴ��Ŀ��(����)��rfid
                                    {
                                        MessageBox.Show("��ȡRFID��Ϣ�����ݲ�һ�£��������̾����������Ƿ���ϣ�", "��ʾ",
                                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    cellRfid = row.Cells["TargetStorageRfid"].Value.ToString();//����Ļ�λrfid
                                }
                                if (listRfid.Contains(cellRfid) || listRfid.Count==0)
                                {
                                    ConfirmMethod(row, billDetail, billDetails, RfidCode);
                                    isRfid = false;
                                }
                            }
                        }
                        break;
                    case "2":
                        foreach (DataGridViewRow row in dgvMain.Rows)
                        {
                            string cellRfid = row.Cells["CellRfid"].Value.ToString();
                            if (BillTypes == "3")
                            {
                                if (!listRfid.Contains(row.Cells["StorageRfid"].Value.ToString()) && quantity == 30)
                                {
                                    MessageBox.Show("��ȡRFID��Ϣʧ�ܣ���ȡ�������������룡", "��ʾ",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                cellRfid = row.Cells["TargetStorageRfid"].Value.ToString();
                            }
                            if (listRfid.Contains(cellRfid) || listRfid.Count == 0)
                            {
                                ConfirmMethod(row, billDetail, billDetails, RfidCode);
                                isRfid = false;
                                break;
                            }
                        }
                        break;
                    default:
                        errInfo = "��鿴ʹ��RFID���ò����Ƿ���ȷ��";
                        break;
                }
                if (isRfid)
                    MessageBox.Show("���ȷ��ʧ�ܣ�ԭ���Ҳ������λRFID��ȵ����ݣ���������" + errInfo + " ," + errString);
                else
                    RfidCode = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ִ��ʧ�ܣ�ԭ��" + ex.Message + "," + errString, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfirmMethod(DataGridViewRow row, BillDetail billDetail, IList<BillDetail> billDetails, string rfidID)
        {
            if (row.Cells["StatusName"].Value.ToString() == "������")
            {
                billDetail.BillNo = row.Cells["@BillNo"].Value.ToString();
                billDetail.BillType = row.Cells["@BillType"].Value.ToString();
                billDetail.DetailID = Convert.ToInt32(row.Cells["DetailID"].Value);
                billDetail.Operator = Environment.MachineName;
                billDetail.StorageRfid = rfidID;//����rfid
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

                //todo RFID ȷ�ϣ���RFID ��¼��

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
                    MessageBox.Show("�������ֳ���ԭ��" + e.Message);
                }
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
                    btnChart.Text = "�б�";
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
                    btnChart.Text = "ͼʾ";
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
                Font font = new Font("����", 9);
                SizeF size = e.Graphics.MeasureString("��1��", font);
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
                        string s = string.Format("��{0}�ŵ�{1}��", shelf[key][i]["ShelfName"], Convert.ToString(j + 1).PadLeft(2, '0'));
                        e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i] - 12 + (j + 1) * cellHeight + adjustHeight);//���ұߵ�����
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
                    z = cellWidth;//�ճ�������������
                }
                g.DrawString(Convert.ToString(j + 1), font, Brushes.DarkCyan, left + j * cellWidth + adjustWidth + z, top);//�����������
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
                    x = x + cellWidth;//�ճ�������������
                g.DrawRectangle(Pens.Blue, new Rectangle(x, y, cellWidth, cellHeight));//����λ�߿�,y ����ɵ����߿�

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
                g.FillRectangle(Brushes.RoyalBlue, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//�������λ��ɫ
            else if (storageNameIn == storagename &&  billType != "4")
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//������λ��ɫ
            else if (billType == "4" && storageNameIn == storagename)
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//���̵㵥��λ��Ϣ����ɫ
            else
                g.FillRectangle(Brushes.White, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//���뵥���޹صĻ�λ����ɫ
        }
        private void FillCell(Graphics g, int top, int row, int column, int quantity, string storAgeName, PaintEventArgs e)
        {
            int x = left + column * cellWidth;
            int y = top + row * cellHeight - 5;

            if (column >= 18)
                x = x + cellWidth;
                g.FillRectangle(Brushes.White, new Rectangle(x + 2, y, cellWidth - 3, cellHeight - 4));//���ջ�λ

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
    }
}

