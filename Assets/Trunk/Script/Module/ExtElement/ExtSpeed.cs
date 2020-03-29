using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 速度拓展元素
/// </summary>
public class ExtSpeed :ExtElement
{
    public override int guid { get { return ExtElementCfg.SPEED_EXT; } }
    protected override float maxKeepTime { get { return 10; } }
    public float addValue = 2;
    public override void OnUse()
    {
        if(holder.syncType==SyncType.UpLoad)
           holder.moveSpeed += addValue;

        Debug.Log("激活速度装置");

    }
    public override void OnOverLay()
    {
        curKeepTime = 0;
        Debug.Log("装置时间增长");
    }
    public override void OnEnd()
    {
        if (holder.syncType == SyncType.UpLoad)
            holder.moveSpeed -= addValue;

        Debug.Log("速度装置失效");
    }
}
