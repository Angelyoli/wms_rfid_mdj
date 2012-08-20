namespace THOK.WES.View
{
    partial class ConfirmDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlPiece = new System.Windows.Forms.Panel();
            this.btnPieceDown = new System.Windows.Forms.Button();
            this.btnPieceUp = new System.Windows.Forms.Button();
            this.lblPiece = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlItem = new System.Windows.Forms.Panel();
            this.btnItemDown = new System.Windows.Forms.Button();
            this.lblItem = new System.Windows.Forms.Label();
            this.btnItemUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pnlPiece.SuspendLayout();
            this.pnlItem.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 38F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 51);
            this.label1.TabIndex = 2;
            this.label1.Text = "件数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 38F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 51);
            this.label2.TabIndex = 2;
            this.label2.Text = "条数";
            // 
            // pnlPiece
            // 
            this.pnlPiece.BackColor = System.Drawing.SystemColors.Window;
            this.pnlPiece.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPiece.Controls.Add(this.btnPieceDown);
            this.pnlPiece.Controls.Add(this.btnPieceUp);
            this.pnlPiece.Controls.Add(this.lblPiece);
            this.pnlPiece.Location = new System.Drawing.Point(148, 14);
            this.pnlPiece.Name = "pnlPiece";
            this.pnlPiece.Size = new System.Drawing.Size(216, 71);
            this.pnlPiece.TabIndex = 4;
            // 
            // btnPieceDown
            // 
            this.btnPieceDown.Image = global::THOK.WES.Properties.Resources.down;
            this.btnPieceDown.Location = new System.Drawing.Point(168, 34);
            this.btnPieceDown.Name = "btnPieceDown";
            this.btnPieceDown.Size = new System.Drawing.Size(46, 35);
            this.btnPieceDown.TabIndex = 9;
            this.btnPieceDown.UseVisualStyleBackColor = true;
            this.btnPieceDown.Click += new System.EventHandler(this.btnPieceDown_Click);
            // 
            // btnPieceUp
            // 
            this.btnPieceUp.Image = global::THOK.WES.Properties.Resources.up;
            this.btnPieceUp.Location = new System.Drawing.Point(168, 0);
            this.btnPieceUp.Name = "btnPieceUp";
            this.btnPieceUp.Size = new System.Drawing.Size(46, 35);
            this.btnPieceUp.TabIndex = 8;
            this.btnPieceUp.UseVisualStyleBackColor = true;
            this.btnPieceUp.Click += new System.EventHandler(this.btnPieceUp_Click);
            // 
            // lblPiece
            // 
            this.lblPiece.AutoSize = true;
            this.lblPiece.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPiece.Location = new System.Drawing.Point(1, 8);
            this.lblPiece.Name = "lblPiece";
            this.lblPiece.Size = new System.Drawing.Size(50, 54);
            this.lblPiece.TabIndex = 7;
            this.lblPiece.Text = "0";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::THOK.WES.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(240, 303);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 47);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "   取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Image = global::THOK.WES.Properties.Resources.accept;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(82, 303);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 47);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "   确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlItem
            // 
            this.pnlItem.BackColor = System.Drawing.SystemColors.Window;
            this.pnlItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlItem.Controls.Add(this.btnItemDown);
            this.pnlItem.Controls.Add(this.lblItem);
            this.pnlItem.Controls.Add(this.btnItemUp);
            this.pnlItem.Location = new System.Drawing.Point(148, 12);
            this.pnlItem.Name = "pnlItem";
            this.pnlItem.Size = new System.Drawing.Size(217, 71);
            this.pnlItem.TabIndex = 7;
            // 
            // btnItemDown
            // 
            this.btnItemDown.Image = global::THOK.WES.Properties.Resources.down;
            this.btnItemDown.Location = new System.Drawing.Point(169, 34);
            this.btnItemDown.Name = "btnItemDown";
            this.btnItemDown.Size = new System.Drawing.Size(46, 35);
            this.btnItemDown.TabIndex = 11;
            this.btnItemDown.UseVisualStyleBackColor = true;
            this.btnItemDown.Click += new System.EventHandler(this.btnItemDown_Click);
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblItem.Location = new System.Drawing.Point(2, 7);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(50, 54);
            this.lblItem.TabIndex = 8;
            this.lblItem.Text = "0";
            // 
            // btnItemUp
            // 
            this.btnItemUp.Image = global::THOK.WES.Properties.Resources.up;
            this.btnItemUp.Location = new System.Drawing.Point(169, 0);
            this.btnItemUp.Name = "btnItemUp";
            this.btnItemUp.Size = new System.Drawing.Size(46, 35);
            this.btnItemUp.TabIndex = 10;
            this.btnItemUp.UseVisualStyleBackColor = true;
            this.btnItemUp.Click += new System.EventHandler(this.btnItemUp_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 28);
            this.panel1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "label3";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(396, 29);
            this.panel2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "label4";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(396, 31);
            this.panel3.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "label5";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.pnlPiece);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 88);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(396, 100);
            this.panel4.TabIndex = 12;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.pnlItem);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 188);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(396, 100);
            this.panel5.TabIndex = 13;
            // 
            // ConfirmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 359);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "作业数量确认";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ConfirmDialog_Paint);
            this.pnlPiece.ResumeLayout(false);
            this.pnlPiece.PerformLayout();
            this.pnlItem.ResumeLayout(false);
            this.pnlItem.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlPiece;
        private System.Windows.Forms.Panel pnlItem;
        private System.Windows.Forms.Label lblPiece;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Button btnPieceUp;
        private System.Windows.Forms.Button btnPieceDown;
        private System.Windows.Forms.Button btnItemDown;
        private System.Windows.Forms.Button btnItemUp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}