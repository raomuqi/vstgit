using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 拓展元素-修改场景对象属性
/// </summary>
public class ExtElement 
{
    public virtual int guid { get { return -1; } }
    public int id = 0;
    protected float curKeepTime = 0;
    protected virtual float maxKeepTime { get; }
    bool ended = false;
   protected  SceneGameObject holder;
  
    public virtual void OnUse() { }
    public virtual void OnUpdate() { }
    public virtual void OnEnd() { }
    /// <summary>
    /// 已有次拓展时
    /// </summary>
    public virtual void OnOverLay()
    {
        //时间重置
        curKeepTime = 0;
    }
    public  void Use(SceneGameObject holder)
    {
        //叠加效果
        if (holder.extList[guid] != null)
        {
            holder.extList[guid].OverLay();
            return;
        }
        //启用拓展
        else
        {
            this.holder = holder;
            ended = false;
            OnUse();
        }
        //非持续类型直接结束
        if (maxKeepTime == 0)
        {
            End();
        }
        else
        {
            holder.extList[guid] = this;
            curKeepTime = 0;
        }
    }
    public void OverLay()
    {
        OnOverLay();
    }
    public  void Update()
    {
        if (ended) return;
        curKeepTime += Time.deltaTime;
        OnUpdate();
        if (curKeepTime >= maxKeepTime)
        {
            End();
        }
    }
    public  void End()
    {
        ended = true;
        OnEnd();
        holder.extList[guid] = null;
    }
}
