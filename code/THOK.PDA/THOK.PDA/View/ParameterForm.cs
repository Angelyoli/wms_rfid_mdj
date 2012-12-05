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
            string connetionString = configUtil.GetConfig("Connection")["ConnectionString"];
            string[] connetions = connetionString.Split(';');
            this.txtServer.Text = connetions[0].Remove(0, (connetions[0].IndexOf('=') + 1));
            this.txtDatabase.Text = connetions[1].Remove(0, (connetions[1].IndexOf('=') + 1));
            this.txtUid.Text = connetions[2].Remove(0, (connetions[2].IndexOf('=') + 1));
            this.txtPwd.Text = connetions[3].Remove(0, (connetions[3].IndexOf('=') + 1));
            string HttpString = configUtil.GetConfig("HttpConnectionStr")["HttpConnStr"];
            this.txtHttpStr.Text = HttpString;
            cbConType.SelectedItem = configUtil.GetConfig("ConnetionType")["Type"];
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
                string connetionString = "server=" + this.txtServer.Text + ";database=" + this.txtDatabase.Text + ";uid=" + txtUid.Text + ";pwd=" + this.txtPwd.Text;
                Dictionary<string, string> connetionList = new Dictionary<string, string>();
                connetionList.Add("ConnectionString", connetionString);
                configUtil.SaveConfig("Connection", connetionList);



                Dictionary<string, string> typeList = new Dictionary<string, string>();
                typeList.Add("Type", cbConType.SelectedItem.ToString());
                configUtil.SaveConfig("ConnetionType", typeList);

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