namespace THOK.WES.View
{
    partial class BaseTaskForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.sslBillID = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslOperator = new System.Windows.Forms.ToolStripStatusLabel();
            this.CyleTimer = new System.Windows.Forms.Timer(this.components);
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.Storage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PieceQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BarQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetStorage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellRfid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.@BillType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StorageRfid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetStorageRfid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PalletTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBatConfirm = new System.Windows.Forms.Button();
            this.btnOpType = new System.Windows.Forms.Button();
            this.plWailt = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBcCompose = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.plWailt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnBcCompose);
            this.pnlTool.Controls.Add(this.btnOpType);
            this.pnlTool.Controls.Add(this.btnBatConfirm);
            this.pnlTool.Controls.Add(this.btnConfirm);
            this.pnlTool.Controls.Add(this.btnCancel);
            this.pnlTool.Controls.Add(this.btnApply);
            this.pnlTool.Controls.Add(this.btnSearch);
            this.pnlTool.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTool.Size = new System.Drawing.Size(804, 46);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.plWailt);
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Controls.Add(this.ssMain);
            this.pnlContent.Location = new System.Drawing.Point(0, 46);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContent.Size = new System.Drawing.Size(804, 162);
            // 
            // pnlMain
            // 
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMain.Size = new System.Drawing.Size(804, 208);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Image = global::THOK.WES.Properties.Resources.accept;
            this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfirm.Location = new System.Drawing.Point(144, 0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(48, 44);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "完成";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Enabled = false;
            this.btnCancel.Image = global::THOK.WES.Properties.Resources.onebit_24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(96, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 44);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnApply.Enabled = false;
            this.btnApply.Image = global::THOK.WES.Properties.Resources.onebit_23;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.Location = new System.Drawing.Point(48, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(48, 44);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "申请";
            this.btnApply.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearch.Image = global::THOK.WES.Properties.Resources.onebit_02;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 44);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslBillID,
            this.sslOperator});
            this.ssMain.Location = new System.Drawing.Point(0, 140);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(804, 22);
            this.ssMain.TabIndex = 0;
            this.ssMain.Text = "statusStrip1";
            // 
            // sslBillID
            // 
            this.sslBillID.Name = "sslBillID";
            this.sslBillID.Size = new System.Drawing.Size(56, 17);
            this.sslBillID.Text = "单据号：";
            // 
            // sslOperator
            // 
            this.sslOperator.Name = "sslOperator";
            this.sslOperator.Size = new System.Drawing.Size(56, 17);
            this.sslOperator.Text = "操作员：";
            // 
            // CyleTimer
            // 
            this.CyleTimer.Enabled = true;
            this.CyleTimer.Interval = 6000;
            this.CyleTimer.Tick += new System.EventHandler(this.CyleTimer_Tick);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeight = 22;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Storage,
            this.BillTypeName,
            this.ProductName,
            this.PieceQuantity,
            this.BarQuantity,
            this.StatusName,
            this.TargetStorage,
            this.BillNo,
            this.Operator,
            this.Total,
            this.CellRfid,
            this.DetailID,
            this.@BillType,
            this.StorageRfid,
            this.ProductCode,
            this.Status,
            this.TargetStorageRfid,
            this.PalletTag});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMain.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(804, 140);
            this.dgvMain.TabIndex = 1;
            // 
            // Storage
            // 
            this.Storage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Storage.DataPropertyName = "StorageName";
            this.Storage.HeaderText = "作业储位";
            this.Storage.Name = "Storage";
            this.Storage.ReadOnly = true;
            this.Storage.Width = 135;
            // 
            // BillTypeName
            // 
            this.BillTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BillTypeName.DataPropertyName = "BillTypeName";
            this.BillTypeName.HeaderText = "类型";
            this.BillTypeName.Name = "BillTypeName";
            this.BillTypeName.ReadOnly = true;
            this.BillTypeName.Width = 60;
            // 
            // ProductName
            // 
            this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "卷烟名称";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 210;
            // 
            // PieceQuantity
            // 
            this.PieceQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PieceQuantity.DataPropertyName = "PieceQuantity";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PieceQuantity.DefaultCellStyle = dataGridViewCellStyle3;
            this.PieceQuantity.HeaderText = "件数";
            this.PieceQuantity.Name = "PieceQuantity";
            this.PieceQuantity.ReadOnly = true;
            this.PieceQuantity.Width = 60;
            // 
            // BarQuantity
            // 
            this.BarQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BarQuantity.DataPropertyName = "BarQuantity";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.BarQuantity.DefaultCellStyle = dataGridViewCellStyle4;
            this.BarQuantity.HeaderText = "条数";
            this.BarQuantity.Name = "BarQuantity";
            this.BarQuantity.ReadOnly = true;
            this.BarQuantity.Width = 60;
            // 
            // StatusName
            // 
            this.StatusName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StatusName.DataPropertyName = "StatusName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StatusName.DefaultCellStyle = dataGridViewCellStyle5;
            this.StatusName.HeaderText = "状态";
            this.StatusName.Name = "StatusName";
            this.StatusName.ReadOnly = true;
            this.StatusName.Width = 80;
            // 
            // TargetStorage
            // 
            this.TargetStorage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TargetStorage.DataPropertyName = "TargetStorageName";
            this.TargetStorage.HeaderText = "目标储位";
            this.TargetStorage.Name = "TargetStorage";
            this.TargetStorage.ReadOnly = true;
            this.TargetStorage.Visible = false;
            // 
            // BillNo
            // 
            this.BillNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BillNo.DataPropertyName = "BillNo";
            this.BillNo.HeaderText = "订单编号";
            this.BillNo.Name = "BillNo";
            this.BillNo.ReadOnly = true;
            this.BillNo.Visible = false;
            // 
            // Operator
            // 
            this.Operator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Operator.DataPropertyName = "Operator";
            this.Operator.HeaderText = "操作员";
            this.Operator.Name = "Operator";
            this.Operator.ReadOnly = true;
            this.Operator.Width = 70;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "SumQuantity";
            this.Total.HeaderText = "总数";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Visible = false;
            // 
            // CellRfid
            // 
            this.CellRfid.DataPropertyName = "CellRfid";
            this.CellRfid.HeaderText = "CellRfid";
            this.CellRfid.Name = "CellRfid";
            this.CellRfid.ReadOnly = true;
            this.CellRfid.Visible = false;
            // 
            // DetailID
            // 
            this.DetailID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DetailID.DataPropertyName = "DetailID";
            this.DetailID.HeaderText = "明细编号";
            this.DetailID.Name = "DetailID";
            this.DetailID.ReadOnly = true;
            this.DetailID.Visible = false;
            this.DetailID.Width = 80;
            // 
            // @BillType
            // 
            this.@BillType.DataPropertyName = "BillType";
            this.@BillType.HeaderText = "BillType";
            this.@BillType.Name = "@BillType";
            this.@BillType.ReadOnly = true;
            this.@BillType.Visible = false;
            // 
            // StorageRfid
            // 
            this.StorageRfid.DataPropertyName = "StorageRfid";
            this.StorageRfid.HeaderText = "StorageRfid";
            this.StorageRfid.Name = "StorageRfid";
            this.StorageRfid.ReadOnly = true;
            this.StorageRfid.Visible = false;
            // 
            // ProductCode
            // 
            this.ProductCode.DataPropertyName = "ProductCode";
            this.ProductCode.HeaderText = "ProductCode";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            this.ProductCode.Visible = false;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Visible = false;
            // 
            // TargetStorageRfid
            // 
            this.TargetStorageRfid.DataPropertyName = "TargetStorageRfid";
            this.TargetStorageRfid.HeaderText = "TargetStorageRfid";
            this.TargetStorageRfid.Name = "TargetStorageRfid";
            this.TargetStorageRfid.ReadOnly = true;
            this.TargetStorageRfid.Visible = false;
            // 
            // PalletTag
            // 
            this.PalletTag.DataPropertyName = "PalletTag";
            this.PalletTag.HeaderText = "盘号";
            this.PalletTag.Name = "PalletTag";
            this.PalletTag.ReadOnly = true;
            this.PalletTag.Visible = false;
            // 
            // btnBatConfirm
            // 
            this.btnBatConfirm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBatConfirm.Enabled = false;
            this.btnBatConfirm.Image = global::THOK.WES.Properties.Resources.accept;
            this.btnBatConfirm.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBatConfirm.Location = new System.Drawing.Point(192, 0);
            this.btnBatConfirm.Name = "btnBatConfirm";
            this.btnBatConfirm.Size = new System.Drawing.Size(48, 44);
            this.btnBatConfirm.TabIndex = 10;
            this.btnBatConfirm.Text = "批量";
            this.btnBatConfirm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBatConfirm.UseVisualStyleBackColor = true;
            this.btnBatConfirm.Click += new System.EventHandler(this.btnBatConfirm_Click);
            // 
            // btnOpType
            // 
            this.btnOpType.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOpType.Image = global::THOK.WES.Properties.Resources.onebit_10;
            this.btnOpType.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOpType.Location = new System.Drawing.Point(240, 0);
            this.btnOpType.Name = "btnOpType";
            this.btnOpType.Size = new System.Drawing.Size(48, 44);
            this.btnOpType.TabIndex = 12;
            this.btnOpType.Text = "正常";
            this.btnOpType.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOpType.UseVisualStyleBackColor = true;
            this.btnOpType.Visible = false;
            this.btnOpType.Click += new System.EventHandler(this.btnOpType_Click);
            // 
            // plWailt
            // 
            this.plWailt.Controls.Add(this.label1);
            this.plWailt.Controls.Add(this.pictureBox1);
            this.plWailt.Location = new System.Drawing.Point(273, 39);
            this.plWailt.Name = "plWailt";
            this.plWailt.Size = new System.Drawing.Size(258, 85);
            this.plWailt.TabIndex = 2;
            this.plWailt.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "正在处理数据，请稍等";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::THOK.WES.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(158, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 38);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnBcCompose
            // 
            this.btnBcCompose.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBcCompose.Image = global::THOK.WES.Properties.Resources.process;
            this.btnBcCompose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBcCompose.Location = new System.Drawing.Point(288, 0);
            this.btnBcCompose.Name = "btnBcCompose";
            this.btnBcCompose.Size = new System.Drawing.Size(48, 44);
            this.btnBcCompose.TabIndex = 14;
            this.btnBcCompose.Text = "组盘";
            this.btnBcCompose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBcCompose.UseVisualStyleBackColor = true;
            this.btnBcCompose.Visible = false;
            this.btnBcCompose.Click += new System.EventHandler(this.btnBcCompose_Click);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExit.Image = global::THOK.WES.Properties.Resources.shut_down;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(336, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 44);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // BaseTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(804, 208);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BaseTaskForm";
            this.Text = "盘点作业";
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.plWailt.ResumeLayout(false);
            this.plWailt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        protected System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel sslBillID;
        private System.Windows.Forms.ToolStripStatusLabel sslOperator;
        protected System.Windows.Forms.Timer CyleTimer;
        protected System.Windows.Forms.DataGridView dgvMain;
        protected System.Windows.Forms.Button btnOpType;
        protected System.Windows.Forms.Button btnBatConfirm;
        private System.Windows.Forms.Panel plWailt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn @BillType;
        private System.Windows.Forms.Button btnExit;
        protected System.Windows.Forms.Button btnBcCompose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Storage;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PieceQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn BarQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetStorage;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellRfid;
        private System.Windows.Forms.DataGridViewTextBoxColumn DetailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StorageRfid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetStorageRfid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PalletTag;
    }
}
