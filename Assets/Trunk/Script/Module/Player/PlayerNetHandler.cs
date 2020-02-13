using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetHandler : BaseNetHandler
{
    PlayerModel model;
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
        ProtoInt loginProto = new ProtoInt();
        loginProto.context = AppCfg.expose.pos;
        Send(ProtoIDCfg.LOGIN, loginProto, ProtoType.Importance);
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    void OnLogin(object p)
    {
        ProtoPlayerInfo proto=   p as ProtoPlayerInfo;
        model.SetPlayerInfo(proto);
    }

    /// <summary>
    /// 玩家状态变更
    /// </summary>
    void OnGetPlayerList(object p)
    {
       ProtoPlayerList proto = p as ProtoPlayerList;
       if (proto != null)
           model.SetPlayerList(proto);
    }
}
