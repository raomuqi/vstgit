

using UnityEngine;

public class SceneNetHandler : BaseNetHandler
{
    SceneModel model;
    PlayerModel _playerModel;
    PlayerModel playerModel { get { if (_playerModel == null) _playerModel = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name); return _playerModel; } }

    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        //激活场景对象
        RegisterSendProto(ProtoIDCfg.ACTIVE_OBJECTS, ReqActiveObject);
        RegisterListenProto(ProtoIDCfg.ACTIVE_OBJECTS, OnActiveObject);
        //创建场景对象
        RegisterSendProto(ProtoIDCfg.CREATE_OBJECTS, ReqCreateObject);
        RegisterListenProto(ProtoIDCfg.CREATE_OBJECTS, OnCreateObjects);
        

        RegisterListenProto(ProtoIDCfg.ENTER_SCENE, OnEnterScene);
        RegisterListenProto(ProtoIDCfg.S_STARTGAME, OnStartGmae);
    }

    /// <summary>
    /// 进入场景
    /// </summary>
    void OnEnterScene(byte[] p)
    {
        ProtoInt proto = ObjectPool.protoPool.GetOrCreate<ProtoInt>(ProtoPool.ProtoRecycleType.Int);
        if (proto.Parse(p))
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
            SendActiveObjectProto(new int[] { 0 });
        }
        var player = playerModel.GetPlayerInfo();
        if (player!=null)
        {
            //炮
            SendActiveObjectProto(new int[] { playerModel.GetPlayerInfo().pos });
        }

        
    }
    void OnStartGmae(byte[] p)
    {
        EventsMgr.FireEvent(EventName.START_GAME);
        UnityEngine.Debug.Log("开始游戏");
    }
    ProtoActiveObjects activeProto;
    void OnActiveObject(byte[] p)
    {

        if (activeProto == null)
            activeProto = new ProtoActiveObjects();
        if (activeProto.Parse(p))
        {
            for (int i = 0; i < activeProto.serverIDs.Length; i++)
            {
                if (SyncCreater.instance != null)
                    SyncCreater.instance.ActiveObject(activeProto.objectIndexs[i], activeProto.serverIDs[i]);
                UnityEngine.Debug.Log("激活对象sID+" + activeProto.serverIDs[i] + " index:" + activeProto.objectIndexs[i]);
            }
        }
    }
  
    /// <summary>
    /// 请求激活对象
    /// </summary>
    void ReqActiveObject(EventArgs args)
    {
        EventIntArrayArgs intArgs = args as EventIntArrayArgs;
        if (intArgs != null && intArgs.t!=null)
        {
            SendActiveObjectProto(intArgs.t);
        }
    }
    
    /// <summary>
    /// 请求创建对象
    /// </summary>
    void ReqCreateObject(EventArgs args)
    {
        EventObjectArgs objectArgs = args as EventObjectArgs;
        if (objectArgs != null && objectArgs.t != null)
        {
            ProtoCreateObject[] list= objectArgs.t as ProtoCreateObject[];
            ProtoSyncObjectList createObjectProto = ObjectPool.protoPool.GetOrCreate<ProtoSyncObjectList>(ProtoPool.ProtoRecycleType.CreateObjects);
            createObjectProto.objList = list;
            Send(ProtoIDCfg.CREATE_OBJECTS, createObjectProto, ProtoType.Importance);
            createObjectProto.Recycle();
        }
    }

    /// 发送请求对象Proto
    void SendActiveObjectProto(int[] prefabIndexs)
    {
        ProtoIntArray p = ObjectPool.protoPool.GetOrCreate<ProtoIntArray>(ProtoPool.ProtoRecycleType.IntArray);
        p.context = prefabIndexs;
        Send(ProtoIDCfg.ACTIVE_OBJECTS, p, ProtoType.Importance);
        p.Recycle();
    }
    void OnCreateObjects(byte[] p)
    {
        ProtoSyncObjectList list = ObjectPool.protoPool.GetOrCreate<ProtoSyncObjectList>(ProtoPool.ProtoRecycleType.CreateObjects);
        if (list.Parse(p))
        {
            for (int i = 0; i < list.objList.Length; i++)
            {
                if (SyncCreater.instance != null)
                    SyncCreater.instance.CreateObject(list.objList[i].objectIndex, list.objList[i].serverID,list.objList[i]);
                UnityEngine.Debug.Log("生成对象sID+" + list.objList[i].serverID + " index:" + list.objList[i].objectIndex);
            }
        }
    }

    protected override void OnClear()
    {
    }
}
