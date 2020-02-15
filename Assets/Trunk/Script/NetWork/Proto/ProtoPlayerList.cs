

using UnityEngine;

public class ProtoPlayerList :ProtoBase
{
    public ProtoPlayerInfo[] players;

    protected override byte[] OnSerialize()
    {
       return ArraySerialize<ProtoPlayerInfo>(players);
    }

    protected override void OnParse(byte[] data)
    {
        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.PlayerInfo,players[i]);
            }
        }
        players = ArrayDeSerializem<ProtoPlayerInfo>(data,ProtoPool.ProtoRecycleType.PlayerInfo);
    }
}
