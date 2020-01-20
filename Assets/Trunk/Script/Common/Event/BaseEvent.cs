
using UnityEngine;


public delegate void EventCallBack(EventArgs args);

public class EventsWarp
{
    public EventsWarp(EventCallBack cbs)
    {
        this.cbs = cbs;
    }
    public EventCallBack cbs;
}


public  class EventArgs
{
}
public class EventFloatArgs: EventArgs
{
    public float t;
}

public class EventVector3Args : EventArgs
{
    public Vector3 t;
}
public class EventObjectArgs : EventArgs
{
    public object t;
}
