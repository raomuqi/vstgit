


using System;


public class SyncObject : ProtoBase
{
    public int serverID=int.MaxValue;
    public int objectIndex = 0;
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    public void SetPos(UnityEngine.Vector3 pos)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
    }
    public void SetRot(UnityEngine.Quaternion rot)
    {
        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;
    }

    public UnityEngine.Vector3 GetPos()
    {
        return new UnityEngine.Vector3(posX, posY, posZ);
    }
    public UnityEngine.Quaternion GetRot()
    {
        return new UnityEngine.Quaternion(rotX, rotY, rotZ, rotW);
    }
    protected override byte[] OnSerialize()
    {
        byte[] serializeBuffer = new byte[36];
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
        temp= BitConverter.GetBytes(objectIndex);
        Array.Copy(temp, 0, serializeBuffer, 32, 4);
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
    }

    protected override void OnRecycle()
    {
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.SyncObject, this);
    }
}

