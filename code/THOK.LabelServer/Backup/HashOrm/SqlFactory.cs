using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DataRabbit.HashOrm
{
    enum SqlType { INSERT, UPDATE }
    class SqlFactory
    {
        private string tableName = "";
        private SqlType sqlType = SqlType.INSERT;

        private StringBuilder updateBuilder = new StringBuilder();
        private StringBuilder fieldBuilder = new StringBuilder();
        private StringBuilder valueBuilder = new StringBuilder();

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sqlType">新增/修改</param>
        public SqlFactory(string tableName, SqlType sqlType)
        {
            this.tableName = tableName;
            this.sqlType = sqlType;
            if (sqlType == SqlType.UPDATE)
                updateBuilder.AppendFormat("UPDATE {0} SET ", tableName);
        }

        /// <summary>
        /// 数字型
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">值</param>
        public void Append(string fieldName, object fieldValue)
        {
            if (sqlType == SqlType.UPDATE)
            {
                updateBuilder.AppendFormat("{0}={1},", fieldName, fieldValue);
            }
            else//INSERT
            {
                fieldBuilder.AppendFormat("{0},", fieldName);
                valueBuilder.AppendFormat("{0},", fieldValue);
            }
        }

        /// <summary>
        /// 字符型/日期型
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">值</param>
        public void AppendQuote(string fieldName, object fieldValue)
        {
            if (sqlType == SqlType.UPDATE)
            {
                updateBuilder.AppendFormat("{0}='{1}',", fieldName, fieldValue);
            }
            else//INSERT
            {
                fieldBuilder.AppendFormat("{0},", fieldName);
                valueBuilder.AppendFormat("'{0}',", fieldValue);
            }
        }

        /// <summary>
        /// 条件（数字型）
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">值</param>
        public void AppendWhere(string fieldName, object fieldValue)
        {
            if (sqlType == SqlType.UPDATE)
            {
                updateBuilder.Remove(updateBuilder.Length - 1, 1);
                updateBuilder.AppendFormat(" WHERE {0}{1}{2}", fieldName, "=", fieldValue);
            }
        }

        /// <summary>
        /// 条件（字符型/日期型）
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">值</param>
        public void AppendWhereQuote(string fieldName, object fieldValue)
        {
            if (sqlType == SqlType.UPDATE)
            {
                updateBuilder.Remove(updateBuilder.Length - 1, 1);
                updateBuilder.AppendFormat(" WHERE {0}{1}'{2}'", fieldName, "=", fieldValue);
            }
        }

        /// <summary>
        /// 取得完整的SQL语句
        /// </summary>
        /// <returns></returns>
        public string CreateSQL()
        {
            string field = fieldBuilder.ToString();
            string values = valueBuilder.ToString();
            return (sqlType == SqlType.UPDATE) ? updateBuilder.ToString() : string.Format("INSERT INTO {0}({1}) VALUES({2})", tableName, field.Substring(0, field.Length - 1), values.Substring(0, values.Length - 1));
        }
        public static string CreateSqlToSelect<TEntity>(IFilter filter, params string[] orderbys) where TEntity : new()
        {
            IHashEntity entity =(IHashEntity) new TEntity();
            string sql = "SELECT ";
            foreach (string colName in entity.ColNames)
            {
                sql = sql + colName + ",";
            }
            sql = sql.Remove(sql.Length - 1, 1) + " FROM {0}";
            sql = System.String.Format(sql, entity.TableName);

            if (filter != null)
            {
                sql = System.String.Format(sql + " where {0}", filter.GetExpression());
            }

            if (orderbys.Count() > 0)
            {
                string orderbyStr = "";
                foreach (string orderby in orderbys)
                {
                    orderbyStr = orderbyStr + orderby + ",";
                }

                sql = sql + " order by " + orderbyStr.Remove(orderbyStr.Length - 1, 1);
            }
            return sql;
        }
        public static string CreateSqlToUpdate(IHashEntity entity)
        {
            Hashtable htalbe = entity.PropertySet;
            if (htalbe[entity.PKeyName] == null)
            {
                throw new Exception("实体没有主键不能用此方法更新");
            }

            SqlFactory sql = new SqlFactory(entity.TableName, SqlType.UPDATE);            

            foreach (string colName in entity.ColNames)
            {
                if (colName != entity.PKeyName && htalbe[colName] != System.DBNull.Value && htalbe[colName] != null)
                {
                    switch (SqlFactory.GetColType(htalbe[colName].GetType()))
                    {
                        case 1:
                            sql.AppendQuote(colName, htalbe[colName]);
                            break;
                        case 2:
                            sql.Append(colName, htalbe[colName]);
                            break;
                        default:
                            sql.Append(colName, htalbe[colName]);
                            break;
                    }
                }
            }

            switch (SqlFactory.GetColType(htalbe[entity.PKeyName].GetType()))
            {
                case 1:
                    sql.AppendWhereQuote(entity.PKeyName, htalbe[entity.PKeyName]);
                    break;
                case 2:
                    sql.AppendWhere(entity.PKeyName, htalbe[entity.PKeyName]);
                    break;
                default:
                    sql.AppendWhere(entity.PKeyName, htalbe[entity.PKeyName]);
                    break;
            }
            return sql.CreateSQL();
        }
        public static int GetColType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                    return 1;
                case TypeCode.DateTime :
                    return 1;
                case TypeCode.Char:
                    return 1;
                default:
                    return 2;
            }
        }
    }
}
