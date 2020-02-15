


using System;
using System.Collections.Generic;
using System.IO;

public class SyncObject : ProtoBase
{
    public int objectID;
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;


    protected override byte[] OnSerialize()
    {
        byte[] serializeBuffer = new byte[32];
        byte[] temp = null;
       // 8 * 4
        temp = BitConverter.GetBytes(objectID);
        Array.Copy(temp, 0, serializeBuffer, 0, 4);
        temp = BitConverter.GetBytes(posX);
        Array.Copy(temp, 0, serializeBuffer, 4, 4);
        temp = BitConverter.GetBytes(posY);
        Array.Copy(temp, 0, serializeBuffer, 8, 4);
        temp = BitConverter.GetBytes(posZ);
        Array.Copy(temp, 0, serializeBuffer, 12, 4);
        temp = BitConverter.GetBytes(rotX);
        Array.Copy(temp, 0, serializeBuffer, 16, 4);
        temp = BitConverter.GetBytes(rotY);
        Array.Copy(temp, 0, serializeBuffer, 20, 4);
        temp = BitConverter.GetBytes(rotZ);
        Array.Copy(temp, 0, serializeBuffer, 24, 4);
        temp = BitConverter.GetBytes(rotW);
        Array.Copy(temp, 0, serializeBuffer, 28, 4);


        return serializeBuffer;
    }

    protected override void OnParse(byte[] data)
    {
        objectID = BitConverter.ToInt32(data, 0);
        posX = BitConverter.ToSingle(data, 4);
        posY = BitConverter.ToSingle(data, 8);
        posZ = BitConverter.ToSingle(data, 12);
        rotX = BitConverter.ToSingle(data, 16);
        rotY = BitConverter.ToSingle(data, 20);
        rotZ = BitConverter.ToSingle(data, 24);
        rotW = BitConverter.ToSingle(data, 28);
    }
}

