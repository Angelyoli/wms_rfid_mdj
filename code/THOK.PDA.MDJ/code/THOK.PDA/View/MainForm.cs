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
        
        private void btnRepositoryOneOut_Click(object sender, EventArgs e)
        {
            string positionType = "03";
            string orderType = "03";
            TaskFormShow(positionType, orderType);
        }
        private void btnRepositoryOneCheck_Click(object sender, EventArgs e)
        {
            string positionType = "03";
            string orderType = "04";
            TaskFormShow(positionType, orderType);
        }
        private void btnRepositoryTwoOut_Click(object sender, EventArgs e)
        {
            string positionType = "04";
            string orderType = "03";
            TaskFormShow(positionType, orderType);
        }
        private void btnRepositoryTwoCheck_Click(object sender, EventArgs e)
        {
            string positionType = "04";
            string orderType = "04";
            TaskFormShow(positionType, orderType);
        }
        private void btnSmallMove_Click(object sender, EventArgs e)
        {
            string positionType = "03";
            string orderType = "02";
            TaskFormShow(positionType, orderType);
        }
        private void btnAbnormalMove_Click(object sender, EventArgs e)
        {
            string positionType = "04";
            string orderType = "02";
            TaskFormShow(positionType, orderType);
        }

        private void TaskFormShow(string positionType, string orderType)
        {
            WaitCursor.Set();
            try
            {
                TaskForm taskFrom = new TaskForm(positionType,orderType);
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
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }            
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