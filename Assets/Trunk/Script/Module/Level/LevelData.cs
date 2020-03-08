using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在资源面板右键Create，创建该类对应的Asset文件
[CreateAssetMenu(fileName = "NewLevelData", menuName = "创建/关卡配置")]
public class LevelData : ScriptableObject
{
    [Header("对象生成配置")]
    public AppearSetData[] appearSets;
    [Header("对象激活出现")]
    public ActiveSetData[] activeSets;


}
[System.Serializable]
public class ActiveSetData
{
    [Header("激活时间")]
    public float time = 10;
    [Header("对象索引")]
    public int[] objectCfgs;
}
[System.Serializable]
public class AppearSetData
{
    [Header("出现时间")]
    public float time = 10;
    [Header("对象")]
    public AppearObjectData[] objectCfgs;
}
[System.Serializable]
public class AppearObjectData
{
    [Header("场景对象ID")]
    public int objectIndex;
    [Header("X轴夹角")]
    public float XAngle = 10;
    [Header("Y轴夹角")]
    public float YAngle = 10;
    [Header("距离")]
    public float distance = 100;
    [Header("血量")]
    public int hp = 100;
    [Header("速度")]
    public float speed = 1;
    [Header("行为配置")]
    public AIStatusData aiCfg;
}

