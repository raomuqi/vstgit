using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Util 
{
   public static void LogByte(byte[] data)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i] + " ");
        }
        Debug.Log(sb.ToString());
        Debug.Log(data.Length);
    }

    /// <summary>
    /// 序列化Proto
    /// </summary>
    public static byte[] SerializeProtoData(byte protoID, ProtoBase obj)
    {
        byte[] result = null;
        if (obj == null)
        {
            result = new byte[Connection.PACKER_OFFSET];
        }
        else
        {
            byte[] src = SerializeUtil.Serialize(obj);

            if (src != null)
            {
                result = new byte[src.Length + Connection.PACKER_OFFSET];
                System.Array.Copy(src, 0, result, Connection.PACKER_OFFSET, src.Length);
            }
            else
            {
                Debug.LogError("proto错误：" + protoID);
                return result;
            }
        }
        result[0] = Connection.PACKER_HEAD;
        result[1] = protoID;
        return result;
    }

    /// <summary>
    /// 直接发Byte[]
    /// </summary>
    public static byte[] SerializeProtoData(byte protoID, byte[] data)
    {
        byte[] result = null;
        if (data == null)
        {
            result = new byte[Connection.PACKER_OFFSET];
        }
        else
        {
            byte[] src = data;
            result = new byte[src.Length + Connection.PACKER_OFFSET];
            System.Array.Copy(src, 0, result, Connection.PACKER_OFFSET, src.Length);
        }
        result[0] = Connection.PACKER_HEAD;
        result[1] = protoID;
        return result;
    }
   
    /// <summary>
    /// 反序列
    /// </summary>
    public static T DeSerializeProto<T>( byte[] data, T d=null) where T:ProtoBase,new()
    {
        T result = d==null? new T(): d;
        if (!result.Parse(data))
        {
            return null;
        }
        return result;
    }
  
    /// <summary>
    /// 反序列UDP proto
    /// </summary>
    public static ProtoUdpWarp DeSerializeUDPProto(byte[] data ,ref ProtoUdpWarp udpWarp) 
    {
        if (!udpWarp.Parse(data))
        {
            return null;
        }
        return udpWarp;
    }

}
