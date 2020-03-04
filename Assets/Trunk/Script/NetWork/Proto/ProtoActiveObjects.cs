using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoActiveObjects:ProtoBase
{
    public int[] serverIDs;
    public int[] objectIndexs;

    protected override byte[] OnSerialize()
    {
        if (serverIDs == null || serverIDs.Length < 1)
            return null;


        int length = serverIDs.Length;
        byte[] serializeBuffer = new byte[length*4*2];

        byte[] temp = null;
        for (int i = 0; i < length * 2; i++)
        {
            int data = i < length ? serverIDs[i] : objectIndexs[i-length];
            temp = BitConverter.GetBytes(data);
            Array.Copy(temp, 0, serializeBuffer,i*4, 4);
        }
        return serializeBuffer;
    }

    protected override void OnParse(byte[] data)
    {
        int length = data.Length / 4 / 2;
        //检测复用数组
        if (serverIDs == null || serverIDs.Length != length)
        {
            serverIDs = new int[length];
            objectIndexs = new int[length];
        }
        for (int i = 0; i < length; i++)
        {
            serverIDs[i] = BitConverter.ToInt32(data, i*4);
            objectIndexs[i] = BitConverter.ToInt32(data, i*4+length*4);
        }
      
    }

    protected override void OnRecycle()
    {
    }
}
