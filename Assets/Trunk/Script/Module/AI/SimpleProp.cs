using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProp : InteractiveScneeGameObject
{
    bool geted = false;
    public int extElemntID=0;
    //0道具ID 1协议行为 2目标ID 3获得的拓展元素
    int[] propProtoData = new int[4];
    protected override void OnSetSync(SyncType type)
    {
        base.OnSetSync(type);
        propProtoData[0] = -1;
        propProtoData[1] = SceneObjectActionCfg.GET_PROP;
        propProtoData[2] = sync.serverID;
        propProtoData[3] = extElemntID;
    }
    protected override void OnUpdate()
    {
        if (Connection.GetInstance().isHost)
        {
            CheckLiftTime();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagCfg.SHIP))
        {
            SceneGameObject sgo = other.gameObject.GetComponent<SceneGameObject>();
            if (sgo != null )
            {
                ReqGetProp(sceneModel.GetPlayerShip());
            }
        }
    }
    public override void SetDamage(int atk, Vector3 point,SceneGameObject sgo)
    {
        hp = hp - atk;
        OnGetDamage(atk, point);
        if (hp < 0)
            ReqGetProp(sgo);
    }
    /// <summary>
    /// 请求获取该道具
    /// </summary>
    public virtual void ReqGetProp(SceneGameObject trigger)
    {
        if (geted || !Connection.GetInstance().isHost)
            return;
        geted = true;
        propProtoData[0] = trigger.sync.serverID;
        RqSyncAction(propProtoData);
        ReqDestroy();

    }
}
