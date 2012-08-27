using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class ConfirmDialog : Form
    {
        private int piece = 0;
        public int Piece
        {
            get { return piece; }
            set { piece = value;}
        }
        private int item = 0;
        public int Item
        {
            get { return item; }
            set {item = value; }
        }

        private string storageName = "";
        private string targetStorageName = "";
        private string operateName = "";
        private string tobaccoName = "";

        public ConfirmDialog(string billType, string storageID,string targetStorageName,string operateName, string tobaccoName)
        {
            InitializeComponent();
            this.storageName = storageID;
            this.targetStorageName = targetStorageName;
            this.operateName = operateName;
            this.tobaccoName = tobaccoName;

            if (billType != "4")
            {
                btnItemUp.Enabled = false;
                btnItemDown.Enabled = false;
                btnPieceUp.Enabled = false;
                btnPieceDown.Enabled = false;
            }
        }

        private void RefreshData()
        {
            lblPiece.Text = piece.ToString();
            lblItem.Text = item.ToString();
            label3.Text = "作业储位：" + this.storageName;
            label4.Text = "操作类型：" + this.operateName + (this.targetStorageName != ""?"->" + this.targetStorageName:""); 
            label5.Text = "卷烟名称：" + this.tobaccoName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnPieceUp_Click(object sender, EventArgs e)
        {
            piece++;
            RefreshData();
        }

        private void btnPieceDown_Click(object sender, EventArgs e)
        {
            piece--;
            if (piece < 0)
                piece = 0;
            RefreshData();
        }

        private void btnItemUp_Click(object sender, EventArgs e)
        {
            item++;
            RefreshData();
        }

        private void btnItemDown_Click(object sender, EventArgs e)
        {
            item--;
            if (item < 0)
                item = 0;
            RefreshData();
        }

        private void ConfirmDialog_Paint(object sender, PaintEventArgs e)
        {
            RefreshData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}