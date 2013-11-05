using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using THOK.PDA.Util;
using THOK.PDA.Service;


namespace THOK.PDA.View
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SystemCache.MainFrom = this;
        }
        private void btnSmallOut_Click(object sender, EventArgs e)
        {
            string positionType = "03";
            TaskFormShow(positionType);
        }
        private void btnAbnormalOut_Click(object sender, EventArgs e)
        {
            string positionType = "04";
            TaskFormShow(positionType);
        }
        private void TaskFormShow(string positionType)
        {
            WaitCursor.Set();
            try
            {
                TaskForm taskFrom = new TaskForm(positionType);
                taskFrom.Show();
                this.Visible = false;
            }
            catch (Exception ex)
            {
                WaitCursor.Restore();
                MessageBox.Show(ex.Message);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private void btnParamenter_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                ParameterForm parameterFrom = new ParameterForm();
                parameterFrom.Show();
                this.Visible = false;
            }
            catch (Exception)
            {
                WaitCursor.Restore();
            }
        }
    }
}