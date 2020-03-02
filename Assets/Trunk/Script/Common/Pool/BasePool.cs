using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasePool<T>
{
    Dictionary<int, Queue<T>> objectList = new Dictionary<int, Queue<T>>();
    protected virtual void OnRecycle(T obj) { }
    public void Recycle(int key, T obj)
    {
        Queue<T> queue;
        if (!objectList.TryGetValue(key, out queue))
        {
            queue = new Queue<T>();
            objectList.Add(key, queue);
        }
        queue.Enqueue(obj);
        OnRecycle(obj);
    }

    public T GetObj(int key)
    {
        Queue<T> queue;
        T obj=default(T);
        if (objectList.TryGetValue(key, out queue))
        {
            if(queue.Count>0)
            obj = queue.Dequeue();
        }
        return obj;
    }

}