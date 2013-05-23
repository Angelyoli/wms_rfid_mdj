using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace THOK.Common
{
    public static class Obj
    {
        public static TEntity Clone<TEntity>(this TEntity o) where TEntity : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            stream.Position = 0;
            return (TEntity)formatter.Deserialize(stream);
        }
    }
}
