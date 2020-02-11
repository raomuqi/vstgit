using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : BaseController<SceneController>
{
    protected override void OnInit()
    {
        RegisterModel(SceneModel.name, new SceneModel());
        RegisterNetHandler(new SceneNetHandler());
        RegisterCommand(new SceneCommand());
    }
}
