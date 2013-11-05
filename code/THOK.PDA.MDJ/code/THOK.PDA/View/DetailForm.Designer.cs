namespace THOK.PDA.View
{
    partial class DetailForm
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbID = new System.Windows.Forms.Label();
            this.lbCellCode = new System.Windows.Forms.Label();
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbPieceQuantity = new System.Windows.Forms.Label();
            this.lbBarQuantity = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbOrderType = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbOrderID = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnBack.Location = new System.Drawing.Point(45, 275);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(77, 32);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "上一步";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnComplete.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnComplete.Location = new System.Drawing.Point(196, 275);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(79, 32);
            this.btnComplete.TabIndex = 5;
            this.btnComplete.Text = "完　成";
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(45, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 30);
            this.label2.Text = "单据明细";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(45, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.Text = "作业号:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(45, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.Text = "货位号:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(45, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.Text = "卷烟名:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(45, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 20);
            this.label5.Text = "件　数:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(45, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 20);
            this.label6.Text = "条　数:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(45, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 20);
            this.label7.Text = "状　态:";
            // 
            // lbID
            // 
            this.lbID.Location = new System.Drawing.Point(108, 41);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(167, 20);
            this.lbID.Text = "ID";
            // 
            // lbCellCode
            // 
            this.lbCellCode.Location = new System.Drawing.Point(108, 98);
            this.lbCellCode.Name = "lbCellCode";
            this.lbCellCode.Size = new System.Drawing.Size(167, 20);
            this.lbCellCode.Text = "CellCode";
            // 
            // lbProductName
            // 
            this.lbProductName.Location = new System.Drawing.Point(108, 128);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(167, 20);
            this.lbProductName.Text = "ProductName";
            // 
            // lbPieceQuantity
            // 
            this.lbPieceQuantity.Location = new System.Drawing.Point(109, 157);
            this.lbPieceQuantity.Name = "lbPieceQuantity";
            this.lbPieceQuantity.Size = new System.Drawing.Size(125, 20);
            this.lbPieceQuantity.Text = "PieceQuantity";
            // 
            // lbBarQuantity
            // 
            this.lbBarQuantity.Location = new System.Drawing.Point(108, 186);
            this.lbBarQuantity.Name = "lbBarQuantity";
            this.lbBarQuantity.Size = new System.Drawing.Size(126, 20);
            this.lbBarQuantity.Text = "BarQuantity";
            // 
            // lbStatus
            // 
            this.lbStatus.Location = new System.Drawing.Point(108, 216);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(167, 20);
            this.lbStatus.Text = "Status";
            // 
            // lbOrderType
            // 
            this.lbOrderType.Location = new System.Drawing.Point(108, 247);
            this.lbOrderType.Name = "lbOrderType";
            this.lbOrderType.Size = new System.Drawing.Size(167, 20);
            this.lbOrderType.Text = "OrderType";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(45, 247);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 20);
            this.label9.Text = "类　型:";
            // 
            // lbOrderID
            // 
            this.lbOrderID.Location = new System.Drawing.Point(108, 69);
            this.lbOrderID.Name = "lbOrderID";
            this.lbOrderID.Size = new System.Drawing.Size(167, 20);
            this.lbOrderID.Text = "OrderID";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(45, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 20);
            this.label10.Text = "主单据:";
            // 
            // DetailForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(320, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbOrderID);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lbOrderType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbBarQuantity);
            this.Controls.Add(this.lbPieceQuantity);
            this.Controls.Add(this.lbProductName);
            this.Controls.Add(this.lbCellCode);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DetailForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.BillDetailForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbCellCode;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbPieceQuantity;
        private System.Windows.Forms.Label lbBarQuantity;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbOrderType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbOrderID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbID;

    }
}

