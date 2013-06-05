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

        /// ȫ��ת��ǵĺ���
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
            return MessageBox.Show(msg, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowError(string msg)
        {
            return MessageBox.Show(msg, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuery(string msg)
        {
            return MessageBox.Show(msg, "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowWarning(string msg)
        {
            return MessageBox.Show(msg, "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// �����ַ�������ȡ�ַ���������DataTable���ֶ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="field">�ֶ�</param>
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
        /// �����ַ���,��ȡ�ַ���������DataRow���ֶ�
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="field">�ֶ���</param>
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
        /// �����ַ�����ȡ���ַ���������String
        /// </summary>
        /// <param name="stringList">�ַ���</param>
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
