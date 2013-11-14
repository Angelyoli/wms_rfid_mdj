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

namespace THOK.PDA.View
{
    public partial class ParameterForm : Form
    {
        private ConfigUtil configUtil = new ConfigUtil();
        public ParameterForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ParameterForm_Load(object sender, EventArgs e)
        {
            string HttpString = configUtil.GetConfig("HttpConnectionStr")["HttpConnStr"];
            this.txtHttpStr.Text = HttpString;
            WaitCursor.Restore();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SystemCache.MainFrom.Visible = true;
            inputPanel1.Enabled = false;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WaitCursor.Set();
            try
            {
                Dictionary<string, string> httpStr = new Dictionary<string, string>();
                httpStr.Add("HttpConnStr", this.txtHttpStr.Text);
                configUtil.SaveConfig("HttpConnectionStr", httpStr);

                inputPanel1.Enabled = false;
                WaitCursor.Restore();
                MessageBox.Show("参数保存成功!请重启系统");
            }
            catch (Exception)
            {
                WaitCursor.Restore();
                MessageBox.Show("参数保存失败!请重启系统");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            inputPanel1.Enabled = !inputPanel1.Enabled;
        }

        private void ParameterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.btnCancel_Click(null, null);
            }
            if (e.KeyCode.ToString() == "Return")
            {
                this.btnSave_Click(null, null);
            }
        }
    }
}