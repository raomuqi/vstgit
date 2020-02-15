

using System;

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public  class ProtoBase
{
    static MemoryStream _serializeBuffer;
    static MemoryStream serializeBuffer
    {
        get
        {
            if (_serializeBuffer == null)
                _serializeBuffer = new MemoryStream();
            return _serializeBuffer;
        }
    }
    public byte[] Serialize()
    {
        try
        {
            return OnSerialize();
         }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            return null;
        }
    }

    public bool Parse(byte[] data)
    {
        try
        {
            OnParse(data);
            return true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            return false;
        }
    }
    /// <summary>
    /// 必须每个组员长度一样
    /// </summary>
    /// <returns></returns>
    protected  byte[] ArraySerialize<T>(ProtoBase[] array) where T :ProtoBase
    {
        byte[] result = null;
        if (array != null && array.Length > 0)
        {
            int count = array.Length;
            byte[] bcout= BitConverter.GetBytes(count);
            byte[] firstData = array[0].Serialize();
            int singleLength = firstData.Length;
            byte[] blength = BitConverter.GetBytes(singleLength);

            result = new byte[singleLength * count + 8];
            
            Array.Copy(bcout,0, result, 0, 4);
            Array.Copy(blength, 0, result, 4, 4);
            Array.Copy(firstData, 0, result, 8, singleLength);
            if (count > 1)
            {
                for (int i = 1; i < array.Length; i++)
                {
                    T obj = array[i] as T;
                    byte[] curData = obj.Serialize();
                    Array.Copy(curData, 0, result, 8 + i* singleLength, singleLength);

                }
            }
          
          
        }
        return result;
    }

    protected T[] ArrayDeSerializem<T>(byte[] array,ProtoPool.ProtoRecycleType poolType= ProtoPool.ProtoRecycleType.None) where T : ProtoBase,new()
    {
        T[] result = null;
      
        int count = BitConverter.ToInt32(array, 0);
        int slenght=BitConverter.ToInt32(array, 4);
        result = new T[count];
        byte[] singleProto = new byte[slenght];
        for (int i = 0; i < count; i++)
        {
            Array.Copy(array, 8+i* slenght, singleProto, 0, slenght);
            T proto;
            if (poolType != ProtoPool.ProtoRecycleType.None)
                proto = ObjectPool.protoPool.GetOrCreate<T>(poolType);
            else
                proto = new T();
            proto.OnParse(singleProto);
            result[i] = proto;
        }
            
        return result;
    }
   
    protected virtual byte[] OnSerialize()
    {
        byte[] temp = null;
        return temp;
    }
    protected  virtual void OnParse(byte[] data)
    {


    }

 

}