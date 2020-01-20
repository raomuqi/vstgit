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
    public static byte[] SerializeProtoData(byte protoID, object obj)
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
        }
        result[0] = Connection.PACKER_HEAD;
        result[1] = protoID;
        return result;
    }

}
