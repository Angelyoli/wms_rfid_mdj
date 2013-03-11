using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using DataRabbit.HashOrm;

namespace THOK.Application.LabelServer
{
    public partial class Server : Form
    {
        private Thread myThread;
        private Adapter[] adapters;

        #region 自动生成不得修改
       
        private Button btStart;
        private Button btStop;
        private Button btExit;
        private StatusStrip statusStrip;
        private NotifyIcon notifyIcon;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem 填入模似数据ToolStripMenuItem;
        private ToolStripMenuItem 清空模拟数据ToolStripMenuItem;
        private PictureBox picStatus;
        private ToolStripMenuItem toolStripMenuItem2;
        private Button btResent;
        private Button btClear;
        private TextBox txtOrderID;
        private Process process1;
        private ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.Timer TimerCounter;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Label label1;
        private Label label3;
        private Label label2;
        private ToolTip toolTip1;
        private Label label6;
        private Label label5;
        private ToolStripMenuItem 设置基础数据ToolStripMenuItem;
        private Label label4;

        public Server()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.btStart = new System.Windows.Forms.Button();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.btStop = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.填入模似数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空模拟数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置基础数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btResent = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.process1 = new System.Diagnostics.Process();
            this.TimerCounter = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Image = ((System.Drawing.Image)(resources.GetObject("btStart.Image")));
            this.btStart.Location = new System.Drawing.Point(273, 92);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(42, 24);
            this.btStart.TabIndex = 0;
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // picStatus
            // 
            this.picStatus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picStatus.BackgroundImage")));
            this.picStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picStatus.Location = new System.Drawing.Point(67, 76);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(100, 114);
            this.picStatus.TabIndex = 1;
            this.picStatus.TabStop = false;
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Image = ((System.Drawing.Image)(resources.GetObject("btStop.Image")));
            this.btStop.Location = new System.Drawing.Point(273, 122);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(42, 24);
            this.btStop.TabIndex = 2;
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btExit
            // 
            this.btExit.Image = ((System.Drawing.Image)(resources.GetObject("btExit.Image")));
            this.btExit.Location = new System.Drawing.Point(273, 152);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(42, 24);
            this.btExit.TabIndex = 3;
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 228);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(423, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "电子标签服务";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.showToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(95, 70);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem2.Text = "显示";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.填入模似数据ToolStripMenuItem,
            this.清空模拟数据ToolStripMenuItem,
            this.设置基础数据ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem1.Text = "测试";
            // 
            // 填入模似数据ToolStripMenuItem
            // 
            this.填入模似数据ToolStripMenuItem.Name = "填入模似数据ToolStripMenuItem";
            this.填入模似数据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.填入模似数据ToolStripMenuItem.Text = "填入测试数据";
            this.填入模似数据ToolStripMenuItem.Click += new System.EventHandler(this.填入模似数据ToolStripMenuItem_Click);
            // 
            // 清空模拟数据ToolStripMenuItem
            // 
            this.清空模拟数据ToolStripMenuItem.Name = "清空模拟数据ToolStripMenuItem";
            this.清空模拟数据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.清空模拟数据ToolStripMenuItem.Text = "清空测试数据";
            this.清空模拟数据ToolStripMenuItem.Click += new System.EventHandler(this.清空模拟数据ToolStripMenuItem_Click);
            // 
            // 设置基础数据ToolStripMenuItem
            // 
            this.设置基础数据ToolStripMenuItem.Name = "设置基础数据ToolStripMenuItem";
            this.设置基础数据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.设置基础数据ToolStripMenuItem.Text = "设置基础数据";
            this.设置基础数据ToolStripMenuItem.Click += new System.EventHandler(this.设置基础数据ToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.showToolStripMenuItem.Text = "show";
            this.showToolStripMenuItem.Visible = false;
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // btResent
            // 
            this.btResent.Image = ((System.Drawing.Image)(resources.GetObject("btResent.Image")));
            this.btResent.Location = new System.Drawing.Point(273, 201);
            this.btResent.Name = "btResent";
            this.btResent.Size = new System.Drawing.Size(42, 24);
            this.btResent.TabIndex = 8;
            this.btResent.UseVisualStyleBackColor = true;
            this.btResent.Click += new System.EventHandler(this.btResent_Click);
            // 
            // btClear
            // 
            this.btClear.Image = ((System.Drawing.Image)(resources.GetObject("btClear.Image")));
            this.btClear.Location = new System.Drawing.Point(369, 201);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(42, 24);
            this.btClear.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btClear, "删除无用的数据");
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // txtOrderID
            // 
            this.txtOrderID.Location = new System.Drawing.Point(68, 204);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(154, 21);
            this.txtOrderID.TabIndex = 10;
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // TimerCounter
            // 
            this.TimerCounter.Interval = 1000;
            this.TimerCounter.Tag = "0";
            this.TimerCounter.Tick += new System.EventHandler(this.TimerCounter_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(238, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "启动";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "暂停";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "退出";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipTitle = "删除";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "清空";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "重发";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "订单号：";
            // 
            // Server
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(423, 250);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOrderID);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.btResent);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.picStatus);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Server";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数字化仓储标签服务管理系统";
            this.Load += new System.EventHandler(this.Server_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void btStart_Click(object sender, EventArgs e)
        {
            try
            {
                TimerCounter.Enabled = false;
                toolStripStatusLabel1.Text = "启动服务！";
                btStart.Enabled = false;

                IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
                IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();

                sy_showinfosAccesser.ExecuteNonQuery("update Sy_ShowInfo set ReadState = 1 ,HardwareReadState = 1 ,ConfirmState = 1 ");
                storagesAccesser.ExecuteNonQuery("Update [Storages] Set [Act] = '',[ProductName] = '',[Contents] = '',[Sign] = 0 ,[Err] = 0 , [NumberShow] = ''");

                IList<Storages> storages = storagesAccesser.Select<Storages>("select Port from storages group by port");

                if (adapters == null)
                {
                    adapters = new Adapter[int.Parse(storages.Last().Port) + 256];
                    foreach (Storages storage in storages)
                    {
                        int port = int.Parse(storage.Port);
                        adapters[port] = new Adapter();
                        adapters[port].Port = port;
                        adapters[port].Start();
                    }

                    Service.InitializeService(this, this.toolStripStatusLabel1, this.TimerCounter, adapters);
                }


                myThread = new Thread(new ThreadStart(Run));
                myThread.Start();

                btStop.Enabled = true;
                btExit.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务启动失败！" + ex.Message, "电子标签服务", MessageBoxButtons.OK);
                btExit_Click(sender, e);
            }
        }       
        private void btStop_Click(object sender, EventArgs e)
        {
            try
            {
                TimerCounter.Enabled = false;                
                btStop.Enabled = false;

                foreach (Adapter adapter in adapters)
                {
                    if (adapter != null)
                    {
                        adapter.Reset();
                    }
                }
                if (myThread != null && myThread.IsAlive == true)
                {
                    myThread.Abort();
                }

                toolStripStatusLabel1.Text = "服务停止！";
                btStart.Enabled = true;
                btExit.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务停止失败！" + ex.Message, "电子标签服务", MessageBoxButtons.OK);
                btExit_Click(sender, e);
            }
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            try
            {
                btExit.Enabled = false;
                foreach (Adapter adapter in adapters)
                {
                    if (adapter != null)
                    {
                        adapter.Stop();
                    }
                }
                if (myThread != null && myThread.IsAlive == true)
                {
                    myThread.Abort();
                }
            }
            catch{}
            finally
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            sy_showinfosAccesser.ExecuteNonQuery("Delete from Sy_ShowInfo");
            //sy_showinfosAccesser.ExecuteNonQuery("Delete from [dbo].[tab_Label]");
            storagesAccesser.ExecuteNonQuery("Update [Storages] Set [Act] = '',[ProductName] = '',[Contents] = '',[Sign] = 0 ,[Err] = 0");
            MessageBox.Show("数据清空成功！", "电子标签服务", MessageBoxButtons.OK);
        }
        private void btResent_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();

            IFilter filter = new Filter(Sy_ShowInfo._OrderMasterID, txtOrderID.Text.Trim(), ComparisonOperators.Equal);

            IList<Sy_ShowInfo> sy_showInfo = sy_showinfosAccesser.Select<Sy_ShowInfo>(filter);

            if (sy_showInfo.Count == 0)
            {
                MessageBox.Show("订单号不存在，请检查订单号是否正确！或数据发送失败！", "电子标签服务", MessageBoxButtons.OK);
                return;
            }
            sy_showinfosAccesser.ExecuteNonQuery("update Sy_ShowInfo set ReadState = 0 ,HardwareReadState = 0 ,ConfirmState = 0 where OrderMasterID = '" + txtOrderID.Text.Trim() + "'");

            MessageBox.Show("订单重发成功！如无显示请检查订单号是否正确！", "电子标签服务", MessageBoxButtons.OK);
        }

