using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseModel
{
    public const string name = "PlayerModel";
    ProtoPlayerInfo[] playerList;
    int playerID = 0;
    protected override void OnInit()
    {
    }
    protected override void OnClear()
    {
    }
    public void SetPlayerInfo(ProtoPlayerInfo playerProto)
    {
        playerID = playerProto.id;
        Debug.Log("玩家ID：" + playerID + " 位置:" + playerProto.pos);
    }
    public void SetPlayerList(ProtoPlayerList playerList)
    {
        this.playerList = playerList.players;
        Debug.Log("更新玩家列表");
    
    }
}
