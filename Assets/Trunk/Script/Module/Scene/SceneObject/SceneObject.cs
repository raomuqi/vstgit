using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject
{
    public SyncObject sync;

    public int objectID;
    public SceneObject(){}
    SceneGameObject sgo;
    public SceneObject(SceneGameObject sgo)
    {
        this.sgo = sgo;
    }

}
