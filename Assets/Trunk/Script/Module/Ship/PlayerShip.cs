using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : SceneGameObject
{

    DOTweenPath doPath;
    public Bounds bounds;
    protected override void  OnAwake()
    {
        sceneModel.SetPlayerShip(this);
    }
    protected override void OnStart()
    {
        doPath = gameObject.GetComponent<DOTweenPath>();
        doPath.DOPause();
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
    }
    protected override void OnUpdate()
    {
    }

   

    void OnGameStart(EventArgs args)
    {
        if (Connection.GetInstance().isHost)
        {
            doPath.DOPlay();
        }
    }
    protected override void OnDestroyed()
    {
        EventsMgr.RemoveEvent(EventName.START_GAME, OnGameStart);
    }
}
