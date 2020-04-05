using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleChecker : MonoBehaviour
{
    SceneGameObject sgo;
    public void Init(SceneGameObject sgo)
    {
        this.sgo = sgo;
    }

    private void OnBecameVisible()
    {
        if (sgo) 
            sgo.SetVisible(true);
    }

    private void OnBecameInvisible()
    {
        if (sgo)
            sgo.SetVisible(false);
    }

}
