using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotiLib
{
     Dictionary<string, EventsWarp> events=new Dictionary<string, EventsWarp>();

    public  Action AddEvent(string cmd, EventCallBack cb)
    {
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

    public  void RemoveEvent(string cmd, EventCallBack cb)
    {
        EventsWarp warp = null;
        if (events.TryGetValue(cmd, out warp))
        {
            warp.cbs -= cb;
            if (warp.cbs == null)
                events.Remove(cmd);
        }
      
    }

    public  void FireEvent(string cmd, EventArgs args=null)
    {
        EventsWarp warp = null;
        if (events.TryGetValue(cmd, out warp))
        {
            if(warp.cbs!=null)
            warp.cbs(args);
        }
    }

    public void Clear()
    {
        events.Clear();
    }
}
