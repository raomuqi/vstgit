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
}
