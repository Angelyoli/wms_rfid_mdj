using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class EntryTaskForm : THOK.WES.View.BaseTaskForm
    {
        public EntryTaskForm()
        {
            InitializeComponent();
            BillTypes = "1";
            btnOpType.Visible = true;
        }
    }
}

