using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成拓展元素实例
/// </summary>
public  class ExtElementFactory
{
    public static ExtElement Get(int elementID,int[] parameter)
    {
        ExtElement element=null;
        switch (elementID)
        {
            case ExtElementCfg.SPEED_EXT:
                element = new ExtSpeed();
                break;
            case ExtElementCfg.GUN_ROW_EXT:
                element = new ExtUpGradeGun();
                break;
            case ExtElementCfg.GUN_CHANGE_BULLET:
                element = new ExtChangeBullet();
                break;
        }
        if (element != null)
            element.SetParameter(parameter);
        return element;
    }
}
