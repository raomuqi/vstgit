using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommand : BaseCommand
{
    public const string UPDATE_INPUT= "UpdateInput";

    const string INPUT_HORIZONTAL = "Horizontal";
    const string INPUT_VERTICAL = "Vertical";
    const string INPUT_FIRE1 = "Fire1";
    const string INPUT_FIRE2 = "Fire2";

    InputModel model;
    SyncModel syncModel;
#if UNITY_EDITOR
    UnityEngine.Profiling.CustomSampler sampler;
#endif
    protected override void OnInit()
    {
        syncModel = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        if (syncModel.IsUploader())
        {
            VRChairSDK.GetInstance().Init();
            VRChairSDK.GetInstance().RegisterBtnChangeCallback(onBtnDown);
        }
        model = InputController.instance.GetModel<InputModel>(InputModel.name);
        Global.instance.AddUpdateFunction(UpdateInput);
#if UNITY_EDITOR
        sampler = UnityEngine.Profiling.CustomSampler.Create("UpdateInput");
#endif
    }
    protected override void OnClear()
    {
        if (syncModel.IsUploader())
        {
            VRChairSDK.GetInstance().Dispose();
        }
        Global.instance.RemoveUpdateFunction(UpdateInput);
    }
    public void onBtnDown(byte index,byte status)
    {

    }
    /// <summary>
    /// 更新输入
    /// </summary>
    public void UpdateInput()
    {
#if UNITY_EDITOR
        sampler.Begin();
#endif
        model.horizontal = Input.GetAxis(INPUT_HORIZONTAL);
        model.vertical = Input.GetAxis(INPUT_VERTICAL);

        if (Input.GetButtonDown(INPUT_FIRE1))
            EventsMgr.FireEvent(EventName.INPUT_BTN1_DOWN);
        else if (Input.GetButton(INPUT_FIRE1))
            EventsMgr.FireEvent(EventName.INPUT_BTN1_DOWNING);
        else if (Input.GetButtonUp(INPUT_FIRE1))
            EventsMgr.FireEvent(EventName.INPUT_BTN1_UP);
        if (Input.GetButtonDown(INPUT_FIRE2))
            EventsMgr.FireEvent(EventName.INPUT_BTN2_DOWN);
        else if (Input.GetButton(INPUT_FIRE2))
            EventsMgr.FireEvent(EventName.INPUT_BTN2_DOWNING);
        else if (Input.GetButtonUp(INPUT_FIRE2))
            EventsMgr.FireEvent(EventName.INPUT_BTN2_UP);
#if UNITY_EDITOR
        sampler.End();
#endif
    }


}
