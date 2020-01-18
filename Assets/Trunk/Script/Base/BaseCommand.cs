
using System.Collections.Generic;


public abstract class BaseCommand  
{
    NotiLib<string> eventLib;
    public void FireCommand(string cmd, EventArgs args)
    {
        if (eventLib != null)
            eventLib.FireEvent(cmd, args);
    }
    public void AddCommand(string cmd, EventCallBack cb)
    {
        if (eventLib == null)
            eventLib =new NotiLib<string>();
        eventLib.AddEvent(cmd, cb);
  
    }
    public void RemoveCommand(string cmd, EventCallBack cb)
    {
        if (eventLib != null)
            eventLib.RemoveEvent(cmd, cb);
    }

   


    public void Init()
    {
        OnInit();
    }
    public void Clear()
    {
        eventLib = null;
        OnClear();
    }
 
  


    protected virtual void OnInit() { }
    protected virtual void OnClear() { }
}
