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

namespace ELDB2
{
    public class HashSelectHandle
    {
        private Hashtable _HashTable = new Hashtable();

        public HashSelectHandle(DataTable table)
        {
            AddToHash(table);
        }

        public HashSelectHandle()
        {
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
                        ((ELDB2.IHashEntity)entity).PropertySet[col.ColumnName] = row[col.ColumnName];
                    }

                    list.Add(entity);
                }
            }
            return list;
        }
        private void AddToHash(DataTable table)
        {
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
        }
        public IList<TEntity> Select<TEntity>(string sql,bool isUsedHash) where TEntity : new()
        {
            List<TEntity> list = new List<TEntity>();
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IRelationAccesser r = scope.NewRelationAccesser();
                DataTable table = r.DoQuery(sql).Tables[0];
                if (isUsedHash)
                {
                    AddToHash(table);
                }
                foreach (DataRow row  in table.Rows)
                {
                    TEntity entity = new TEntity();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        ((ELDB2.IHashEntity)entity).PropertySet[col.ColumnName] = row[col.ColumnName];
                    }

                    list.Add(entity);
                }

                scope.Commit();
            }
            return list;
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
        public void Update<TEntity,TKey>(TEntity entity)
        {
            IHashEntity ihe = (IHashEntity)entity;
            DataRabbit.IEntity<TKey> ien = (DataRabbit.IEntity<TKey> )entity;
            string SQL = String.Format("UPDATE [{0}] SET ",ihe.TableName);
            Hashtable htalbe = ihe.PropertySet;
            string KeyName = "";
            int i = 0;
            foreach (string colName in htalbe.Keys)
            {
                if (!htalbe[colName].Equals(ien.GetPKeyValue()))
                {
                    SQL = SQL + String.Format("[{0}] = '{1}'", colName, htalbe[colName]);
                    if (i < htalbe.Count - 2)
                    {
                        SQL = SQL + " , ";
                    }
                    i++;
                }
                else
                {
                    KeyName = colName;
                }
            }
            if (KeyName != "")
            {
                SQL = SQL + String.Format("WHERE [{0}] = '{1}'", KeyName, ien.GetPKeyValue());
            }
            else
            {
                throw new Exception("未设置主建，不能用此方法更新对像！请检查！");
            }
            ExecuteNonQuery(SQL);
        }
        public void ExecuteNonQuery(string sql)
        {
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IRelationAccesser r = scope.NewRelationAccesser();
                r.DoCommand(sql);
                scope.Commit();
            }
        }
    }
}
