using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace THOK.PDA.Util
{
    public class WaitCursor
    {
        public static void Set()
        {
            W32Wrapper.SetWaitCursor();
        }
                     
        public static void Restore()
        {
            W32Wrapper.Restore();
        }
    }

    internal class W32Wrapper
    {
        [DllImport("W32DLL.dll")]
        internal static extern void SetWaitCursor();

        [DllImport("W32DLL.dll")]
        internal static extern void Restore();
    }
}
