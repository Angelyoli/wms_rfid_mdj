using System;
namespace DataRabbit.HashOrm
{
    public interface IHashOrmAccesser
    {
        void AddTableThash(System.Data.DataTable table);
        void Delete<TEntity>(System.Collections.Generic.IList<TEntity> entitys);
        void Delete(IHashEntity entity);
        System.Data.DataTable EntityToTable(System.Collections.Generic.List<IHashEntity> entitys);
        void ExecuteNonQuery(string sql);
        System.Data.DataTable ExecuteWithQuery(string sql);
        System.Collections.Generic.IList<T> HashToList<T>(System.Collections.Hashtable htable);
        void Insert<TEntity>(System.Collections.Generic.IList<TEntity> entitys, bool PkeyIsIdentity);
        void Insert(IHashEntity entity, bool PkeyIsIdentity);
        DataRabbit.DBAccessing.Application.TransactionScope Scope { get; }
        System.Collections.Hashtable Select<TValue>(string colName, TValue colValue);
        System.Collections.Generic.IList<TEntity> Select<TEntity>(IFilter filter, params string[] orderbys) where TEntity : new();
        System.Collections.Generic.IList<TEntity> Select<TEntity>(string sql) where TEntity : new();
        System.Collections.Generic.IList<TEntity> Select<TEntity, TValue>(string colName, TValue colValue) where TEntity : new();
        void SelectToHash<TEntity>(IFilter filter, params string[] orderbys) where TEntity : new();
        void SelectToHash(string sql);
        System.Collections.Hashtable TableToHash(System.Data.DataTable table);
        void Update(IHashEntity entity);
        void Update<TEntity>(System.Collections.Generic.IList<TEntity> entitys);
    }
}
