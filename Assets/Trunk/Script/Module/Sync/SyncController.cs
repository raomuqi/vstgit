using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncController : BaseController<SyncController>
{
    protected override void OnInit()
    {
        RegisterModel(SyncModel.name, new SyncModel());
        RegisterCommand(new SyncCommand());
        RegisterNetHandler(new SyncNetHandler());
    }
    protected override void OnClose()
    {
    }
  
}
