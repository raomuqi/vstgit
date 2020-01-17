using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Global : MonoBehaviour {
     Connection connection;
       // Use this for initialization
    void Awake ()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        InitAppCfg();
        InitConnection();
        InitModule();
	}
  
    void Update ()
    {
        //更新输入
        InputController.instance.FireCommand(InputCommand.UPDATE_INPUT);
    }
    void InitConnection()
    {
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
            Debug.LogWarning("获取应用配置成功");
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
    /// 初始化模块
    /// </summary>
    void InitModule()
    {
        //输入模块
        InputController.instance.InitModule();

    }
}
