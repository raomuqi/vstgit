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

    public static  byte[] SerializeProtoData(object obj)
    {
        byte[] result = null;
        byte[] src = SerializeUtil.Serialize(obj);
        if (src != null)
        {
            result = new byte[src.Length + 2];
            System.Array.Copy(src, 0, result, 2, src.Length);
            src[0] = Connection.PACKER_HEAD;
            src[1] = 1;
        }

        return result;

    }
}
