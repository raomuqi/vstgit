using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommand : BaseCommand
{
   
    protected override void OnInit()
    {
        VRChairSDK.GetInstance().Init();
        VRChairSDK.GetInstance().RegisterBtnChangeCallback(onBtnDown);
    }

    public void onBtnDown(byte index,byte status)
    {

    }


    protected override void OnClear()
    {
        VRChairSDK.GetInstance().Dispose();
    }
}
