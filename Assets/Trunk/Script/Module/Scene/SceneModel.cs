using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModel : BaseModel
{
    public const string name = "SceneModel";
    public int mapID = 0;
    SyncModel syncModel;
    Dictionary<int, SceneGameObject> sceneObjs = new Dictionary<int, SceneGameObject>();
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

    public PlayerShip GetPlayerShip()
    {
        return playerShip;
    }
    /// <summary>
    /// 移除场景物体
    /// </summary>
    public void RemoveSceneObject(SceneGameObject sObject)
    {
        if (sceneObjs.ContainsKey(sObject.sync.serverID))
        {
            sceneObjs.Remove(sObject.sync.serverID);
        }
    }

    public void AddSceneObject(SceneGameObject sObject)
    {
        if (!sceneObjs.ContainsKey(sObject.sync.serverID))
        {
            sceneObjs.Add(sObject.sync.serverID,sObject);
        }
    }

    public SceneGameObject GetSceneObject(int serverID)
    {
        SceneGameObject result = null;
        sceneObjs.TryGetValue(serverID, out result);
        return result;
    }

    public void SetSceneObjectAciton(int serverID,int[] data)
    {
        SceneGameObject obj= GetSceneObject(serverID);
        if (obj != null)
            obj.OnGetAction(data);
    }
}
