using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCommand : BaseCommand
{
    public const string LOAD_SCENE = "LOAD_SCENE";
    public const string SET_PLAYER_SHIP = "SET_PLAYER_SHIP";
    SceneModel model;
    SyncModel syncModel;
    protected override void OnInit()
    {
        model = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        syncModel = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        AddCommand(LOAD_SCENE, LoadScene);
        AddCommand(SET_PLAYER_SHIP, InitPlayerShip);
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

    /// <summary>
    /// 初始化飞船
    /// </summary>
    void InitPlayerShip(EventArgs args)
    {
        EventObjectArgs shipArgs = args as EventObjectArgs;
        PlayerShip ship = shipArgs.t as PlayerShip;
        model.SetPlayerShip(ship);
        ship.SetSyncStatus(ship.sceneObject.objectID, true);


    }
}
