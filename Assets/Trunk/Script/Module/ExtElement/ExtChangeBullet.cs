using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtChangeBullet : ExtElement
{
    public override int guid { get { return ExtElementCfg.GUN_CHANGE_BULLET; } }
    protected override float maxKeepTime { get { return 0; } }
    public int bulletID = 0;
    public override void SetParameter(int[] par)
    {
        bulletID = par[4];
    }
    public override void OnUse()
    {
        BaseGun gun = holder as BaseGun;
        if (gun != null && gun.emitter != null)
            gun.emitter.ChangeBullet(SyncCreater.instance.bulletArray.bulletArray[bulletID]);
        Debug.Log(holder.gameObject.name+"换弹" +  bulletID);

    }
    public override void OnOverLay()
    {

    }
    public override void OnEnd()
    {



    }
}