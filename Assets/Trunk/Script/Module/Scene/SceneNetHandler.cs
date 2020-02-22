

using UnityEngine;

public class SceneNetHandler : BaseNetHandler
{
    SceneModel model;
    PlayerModel _playerModel;
    PlayerModel playerModel { get { if (_playerModel == null) _playerModel = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name); return _playerModel; } }
    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);

        RegisterSendProto(ProtoIDCfg.CREATE_OBJECTS, ReqCreateObject);

        RegisterListenProto(ProtoIDCfg.ENTER_SCENE, OnEnterScene);
        RegisterListenProto(ProtoIDCfg.S_STARTGAME, OnStartGmae);
        RegisterListenProto(ProtoIDCfg.CREATE_OBJECTS, OnCreateObject);
    }

    /// <summary>
    /// 进入场景
    /// </summary>
    void OnEnterScene(byte[] p)
    {
        ProtoInt proto =   Util.DeSerializeProto<ProtoInt>(p);
        if (proto != null)
        {
            EventLoadSceneArgs loadSceneCfg = new EventLoadSceneArgs();
            loadSceneCfg.index = proto.context;
            loadSceneCfg.progress = null;
            loadSceneCfg.complete = SendEnterScene;
            MonoHelper.GetInstance().LoadSceneAsync(loadSceneCfg);
            proto.Recycle();
        }
    }

    /// <summary>
    /// 切换场景完成
    /// </summary>
    void SendEnterScene(int mapID)
    {
        ProtoInt intArgs = ObjectPool.protoPool.GetOrCreate<ProtoInt>(ProtoPool.ProtoRecycleType.Int);
        intArgs.context = mapID;
        Send(ProtoIDCfg.ENTER_SCENE, intArgs,ProtoType.Importance);
        intArgs.Recycle();
        //船
        if (Connection.GetInstance().isHost)
        {
            SendCreateObjectProto(0);
        }
        var player = playerModel.GetPlayerInfo();
        if (player!=null)
        {
            //炮
            SendCreateObjectProto(playerModel.GetPlayerInfo().pos);
        }

        
    }
    void OnStartGmae(byte[] p)
    {
        EventsMgr.FireEvent(EventName.START_GAME);
        UnityEngine.Debug.Log("开始游戏");
    }
    void OnCreateObject(byte[] p)
    {
        SyncObject syncObj =  ObjectPool.protoPool.GetOrCreate<SyncObject>(ProtoPool.ProtoRecycleType.SyncObject);
        Util.DeSerializeProto<SyncObject>(p, syncObj);
        if (syncObj != null)
        {
            if (SyncCreater.instance != null)
                SyncCreater.instance.CreateObject(syncObj.objectIndex,syncObj.serverID, syncObj);

            UnityEngine.Debug.Log("生成对象sID+" + syncObj.serverID +" index:" + syncObj.objectIndex);
            syncObj.Recycle();
        }
    }
    /// <summary>
    /// 请求创建对象
    /// </summary>
    void ReqCreateObject(EventArgs args)
    {
        EventIntArgs intArgs = args as EventIntArgs;
        if (intArgs != null)
        {
            int prefabIndex = intArgs.t;
            SendCreateObjectProto(prefabIndex);
        }
    }

    /// 发送请求对象Proto
    void SendCreateObjectProto(int prefabIndex)
    {
        SyncObject proto = ObjectPool.protoPool.GetOrCreate<SyncObject>(ProtoPool.ProtoRecycleType.SyncObject);
        proto.objectIndex = prefabIndex;
        Send(ProtoIDCfg.CREATE_OBJECTS, proto, ProtoType.Importance);
        proto.Recycle();

    }
   
    protected override void OnClear()
    {
    }
}
