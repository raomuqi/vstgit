using System.Collections;


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
}
