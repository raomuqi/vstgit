using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr :MonoBehaviour
{
    static ResourceMgr instance = null;
    public static ResourceMgr GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("[ResourceMgr]");
            instance = go.AddComponent<ResourceMgr>();
        }
        return instance;
    }
    public System.Action Load(string path,System.Action<Object> cb)
    {
        Coroutine co = StartCoroutine(IELoad(path,cb));
        return () => { StopCoroutine(co); };
    }
    IEnumerator IELoad(string path, System.Action<Object> cb)
    {
        yield return null;
        Object obj = Resources.Load(path);
        if (cb != null)
            cb(obj);
    }
    public System.Action PreLoad(string[] paths, System.Action cb)
    {
        Coroutine co = StartCoroutine(IEPreLoad(paths, cb));
        return () => { StopCoroutine(co); };
    }
    IEnumerator IEPreLoad(string[] paths, System.Action cb)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
            yield return rr;
        }
        if (cb != null)
            cb();
    }
    public System.Action LoadAsync(string path, System.Action<Object> cb)
    {
      Coroutine co=   StartCoroutine(IELoadAsync(path, cb));
        return () => { StopCoroutine(co); };
    }
    IEnumerator IELoadAsync(string path, System.Action<Object> cb)
    {
        ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
        yield return rr;
        if (cb != null)
            cb(rr.asset);
    }
}
