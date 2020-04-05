using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    public static void DebugDraw(this Bounds bounds, Matrix4x4 worldMat, Color color)
    {
        DebugDrawBox(bounds.ToWorldVertexs(worldMat), color);
    }
    public static Vector3[] ToWorldVertexs(this Bounds bounds, Matrix4x4? worldMat)
    {
        Vector3[] vs = bounds.ToVertexs();
        if (worldMat.HasValue)
        {
            Matrix4x4 mat = worldMat.Value;
            for (int i = 0; i < 8; i++)
            {
                vs[i] = mat.MultiplyPoint(vs[i]);
            }
        }
        return vs;
    }
    public static void DebugDraw(this Bounds bounds, Color color)
    {
        DebugDrawBox(bounds.ToVertexs(), color);
    }
    public static Vector3[] ToVertexs(this Bounds bounds)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        return new Vector3[8]
        {
        new Vector3(min.x, min.y, min.z),
        new Vector3(min.x, min.y, max.z),
        new Vector3(max.x, min.y, max.z),
        new Vector3(max.x, min.y, min.z),
        new Vector3(min.x, max.y, min.z),
        new Vector3(min.x, max.y, max.z),
        new Vector3(max.x, max.y, max.z),
        new Vector3(max.x, max.y, min.z)
        };
    }


    private static void DebugDrawBox(Vector3[] vs, Color color)
    {
        Debug.DrawLine(vs[0], vs[1], color);
        Debug.DrawLine(vs[1], vs[2], color);
        Debug.DrawLine(vs[2], vs[3], color);
        Debug.DrawLine(vs[3], vs[0], color);
        Debug.DrawLine(vs[4], vs[5], color);
        Debug.DrawLine(vs[5], vs[6], color);
        Debug.DrawLine(vs[6], vs[7], color);
        Debug.DrawLine(vs[7], vs[4], color);
        Debug.DrawLine(vs[0], vs[4], color);
        Debug.DrawLine(vs[1], vs[5], color);
        Debug.DrawLine(vs[2], vs[6], color);
        Debug.DrawLine(vs[3], vs[7], color);
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
            result.Recycle();
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
    /// <summary>
    /// 复制实例
    /// </summary>
    /// <returns></returns>
    public static object CopyInstance(object tIn)
    {
      byte[] sdata=  ObjectToBytes(tIn);
        return BytesToObject(sdata); 
    }
    public static byte[] ObjectToBytes(object obj)
    {
         var   bf = new BinaryFormatter();
        var memory = new MemoryStream();
        byte[] data = null;
        memory.Flush();
        memory.Position = 0;
        bf.Serialize(memory, obj);
        return memory.GetBuffer();
    }

    public static object BytesToObject(byte[] data)
    {
         var   bf = new BinaryFormatter();
        var    memory = new MemoryStream();
        memory.Flush();
        memory.Write(data, 0, data.Length);
        memory.Position = 0;
        object obj = null;
            obj = bf.Deserialize(memory);
        return obj;
    }
}
