using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//在资源面板右键Create，创建该类对应的Asset文件
[CreateAssetMenu(fileName = "NewAIData", menuName = "创建/AI行为配置")]
public class AIStatusData : ScriptableObject
{
    [Header("循环AI状态")]
    public bool statusLoop = true;
    [Header("AI状态数组")]
    public AIState[] statusArray;
}
[System.Serializable]
public class AIState
{
    [Header("移动行为")]
    public BaseAI.AIActionEnum action;
    [Header("攻击行为")]
    public BaseAI.AIFireEnum fire;
    [Header("持续时间(-1自动）(0持续)")]
    public float keepTime = 7;
    [Header("随机切换下个状态")]
    public bool randomNext = false;
}

