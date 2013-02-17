using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.Zeng.ComfixtureHandle.el103
{
    public enum TextColor { Red = 0, Green = 1 }

    public enum ShowModel { ShowDevice = 0x10, OneBitShowDevice = 0x11, TwoBitShowDevice = 0x12, ThreeBitShowDevice = 0x13, FourBitShowDevice = 0x14, FiveBitShowDevice = 0x15 }

    public enum FlashState { Unflash = 0, Flash = 1, QuickFlash = 2 }

    public enum FlashModel { NotUsing = 0,GoOutImmediately = 1, GoOutWhenIsZero = 2, GoOut5sOfter = 3 }

    public enum FuntionState { Unchanging = 0, Start = 1, Close = 2 }
}
