using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModel : BaseModel
{
    public const string name = "SceneModel";
    public int mapID = 0;
    public List<SyncObject> syncList = new List<SyncObject>();
    Dictionary<int, SceneObject> sceneObjs = new Dictionary<int, SceneObject>();
    PlayerShip playerShip;
    protected override void OnInit()
    {
    }
    protected override void OnClear()
    {
    }
    public void SetPlayerShip(PlayerShip inputShip)
    {
        playerShip = inputShip;
        SyncObject sync= new SyncObject();
        sync.objectID = 0;
        playerShip.sceneObject.objectID = 0;
        playerShip.sceneObject.sync = sync;
        syncList.Add(sync);
        sceneObjs.Add(0, inputShip.sceneObject);
    }

    public void RemoveSceneObject(SceneObject sObject)
    {
        if (syncList.Contains(sObject.sync))
        {
            syncList.Remove(sObject.sync);
        }
        if (sceneObjs.ContainsKey(sObject.objectID))
        {
            sceneObjs.Remove(sObject.objectID);
        }
    }
}
