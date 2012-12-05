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
            this.btnOut = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnParamenter = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOut
            // 
            this.btnOut.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnOut.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnOut.Location = new System.Drawing.Point(4, 49);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(112, 87);
            this.btnOut.TabIndex = 0;
            this.btnOut.Text = "出库作业";
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnIn
            // 
            this.btnIn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnIn.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnIn.Location = new System.Drawing.Point(122, 49);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(112, 87);
            this.btnIn.TabIndex = 1;
            this.btnIn.Text = "入库作业";
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheck.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnCheck.Location = new System.Drawing.Point(4, 142);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(112, 87);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "盘点作业";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnMove
            // 
            this.btnMove.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnMove.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnMove.Location = new System.Drawing.Point(122, 142);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(112, 87);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "移库作业";
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnParamenter
            // 
            this.btnParamenter.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnParamenter.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnParamenter.Location = new System.Drawing.Point(4, 235);
            this.btnParamenter.Name = "btnParamenter";
            this.btnParamenter.Size = new System.Drawing.Size(112, 32);
            this.btnParamenter.TabIndex = 4;
            this.btnParamenter.Text = "参数设置";
            this.btnParamenter.Click += new System.EventHandler(this.btnParamenter_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnExit.Location = new System.Drawing.Point(122, 235);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(112, 32);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "关闭";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(4, 285);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 32);
            this.label1.Text = "天海欧康科技信息(厦门)有限公司       版权所有        ©2010-2020";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 30);
            this.label2.Text = "仓储引导作业系统";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(243, 320);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnParamenter);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnOut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnParamenter;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}

