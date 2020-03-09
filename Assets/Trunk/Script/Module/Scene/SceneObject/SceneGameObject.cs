﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameObject : MonoBehaviour
{
    public int hp = 100;
    public float moveSpeed = 2;

    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }
    protected virtual void OnSetSync(SyncType type) { }
    protected virtual void OnDestroyed() { }

    public SyncObject _sync;
    public SyncObject sync
    {
        get
        {
            if (_sync == null)
                _sync = new SyncObject();
            return _sync;
        }
    }
    SceneModel _sceneModel;
    protected SceneModel sceneModel{
        get {
            if (_sceneModel==null)
                _sceneModel = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
            return _sceneModel;
        }
    }

    bool syncIng=false;
    private Vector3 latestCorrectPos = Vector3.zero;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 errorVector = Vector3.zero;
    private double lastTime = 0;
    [Header("控制权(-1:本地 0:主机  >0:玩家ID)")]
    public int controlPos = -1;
    public SyncSetting syncSetting=SyncSetting.LocalPosAndRot;
    protected SyncModel _syncModel;
    protected SyncType syncType=SyncType.None;
    protected SyncModel syncModel
    {
        get
        {
            if (_syncModel == null)
                _syncModel=  SyncController.instance.GetModel<SyncModel>(SyncModel.name);
            return _syncModel;
        }
    }
 
    private void Awake()
    {
    
        OnAwake();

    }
    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    private void OnDestroy()
    {
        OnDestroyed();
    }
    public void UplodeSync()
    {
        if (syncType==SyncType.UpLoad)
        {

            if (syncSetting == SyncSetting.LocalPos || syncSetting == SyncSetting.LocalPosAndRot)
            {
                sync.SetPos(transform.localPosition);
            }
            else if (syncSetting == SyncSetting.WorldPos || syncSetting == SyncSetting.WorldPosAndRot)
            {
                sync.SetPos(transform.position);
            }

            if (syncSetting == SyncSetting.LocalRot || syncSetting == SyncSetting.LocalPosAndRot)
            {
                sync.SetRot(transform.localRotation);
            }
            else if (syncSetting == SyncSetting.WorldRot || syncSetting == SyncSetting.WorldPosAndRot)
            {
                sync.SetRot(transform.rotation);
            }
        }
        else if(syncType==SyncType.UpDate)
        {
           
            SmoothPos(sync.GetPos());
            SmoothRot(sync.GetRot());
        }
    }
   
    void SmoothPos(Vector3 pos)
    {
        double timeDiffOfUpdates = Time.time - this.lastTime;
        this.lastTime = Time.time;
        this.movementVector = (pos - this.latestCorrectPos) / (float)timeDiffOfUpdates;
        this.errorVector = (pos - transform.localPosition) / (float)timeDiffOfUpdates;
        this.latestCorrectPos = pos;
        Vector3 temp = (this.movementVector + this.errorVector) * 0.98f * Time.deltaTime;
      
        if (syncSetting == SyncSetting.LocalPos || syncSetting == SyncSetting.LocalPosAndRot)
        {
           // transform.localPosition = pos;
            transform.localPosition += temp;
        }
        else if (syncSetting == SyncSetting.WorldPos || syncSetting == SyncSetting.WorldPosAndRot)
        {
          //  transform.position = pos;
           transform.position += temp;
        }
    }
    void SmoothRot(Quaternion rot)
    {
        if (syncSetting == SyncSetting.LocalRot || syncSetting == SyncSetting.LocalPosAndRot)
        {
           // transform.localRotation = rot;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * 10);
        }
        else if (syncSetting == SyncSetting.WorldRot || syncSetting == SyncSetting.WorldPosAndRot)
        {
           // transform.rotation = rot;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
        }
    }
    public Vector3 GetStartPos()
    {
        if (syncSetting == SyncSetting.LocalPos || syncSetting == SyncSetting.LocalPosAndRot)
        {
            return transform.localPosition;
        }
        else if (syncSetting == SyncSetting.WorldPos || syncSetting == SyncSetting.WorldPosAndRot)
        {
            return transform.position;
        }
        return transform.localPosition;
    }
    public Quaternion GetStartRot()
    {
        if (syncSetting == SyncSetting.LocalRot || syncSetting == SyncSetting.LocalPosAndRot)
        {
            return transform.localRotation;
        }
        else if (syncSetting == SyncSetting.WorldRot || syncSetting == SyncSetting.WorldPosAndRot)
        {
            return transform.rotation;
        }
        return transform.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (syncIng)
        {
            UplodeSync();
        }
        OnUpdate();
    }
    public void ReqDestroy()
    {
        EventIntArrayArgs obj = new EventIntArrayArgs();
        obj.t = new int[] { sync.serverID};
        SyncController.instance.SendNetMsg(ProtoIDCfg.REMOVE_OBJECTS, obj);

    }
    public void RemoveObject()
    {
        RemoveSync();
       sceneModel.RemoveSceneObject(this);
        GameObject.DestroyImmediate(gameObject);
    }
    void RemoveSync()
    {
        if (_sync != null)
        {
            Debug.Log("销毁对象"+sync.serverID);
            syncIng = false;
            if (syncType == SyncType.UpLoad)
                syncModel.RemoveUpLoadObj(sync);
            else if (syncType == SyncType.UpDate)
                syncModel.RemoveUpDataObj(sync);
        }
    }
    /// <summary>
    /// 设置同步
    /// </summary>
    public void SetSyncStatus(int prefabIndex, int serverID, bool needSync,int pos,Vector3 position,Quaternion rot)
    {
        if (controlPos == -1)
        {
            syncType = SyncType.None;
            return;
        }
        else if (controlPos == 0 && Connection.GetInstance().isHost)
            syncType = SyncType.UpLoad;
        else if (controlPos == pos)
            syncType = SyncType.UpLoad;
        else
            syncType = SyncType.UpDate;

        if (needSync)
        {
            sync.serverID = serverID;
            sync.objectIndex = prefabIndex;

            if (syncSetting == SyncSetting.LocalPos || syncSetting == SyncSetting.LocalPosAndRot)
            {
                transform.localPosition = position;
                latestCorrectPos = transform.localPosition ;
            }
            else if (syncSetting == SyncSetting.WorldPos || syncSetting == SyncSetting.WorldPosAndRot)
            {
                transform.position = position;
                latestCorrectPos = transform.position ;
            }
            SmoothRot(rot);

            if (syncType==SyncType.UpLoad)
                syncModel.AddUpLoadList(sync);
            else if (syncType == SyncType.UpDate)
                syncModel.AddUpUpdateList(sync);
        }
        else
        {
            if (sync != null)
            {
                if (syncType == SyncType.UpLoad)
                    syncModel.RemoveUpLoadObj(sync);
                else if(syncType==SyncType.UpDate)
                    syncModel.RemoveUpDataObj(sync);
            }
        }
        syncIng = needSync;
        OnSetSync(syncType);
    }
    /// <summary>
    /// 同步操作
    /// </summary>
    public virtual void SyncAction(int[] intArray){ }

    /// <summary>
    /// 请求同步操作
    /// </summary>
    public virtual void RqSyncAction(int[] data)
    {
        EventIntArrayArgs args = new EventIntArrayArgs();
        args.t = data;
        SyncController.instance.SendNetMsg(ProtoIDCfg.OBJECT_ACTION, args);
    }
    /// <summary>
    /// 受伤表现
    /// </summary>
    public virtual void OnGetDamage(int damage,Vector3 point) { }

    public virtual void SetDamage(int atk,Vector3 point)
    {
        hp = hp - atk;
        OnGetDamage(atk, point);
        if (Connection.GetInstance().isHost)
        {
            if (hp < 0)
                ReqDestroy();
        }
    }
}
