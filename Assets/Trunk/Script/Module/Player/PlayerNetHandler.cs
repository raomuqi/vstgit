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
        Send(ProtoIDCfg.LOGIN, null, ProtoType.Importance);
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    void OnLogin(EventArgs arg)
    {
        EventObjectArgs objArg= arg as EventObjectArgs;
        if (objArg != null)
        {
           ProtoPlayerInfo proto=   objArg.t as ProtoPlayerInfo;
            if (proto != null)
                model.SetPlayerInfo(proto);
        }
    }

    /// <summary>
    /// 玩家状态变更
    /// </summary>
    void OnGetPlayerList(EventArgs arg)
    {
        EventObjectArgs objArg = arg as EventObjectArgs;
        if (objArg != null)
        {
            ProtoPlayerList proto = objArg.t as ProtoPlayerList;
            if (proto != null)
                model.SetPlayerList(proto);
        }
    }
}
