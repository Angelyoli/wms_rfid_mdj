using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing.ORM;
using DataRabbit.DBAccessing.Relation;
using DataRabbit;

namespace DataRabbit.HashOrm
{
    class HashOrmAccesser : DataRabbit.HashOrm.IHashOrmAccesser 
    {
        private Hashtable _HashTable = new Hashtable();
        private TransactionScope _scope = null;
        private IRelationAccesser _accesser = null;

        public TransactionScope Scope
        {
            get 
            {
                if (_scope == null)
                {
                    _scope = DBFactory.NewTransactionScope(false);
                }
                return _scope; 
            }
        }

        public HashOrmAccesser() { }
        public HashOrmAccesser(TransactionScope Transcope)
        {
            if (Transcope == null)
            {
                throw new Exception("传入的TransactionScope为空");
            }          
            _scope = Transcope;
            if (_accesser == null)
            {
                _accesser = _scope.NewRelationAccesser();
            }
        }

        public Hashtable Select<TValue>(string colName, TValue colValue)
        {
            if (!_HashTable.ContainsKey(colName) || !((Hashtable)_HashTable[colName]).ContainsKey(colValue))
            {
                return  new Hashtable ();
            }
            return (Hashtable)((Hashtable)_HashTable[colName])[colValue];
        }
        public IList<TEntity> Select<TEntity, TValue>(string colName, TValue colValue) where TEntity : new()
        {
            List<TEntity> list = new List<TEntity>();
            Hashtable tb = this.Select<TValue>(colName, colValue);
            if (tb.Count > 0)
            {
                for (int i = 1; i <= tb.Count; i++)
                {
                    DataRow row = (DataRow)tb[i];
                    TEntity entity = new TEntity();
                    foreach (DataColumn col  in row.Table.Columns)
                    {
                        if ( row[col.ColumnName] != System.DBNull.Value)
                        {
                            ((IHashEntity)entity).PropertySet[col.ColumnName] = row[col.ColumnName];
                        }                        
                    }

                    list.Add(entity);
                }
            }
            return list;
        }

        public void AddTableThash(DataTable table)
        {
            _HashTable = TableToHash(table);
        }
        public void SelectToHash(string sql)
        {
            _HashTable = TableToHash(ExecuteWithQuery(sql));
        }
        public void SelectToHash<TEntity>(IFilter filter, params string[] orderbys) where TEntity : new()
        {
            string sql = SqlFactory.CreateSqlToSelect<TEntity>(filter, orderbys);
            _HashTable = TableToHash(ExecuteWithQuery(sql));
        }

