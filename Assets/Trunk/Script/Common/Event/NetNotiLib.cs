using System;
using System.Collections.Generic;

public delegate void ProtoCallBack(byte[] proto);



public class NetNotiLib<T> 
{
    public class NetEventsWarp
    {
        public NetEventsWarp(ProtoCallBack cbs)
        {
            this.cbs = cbs;
        }
        public ProtoCallBack cbs;
    }

    Dictionary<T, NetEventsWarp> events;
    public Action AddEvent(T cmd, ProtoCallBack cb)
    {
        if (events == null)
            events = new Dictionary<T, NetEventsWarp>();
        NetEventsWarp warp = null;
        if (events.TryGetValue(cmd, out warp))
        {
            warp.cbs += cb;
        }
        else
        {
            events.Add(cmd, new NetEventsWarp(cb));
        }
        return () =>
        {
            RemoveEvent(cmd, cb);
        };
    }

    public void RemoveEvent(T cmd, ProtoCallBack cb)
    {
        if (events != null)
        {
            NetEventsWarp warp = null;
            if (events.TryGetValue(cmd, out warp))
            {
                warp.cbs -= cb;
                if (warp.cbs == null)
                    events.Remove(cmd);
            }
        }
    }

    public void FireEvent(T cmd, byte[] proto = null)
    {
        if (events != null)
        {
            NetEventsWarp warp = null;
            if (events.TryGetValue(cmd, out warp))
            {
                if (warp.cbs != null)
                    warp.cbs(proto);
            }
        }
    }

    public void Clear()
    {
        if (events != null)
            events.Clear();
    }
}

