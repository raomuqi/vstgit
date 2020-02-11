using System;
using System.Collections.Generic;

public class NotiLib<T>
{
    Dictionary<T, EventsWarp> events;
    public  Action AddEvent(T cmd, EventCallBack cb)
    {
        if (events == null) 
            events = new Dictionary<T, EventsWarp>();
        EventsWarp warp =null;
        if (events.TryGetValue(cmd, out warp))
        {
            warp.cbs += cb;
        }
        else
        {
            events.Add(cmd, new EventsWarp(cb));
        }
        return () => {
            RemoveEvent(cmd,cb);
          };
    }

    public  void RemoveEvent(T cmd, EventCallBack cb)
    {
        if (events!=null)
        {
            EventsWarp warp = null;
            if (events.TryGetValue(cmd, out warp))
            {
                warp.cbs -= cb;
                if (warp.cbs == null)
                    events.Remove(cmd);
            }
        }
    }

    public  void FireEvent(T cmd, EventArgs args=null)
    {
        if (events != null)
        {
            EventsWarp warp = null;
            if (events.TryGetValue(cmd, out warp))
            {
                if (warp.cbs != null)
                    warp.cbs(args);
            }
        }
    }

    public void Clear()
    {
        if (events != null)
            events.Clear();
    }
}
