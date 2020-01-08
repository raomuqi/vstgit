

using System;

/// <summary>
/// 全局事件
/// </summary>
public static class EventsMgr 
{
    public static NotiLib notiLib=new NotiLib();
    public static Action AddEvent(string cmd, EventCallBack cb)
    {
        return notiLib.AddEvent(cmd, cb);
    }

    public static void RemoveEvent(string cmd, EventCallBack cb)
    {
         notiLib.RemoveEvent(cmd, cb);
    }

    public static void FireEvent(string cmd, EventArgs args = null)
    {
        notiLib.FireEvent(cmd, args);
    }
}
