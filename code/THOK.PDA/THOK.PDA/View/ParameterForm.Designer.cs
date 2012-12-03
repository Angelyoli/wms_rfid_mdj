namespace THOK.PDA.View
{
    partial class ParameterForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.btnStart = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbConType = new System.Windows.Forms.ComboBox();
            this.txtHttpStr = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.Location = new System.Drawing.Point(6, 275);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 32);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保  存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnCancel.Location = new System.Drawing.Point(174, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 30);
            this.label2.Text = "参 数 设 置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(73, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.Text = "数据库连接";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(-2, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.Text = "服务器名称";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(90, 124);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(144, 23);
            this.txtServer.TabIndex = 8;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(90, 153);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(144, 23);
            this.txtDatabase.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(-2, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 20);
            this.label4.Text = "数 据 库 名";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(90, 215);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(144, 23);
            this.txtPwd.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(-2, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 20);
            this.label5.Text = "密     码";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtUid
            // 
            this.txtUid.Location = new System.Drawing.Point(90, 184);
            this.txtUid.Name = "txtUid";
            this.txtUid.Size = new System.Drawing.Size(144, 23);
            this.txtUid.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(-2, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.Text = "用  户  名";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnStart.Location = new System.Drawing.Point(91, 275);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(63, 32);
            this.btnStart.TabIndex = 23;
            this.btnStart.Text = "键  盘";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(81, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.Text = "连接方式";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(-2, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 20);
            this.label8.Text = "连 接 类 型";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbConType
            // 
            this.cbConType.Items.Add("网络连接");
            this.cbConType.Items.Add("USB连接");
            this.cbConType.Location = new System.Drawing.Point(91, 75);
            this.cbConType.Name = "cbConType";
            this.cbConType.Size = new System.Drawing.Size(143, 23);
            this.cbConType.TabIndex = 35;
            // 
            // txtHttpStr
            // 
            this.txtHttpStr.Location = new System.Drawing.Point(90, 244);
            this.txtHttpStr.Name = "txtHttpStr";
            this.txtHttpStr.Size = new System.Drawing.Size(144, 23);
            this.txtHttpStr.TabIndex = 44;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 20);
            this.label9.Text = "web地址：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ParameterForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(243, 320);
            this.ControlBox = false;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtHttpStr);
            this.Controls.Add(this.cbConType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtUid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "ParameterForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ParameterForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParameterForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUid;
        private System.Windows.Forms.Label label6;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbConType;
        private System.Windows.Forms.TextBox txtHttpStr;
        private System.Windows.Forms.Label label9;

    }
}

