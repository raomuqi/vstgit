using System;

[Serializable]
public class ProtoPlayerInfo : ProtoBase
{
    public byte hp;
    public byte id;
    public int score;
    public int connectStatus;//0 未登录 1在线 2掉线
    public int mapID = 0;
    public int pos;
}
