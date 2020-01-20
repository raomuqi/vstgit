
using System.Collections.Generic;
using UnityEngine;
public interface IController
{
    void FireCommand(string cmd, EventArgs args);
    void RegisterCommand(BaseCommand command);
    void Close();
    void ReStart();
    T GetModel<T>(string modelName) where T : BaseModel;
}

public abstract class BaseController<T>: IController where T : class, new()
{
    List<BaseCommand> commandList ;
    List<BaseNetHandler> netList;
    Dictionary<string, BaseModel> modelList;
    private static T s_instance;
    public static T instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new T();
            }
            return s_instance;
        }
    }

    public void InitModule()
    {
        commandList = new List<BaseCommand>();
        netList = new List<BaseNetHandler>();
        modelList = new Dictionary<string, BaseModel>();
        ControllerMgr.AddModule(s_instance as IController);
        OnInit();
        OnInited();
    }
    /// <summary>
    /// 添加Command Model
    /// </summary>
     protected virtual void OnInit(){}
    protected virtual void OnClose() { }
    void OnInited()
    {
        foreach (var model in modelList)
        {
            model.Value.Init();
        }
        for (int i = 0; i < commandList.Count; i++)
        {
            commandList[i].Init();
        }
        for (int i = 0; i < netList.Count; i++)
        {
            netList[i].Init();
        }
    }

    public void SendNetMsg(byte cmd, EventArgs args=null)
    {
        for (int i = 0; i < netList.Count; i++)
        {
            netList[i].SendProto(cmd,args);
        }
    }
    /// <summary>
    /// 触发 Command
    /// </summary>
    public void FireCommand(string cmd, EventArgs args=null)
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            BaseCommand command = commandList[i];
            command.FireCommand(cmd, args);
        }
    }
    /// <summary>
    ///注册  Command
    /// </summary>
    public void RegisterCommand(BaseCommand command)
    {
        if (!commandList.Contains(command))
        {
            commandList.Add(command);
        }
    }
    /// <summary>
    ///注册  NetHandle
    /// </summary>
    public void RegisterNetHandler(BaseNetHandler handler)
    {
        if (!netList.Contains(handler))
        {
            netList.Add(handler);
        }
    }
    /// <summary>
    /// 注册model
    /// </summary>
    public void RegisterModel(string modelName, BaseModel model)
    {
        if (modelList.ContainsKey(modelName))
        {
            Debug.LogWarning("重复注册Model:" + modelName);
        }
        else
        {
            modelList.Add(modelName,model);
        }
    }

    public void RemoveModel(string modelName)
    {
        if (modelList.ContainsKey(modelName))
        {
            modelList.Remove(modelName);
        }
    }

    public T GetModel<T>(string modelName) where T : BaseModel
    {
        return (T)modelList[modelName];
    }

    public void Close()
    {
        OnClose();
        ControllerMgr.RemoveModule(s_instance as IController);
        for (int i = 0; i < commandList.Count; i++)
        {
            commandList[i].Clear();
        }
        for (int i = 0; i < netList.Count; i++)
        {
            netList[i].Clear();
        }
        foreach (var model in modelList)
        {
            model.Value.Clear();
        }
        commandList = null;
        modelList = null;
        netList = null;
    }

    public void ReStart()
    {
        Close();
        InitModule();
    }
}
