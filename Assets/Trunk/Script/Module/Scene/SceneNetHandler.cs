

public class SceneNetHandler : BaseNetHandler
{
    SceneModel model;
    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        RegisterListenProto(ProtoIDCfg.ENTER_SCENE, OnEnterScene);
        RegisterListenProto(ProtoIDCfg.S_STARTGAME, OnStartGmae);

    }

    /// <summary>
    /// 进入场景
    /// </summary>
    void OnEnterScene(byte[] p)
    {
        ProtoInt proto = Util.DeSerializeProto<ProtoInt>(p);
        if (proto != null)
        {
            EventLoadSceneArgs loadSceneCfg = new EventLoadSceneArgs();
            loadSceneCfg.index = proto.context;
            loadSceneCfg.progress = null;
            loadSceneCfg.complete = SendEnterScene;
            MonoHelper.GetInstance().LoadSceneAsync(loadSceneCfg);
        }
      
    }

    void SendEnterScene(int mapID)
    {
        ProtoInt intArgs = ObjectPool.protoPool.GetOrCreate<ProtoInt>(ProtoPool.ProtoRecycleType.Int);
        intArgs.context = mapID;
        Send(ProtoIDCfg.ENTER_SCENE, intArgs,ProtoType.Importance);
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.Int, intArgs);
    }

    void OnStartGmae(byte[] p)
    {
        EventsMgr.FireEvent(EventName.START_GAME);
        UnityEngine.Debug.Log("开始游戏");
    }
    protected override void OnClear()
    {
    }
}
