
using System.Collections.Generic;


public abstract class BaseCommand  
{
    Dictionary<string, EventCallBack> events;
    public void FireCommand(string cmd, EventArgs args)
    {

        EventCallBack cbs;
        if (events != null && events.TryGetValue(cmd,out cbs))
        {
            cbs(args);
        }
    }
    public void AddCommand(string cmd, EventCallBack cb)
    {
        if (events == null)
            events = new Dictionary<string, EventCallBack>();
        EventCallBack actions;
        if (events.TryGetValue(cmd, out actions))
        {
            actions += cb;
        }
        else
        {
            events.Add(cmd, cb);
        }
    }
    public void RemoveCommand(string cmd, EventCallBack cb)
    {
        EventCallBack output;
        if (events.TryGetValue(cmd, out output))
        {
            output -= cb;
        }
    }

   


    public void Init()
    {
        OnInit();
    }
    public void Clear()
    {
        OnClear();
    }
 
  


    protected virtual void OnInit() { }
    protected virtual void OnClear() { }
}
