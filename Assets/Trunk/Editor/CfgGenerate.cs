using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public static class CfgGenerate 
{
    [MenuItem("配置工具/生成默认配置")]
    public static void GenerateExposeCfg()
    {
        string path = AppCfg.CfgPath;
        ExposeCfg cfg = new ExposeCfg();
        string json = JsonUtility.ToJson(cfg);
        File.WriteAllText(path, json);
        Application.OpenURL(path);
        Debug.Log(path);
    }
}