        public DataTable EntityToTable(List<IHashEntity> entitys)
        {
            DataTable table = new DataTable();

            foreach (IHashEntity entity in entitys)
            {
                Hashtable htalbe = entity.PropertySet;
                DataRow row = table.NewRow();
                foreach (string colName in htalbe.Keys)
                {
                    if (!table.Columns.Contains(colName))
                    {
                        table.Columns.Add(colName);
                    }
                    row[colName] = htalbe[colName];
                }
                table.Rows.Add(row);
            }
            return table;

        }
        public Hashtable TableToHash(DataTable table)
        {
            Hashtable _HashTable = new Hashtable();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (!_HashTable.ContainsKey(col.ColumnName))
                        {
                            Hashtable t1 = new Hashtable();
                            _HashTable.Add(col.ColumnName, t1);
                        }

                        if (!((Hashtable)_HashTable[col.ColumnName]).ContainsKey(row[col]))
                        {
                            Hashtable t3 = new Hashtable();
                            ((Hashtable)_HashTable[col.ColumnName]).Add(row[col], t3);
                        }

                        Hashtable t4 = (Hashtable)(_HashTable[col.ColumnName]);
                        t4 = (Hashtable)(t4[row[col]]);
                        t4.Add(t4.Count + 1, row);
                    }
                }
            }
            return _HashTable;
        }
        public IList<T> HashToList<T>(Hashtable htable)
        {
            List<T> list = new List<T>();
            if (htable.Count > 0)
            {
                for (int i = 1; i <= htable.Count; i++)
                {
                    list.Add((T)htable[i]);
                }
            }
            return list;
        }

        public void Insert(IHashEntity entity, bool PkeyIsIdentity)
        {
            SqlFactory sql = new SqlFactory(entity.TableName, SqlType.INSERT);

            Hashtable htalbe = entity.PropertySet;

            foreach (string colName in entity.ColNames)
            {
                if (!PkeyIsIdentity || colName != entity.PKeyName)
                {
                    if (htalbe[colName] != null)
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
            }

            ExecuteNonQuery(sql.CreateSQL());
        }
        public void Insert<TEntity>(IList<TEntity> entitys, bool PkeyIsIdentity)
        {
            foreach (IHashEntity entity in entitys)
            {
                Insert(entity, PkeyIsIdentity);
            }
        }

        public void Delete(IHashEntity entity)
        {
            string sql;
            switch (SqlFactory.GetColType(entity.PropertySet[entity.PKeyName].GetType()))
            {
                case 1:
                    sql = "DELETE * FROM [{0}] WHERE {1} = '{2}'";
                    break;
                case 2:
                    sql = "DELETE * FROM [{0}] WHERE {1} = {2}";
                    break;
                default:
                    sql = "DELETE * FROM [{0}] WHERE {1} = '{2}'";
                    break;
            }
           
            ExecuteNonQuery(string.Format(sql,entity.TableName, entity.PKeyName, entity.PropertySet[entity.PKeyName]));
        }
        public void Delete<TEntity>(IList<TEntity> entitys)
        {
            foreach (IHashEntity entity in entitys)
            {
                Delete(entity);
            }
        }
        public void Delete<TEntity>(IFilter filter) where TEntity : new()
        {
            IHashEntity entity =(IHashEntity) new TEntity();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE * FROM [{0}] ", entity.TableName);

            if (filter != null)
            {
                strSql.AppendFormat(" where {0}", filter.GetExpression());
            }
            ExecuteNonQuery(strSql.ToString());
        }

        public void Update(IHashEntity entity)
        {
            string sql = SqlFactory.CreateSqlToUpdate(entity);
            ExecuteNonQuery(sql);
        }
        public void Update<TEntity>(IList<TEntity> entitys)
        {
            foreach (IHashEntity  entity in entitys)
            {
                Update(entity);
            }
        }
        public void Update(IHashEntity entity, IFilter filter)
        {
            Hashtable htalbe = entity.PropertySet;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET ", entity.TableName);

            foreach (string colName in entity.ColNames)
            {
                if (colName != entity.PKeyName && htalbe[colName] != System.DBNull.Value && htalbe[colName] != null)
                {
                    switch (SqlFactory.GetColType(htalbe[colName].GetType()))
                    {
                        case 1:
                            strSql.AppendFormat("{0}='{1}',", colName, htalbe[colName]);
                            break;
                        case 2:
                            strSql.AppendFormat("{0}={1},", colName, htalbe[colName]);
                            break;
                        default:
                            strSql.AppendFormat("{0}='{1}',", colName, htalbe[colName]);
                            break;
                    }
                }
            }

            strSql.Remove(strSql.Length - 1, 1);

            if (filter != null)
            {
                strSql.AppendFormat(" where {0}", filter.GetExpression());
            }
            ExecuteNonQuery(strSql.ToString());
        }

        public IList<TEntity> Select<TEntity>(string sql) where TEntity : new()
        {
            List<TEntity> list = new List<TEntity>();

            DataTable table = ExecuteWithQuery(sql);

            foreach (DataRow row in table.Rows)
            {
                TEntity entity = new TEntity();
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (row[col.ColumnName] != System.DBNull.Value)
                    {
                        ((IHashEntity)entity).PropertySet[col.ColumnName] = row[col.ColumnName];
                    }
                }
                list.Add(entity);
            }
            return list;
        }
        public IList<TEntity> Select<TEntity>(IFilter filter,params string [] orderbys) where TEntity : new()
        {
            string sql = SqlFactory.CreateSqlToSelect<TEntity>(filter, orderbys);
            return Select<TEntity>(sql);
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                _accesser.DoCommand(sql);
            }
            catch (Exception e)
            {
                
                throw new Exception (sql + e.Message ,e);
            }

        }
        public DataTable ExecuteWithQuery(string sql)
        {
            try
            {
                return _accesser.DoQuery(sql).Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception(sql + e.Message, e);
            }

        }
    }
}
