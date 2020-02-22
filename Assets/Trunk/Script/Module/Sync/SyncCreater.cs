
using UnityEngine;

public class SyncCreater : MonoBehaviour
{
    public static SyncCreater instance;
    public GameObject[] preSetPrefabs;
    public GameObject[] cratePrefabs;
    PlayerModel _playerModel;
    PlayerModel playerModel { get { if (_playerModel == null) _playerModel = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name);return _playerModel; } }
     SceneModel _sceneModel;
    SceneModel sceneModel
    {
        get
        {
            if (_sceneModel == null)
                _sceneModel = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
            return _sceneModel;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public GameObject GetPrefab(int objectIndex)
    {
        GameObject go=null;
        bool isPreSet = objectIndex >= 0;
        int index = isPreSet ? objectIndex : -objectIndex;
        if (isPreSet)
        {
            if (index < preSetPrefabs.Length)
            {
                go = preSetPrefabs[index];
            }
        }
        else
        {
            if (index < preSetPrefabs.Length)
            {
                go = cratePrefabs[index];
            }
        }
        return go;
    }
    /// <summary>
    /// 创建场景对象
    /// </summary>
    /// <param name="objInfo"></param>
    public void CreateObject(int objectIndex,int serverID,SyncObject sync)
    {
        bool isPreSet = objectIndex >= 0;
        int index = isPreSet ? objectIndex : -objectIndex;
        if (isPreSet)
        {
            if (index < preSetPrefabs.Length)
            {
                GameObject go = preSetPrefabs[index];
                if (go == null)
                    return;
                go.SetActive(true);
                SceneGameObject sgo = go.GetComponent<SceneGameObject>();
                if (sgo != null)
                {
                    ProtoPlayerInfo selfInfo = playerModel.GetPlayerInfo();
                    if (selfInfo != null)
                    {
                        sgo.SetSyncStatus(objectIndex, serverID, true, selfInfo.pos, sgo.GetStartPos(), sgo.GetStartRot());
                        sceneModel.AddSceneObject(sgo.sceneObject);
                    }
                }

            }
        }
        else
        {
            if (index < preSetPrefabs.Length)
            {

                GameObject prefab =  cratePrefabs[index];
                if (prefab == null)
                    return;
                GameObject  go = GameObject.Instantiate(prefab);
                SceneGameObject sgo = go.GetComponent<SceneGameObject>();
                if (sgo != null)
                {
                    ProtoPlayerInfo selfInfo = playerModel.GetPlayerInfo();
                    if (selfInfo != null)
                    {
                        sgo.SetSyncStatus(objectIndex, serverID, true, selfInfo.pos, sync.GetPos(), sync.GetRot());
                        sceneModel.AddSceneObject(sgo.sceneObject);
                    }
                }
            }
        }
    }
}
