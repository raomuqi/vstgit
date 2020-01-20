using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolForGameObject : BasePool<GameObject>
{
    GameObject root;
    public PoolForGameObject()
    {
        root = new GameObject("[GameObjectPool]");
        GameObject.DontDestroyOnLoad(root);
    }
    protected override void OnRecycle(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(root.transform);
    }
}