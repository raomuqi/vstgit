

using UnityEngine;

public static class AppCfg 
{

    public static ExposeCfg expose;

    public static string CfgPath { get { return Application.persistentDataPath + "/" + Application.productName + "Cfg.txt"; } }

}
