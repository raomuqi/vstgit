using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public SceneGameObject master;
    public int pookKey { get; set; }
    protected Vector3 dir;
    
    public string tagetTag { get; set; }
    public int power = 1;

    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }
    protected virtual void OnRecycle() { }
    protected virtual void OnReset() { }
    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }
    public void ResetBullet()
    {
        OnReset();
    }
    public void Recycle()
    {
        OnRecycle();
        if (pookKey != -1)
        {
            ObjectPool.goPool.Recycle(pookKey, gameObject);
        }
        else
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
 
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
}
