namespace THOK.PDA.View
{
    partial class MainForm
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
            this.btnParamenter = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAbnormalMove = new System.Windows.Forms.Button();
            this.btnSmallMove = new System.Windows.Forms.Button();
            this.btnRepositoryTwoOut = new System.Windows.Forms.Button();
            this.btnRepositoryTwoCheck = new System.Windows.Forms.Button();
            this.btnRepositoryOneOut = new System.Windows.Forms.Button();
            this.btnRepositoryOneCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnParamenter
            // 
            this.btnParamenter.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnParamenter.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnParamenter.Location = new System.Drawing.Point(32, 237);
            this.btnParamenter.Name = "btnParamenter";
            this.btnParamenter.Size = new System.Drawing.Size(120, 26);
            this.btnParamenter.TabIndex = 7;
            this.btnParamenter.Text = "参数设置";
            this.btnParamenter.Click += new System.EventHandler(this.btnParamenter_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnExit.Location = new System.Drawing.Point(172, 237);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 26);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "关闭";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(49, 281);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 32);
            this.label1.Text = "天海欧康科技信息(厦门)有限公司       版权所有        ©2010-2020";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(56, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 30);
            this.label2.Text = "仓储引导作业系统";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnAbnormalMove
            // 
            this.btnAbnormalMove.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnAbnormalMove.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnAbnormalMove.Location = new System.Drawing.Point(172, 166);
            this.btnAbnormalMove.Name = "btnAbnormalMove";
            this.btnAbnormalMove.Size = new System.Drawing.Size(120, 52);
            this.btnAbnormalMove.TabIndex = 6;
            this.btnAbnormalMove.Text = "异型烟移库";
            this.btnAbnormalMove.Click += new System.EventHandler(this.btnAbnormalMove_Click);
            // 
            // btnSmallMove
            // 
            this.btnSmallMove.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnSmallMove.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSmallMove.Location = new System.Drawing.Point(32, 166);
            this.btnSmallMove.Name = "btnSmallMove";
            this.btnSmallMove.Size = new System.Drawing.Size(120, 52);
            this.btnSmallMove.TabIndex = 5;
            this.btnSmallMove.Text = "小品种移库";
            this.btnSmallMove.Click += new System.EventHandler(this.btnSmallMove_Click);
            // 
            // btnRepositoryTwoOut
            // 
            this.btnRepositoryTwoOut.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnRepositoryTwoOut.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnRepositoryTwoOut.Location = new System.Drawing.Point(172, 50);
            this.btnRepositoryTwoOut.Name = "btnRepositoryTwoOut";
            this.btnRepositoryTwoOut.Size = new System.Drawing.Size(120, 52);
            this.btnRepositoryTwoOut.TabIndex = 3;
            this.btnRepositoryTwoOut.Text = "二号库出库";
            this.btnRepositoryTwoOut.Click += new System.EventHandler(this.btnRepositoryTwoOut_Click);
            // 
            // btnRepositoryTwoCheck
            // 
            this.btnRepositoryTwoCheck.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnRepositoryTwoCheck.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnRepositoryTwoCheck.Location = new System.Drawing.Point(172, 108);
            this.btnRepositoryTwoCheck.Name = "btnRepositoryTwoCheck";
            this.btnRepositoryTwoCheck.Size = new System.Drawing.Size(120, 52);
            this.btnRepositoryTwoCheck.TabIndex = 4;
            this.btnRepositoryTwoCheck.Text = "二号库盘点";
            this.btnRepositoryTwoCheck.Click += new System.EventHandler(this.btnRepositoryTwoCheck_Click);
            // 
            // btnRepositoryOneOut
            // 
            this.btnRepositoryOneOut.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnRepositoryOneOut.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnRepositoryOneOut.Location = new System.Drawing.Point(32, 50);
            this.btnRepositoryOneOut.Name = "btnRepositoryOneOut";
            this.btnRepositoryOneOut.Size = new System.Drawing.Size(120, 52);
            this.btnRepositoryOneOut.TabIndex = 0;
            this.btnRepositoryOneOut.Text = "一号库出库";
            this.btnRepositoryOneOut.Click += new System.EventHandler(this.btnRepositoryOneOut_Click);
            // 
            // btnRepositoryOneCheck
            // 
            this.btnRepositoryOneCheck.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnRepositoryOneCheck.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnRepositoryOneCheck.Location = new System.Drawing.Point(32, 108);
            this.btnRepositoryOneCheck.Name = "btnRepositoryOneCheck";
            this.btnRepositoryOneCheck.Size = new System.Drawing.Size(120, 52);
            this.btnRepositoryOneCheck.TabIndex = 2;
            this.btnRepositoryOneCheck.Text = "一号库盘点";
            this.btnRepositoryOneCheck.Click += new System.EventHandler(this.btnRepositoryOneCheck_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(320, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnRepositoryOneCheck);
            this.Controls.Add(this.btnRepositoryOneOut);
            this.Controls.Add(this.btnSmallMove);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnParamenter);
            this.Controls.Add(this.btnRepositoryTwoCheck);
            this.Controls.Add(this.btnRepositoryTwoOut);
            this.Controls.Add(this.btnAbnormalMove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnParamenter;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAbnormalMove;
        private System.Windows.Forms.Button btnSmallMove;
        private System.Windows.Forms.Button btnRepositoryTwoOut;
        private System.Windows.Forms.Button btnRepositoryTwoCheck;
        private System.Windows.Forms.Button btnRepositoryOneOut;
        private System.Windows.Forms.Button btnRepositoryOneCheck;

    }
}

