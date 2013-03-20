using System;
namespace ELDB2
{
    public interface IHashEntity
    {
        System.Collections.Hashtable PropertySet { get; }
        String TableName{ get; }
        String PkName { get; }
    }
}
