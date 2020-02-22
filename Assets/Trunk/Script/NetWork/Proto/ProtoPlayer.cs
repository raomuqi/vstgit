
using System;

public class ProtoPlayerInfo : ProtoBase
{
    public byte hp;
    public byte id;
    public byte connectStatus;//0 未登录 1在线 2掉线
    public byte pos;

    public int score;
    public int mapID = 0;

    protected override byte[] OnSerialize()
    {

        byte[] bscore= BitConverter.GetBytes(score);
        byte[] bmapID = BitConverter.GetBytes(mapID);

        byte[] reslut = new byte[4 + 2 * 4];
        reslut[0] = hp;
        reslut[1] = id;
        reslut[2] = connectStatus;
        reslut[3] = pos;

        reslut[4] = bscore[0];
        reslut[5] = bscore[1];
        reslut[6] = bscore[2];
        reslut[7] = bscore[3];

        reslut[8] = bmapID[0];
        reslut[9] = bmapID[1];
        reslut[10] = bmapID[2];
        reslut[11] = bmapID[3];

        return reslut;
    }

    protected override void OnParse(byte[] data)
    {
        hp = data[0];
        id = data[1];
        connectStatus = data[2];
        pos = data[3];
        score = BitConverter.ToInt32(data, 4);
        mapID = BitConverter.ToInt32(data, 8);
    }

    protected override void OnRecycle()
    {
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.PlayerInfo, this);
    }
}
