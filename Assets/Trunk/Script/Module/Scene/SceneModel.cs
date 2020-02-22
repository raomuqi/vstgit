using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModel : BaseModel
{
    public const string name = "SceneModel";
    public int mapID = 0;
    SyncModel syncModel;
    Dictionary<int, SceneObject> sceneObjs = new Dictionary<int, SceneObject>();
    PlayerShip playerShip;
    protected override void OnInit()
    {
        syncModel = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
    }
    protected override void OnClear()
    {
    }

    /// <summary>
    /// 设置飞船数据
    /// </summary>
    public void SetPlayerShip(PlayerShip inputShip)
    {
        playerShip = inputShip;
    }

 
    /// <summary>
    /// 移除场景物体
    /// </summary>
    public void RemoveSceneObject(SceneObject sObject)
    {
        if (sceneObjs.ContainsKey(sObject.sync.serverID))
        {
            sceneObjs.Remove(sObject.sync.serverID);
        }
    }

    public void AddSceneObject(SceneObject sObject)
    {
        if (!sceneObjs.ContainsKey(sObject.sync.serverID))
        {
            sceneObjs.Add(sObject.sync.serverID,sObject);
        }
    }
}
