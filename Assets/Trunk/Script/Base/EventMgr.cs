using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全局事件
/// </summary>
public static class EventMgr 
{
    static Dictionary<string, EventCallBack> events=new Dictionary<string, EventCallBack>();

    /// <summary>
    /// 还回移除的action
    /// </summary>
    public static Action AddEvent(string cmd, EventCallBack cb)
    {
        EventCallBack actions;
        if (events.TryGetValue(cmd, out actions))
        {
            actions += cb;
        }
        else
        {
            events.Add(cmd, cb);
        }
        return () => {
                actions -= cb;
          };
    }
    public static void RemoveEvent(string cmd, EventCallBack cb)
    {
        EventCallBack actions;
        if (events.TryGetValue(cmd, out actions))
        {
            actions -= cb;
        }
      
    }

    public static void FireEvent(string cmd, EventArgs args=null)
    {
        EventCallBack cbs;
        if (events != null && events.TryGetValue(cmd, out cbs))
        {
            cbs(args);
        }
    }
}
