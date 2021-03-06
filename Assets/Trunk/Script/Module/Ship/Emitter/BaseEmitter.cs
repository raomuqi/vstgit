﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEmitter : MonoBehaviour
{
    public SceneGameObject master;
    protected string tagetTag = string.Empty;
    protected virtual void OnFire(byte fireStatue, Vector3 dir) { }
    protected virtual void OnUpdate() { }
    public void Fire(byte fireStatue, Vector3 dir)
    {
        OnFire(fireStatue, dir);
    }

    public void SetTag(string tag, SceneGameObject master)
    {
        tagetTag = tag;
        this.master = master;
    }

    private void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// 更换子弹
    /// </summary>
    public virtual void ChangeBullet(GameObject prefab) { }
    /// <summary>
    /// 升级
    /// </summary>
    public virtual void UpGrade(int addValue) { }
}
