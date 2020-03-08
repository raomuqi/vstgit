﻿using System.Collections;


public static class ProtoIDCfg
{
    /// <summary>
    /// 心跳包
    /// </summary>
    public const byte HEARTBEAT = 0;
    /// <summary>
    /// 登录协议
    /// </summary>
    public const byte LOGIN = 1;
    /// <summary>
    /// 推送玩家列表
    /// </summary>
    public const byte S_PLAYERS= 2;
    /// <summary>
    /// 进入场景
    /// </summary>
    public const byte ENTER_SCENE = 3;
    /// <summary>
    /// 初始化船仓信息
    /// </summary>
    public const byte S_INITSHIP = 4;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public const byte S_STARTGAME = 5;
    /// <summary>
    /// 同步对象
    /// </summary>
    public const byte SYNC_OBJECTS = 6;
    /// <summary>
    /// 激活对象
    /// </summary>
    public const byte ACTIVE_OBJECTS = 7;
    /// <summary>
    /// 创建对象
    /// </summary>
    public const byte CREATE_OBJECTS = 8;
    /// <summary>
    /// 同步输入
    /// </summary>
    public const byte SYNC_INPUT = 9;
    /// <summary>
    /// 删除对象
    /// </summary>
    public const byte REMOVE_OBJECTS = 10;
}
