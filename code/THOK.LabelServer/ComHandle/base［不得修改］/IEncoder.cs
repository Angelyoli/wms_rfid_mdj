using System;
namespace THOK.Zeng.ComfixtureHandle
{
    internal interface IEncoder
    {
        object Decode(string Code);
        byte[] Encode(object CMD);
    }
}
