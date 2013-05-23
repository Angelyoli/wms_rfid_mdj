namespace THOK.WES.View
{
    partial class CellDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.pgCell = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(376, 329);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // pgCell
            // 
            this.pgCell.BackColor = System.Drawing.SystemColors.Control;
            this.pgCell.CommandsBackColor = System.Drawing.Color.Maroon;
            this.pgCell.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pgCell.HelpVisible = false;
            this.pgCell.Location = new System.Drawing.Point(0, 0);
            this.pgCell.Name = "pgCell";
            this.pgCell.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgCell.Size = new System.Drawing.Size(486, 306);
            this.pgCell.TabIndex = 0;
            // 
            // CellDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 364);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pgCell);
            this.Name = "CellDialog";
            this.Text = "CellDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PropertyGrid pgCell;
    }
}