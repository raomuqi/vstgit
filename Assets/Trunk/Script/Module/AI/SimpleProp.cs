using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProp : InteractiveScneeGameObject
{
    bool geted = false;
    [Header("道具ID(1:加速,2子弹加排,3换子弹)")]
    public int extElemntID=0;
    [Header("子弹加排:(填加排数) 换子弹:(子弹ID)")]
    public int propParameter1 = 1;
    [Header("备用)")]
    public int propParameter2 = 0;
    //0道具ID 1协议行为 2目标ID 3获得的拓展元素 4-5道具参数
    int[] propProtoData = new int[6];
    protected override void OnSetSync(SyncType type)
    {
        base.OnSetSync(type);
        propProtoData[0] = -1;
        propProtoData[1] = SceneObjectActionCfg.GET_PROP;
        propProtoData[2] = sync.serverID;
        propProtoData[3] = extElemntID;
        propProtoData[4] = propParameter1;
        propProtoData[5] = propParameter2;
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
    protected override void OnBeVisible()
    {
        base.OnBeVisible();
    }
    protected override void OnBeInVisible()
    {
        base.OnBeInVisible();
    }
}
