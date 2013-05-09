using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.WMS.DownloadWms
{
   public class UtinString
    {

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
                    list += "'" + row["" + field + ""].ToString() + "',";
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
        public static string MakeString(DataTable dt, string field)
        {
            string list = "''";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list += ",'" + row["" + field + ""].ToString() + "'";
                }
            }
            return list;
        }

        /// <summary>
        /// �����ַ���,��ȡ�ַ���������DataRow���ֶ�
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="field">�ֶ���</param>
        /// <returns></returns>
        public static string MakeString(DataRow[] dr, string field)
        {
            string list = "''";
            if (dr.Length != 0)
            {
                foreach (DataRow row in dr)
                {
                    list += ",'" + row["" + field + ""].ToString() + "'";
                }
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
                    list += "'" + row["" + field + ""].ToString() + "',";
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
            for (int i = 0; i < arraryList.Length; i++)
            {
                list += ",'" + arraryList[i] + "'";
            }
            return list;
        }
    }
}
