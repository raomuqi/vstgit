using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreater : MonoBehaviour
{
    bool start = false;
    float gameTime = 0;
    public LevelData levelData;
    //记录创建状态
    bool[] appearTag;
    //记录激活状态
    bool[] activeTag;
   static int createID = 0;
    static Dictionary<int, AppearObjectData> objsCfgDic;
    void Awake()
    {
        if (Connection.GetInstance().isHost)
        {
            objsCfgDic = new Dictionary<int, AppearObjectData>();
            EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
            if (levelData.appearSets != null && levelData.appearSets.Length > 0)
                appearTag = new bool[levelData.appearSets.Length];
            if(levelData.activeSets!=null && levelData.activeSets.Length>0)
                activeTag = new bool[levelData.activeSets.Length];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (start && Connection.GetInstance().isHost)
        {
            gameTime += Time.deltaTime;
            CheckCreate();
            CheckActiveObject();

        }
    }
    void OnGameStart(EventArgs args)
    {
        gameTime = 0;
        start = true;
    }
    public static int GetCreateID()
    {
        createID++;
        return createID;
    }
    //检查创建对象
    void CheckCreate()
    {
        if (appearTag != null)
        {
            for (int i = 0; i < appearTag.Length; i++)
            {
                if (appearTag[i] == false)
                {
                    AppearSetData apData = levelData.appearSets[i];
                    if (gameTime >= apData.time)
                    {
                        appearTag[i] = true;
                        Debug.Log("第" + i + "波");
                        ProtoCreateObject[] list = new ProtoCreateObject[apData.objectCfgs.Length];
                        for (int z = 0; z < list.Length; z++)
                        {
                            var objData = apData.objectCfgs[z];
                                
                            list[z] = new ProtoCreateObject();
                            list[z].objectIndex = objData.objectIndex;
                            int id = GetCreateID();
                            list[z].hashCode = id;
                            AddObjectCfg(id, objData);
                            list[z].SetPos(GetPosByRadian(transform.position, objData.XAngle, objData.distance, objData.YAngle));
                        }
                        var t = new EventObjectArgs();
                        t.t = list;
                        SceneController.instance.SendNetMsg(ProtoIDCfg.CREATE_OBJECTS, t);
                    }
                }
            }
        }

    }
    public static void AddObjectCfg(int id, AppearObjectData data)
    {
        if (objsCfgDic == null) return;
        if (!objsCfgDic.ContainsKey(id))
        {
            objsCfgDic.Add(id, data);
        }
        else
        {
            Debug.LogError("重复id");
        }
    }

    public static AppearObjectData GetObjectCfg(int id)
    {
        if (objsCfgDic == null) return null;
        AppearObjectData data;
        objsCfgDic.TryGetValue(id, out data);
        objsCfgDic.Remove(id);
        return data;

    }
    void CheckActiveObject()
    {
        //检查激活对象
        if (activeTag != null)
        {
            for (int i = 0; i < activeTag.Length; i++)
            {
                if (activeTag[i] == false)
                {
                    var acData = levelData.activeSets[i];
                    if (gameTime >= acData.time)
                    {
                        activeTag[i] = true;
                        EventIntArrayArgs e = new EventIntArrayArgs();
                        e.t = acData.objectCfgs;
                        SceneController.instance.SendNetMsg(ProtoIDCfg.ACTIVE_OBJECTS, e);
                    }
                }

            }
        }
    }
    public  Vector3 GetPosByRadian(Vector3 pos, float angleX, float r,float angleY=0)
    {
        angleX = (float)(angleX / 180 *System.Math.PI);
        angleY = (float)(angleY / 180 * System.Math.PI);
        float x = Mathf.Sin(angleX) * r ;
        float y= Mathf.Sin(angleY) * r ;
        float z = Mathf.Cos(angleX) * r ;
        return transform.localToWorldMatrix.MultiplyPoint(new  Vector3(x, y, z));
    }
}
