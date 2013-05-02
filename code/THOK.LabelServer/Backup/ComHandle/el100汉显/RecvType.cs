using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace THOK.Zeng.ComfixtureHandle.el100
{
    enum RecvType
    {
        Correct,
        CheckError,
        /// <summary>
        /// ³¬Ê±
        /// </summary>
        Timeout,
        Others,
        NotExist,
        KeyEvent,
        EndError,
        SRamFull,
        DeleteTable,
        ResetCorrect,
        ResetError
    }
}

