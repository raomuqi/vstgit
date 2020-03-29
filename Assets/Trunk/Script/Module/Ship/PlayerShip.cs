using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShip : SceneGameObject
{

   public  DOTweenPath doPath;
   // public Bounds bounds;
    public UnityEvent onHitEvent;
    InputModel inputModel;
    public float moveArea = 5;
    protected override void  OnAwake()
    {
        sceneModel.SetPlayerShip(this);
    }
    protected override void OnStart()
    {
        //doPath = gameObject.GetComponent<DOTweenPath>();
        //doPath.DOPause();
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
    }
    protected override void OnSetSync(SyncType type)
    {
        if (syncType == SyncType.UpLoad)
        {
            inputModel = InputController.instance.GetModel<InputModel>(InputModel.name);
        }
    }

    protected override void OnUpdate()
    {
        if (syncType == SyncType.UpLoad)
        {
            float h = Mathf.Lerp(-moveArea, moveArea, (inputModel.horizontal + 1f) * 0.5f);
            float v = Mathf.Lerp(-moveArea, moveArea, (inputModel.vertical + 1f) * 0.5f);
            Vector3 target = new Vector3(h, v, 0);
            float rx = Mathf.LerpAngle(transform.localEulerAngles.x, -v, Time.deltaTime);
            float ry = Mathf.LerpAngle(transform.localEulerAngles.y, h, Time.deltaTime);
            transform.localEulerAngles = new Vector3(rx, ry, 0);
            //  Vector3 dir =Vector3.Normalize(target- transform.localPosition);
            //  transform.localPosition += dir * Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(h, v, 0), Time.deltaTime * moveSpeed);

        }
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
    public override void OnGetAction(int[] intArray)
    {
        int action = intArray[1];
        switch (action)
        {
            case SceneObjectActionCfg.GET_PROP:
                int scrObject = intArray[2];
                int extID = intArray[3];
                ExtElementFactory.Get(extID).Use(this);
                break;
        }
    }
}
