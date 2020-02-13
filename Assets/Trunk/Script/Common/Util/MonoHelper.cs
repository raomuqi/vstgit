using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoHelper : MonoBehaviour
{
    static MonoHelper instance;
    EventLoadSceneArgs loadSceneArgs;
    AsyncOperation loadSceneAsy;
    Coroutine loadSceneCor;
    public static MonoHelper GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("[MonoHelper]");
            GameObject.DontDestroyOnLoad(go);
           // go.hideFlags = HideFlags.HideAndDontSave;
            instance = go.AddComponent<MonoHelper>();
        }
        return instance;
    }
    private void Update()
    {
        if (loadSceneArgs != null && loadSceneAsy!=null)
        {
            if(loadSceneArgs.progress!=null)
                loadSceneArgs.progress(loadSceneAsy.progress);
        }
    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    public void LoadSceneAsync(EventLoadSceneArgs loadSceneCfg)
    {
        if (loadSceneCor != null)
            StopCoroutine(loadSceneCor);
          loadSceneCor = StartCoroutine(LoadScene(loadSceneCfg));
    }
    IEnumerator LoadScene(EventLoadSceneArgs loadSceneCfg)
    {
        loadSceneArgs = loadSceneCfg;
         loadSceneAsy = SceneManager.LoadSceneAsync(loadSceneArgs.index);
        yield return loadSceneAsy;
        if (loadSceneArgs != null && loadSceneArgs.complete != null)
            loadSceneArgs.complete(loadSceneCfg.index);
        yield return null;
        loadSceneArgs = null;
        loadSceneAsy = null;
        loadSceneCor = null;

    }
   
    /// <summary>
    /// 开启携程
    /// </summary>
    public Coroutine StartCoroutineInMono(IEnumerator routine)
    {
        Coroutine cor= StartCoroutine(routine);
        return cor;
    }
    /// <summary>
    /// 关闭携程
    /// </summary>
    public void StopCoroutineInMono(Coroutine cor)
    {
        StopCoroutine(cor);
    }

    public Action Delay(float delayTime,System.Action action,int invokeTime=1)
    {
        Coroutine co=  StartCoroutine(IEDelay(delayTime, action, invokeTime));
        return () => { StopCoroutine(co); };
    }
    IEnumerator IEDelay(float delayTime, System.Action action, int invokeTime)
    {
        for (int i = 0; i < invokeTime; i++)
        {
            yield return new WaitForSeconds(delayTime);
            action();
        }

    }
}
