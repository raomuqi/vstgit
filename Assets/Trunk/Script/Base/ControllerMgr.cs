using System.Collections.Generic;


public static class ControllerMgr
{
    public static List<IController> s_lstModules = new List<IController>();

    public static void AddModule(IController module)
    {
        if (s_lstModules.IndexOf(module) == -1)
        {
            s_lstModules.Add(module);
        }
    }

    public static void RemoveModule(IController module)
    {
        if (s_lstModules.Contains(module))
        {
            s_lstModules.Remove(module);
        }
    }

}
