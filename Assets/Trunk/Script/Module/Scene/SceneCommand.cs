using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCommand : BaseCommand
{
    public const string LOAD_SCENE = "LOAD_SCENE";
    SceneModel model;
    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        AddCommand(LOAD_SCENE, LoadScene);

    }

    protected override void OnClear()
    {
    }
    public void LoadScene(EventArgs args)
    {
        EventLoadSceneArgs loadArgs = args as EventLoadSceneArgs;
        if (loadArgs != null)
            MonoHelper.GetInstance().LoadSceneAsync(loadArgs);
        else
            Debug.LogError("参数错误");
    }
    void InitPlayerShip()
    {


    }
}
