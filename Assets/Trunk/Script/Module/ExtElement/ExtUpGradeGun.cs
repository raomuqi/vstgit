using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 发射器增加一排子弹
/// </summary>
public class ExtUpGradeGun : ExtElement
{
    public override int guid { get { return ExtElementCfg.GUN_ROW_EXT; } }
    protected override float maxKeepTime { get { return 0; } }
    public int addValue = 10;
    public override void SetParameter(int[] par)
    {
        addValue = par[4];
    }
    public override void OnUse()
    {
       BaseGun gun= holder as BaseGun;
        if (gun != null && gun.emitter!=null)
            gun.emitter.UpGrade(addValue);
        Debug.Log("升级子弹"+holder.gameObject.name);

    }
    public override void OnOverLay()
    {
      
    }
    public override void OnEnd()
    {
      

     
    }
}
