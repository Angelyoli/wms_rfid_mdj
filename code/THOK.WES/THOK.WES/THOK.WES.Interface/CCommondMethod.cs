using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WES.Interface
{
    class CCommondMethod
    {
        /// <summary>
        /// 字符串转16进制数组，字符串以空格分割
        /// </summary>
        /// <param name="strHexValue"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string strHexValue)
        {
            string[] strAryHex = strHexValue.Split(' ');
            byte[] btAryHex = new byte[strAryHex.Length];

            try
            {
                int nIndex = 0;
                foreach (string strTemp in strAryHex)
                {
                    btAryHex[nIndex] = Convert.ToByte(strTemp, 16);
                    nIndex++;
                }
            }
            catch (System.Exception ex)
            {

            }

            return btAryHex;
        }

        /// <summary>
        /// 字符数组转为16进制数组
        /// </summary>
        /// <param name="strAryHex"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public static byte[] StringArrayToByteArray(string[] strAryHex, int nLen)
        {
            if (strAryHex.Length < nLen)
            {
                nLen = strAryHex.Length;
            }

            byte[] btAryHex = new byte[nLen];

            try
            {
                int nIndex = 0;
                foreach (string strTemp in strAryHex)
                {
                    btAryHex[nIndex] = Convert.ToByte(strTemp, 16);
                    nIndex++;
                }
            }
            catch (System.Exception ex)
            {

            }

            return btAryHex;
        }

        /// <summary>
        /// 16进制字符数组转成字符串
        /// </summary>
        /// <param name="btAryHex"></param>
        /// <param name="nIndex"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] btAryHex, int nIndex, int nLen)
        {
            if (nIndex + nLen > btAryHex.Length)
            {
                nLen = btAryHex.Length - nIndex;
            }

            string strResult = string.Empty;

            for (int nloop = nIndex; nloop < nIndex + nLen; nloop++)
            {
                string strTemp = string.Format("{0:X2}", btAryHex[nloop]);

                strResult += strTemp;
            }

            return strResult;
        }

        /// <summary>
        /// 将字符串按照指定长度截取并转存为字符数组，空格忽略
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="nLength"></param>
        /// <returns></returns>
        public static string[] StringToStringArray(string strValue, int nLength)
        {
            string[] strAryResult = null;

            if (!string.IsNullOrEmpty(strValue))
            {
                System.Collections.ArrayList strListResult = new System.Collections.ArrayList();
                string strTemp = string.Empty;
                int nTemp = 0;

                for (int nloop = 0; nloop < strValue.Length; nloop++)
                {
                    if (strValue[nloop] == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        nTemp++;

                        //校验截取的字符是否在A~F、0~9区间，不在则直接退出
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^(([A-F])*(\d)*)$");
                        if (!reg.IsMatch(strValue.Substring(nloop, 1)))
                        {
                            return strAryResult;
                        }

                        strTemp += strValue.Substring(nloop, 1);

                        //判断是否到达截取长度
                        if ((nTemp == nLength) || (nloop == strValue.Length - 1 && !string.IsNullOrEmpty(strTemp)))
                        {
                            strListResult.Add(strTemp);
                            nTemp = 0;
                            strTemp = string.Empty;
                        }
                    }
                }

                if (strListResult.Count > 0)
                {
                    strAryResult = new string[strListResult.Count];
                    strListResult.CopyTo(strAryResult);
                }
            }

            return strAryResult;
        }
    }
}
