using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : SceneGameObject
{
   public BaseEmitter emitter;

    /// <summary>
    /// 0关 1开
    /// </summary>
    public virtual void SetFire(byte fireStatue,Vector3 dir)
    {
        if (emitter != null)
            emitter.Fire(fireStatue, dir);
    }
    public void SetTag(string tag)
    {
        emitter.SetTag(tag);
    }
}
