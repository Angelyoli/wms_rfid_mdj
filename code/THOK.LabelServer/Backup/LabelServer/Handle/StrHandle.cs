using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Application.LabelServer
{
    class StrHandle
    {
        public static String GetStringWith(String str, int length)
        {
            str = str.PadRight(length, " "[0]);
            char [] strs = str.ToCharArray();
            str = "";
            int i = 0;  
            foreach (char s in strs)
            {
                str = str + s.ToString();
                i = i + Encoding.Default.GetByteCount(s.ToString());
                if (i == length || i == length -1 )
                {
                    str = str.Substring(0, str.Length  - 2) + "    ";
                    break;
                }
            }

            str = str.PadRight(length, " "[0]);

            int bytecount = Encoding.Default.GetByteCount(str);
            int strlength = str.Length;
            int zh_count = bytecount - strlength;

            str = str.Substring(0, length - zh_count);

            return str;
        }
    }
}
