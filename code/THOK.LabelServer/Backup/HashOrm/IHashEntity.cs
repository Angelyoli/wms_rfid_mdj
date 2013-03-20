using System;
namespace DataRabbit.HashOrm
{
    public interface IHashEntity
    {
        System.Collections.Hashtable PropertySet { get; }
        String TableName { get; }
        String PKeyName { get; }
        String[] ColNames { get; }
    }
}
