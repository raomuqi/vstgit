using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoPool
{
    public enum ProtoRecycleType
    {
        None,
        SyncObject,
        Int,
        PlayerInfo,
        String,
        IntArray,
        CreateObject,
        CreateObjects
    }
    Dictionary<ProtoRecycleType, Stack<ProtoBase>> pool = new Dictionary<ProtoRecycleType, Stack<ProtoBase>>();

    public void Recycle(ProtoRecycleType type, ProtoBase proto)
    {
        Stack<ProtoBase> stack;
        if (!pool.TryGetValue(type, out stack))
        {
            stack = new Stack<ProtoBase>();
            stack.Push(proto);
        }
        stack.Push(proto);
    }

    public T GetOrCreate<T>(ProtoRecycleType type) where T : ProtoBase, new()
    {
        T result=Get<T>(type);
        if (result == null)
            result = new T();
        return result;

    }
    public T Get<T>(ProtoRecycleType type) where T : ProtoBase, new()
    {
        Stack<ProtoBase> stack;
        T result = default(T);
        if (pool.TryGetValue(type, out stack))
        {
            ProtoBase p = null;
            p = stack.Pop();
            if (p != null)
                result = p as T;
        }

        return result;

    }
}
