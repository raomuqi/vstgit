
using System;

public class ProtoCreateObject : SyncObject
{
    public int hashCode = 0;
    protected override byte[] OnSerialize()
    {
        byte[] serializeBuffer = new byte[40];
        byte[] temp = null;
        // 4* 9
        temp = BitConverter.GetBytes(serverID);
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
        temp = BitConverter.GetBytes(objectIndex);
        Array.Copy(temp, 0, serializeBuffer, 32, 4);
        temp = BitConverter.GetBytes(hashCode);
        Array.Copy(temp, 0, serializeBuffer, 36, 4);
  
        return serializeBuffer;
    }

    protected override void OnParse(byte[] data)
    {
        serverID = BitConverter.ToInt32(data, 0);
        posX = BitConverter.ToSingle(data, 4);
        posY = BitConverter.ToSingle(data, 8);
        posZ = BitConverter.ToSingle(data, 12);
        rotX = BitConverter.ToSingle(data, 16);
        rotY = BitConverter.ToSingle(data, 20);
        rotZ = BitConverter.ToSingle(data, 24);
        rotW = BitConverter.ToSingle(data, 28);
        objectIndex = BitConverter.ToInt32(data, 32);
        hashCode = BitConverter.ToInt32(data, 36);
    
    }

    protected override void OnRecycle()
    {
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.CreateObject, this);
    }
}
