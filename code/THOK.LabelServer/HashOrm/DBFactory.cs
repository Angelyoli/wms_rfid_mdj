using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRabbit;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.ORM;
using DataRabbit.DBAccessing.Relation;
using System.Collections;

namespace DataRabbit.HashOrm
{
    public sealed class DBFactory
    {
        private static readonly object lock1 = new object();
        private static readonly object lock2 = new object();
        private static TransactionScopeFactory _TransactionScopeFactory = null;
        private static TransactionScope _TransactionScope = null;
        private static Hashtable Accessers = new Hashtable();
        private static Hashtable _TransactionScopeFactorys = new Hashtable();

        /// <summary>
        /// 实现全局单件的事务空间工厂
        /// </summary>
        /// <returns>事务空间工厂</returns>
        private static TransactionScopeFactory NewTransactionScopeFactory()
        {
            if (_TransactionScopeFactory == null)
            {
                lock (lock1)
                {
                    if (_TransactionScopeFactory == null)
                    {
                        DataConfiguration config = (new DataConfig()).CreateConfig();
                        TransactionScopeFactory factory = new TransactionScopeFactory();
                        factory.DataConfiguration = config;
                        factory.Initialize();
                        _TransactionScopeFactory = factory;
                    }
                }
            }
            return _TransactionScopeFactory;
        }
        public static TransactionScopeFactory NewTransactionScopeFactory(string key)
        {
            if (_TransactionScopeFactorys[key] == null)
            {
                DataConfiguration config = (new DataConfig(@".\" + key + ".xml")).CreateConfig();
                TransactionScopeFactory factory = new TransactionScopeFactory();
                factory.DataConfiguration = config;
                factory.Initialize();
                _TransactionScopeFactorys[key] = factory;
            }
            return (TransactionScopeFactory)_TransactionScopeFactorys[key];
        }


        /// <summary>
        /// 实现无事务状态管理的全局单件 事务空间 TransactionScope
        /// 实现有事务状态管理 事务空间 TransactionScope 的构造器
        /// </summary>
        /// <param name="enlbleTrans">是否使用事务管理，如不使用则统一强制使用全局单件 事务空间。</param>
        /// <returns>事务空间对象</returns>
        public static TransactionScope NewTransactionScope(bool enlbleTrans)
        {
            TransactionScope scope = null;
            if (enlbleTrans)
            {
                scope = NewTransactionScopeFactory().NewTransactionScope(enlbleTrans);
            }
            else
            {
                if (_TransactionScope == null)
                {
                    lock (lock2)
                    {
                        if (_TransactionScope == null)
                        {
                            _TransactionScope = NewTransactionScopeFactory().NewTransactionScope(enlbleTrans);
                        }                        
                    }
                }
                scope = _TransactionScope;
            }
            return scope;
        }

        #region 使用特定事务空间进行事务管理
        public static IOrmAccesser<TEntity> NewOrmAccesser<TEntity>(TransactionScope scope)
        {
            IOrmAccesser<TEntity> accesser = scope.NewOrmAccesser<TEntity>();
            return accesser;
        }
        public static IRelationAccesser NewRelationAccesser(TransactionScope scope)
        {
            IRelationAccesser accesser = scope.NewRelationAccesser();
            return accesser;
        }
        public static ISPAccesser NewSPAccesser(TransactionScope scope)
        {
            ISPAccesser accesser = scope.NewSPAccesser();
            return accesser;
        }
        public static IHashOrmAccesser NewHashOrmAccesser(TransactionScope scope)
        {
            IHashOrmAccesser accesser = new HashOrmAccesser(scope);
            return accesser;
        }       
        #endregion

        #region 使用全局单件无事务状态事务空间进行事务管理
        public static IOrmAccesser<TEntity> NewOrmAccesser<TEntity>()
        {
            IOrmAccesser<TEntity> accesser = NewTransactionScope(false).NewOrmAccesser<TEntity>();
            return accesser;
        }
        public static IRelationAccesser NewRelationAccesser()
        {
            IRelationAccesser accesser = NewTransactionScope(false).NewRelationAccesser();
            return accesser;
        }
        public static ISPAccesser NewSPAccesser()
        {
            ISPAccesser accesser = NewTransactionScope(false).NewSPAccesser();
            return accesser;
        }
        public static IHashOrmAccesser NewHashOrmAccesser()
        {
            IHashOrmAccesser accesser = new HashOrmAccesser(NewTransactionScope(false));
            return accesser;
        }        
        #endregion

        #region 使用全局单件无事务状态事务空间创建全局单件的访问器
        public static IOrmAccesser<TEntity> GetOnlyOrmAccesser<TEntity>(string entityName)
        {
            if (Accessers[entityName] == null)
            {
                Accessers[entityName] = NewTransactionScope(false).NewOrmAccesser<TEntity>();
            }

            return (IOrmAccesser<TEntity>)Accessers[entityName];
        }
        public static IRelationAccesser GetOnlyRelationAccesser()
        {
            if (Accessers["IRelationAccesser"] == null)
            {
                Accessers["IRelationAccesser"] = NewTransactionScope(false).NewRelationAccesser();
            }
            return (IRelationAccesser)Accessers["IRelationAccesser"];
        }
        public static ISPAccesser GetOnlySPAccesser()
        {
            if (Accessers["ISPAccesser"] == null)
            {
                Accessers["ISPAccesser"] = NewTransactionScope(false).NewSPAccesser();
            }
            return (ISPAccesser)Accessers["ISPAccesser"];
        }
        #endregion


    }
}
