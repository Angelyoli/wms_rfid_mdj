using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace THOK.WES
{
    public class THOKUtil
    {
        public static string ToDBC(string s)
        {
            char[] c = s.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = ToDBC(c[i]);
            }
            return new string(c);
        }

        /// 全角转半角的函数
        public static char ToDBC(char c)
        {
            if (c == 12288)
                return (char)32;
            else if (c > 65280 && c < 65375)
                return (char)(c - 65248);
            else
                return c;
        }

        public static DialogResult ShowInfo(string msg)
        {
            return MessageBox.Show(msg, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowError(string msg)
        {
            return MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuery(string msg)
        {
            return MessageBox.Show(msg, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowWarning(string msg)
        {
            return MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void EnableFilter(DataGridView gridView)
        {
            if (gridView.DataSource is BindingSource)
                ((BindingSource)gridView.DataSource).Filter = "";

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                if (column is DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn)
                    ((DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn)column).FilteringEnabled = true;
            }
        }

        /// <summary>
        /// 处理字符串，截取字符，传来的DataTable和字段
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static string StringMake(DataTable dt, string field)
        {
            string list = "";
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list += row["" + field + ""].ToString() + ",";
                }
                list = list.Substring(0, list.Length - 1);
            }
            return list;
        }

        /// <summary>
        /// 处理字符串,截取字符，传来的DataRow和字段
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public static string StringMake(DataRow[] dr, string field)
        {
            string list = "";
            if (dr.Length != 0)
            {
                foreach (DataRow row in dr)
                {
                    list += row["" + field + ""].ToString() + ",";
                }
                list = list.Substring(0, list.Length - 1);
            }
            return list;
        }

        /// <summary>
        /// 处理字符串，取得字符，传来的String
        /// </summary>
        /// <param name="stringList">字符串</param>
        /// <returns></returns>
        public static string StringMake(string stringList)
        {
            string list = "''";
            string[] arraryList = stringList.Split(',');
            //if(stringList.Equals(""))
            for (int i = 0; i < arraryList.Length; i++)
            {
                list += ",'" + arraryList[i] + "'";
            }
            return list;
        }

    }
}
