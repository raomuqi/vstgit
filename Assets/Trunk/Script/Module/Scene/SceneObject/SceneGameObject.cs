using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameObject : MonoBehaviour
{
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }
    protected virtual void OnDestroyed() { }
    public SceneObject sceneObject;
    bool syncIng=false;
    private Vector3 latestCorrectPos = Vector3.zero;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 errorVector = Vector3.zero;
    private double lastTime = 0;

    public SceneGameObject()
    {
        sceneObject = new SceneObject(this);
    }
    private void Awake()
    {
        this.latestCorrectPos = transform.position;
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
        if (SyncModel.isUploader)
        {
             
            sceneObject.sync.posX = transform.position.x;
            sceneObject.sync.posY = transform.position.y;
            sceneObject.sync.posZ = transform.position.z;
            sceneObject.sync.rotX = transform.rotation.x;
            sceneObject.sync.rotY = transform.rotation.y;
            sceneObject.sync.rotZ = transform.rotation.z;
            sceneObject.sync.rotW = transform.rotation.w;
        }
        else
        {
            Vector3 pos = new Vector3(sceneObject.sync.posX, sceneObject.sync.posY, sceneObject.sync.posZ);
            Quaternion rot = new Quaternion(sceneObject.sync.rotX, sceneObject.sync.rotY, sceneObject.sync.rotZ, sceneObject.sync.rotW);
           /// transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
           transform.rotation =Quaternion.Lerp(transform.rotation, rot,Time.deltaTime*0.5f);

            double timeDiffOfUpdates = Time.time - this.lastTime;
            this.movementVector = (pos - this.latestCorrectPos) / (float)timeDiffOfUpdates;
            this.errorVector = (pos - transform.localPosition) / (float)timeDiffOfUpdates;
            this.latestCorrectPos = pos;
            transform.position += (this.movementVector + this.errorVector) * 0.98f * Time.deltaTime;
        }
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
    public void SetSyncStatus(int objectID,bool needSync)
    {
        SyncModel syncModel = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        if (needSync)
        {
            if (sceneObject.sync == null)
            {
                sceneObject.sync = new SyncObject();
                sceneObject.objectID = objectID;
            }
            syncModel.AddSyncObj(sceneObject.sync);
        }
        else
        {
            if(sceneObject.sync != null)
               syncModel.RemoveSyncObj(sceneObject.sync);
        }
        syncIng = needSync;
    }
    
}
