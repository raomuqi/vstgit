using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolForGameObject : BasePool<GameObject>
{
    Transform root;
    public PoolForGameObject()
    {
        GameObject go= new GameObject("[GameObjectPool]");
        root = go.transform;
        GameObject.DontDestroyOnLoad(root);
    }
    protected override void OnRecycle(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(root);
    }
}