using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShip : SceneGameObject
{

    DOTweenPath doPath;
    public Bounds bounds;
    public UnityEvent onHitEvent;
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
    /// <summary>
    /// 受伤表现
    /// </summary>
    public override void OnGetDamage(int damage, Vector3 point)
    {
        if(onHitEvent != null){
            onHitEvent.Invoke();
        }
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
