using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : SceneGameObject
{

    SceneModel sceneModel;
    private void OnStart()
    {
        sceneModel = SceneController.instance.GetModel<SceneModel>(SceneModel.name);
        sceneModel.SetPlayerShip(this);
    }
  
}
