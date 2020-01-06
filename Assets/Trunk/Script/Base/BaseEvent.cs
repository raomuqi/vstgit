
using UnityEngine;


public delegate void EventCallBack(EventArgs args);

public  class EventArgs
{
    public object[] data;
}
public class EventFloatArgs: EventArgs
{
    public float t;
}

public class EventVector3Args : EventArgs
{
    public Vector3 t;
}