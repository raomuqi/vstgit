using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject
{
    SyncObject _sync;
    public SyncObject sync
    {
        get { if (_sync == null)
                _sync = new SyncObject();
            return _sync;
                    }
    }

    public SceneObject(){}
    SceneGameObject sgo;
    public SceneObject(SceneGameObject sgo)
    {
        this.sgo = sgo;
    }
  
}
