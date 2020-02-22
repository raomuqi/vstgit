using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameObject : MonoBehaviour
{
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }
    protected virtual void OnDestroyed() { }
    SceneObject _sceneObject;
    public SceneObject sceneObject
    {
        get
        {
            if (_sceneObject == null)
                _sceneObject = new SceneObject(this);
            return _sceneObject;
        }
    }
    bool syncIng=false;
    private Vector3 latestCorrectPos = Vector3.zero;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 errorVector = Vector3.zero;
    private double lastTime = 0;
    [Header("控制权(0:主机  >0:玩家ID)")]
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
                sceneObject.sync.SetPos(transform.localPosition);
            }
            else if (syncSetting == SyncSetting.WorldPos || syncSetting == SyncSetting.WorldPosAndRot)
            {
                sceneObject.sync.SetPos(transform.position);
            }

            if (syncSetting == SyncSetting.LocalRot || syncSetting == SyncSetting.LocalPosAndRot)
            {
                sceneObject.sync.SetRot(transform.localRotation);
            }
            else if (syncSetting == SyncSetting.WorldRot || syncSetting == SyncSetting.WorldPosAndRot)
            {
                sceneObject.sync.SetRot(transform.rotation);
            }
        }
        else
        {
           
            SmoothPos(sceneObject.sync.GetPos());
            SmoothRot(sceneObject.sync.GetRot());
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
    /// <summary>
    /// 设置同步
    /// </summary>
    public void SetSyncStatus(int prefabIndex, int serverID, bool needSync,int pos,Vector3 position,Quaternion rot)
    {
        if (controlPos == 0 && Connection.GetInstance().isHost)
            syncType= SyncType.UpLoad;
        else if(controlPos == pos)
            syncType = SyncType.UpLoad;
        else
            syncType = SyncType.UpDate;

        if (needSync)
        {
            sceneObject.sync.serverID = serverID;
            sceneObject.sync.objectIndex = prefabIndex;

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
                syncModel.AddUpLoadList(sceneObject.sync);
            else
                syncModel.AddUpUpdateList(sceneObject.sync);
        }
        else
        {
            if (sceneObject.sync != null)
            {
                if (syncType == SyncType.UpLoad)
                    syncModel.RemoveUpLoadObj(sceneObject.sync);
                else
                    syncModel.RemoveUpDataObj(sceneObject.sync);
            }
        }
        syncIng = needSync;
    }
    
}
