
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializeUtil 
{
    BinaryFormatter bf;
    MemoryStream memory;
    public SerializeUtil()
    {
        bf = new BinaryFormatter();
        memory = new MemoryStream();
    }
    public byte[] Serialize(object obj)
    {
        byte[] data = null;
        memory.Flush();
        memory.Position = 0;
        bf.Serialize(memory, obj);
        return memory.GetBuffer();
    }

    public object Deserialize(byte[] data)
    {
        memory.Flush();
        memory.Write(data, 0, data.Length);
        memory.Position = 0;
        object obj = bf.Deserialize(memory);
        return obj;
    }
    public object Deserialize(byte[] data,int index,int length)
    {
        memory.Flush();
        memory.Write(data, index, length);
        memory.Position = 0;
        object obj = bf.Deserialize(memory);
        return obj;
    }

    public void Dispose()
    {
        memory.Dispose();
    }
}
