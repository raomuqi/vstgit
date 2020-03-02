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

    protected override void OnInit()
    {
        if (Connection.GetInstance().isHost)
        {
            VRChairSDK.GetInstance().Init();
            VRChairSDK.GetInstance().RegisterBtnChangeCallback(onBtnDown);
        }
        model = InputController.instance.GetModel<InputModel>(InputModel.name);
        Global.instance.AddUpdateFunction(UpdateInput);

    }
    protected override void OnClear()
    {
        if (Connection.GetInstance().isHost)
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

        model.horizontal = Input.GetAxis(INPUT_HORIZONTAL);
        model.vertical = Input.GetAxis(INPUT_VERTICAL);
        bool changeInput = false;
        if (Input.GetButtonDown(INPUT_FIRE1))
        {
            model.selfFires[0] = 1;
            changeInput = true;
        }
        else if (Input.GetButtonUp(INPUT_FIRE1))
        {
            model.selfFires[0] = 0;
            changeInput = true;
        }
        if (Input.GetButtonDown(INPUT_FIRE2))
        {
            model.selfFires[1] = 1;
            changeInput = true;
        }
        else if (Input.GetButtonUp(INPUT_FIRE2))
        {
            model.selfFires[1] = 0;
            changeInput = true;
        }
        if(changeInput)
           SyncController.instance.SendNetMsg(ProtoIDCfg.SYNC_INPUT);


    }


}
