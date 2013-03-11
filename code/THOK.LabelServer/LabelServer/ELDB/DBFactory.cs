using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRabbit;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.ORM;

namespace ELDB2
{
    
    public class DBFactory
    {
        private static TransactionScopeFactory _TransactionScopeFactory = null;
        public static TransactionScopeFactory NewTransactionScopeFactory()
        {
            if (_TransactionScopeFactory == null)
            {
                DataConfiguration config = new DataConfiguration(DataBaseType.SqlServer, ".", "sa", "ynnuyonw", "Elinterface", null);
                TransactionScopeFactory factory = new TransactionScopeFactory();
                factory.DataConfiguration = config;
                factory.Initialize();
                _TransactionScopeFactory = factory;
            }
            return _TransactionScopeFactory;
        }
        public static TransactionScope NewTransactionScope(bool enlbleTrans)
        {
            //使用ReadUncommitted隔离级别 读取数据
            TransactionScope scope = NewTransactionScopeFactory().NewTransactionScope(enlbleTrans); 
            return scope;
        }
        public static IOrmAccesser<T> NewOrmAccesser<T>(TransactionScope scope)
        {
            IOrmAccesser<T> accesser = scope.NewOrmAccesser<T>();
            return accesser;
        }
    }
}
