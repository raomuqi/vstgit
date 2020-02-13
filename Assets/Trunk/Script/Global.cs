using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Global : MonoBehaviour {
     Connection connection;
    System.Action OnUpdate;
    public static Global instance;
       // Use this for initialization
    void Awake ()
    {
        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
        InitAppCfg();
        if (AppCfg.expose.IsDebuger == 1)
            InitDebugeTool();
        InitConnection();
        InitModule();
       
    
    }

    private void Start()
    {
         PlayerController.instance.SendNetMsg(ProtoIDCfg.LOGIN);
    }
    void Update ()
    {
        //更新输入
        InputController.instance.FireCommand(InputCommand.UPDATE_INPUT);
        connection.OnUpdate();
        if (OnUpdate != null)
            OnUpdate();


    }

    /// <summary>
    /// 注册Update回调
    /// </summary>
    public void AddUpdateFunction(System.Action fun)
    {
        OnUpdate += fun;
    }
    /// <summary>
    /// 移除Update回调
    /// </summary>
    public void RemoveUpdateFunction(System.Action fun)
    {
        OnUpdate -= fun;
    }
    void InitConnection()
    {
        connection = Connection.GetInstance();
        connection.Init();
    }

    /// <summary>
    /// 初始化配置
    /// </summary>
    void InitAppCfg()
    {
        try
        {
            string json = File.ReadAllText(AppCfg.CfgPath);
            ExposeCfg expose = JsonUtility.FromJson<ExposeCfg>(json);
            AppCfg.expose = expose;
            Debug.Log("获取应用配置成功");
        }
        catch(Exception e)
        {
            Debug.LogWarning("获取应用配置失败"+e.Message);
            AppCfg.expose = new ExposeCfg();
            string json = JsonUtility.ToJson(AppCfg.CfgPath);
            File.WriteAllText(AppCfg.CfgPath, json);
            Debug.LogWarning(AppCfg.CfgPath);
        }

    }
    /// <summary>
    /// 调试工具
    /// </summary>
    void InitDebugeTool()
    {
        ResourceMgr.GetInstance().Load("LogView",
              (loadObject) =>
              {
                  GameObject logView = Instantiate(loadObject) as GameObject;
                  logView.transform.SetParent(transform);
              });
      
    }
    /// <summary>
    /// 初始化模块
    /// </summary>
    void InitModule()
    {
        //输入模块
        InputController.instance.InitModule();
        //玩家模块
        PlayerController.instance.InitModule();
        //场景模块
        SceneController.instance.InitModule();
    }
  
    private void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
            connection = null;
        }
    }
}
