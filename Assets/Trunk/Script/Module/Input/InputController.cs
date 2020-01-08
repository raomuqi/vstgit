using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController: BaseController<InputController>
{
    protected override void OnInit()
    {
            RegisterCommand(new InputCommand());
            RegisterModel(InputModel.name, new InputModel());
    }
    protected override void OnClose()
    {


    }
}
