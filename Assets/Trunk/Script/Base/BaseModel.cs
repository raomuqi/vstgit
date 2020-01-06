using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBaseModel
{
    void Init();
    void Clear();
}
public abstract class BaseModel :IBaseModel 
{
    /// <summary>
    /// 自动调用
    /// </summary>
    public void Clear()
    {
        OnClear();
    }
    /// <summary>
    /// 自动调用
    /// </summary>
    public void Init()
    {
        OnInit();
    }
    protected virtual void OnClear(){}
    protected virtual void OnInit() { }
}
