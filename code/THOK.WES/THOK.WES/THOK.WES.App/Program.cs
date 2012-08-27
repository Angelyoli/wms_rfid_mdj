using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace THOK.WES.App
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            THOK.AF.Config config = new THOK.AF.Config();
            THOK.AF.MainFrame mainFrame = new THOK.AF.SDIFrame();
            mainFrame.InitializeFrame(config);
            mainFrame.Icon = new Icon(@"./Image/App.ico");
            //mainFrame.ControlBox = false;
            mainFrame.WindowState = FormWindowState.Maximized;
            Application.Run(mainFrame);   
        }
    }
}