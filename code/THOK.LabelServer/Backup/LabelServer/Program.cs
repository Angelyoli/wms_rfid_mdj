using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace THOK.Application.LabelServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Server());
        }
    }
}
