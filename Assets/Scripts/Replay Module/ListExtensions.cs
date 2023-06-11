using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ListExtensions
{
    public static List<T> DeepCopy<T>(this List<T> source)
    {
        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Position = 0;
            return (List<T>) formatter.Deserialize(stream);
        }
    }
}