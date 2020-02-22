﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : SceneGameObject
{

    SceneModel sceneModel;
    DOTweenPath doPath;
    protected override void  OnAwake()
    {
     
    }
    protected override void OnStart()
    {
        doPath = gameObject.GetComponent<DOTweenPath>();
        doPath.DOPause();
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
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
