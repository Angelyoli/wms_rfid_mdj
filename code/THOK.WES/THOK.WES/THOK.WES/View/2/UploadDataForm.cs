using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.WES.Dal;
using THOK.WES;

namespace THOK.WES.View
{
    public partial class UploadDataForm : THOK.AF.View.ToolbarForm
    {

        private DataTable uploadDataTable = null;
        private BillDal billDal = new BillDal();
        PDAUploadDataDal pdaDal = new PDAUploadDataDal();

        public UploadDataForm()
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;
            sslOperator.Text = "操作员:  "+Environment.MachineName;
        }     

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {           
            this.uploadDataTable = billDal.GetUploadData();
            this.dgvMain.DataSource = uploadDataTable;
            if (uploadDataTable.Rows.Count > 0)
            {
                this.btnUpload.Enabled = true;
            }
            else
            {
                this.btnUpload.Enabled = false;
            }            
        }       

        private void btnUpload_Click(object sender, EventArgs e)
        {
            billDal.UploadData(uploadDataTable,false);
            this.btnImport_Click(null,null);
            MessageBox.Show("上传成功!");
        }

        private void btnPda_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdaDal.IsConnetion())
                {
                    pdaDal.InsertData();
                    MessageBox.Show("导入成功");
                    this.btnImport_Click(null, null);
                }
                else
                {
                    MessageBox.Show("请连接PDA!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
    }
}

