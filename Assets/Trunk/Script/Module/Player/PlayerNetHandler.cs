using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetHandler : BaseNetHandler
{
    PlayerModel model;
    ProtoPlayerList playerListProto;
    protected override void OnInit()
    {
        model = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name);
        RegisterSendProto(ProtoIDCfg.LOGIN, Login);
        RegisterListenProto(ProtoIDCfg.LOGIN, OnLogin);

        RegisterListenProto(ProtoIDCfg.S_PLAYERS, OnGetPlayerList);
    }
    protected override void OnClear()
    {
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    void Login(EventArgs arg)
    {
   
        byte[] pos = new byte[1];
        pos[0] = (byte)AppCfg.expose.pos;
        Send(ProtoIDCfg.LOGIN, pos, ProtoType.Importance);
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    void OnLogin(byte[] pData)
    {
        ProtoPlayerInfo proto=   Util.DeSerializeProto<ProtoPlayerInfo>(pData);
        if (proto != null)
        {
            model.SetPlayerInfo(proto);
            Connection.GetInstance().RemoveBroadCastPort(proto.id);
            Connection.GetInstance().SetUpMutiDataUdp(proto.id);
        }
    }

    /// <summary>
    /// 玩家状态变更
    /// </summary>
    void OnGetPlayerList(byte[] p)
    {
        if (playerListProto == null)
            playerListProto = new ProtoPlayerList();

        Util.DeSerializeProto<ProtoPlayerList>(p, playerListProto);

        if (Connection.GetInstance().differentUdpPort && playerListProto != null)
        {
            ProtoPlayerInfo selfInfo = model.GetPlayerInfo();
            int selfID = 255;
            selfID = selfInfo == null ? selfID : selfInfo.id;
            for (int i = 0; i < playerListProto.players.Length; i++)
            {
                ProtoPlayerInfo info = playerListProto.players[i];
                if (selfID != info.id)
                {
                    Connection.GetInstance().AddBroadCastPort(info.id);
                }
            }
        }
        Debug.Log("刷新玩家信息");

    }
}
