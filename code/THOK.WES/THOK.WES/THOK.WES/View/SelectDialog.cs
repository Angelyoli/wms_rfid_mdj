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
        public string SelectedBillID
        {
            get { return cbBillID.SelectedValue.ToString(); }
        }

        public SelectDialog(BillMaster [] billMasters)
        {
            InitializeComponent();
            cbBillID.DataSource = billMasters;
            cbBillID.DisplayMember = "BillNo";
            cbBillID.ValueMember = "BillNo";
            cbBillID.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}