using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEmitter : MonoBehaviour
{
    BaseGun gun;
    protected string tagetTag = string.Empty;
    protected virtual void OnFire(byte fireStatue, Vector3 dir) { }
    protected virtual void OnUpdate() { }
    public void Fire(byte fireStatue,Vector3 dir)
    {
        OnFire(fireStatue, dir);
    }
    public void SetTag(string tag)
    {
        tagetTag=tag;
    }
    public void Init(BaseGun inputGun)
    {
        gun = inputGun;
    }
    private void Update()
    {
        OnUpdate();
    }
}
