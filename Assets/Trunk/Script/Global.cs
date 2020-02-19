using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Global : MonoBehaviour {
   public Connection connection { get; set; }
    System.Action OnUpdate;
    public static Global instance;
       // Use this for initialization
    void Awake ()
    {
        instance = this;
        Application.runInBackground = true;
        GameObject.DontDestroyOnLoad(gameObject);
        InitAppCfg();
        if (AppCfg.expose.IsDebuger == 1)
            InitDebugeTool();
        InitConnection();
        InitModule();
       
    
    }

    private void Start()
    {
    }
    void Update ()
    {
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
        connection.onConnect += OnConnectSuccess;
    }
    /// <summary>
    /// 连接成功
    /// </summary>
    void OnConnectSuccess()
    {
        PlayerController.instance.SendNetMsg(ProtoIDCfg.LOGIN);
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
            ExposeCfg cfg = new ExposeCfg();
            string json = JsonUtility.ToJson(cfg);
            AppCfg.expose = cfg;
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
        //同步模块
        SyncController.instance.InitModule();
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
