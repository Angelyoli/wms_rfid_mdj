using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace THOK.Zeng.ComfixtureHandle.el102
{
    enum RecvType
    {
        Correct,
        CheckError,
        /// <summary>
        /// ��ʱ
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

