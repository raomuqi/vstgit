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
        SetTag(TagCfg.AI, this);
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
            // float h = Mathf.Lerp(-90, 90, (inputModel.horizontal + 1f) * 0.5f);
            // float v = Mathf.Lerp(-90, 90, (inputModel.vertical + 1f) * 0.5f);
            // transform.localEulerAngles = new Vector3(-v, h, 0);
            SetFire(fires[fireIndex], transform.forward);
        }
        else if (syncType == SyncType.UpDate)
        {
            SetFire(fires[fireIndex], transform.forward);
        }
    }
    public override void OnGetAction(int[] intArray)
    {
        int action = intArray[1];
        switch (action)
        {
            case SceneObjectActionCfg.GET_PROP:
                int scrObject = intArray[2];
                int extID = intArray[3];
                   ExtElementFactory.Get(extID,intArray).Use(sceneModel.GetPlayerShip());
                break;
        }
    }
}
