

using System.IO;
using UnityEngine;

public static class AppCfg 
{

    public static ExposeCfg expose;
#if UNITY_EDITOR
    public static string CfgPath
    {
        get
        {
            string folder= Application.persistentDataPath + "/Editor/" ;
            string filesName = Application.productName + "Cfg.txt";
            if (!Directory.Exists(Path.GetDirectoryName(folder)))
                Directory.CreateDirectory(folder);
            return folder+filesName;
        }
    }
#else

     public static string CfgPath
    {
        get
        {
            string folder= Application.persistentDataPath;
            string filesName = Application.productName + "Cfg.txt";
            return folder+"/"+filesName;
        }
    }
#endif

}
