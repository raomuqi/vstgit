using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class CfgGenerate 
{
    [MenuItem("配置工具/生成默认配置")]
    public static void GenerateExposeCfg()
    {
        try
        {
            string path = AppCfg.CfgPath;
            ExposeCfg cfg = new ExposeCfg();
            string json = JsonUtility.ToJson(cfg);
            File.WriteAllText(path, json);
            Application.OpenURL(path);
            Debug.Log(path);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    [MenuItem("配置工具/删除默认配置")]
    public static void DeleteExposeCfg()
    {
        string path = AppCfg.CfgPath;

        try
        {
            File.Delete(path);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
      
    }

    [MenuItem("配置工具/打开配置")]
    public static void OpenExposeCfg()
    {
        string path = AppCfg.CfgPath;
        try
        {
            Application.OpenURL(path);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    [MenuItem("配置工具/打开配置文件夹")]
    public static void OpenExposeCfgFolder()
    {
        string path = AppCfg.CfgPath;
        try
        {
            Application.OpenURL(Path.GetDirectoryName(path));
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }

  
}
