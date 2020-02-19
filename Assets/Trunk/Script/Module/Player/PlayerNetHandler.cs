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
            Global.instance.connection.SetUpMutiDataUdp(proto.id);
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

        if ( Connection.differentUdpPort && playerListProto != null )
        {
            ProtoPlayerInfo selfInfo = model.GetPlayerInfo();
            if (selfInfo != null)
            {
                for (int i = 0; i < playerListProto.players.Length; i++)
                {
                    ProtoPlayerInfo info = playerListProto.players[i];
                    if (selfInfo.id != info.id)
                    {
                        Global.instance.connection.AddBroadCastPort(info.id);
                    }
                }
            }
        }
    }
}
