using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController<PlayerController>
{
    protected override void OnInit()
    {
        RegisterModel(PlayerModel.name, new PlayerModel());
        RegisterNetHandler(new PlayerNetHandler());
    }
}
