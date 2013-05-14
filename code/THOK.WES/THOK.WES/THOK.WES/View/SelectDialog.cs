using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Interface.Model;

namespace THOK.WES.View
{
    public partial class SelectDialog : Form
    {
        ListBox lst = new ListBox();
        private string selectedBillID;
        public string SelectedBillID
        {
            get { return selectedBillID; }
            set { selectedBillID = value; }
        }

        public SelectDialog(BillMaster [] billMasters)
        {
            InitializeComponent();
            //cbBillID.DataSource = billMasters;
            foreach (var row in billMasters)
            {
                cbBillIDCheck.Items.Add(row.BillNo);
            }          
            //cbBillIDCheck.Size.Height = 59;
            cbBillIDCheck.DisplayMember = "BillNo";
            cbBillIDCheck.ValueMember = "BillNo";
            cbBillIDCheck.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedBillID = this.cbBillIDCheck.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}