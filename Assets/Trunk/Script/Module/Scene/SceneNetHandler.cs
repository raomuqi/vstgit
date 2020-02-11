

public class SceneNetHandler : BaseNetHandler
{
    SceneModel model;
    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        RegisterListenProto(ProtoIDCfg.ENTER_SCENE, OnEnterScene);
 
    }
 
    /// <summary>
    /// 进入场景
    /// </summary>
    void OnEnterScene(EventArgs arg)
    {
        EventObjectArgs objArg = arg as EventObjectArgs;
        ProtoInt intArg = objArg.t as ProtoInt;
        EventLoadSceneArgs loadSceneCfg = new EventLoadSceneArgs();
        loadSceneCfg.index = intArg.context;
        loadSceneCfg.progress = null;
        loadSceneCfg.complete = SendEnterScene;
        MonoHelper.GetInstance().LoadSceneAsync(loadSceneCfg);
      
    }

    void SendEnterScene(int mapID)
    {
        ProtoInt intArgs = new ProtoInt();
        intArgs.context = mapID;
        Send(ProtoIDCfg.ENTER_SCENE, intArgs,ProtoType.Importance);
    }
    protected override void OnClear()
    {
    }
}
