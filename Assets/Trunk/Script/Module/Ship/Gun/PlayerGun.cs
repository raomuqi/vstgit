using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : BaseGun
{
    InputModel inputModel;
    /// <summary>
    /// 按钮索引
    /// </summary>
    public int fireIndex = 0;
    byte[] fires;
    protected override void OnAwake()
    {
    }
    protected override void OnStart()
    {
        emitter.SetTag(TagCfg.AI);
    }
    protected override void OnSetSync(SyncType type)
    {
        if (syncType == SyncType.UpLoad)
        {
            inputModel = InputController.instance.GetModel<InputModel>(InputModel.name);
            fires = inputModel.selfFires;
        }
        else if (syncType == SyncType.UpDate)
        {
            inputModel = InputController.instance.GetModel<InputModel>(InputModel.name);
            fires = syncModel.GetPosInput((byte)controlPos);
        }
    }
    protected override void OnUpdate()
    {
        if (syncType == SyncType.UpLoad)
        {
            SetFire(fires[fireIndex], transform.forward);
        }
        else if (syncType == SyncType.UpDate)
        {
            SetFire(fires[fireIndex], transform.forward);
        }
    }
  
}
