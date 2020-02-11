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
    protected override void OnInit()
    {
        VRChairSDK.GetInstance().Init();
        model = InputController.instance.GetModel<InputModel>(InputModel.name);
        VRChairSDK.GetInstance().RegisterBtnChangeCallback(onBtnDown);
        AddCommand(UPDATE_INPUT, UpdateInput);
    }
    protected override void OnClear()
    {
        VRChairSDK.GetInstance().Dispose();
    }
    public void onBtnDown(byte index,byte status)
    {

    }
    /// <summary>
    /// 更新输入
    /// </summary>
    /// <param name="args"></param>
    public void UpdateInput(EventArgs args)
    {
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
    }

  
}
