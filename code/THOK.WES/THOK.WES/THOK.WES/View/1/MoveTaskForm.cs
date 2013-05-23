using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class MoveTaskForm : THOK.WES.View.BaseTaskForm
    {
        public MoveTaskForm()
        {
            InitializeComponent();
            BillTypes = "3";
            btnOpType.Visible = true;
            btnBcCompose.Visible = true;
            dgvMain.Columns["TARGETSTORAGE"].Visible = true;
            dgvMain.Columns["Total"].Visible = true;
        }
    }
}

