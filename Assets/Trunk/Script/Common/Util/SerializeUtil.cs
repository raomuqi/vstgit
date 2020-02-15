
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SerializeUtil 
{
    public static byte[] Serialize(ProtoBase proto)
    {
        return proto.Serialize();
    }
    public static byte[] GetContextData(byte[] data, int index, int length)
    {
        byte[] context = new byte[length];
        System.Array.Copy(data, index, context, 0, length);
        return context;
    }
    //static BinaryFormatter bf;
    //static  MemoryStream memory;


    //public static byte[] Serialize(object obj)
    //{
    //    if(bf==null)
    //        bf = new BinaryFormatter();
    //    if(memory==null)
    //        memory = new MemoryStream();
    //    byte[] data = null;
    //    memory.Flush();
    //    memory.Position = 0;
    //    try
    //    {
    //        bf.Serialize(memory, obj);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e.Message);
    //    }
    //    return memory.GetBuffer();
    //}

    //public static object Deserialize(byte[] data)
    //{
    //    if (bf == null)
    //        bf = new BinaryFormatter();
    //    if (memory == null)
    //        memory = new MemoryStream();
    //    memory.Flush();
    //    memory.Write(data, 0, data.Length);
    //    memory.Position = 0;
    //    object obj = null;
    //    try
    //    {
    //        obj = bf.Deserialize(memory);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e.Message);
    //    }
    //    return obj;
    //}



    //public static object Deserialize(byte[] data,int index,int length)
    //{
    //    if (bf == null)
    //        bf = new BinaryFormatter();
    //    if (memory == null)
    //        memory = new MemoryStream();
    //    memory.Flush();
    //    memory.Position = 0;
    //    memory.Write(data, index, length);
    //    memory.Position = 0;
    //    object obj = null;
    //    try
    //    {
    //        obj = bf.Deserialize(memory);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e.Message);
    //    }
    //    return obj;
    //}

    //public static void Dispose()
    //{
    //    if (memory != null)
    //    {
    //        memory.Dispose();
    //        memory = null;
    //    }
    //    if (bf != null)
    //    {
    //        bf = null;
    //    }
    //}
}
