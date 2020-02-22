using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : SceneGameObject
{
    InputModel inputModel;
    protected override void OnAwake()
    {
    }
    protected override void OnStart()
    {
        inputModel = InputController.instance.GetModel<InputModel>(InputModel.name);
    }
    protected override void OnUpdate()
    {
        if (syncType != SyncType.UpDate)
        {
            float h = Mathf.Lerp(-90, 90, (inputModel.horizontal + 1f) * 0.5f);
            float v = Mathf.Lerp(-90, 90, (inputModel.vertical + 1f) * 0.5f);
            transform.localEulerAngles = new Vector3(-v, h, 0);
        }
    }
}
