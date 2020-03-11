using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : SceneGameObject
{

    public DOTweenPath doPath;
    InputModel inputModel;
    public float moveArea = 5;
    protected override void  OnAwake()
    {
        sceneModel.SetPlayerShip(this);
    }
    protected override void OnStart()
    {
      //  doPath = gameObject.GetComponent<DOTweenPath>();
       // doPath.DOPause();
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
      
        if (syncType==SyncType.UpLoad)
        {
            float h = Mathf.Lerp(-moveArea, moveArea, (inputModel.horizontal + 1f) * 0.5f);
            float v = Mathf.Lerp(-moveArea, moveArea, (inputModel.vertical + 1f) * 0.5f);
            Vector3 target = new Vector3(h, v, 0);
            float rx = Mathf.LerpAngle(transform.localEulerAngles.x, -v, Time.deltaTime * 10);
            float ry = Mathf.LerpAngle(transform.localEulerAngles.y, h, Time.deltaTime * 10);
            transform.localEulerAngles = new Vector3(rx, ry, 0);
          //  Vector3 dir =Vector3.Normalize(target- transform.localPosition);
          //  transform.localPosition += dir * Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(h, v, 0),Time.deltaTime* moveSpeed);

        }
    }
    /// <summary>
    /// 受伤表现
    /// </summary>
    public override void OnGetDamage(int damage, Vector3 point)
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
