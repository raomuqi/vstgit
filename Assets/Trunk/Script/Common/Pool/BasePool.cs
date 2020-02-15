using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasePool<T>
{
    Queue<T> objectList = new Queue<T>();
    public void Recycle(T obj)
    {
        OnRecycle(obj);
        objectList.Enqueue(obj);
    }

    public T GetObj()
    {
        T obj = objectList.Dequeue();
        OnGet(obj);
        return obj;
    }
    protected virtual void OnRecycle(T obj) { }
    protected virtual void OnGet(T obj) { }
}