using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.WES.View
{
    public partial class ExitForm : THOK.AF.View.ToolbarForm
    {
        public ExitForm()
        {
            Application.Exit();
            InitializeComponent();            
        }
    }
}