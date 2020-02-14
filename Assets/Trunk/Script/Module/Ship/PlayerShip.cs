using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : SceneGameObject
{

    SceneModel sceneModel;
    SyncModel syncModel;
    DOTweenPath doPath;
    protected override void  OnAwake()
    {
        sceneModel = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        syncModel = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        doPath = gameObject.GetComponent<DOTweenPath>();
        doPath.DOPause();
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
    }
    protected override void OnStart()
    {
        
        EventObjectArgs args = new EventObjectArgs();
        args.t = this;
        SceneController.instance.FireCommand(SceneCommand.SET_PLAYER_SHIP, args);
    }
    void OnGameStart(EventArgs args)
    {
        if (syncModel.IsUploader())
        {
            doPath.DOPlay();
        }
    }
    protected override void OnDestroyed()
    {
        EventsMgr.RemoveEvent(EventName.START_GAME, OnGameStart);
    }
}