        private void Run()
        {
            try
            {
                Service.Run();
            }
            catch (Exception e)
            {
            }
        }

        #region 托盘菜单功能        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible  = true;
        }

        private void 填入模似数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();

            IList<Storages> storages = storagesAccesser.Select<Storages>(new Filter(),Storages._Address);

            foreach (Storages storage in storages)
            {
                try
                {
                    sy_showinfosAccesser.ExecuteNonQuery ("insert [Sy_ShowInfo]  values ('0', 'OrderDetailID','" + storage.StorageID.Trim() + "',1,'红金龙红金龙',30,12,0,0,0,'')");
                } catch { }
            }
        }
        private void 清空模拟数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser sy_showinfosAccesser = DBFactory.NewHashOrmAccesser();
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            sy_showinfosAccesser.ExecuteNonQuery("Delete from Sy_ShowInfo");
            storagesAccesser.ExecuteNonQuery("Update [Storages] Set [Act] = '',[ProductName] = '',[Contents] = '',[Sign] = 0 ,[Err] = 0 ,[NumberShow] =''");
            MessageBox.Show("数据清空成功！", "电子标签服务", MessageBoxButtons.OK);
        }
        private void 设置基础数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Set()).Show();
        }

        #endregion


        private void TimerCounter_Tick(object sender, EventArgs e)
        {
            if (Type.GetTypeCode(TimerCounter.Tag.GetType()) == TypeCode.Int32)
            {
                TimerCounter.Tag = Convert.ToInt32(TimerCounter.Tag) + 1;
                toolStripStatusLabel1.Text = toolStripStatusLabel1.Tag  + "  用时：" + TimerCounter.Tag + "  秒  ";
            }
        }
        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        #region  程序运行控制只允许一个进程运行。      
         
        [DllImport("user32")]
        public static extern long ShowWindow(long hwnd , long nCmdShow);
        [DllImport("user32")]
        public static extern long SetForegroundWindow(long hwnd);
        public  const uint  WM_SYSCOMMAND = 0x112;
        public const uint  SC_RESTORE = 0xF120;
        //[DllImport("user32")]
        //public static extern long SendMessage (long  hwnd ,long  wMsg, long  wParam,object lParam );
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
        IntPtr hWnd, // handle to destination window 
        uint Msg, // message 
        uint wParam, // first message parameter 
        uint lParam // second message parameter 
        ); 
        private void Server_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("LabelServer").Count() > 1)
            {
                if (MessageBox.Show("电子标签服务已启动，将自动退出本程序！", "电子标签服务", MessageBoxButtons.OK).ToString() == "OK")
                {
                    foreach (Process p in Process.GetProcessesByName("LabelServer"))
                    {
                        if (this.Handle != ReadHandle())
                        {
                            SendMessage(ReadHandle(), WM_SYSCOMMAND, SC_RESTORE, 0);

                            SetForegroundWindow((long)ReadHandle());
                        }
                    }
                    System.Windows.Forms.Application.Exit();
                    return;
                }
            }
            else
            {
                WriteReg();
            }
                
        }
        protected override void WndProc(ref Message m)
        {
            if(m.Msg  == (int)WM_SYSCOMMAND && m.WParam.ToInt64 () == SC_RESTORE)
            {
                this.Visible = true;
            }
            base.WndProc(ref m);
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessage((IntPtr)ReadHandle(), WM_SYSCOMMAND, SC_RESTORE, 0);
        }    
        private void WriteReg()
        {
            // Create a subkey named Test9999 under HKEY_CURRENT_USER.
            RegistryKey test9999 =Registry.CurrentUser.CreateSubKey("LabelServer");
            // Create two subkeys under HKEY_CURRENT_USER\Test9999. The
            // keys are disposed when execution exits the using statement.
            using (RegistryKey  testSettings = test9999.CreateSubKey("Server"))
            {
                // Create data for the TestSettings subkey.
                testSettings.SetValue("handle", this.Handle);
            }
        }
        private IntPtr  ReadHandle()
        {
            // Create a subkey named Test9999 under HKEY_CURRENT_USER.
            RegistryKey test9999 =Registry.CurrentUser.OpenSubKey("LabelServer");
            // Create two subkeys under HKEY_CURRENT_USER\Test9999. The
            // keys are disposed when execution exits the using statement.
            using (RegistryKey  testSettings = test9999.OpenSubKey("Server"))
            {
                // Create data for the TestSettings subkey.
                return (IntPtr) Convert.ToInt32 ( testSettings.GetValue("handle").ToString());
            }
        }
        #endregion

        
    }
}
