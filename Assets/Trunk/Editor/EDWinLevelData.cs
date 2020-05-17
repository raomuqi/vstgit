using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EDWinLevelData : EditorWindow
{

    private Vector2 listPos;
    Vector2 dataPos;
    LevelData levelData;
    List<AppearSetData> createList = new List<AppearSetData>();
    List<ActiveSetData> activeList = new List<ActiveSetData>();
    List<AppearSetData> aiList = new List<AppearSetData>();
    List<AppearSetData> propList = new List<AppearSetData>();
    LevelData lastLevelData;
    AppearSetData curCreateData ;
    string dataTitle = string.Empty;
    int inserIndex = 0;
    int createBar = 0;
    int listBar = 0;
    int createListType = 0;
    int lastListType = 0;
    [MenuItem("配置工具/关卡配置编辑器")]
    static public void ShowWindow()
    {
        EDWinLevelData window= EditorWindow.GetWindow<EDWinLevelData>();
        window.Show();
    }

    private void OnDestroy()
    {
        if (EditorUtility.DisplayDialog("提示", "即将关闭窗口是否保存", "保存", "扔掉"))
            Save();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        levelData = EditorGUILayout.ObjectField(levelData, typeof(LevelData)) as LevelData;
        if (levelData!=null && GUILayout.Button("Save"))
        {
            Save();
        }
        EditorGUILayout.EndHorizontal();
        if (lastLevelData != levelData)
        {
            lastLevelData = levelData;
            OnLevelDataLoad();
        }
        if (levelData != null)
        {
            DrawSelectList();
        }

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < aiStatus.Length; i++)
        {
            aiStatus[i] = EditorGUILayout.ObjectField(aiStatus[i], typeof(AIStatusData)) as AIStatusData;
        }
        if (GUILayout.Button("创建默认配置"))
        {
            CreateDefaultSets();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    void Save()
    {
        if (levelData == null)
            return;
        for(int i = 0; i < aiList.Count; i++)
        {
            createList.Add(aiList[i]);
        }
        for (int i = 0; i < propList.Count; i++)
        {
            createList.Add(propList[i]);
        }
        levelData.appearSets = createList.ToArray();
        levelData.SetDirty();
        AssetDatabase.SaveAssets();
    }
    void OnLevelDataLoad()
    {
        curCreateData = null;
        createList.Clear();
        activeList.Clear();
        aiList.Clear();
        propList.Clear();
        dataTitle = string.Empty;
     
        if (levelData == null)
            return;
        if (levelData.appearSets != null)
        {
            AppearSetData[] createArray = levelData.appearSets;
            for (int i = 0; i < createArray.Length; i++)
            {
                if (createArray[i].type == AppearType.AI)
                {
                    aiList.Add(createArray[i]);
                }
                else if (createArray[i].type == AppearType.PROP)
                {
                    propList.Add(createArray[i]);
                }
              //  createList.Add(createArray[i]);
            }
        }
        if (levelData.activeSets != null)
        {
            ActiveSetData[] activeArray = levelData.activeSets;
            for (int i = 0; i < activeArray.Length; i++)
            {
                activeList.Add(activeArray[i]);
            }
        }
    }
    /// <summary>
    /// 选择列表
    /// </summary>
    void DrawSelectList()
    {
        EditorGUILayout.BeginVertical();
        listBar = GUILayout.Toolbar(listBar, new GUIContent[]
            {
                new GUIContent("创建列表"),
                new GUIContent("激活列表")
            });
        switch (listBar)
        {
            case 0:
                {
                    DrawSelectTypeList();
                }
                break;
            case 1:
                {
                    GUILayout.Label("暂时无需开发");
                }
                break;

        }
        EditorGUILayout.EndVertical();
    }
    /// <summary>
    /// 选择列表类型
    /// </summary>
    void DrawSelectTypeList()
    {
        EditorGUILayout.BeginVertical();
        createListType = GUILayout.Toolbar(createListType, new GUIContent[]
            {
                new GUIContent("AI"),
                new GUIContent("道具")
            });
        if (lastListType != createListType)
        {
            lastListType = createListType;
            curCreateData=null;
        }
        switch (createListType)
        {
            case 0:
                {
                    DrawCreateListLayer(aiList);
                }
                break;
            case 1:
                {
                    DrawCreateListLayer(propList);
                }
                break;

        }
        EditorGUILayout.EndVertical();
    }
    /// <summary>
    /// 列表/数据
    /// </summary>
    void DrawCreateListLayer(List<AppearSetData> list)
    {
        createBar = GUILayout.Toolbar(createBar, new GUIContent[]
            {
                new GUIContent("列表数据"),
                new GUIContent(dataTitle+"数据")
            });
        switch (createBar)
        {
            case 0:
                {
                    DrawCreateList(list);

                }
                break;
            case 1:
                {
                    DrawCreateData(list);
                }
                break;

        }
    }
    AppearSetData copyAppearData = null;
    int copyDataIndex = -1;
    /// <summary>
    /// 绘制创建列表
    /// </summary>
    void DrawCreateList(List<AppearSetData> list)
    {
        EditorGUILayout.BeginHorizontal();
        inserIndex = EditorGUILayout.IntField("目标索引",inserIndex);
        if (GUILayout.Button("插入新配置在:"+inserIndex))
        {
            var item = new AppearSetData();
            if (createListType == 0)
                item.type = AppearType.AI;
            else if (createListType == 1)
                item.type = AppearType.PROP;
            list.Insert(inserIndex, item);
        }
        if (copyAppearData != null && GUILayout.Button("复制:"+copyDataIndex+"插入:" + inserIndex))
        {
            var copye= Util.CopyInstance(copyAppearData) as AppearSetData; ;
            if (createListType == 0)
                copye.type = AppearType.AI;
            else if (createListType == 1)
                copye.type = AppearType.PROP;
            list.Insert(inserIndex, copye);
        }
      
        if (GUILayout.Button("清除所有"))
        {
            list.Clear();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        listPos = EditorGUILayout.BeginScrollView(listPos);
        for (int i = 0; i < list.Count; i++)
        {
            int curIndex = i;
            var curItem= list[curIndex];
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            string count = curItem.objectCfgs == null ? "0" : curItem.objectCfgs.Length.ToString();
            string text = ("出现时间:" + curItem.time + " 对象数量:" + count);
            EditorGUILayout.LabelField("第" + curIndex + "波");
            EditorGUILayout.LabelField(text);
            if (GUILayout.Button("修改"))
            {
                dataTitle = "第" + curIndex + "波";
                curCreateData = curItem;
                createBar = 1;
            }
            if (GUILayout.Button("复制"))
            {
                copyAppearData = curItem;
                copyDataIndex = curIndex;


            }
            if (GUILayout.Button("删除"))
            {
                list.Remove(curItem);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    int createCountRangeMin = 0;
    int createCountRangeMax = 5;
    int randomAIStatusCount = 1;
    int setObjectIndex = 0;
    float randomCreateDistanceMin =500;
    float randomCreateDistanceMax =700;
    float xAngleMin = 0;
    float xAngleMax = 10;
    float yAngleMin = 0;
    float yAngleMax = 10;
    int hpMin = 0;
    int hpMax = 100;
    float speedMin = 1;
    float speedMax = 2;
    float destroyTimeMin = 300;
    float destroyTimeMax = 300;
    AIStatusData[] randomAiStatus;
    byte[] openStatus;
    int[] randomObjectIndexs;
    int randomObjectCount = 1;
    /// <summary>
    /// 绘制创建数据
    /// </summary>
    void DrawCreateData(List<AppearSetData> list)
    {
        if (curCreateData == null)
        {
            if (list.Count > 0)
            {
                dataTitle = "第0波";
                curCreateData = list[0];
            }
            else
            {
                EditorGUILayout.LabelField("必须先编辑列表");
            }
        }
        else
        {
            dataPos = EditorGUILayout.BeginScrollView(dataPos);
            EditorGUILayout.BeginVertical();
            curCreateData.time = EditorGUILayout.FloatField("时间点:", curCreateData.time);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(20);

            EditorGUILayout.BeginVertical();
            randomObjectCount = EditorGUILayout.IntField("随机对象数组大小:", randomObjectCount);
            if (randomObjectCount < 1)
                randomObjectCount = 1;
            EditorGUILayout.EndVertical();
            //随机对象索引列表UI
            if (randomObjectIndexs == null)
            {
                randomObjectIndexs = new int[randomObjectCount];
            }
            else
            {
                if (randomObjectIndexs.Length != randomObjectCount)
                {
                    int[] temp = randomObjectIndexs;
                    randomObjectIndexs = new int[randomObjectCount];
                    for (int i = 0; i < randomObjectCount; i++)
                    {
                        if (temp.Length - 1 < i)
                            break;
                        randomObjectIndexs[i] = temp[i];
                    }
                }
            }
            for (int i = 0; i < randomObjectCount; i++)
            {
                EditorGUILayout.BeginVertical();
                randomObjectIndexs[i] = EditorGUILayout.IntField("随机对象Index:", randomObjectIndexs[i]);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(20);
            //随机数量UI
            EditorGUILayout.BeginHorizontal();
            createCountRangeMin = EditorGUILayout.IntField("随机数量最小值:", createCountRangeMin);
            if (createCountRangeMin < 1)
                createCountRangeMin = 1;
            createCountRangeMax = EditorGUILayout.IntField("随机数量最大值:", createCountRangeMax);
            EditorGUILayout.EndHorizontal();
            //随机距离UI
            EditorGUILayout.BeginHorizontal();
            randomCreateDistanceMin = EditorGUILayout.FloatField("随机距离最小值:", randomCreateDistanceMin);
            randomCreateDistanceMax = EditorGUILayout.FloatField("随机距离最大值:", randomCreateDistanceMax);
            EditorGUILayout.EndHorizontal();
            //随机XY角度UI
            EditorGUILayout.BeginHorizontal();
            xAngleMin = EditorGUILayout.FloatField("随机x夹角最小:", xAngleMin);
            xAngleMax = EditorGUILayout.FloatField("随机x夹角最大:", xAngleMax);
            yAngleMin = EditorGUILayout.FloatField("随机y夹角最小:", yAngleMin);
            yAngleMax = EditorGUILayout.FloatField("随机y夹角最大:", yAngleMax);
            EditorGUILayout.EndHorizontal();
            //随机属性UI
            EditorGUILayout.BeginHorizontal();
            hpMin = EditorGUILayout.IntField("随机hp最小值:", hpMin);
            hpMax = EditorGUILayout.IntField("随机hp最大值:", hpMax);
            speedMin = EditorGUILayout.FloatField("随机速度最小:", speedMin);
            speedMax = EditorGUILayout.FloatField("随机速度最大:", speedMax);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            destroyTimeMin = EditorGUILayout.FloatField("随机销毁时间最小:", destroyTimeMin);
            destroyTimeMax = EditorGUILayout.FloatField("随机销毁时间最大:", destroyTimeMax);
            EditorGUILayout.EndHorizontal();
            //AI
            if (createListType == 0)
            {
                //随机AI配置UI
                EditorGUILayout.Space(20);
                randomAIStatusCount = EditorGUILayout.IntField("随机AI配置(>0):", randomAIStatusCount);
                randomAIStatusCount = randomAIStatusCount <= 0 ? 1 : randomAIStatusCount;
                if (randomAiStatus == null)
                {
                    randomAiStatus = new AIStatusData[randomAIStatusCount];
                    openStatus = new byte[randomAIStatusCount];
                }
                else
                {
                    if (randomAiStatus.Length != randomAIStatusCount)
                    {
                        AIStatusData[] temp = randomAiStatus;
                        randomAiStatus = new AIStatusData[randomAIStatusCount];
                        openStatus = new byte[randomAIStatusCount];
                        for (int i = 0; i < randomAIStatusCount; i++)
                        {
                            if (temp.Length - 1 < i)
                                break;
                            randomAiStatus[i] = temp[i];
                        }
                    }
                }
                for (int i = 0; i < randomAIStatusCount; i++)
                {
                    EditorGUILayout.BeginVertical();
                    randomAiStatus[i] = EditorGUILayout.ObjectField(randomAiStatus[i], typeof(AIStatusData)) as AIStatusData;
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.Space(20);
            }
            //生成随机数据
            if (GUILayout.Button("随机数量(会重置所有Item)"))
            {
                if (createListType==0 && randomAiStatus[0] == null)
                {
                    EditorUtility.DisplayDialog("提示", "没放入AI配置", "OK");
                }
                else
                {
                    int count = Random.Range(createCountRangeMin, createCountRangeMax);
                    curCreateData.objectCfgs = new AppearObjectData[count];
                    for (int i = 0; i < count; i++)
                    {
                        var temp = new AppearObjectData();
                        temp.objectIndex = randomObjectIndexs[Random.Range(0, randomObjectCount)]; ;
                        temp.speed = Random.Range(speedMin, speedMax);
                        temp.hp = Random.Range(hpMin, hpMax);
                        temp.distance = Random.Range(randomCreateDistanceMin, randomCreateDistanceMax);
                        temp.XAngle = Random.Range(xAngleMin, xAngleMax);
                        temp.YAngle = Random.Range(yAngleMin, yAngleMax);
                        temp.destroyTime = Random.Range(destroyTimeMin, destroyTimeMax);
                        if(createListType == 0)
                         temp.aiCfg = randomAiStatus[Random.Range(0, randomAIStatusCount)];
                        curCreateData.objectCfgs[i] = temp;
                    }
                }
            }
            if (curCreateData.objectCfgs != null && curCreateData.objectCfgs.Length>0)
            {
                if (openStatus == null || openStatus.Length != curCreateData.objectCfgs.Length)
                    openStatus = new byte[curCreateData.objectCfgs.Length];
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Item:");
                for (int i = 0; i < curCreateData.objectCfgs.Length; i++)
                {
                    EditorGUILayout.BeginVertical();
                    if (GUILayout.Button("对象:" + i.ToString()))
                    {
                        if (openStatus[i] == 0)
                        {
                            openStatus[i] = 1;
                        }
                        else
                        {
                            openStatus[i] = 0;
                        }
                    }
                    if (openStatus[i] == 1)
                    {
                        DrawAppearObjectData(curCreateData.objectCfgs[i]);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }

    }
    void DrawAppearObjectData(AppearObjectData data)
    {
        EditorGUILayout.BeginVertical();
        data.objectIndex = EditorGUILayout.IntField("对象索引:", data.objectIndex);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.speed = EditorGUILayout.FloatField("速度:", data.speed);
        if (GUILayout.Button("随机"))
            data.speed = Random.Range(speedMin, speedMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.hp = EditorGUILayout.IntField("HP:", data.hp);
        if (GUILayout.Button("随机"))
            data.hp = Random.Range(hpMin, hpMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.distance = EditorGUILayout.FloatField("出现距离:", data.distance);
        if (GUILayout.Button("随机"))
            data.distance = Random.Range(randomCreateDistanceMin, randomCreateDistanceMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

         EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.XAngle = EditorGUILayout.FloatField("X夹角:", data.XAngle);
        if (GUILayout.Button("随机"))
            data.XAngle = Random.Range(xAngleMin, xAngleMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.YAngle = EditorGUILayout.FloatField("Y夹角:", data.YAngle);
        if (GUILayout.Button("随机"))
            data.YAngle = Random.Range(yAngleMin, yAngleMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        data.destroyTime = EditorGUILayout.FloatField("销毁时间:", data.destroyTime);
        if (GUILayout.Button("随机"))
            data.destroyTime = Random.Range(destroyTimeMin, destroyTimeMax);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        if (createListType == 0)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            data.aiCfg = EditorGUILayout.ObjectField("AI配置", data.aiCfg, typeof(AIStatusData)) as AIStatusData;
            if (GUILayout.Button("随机"))
                data.aiCfg = randomAiStatus[Random.Range(0, randomAIStatusCount)];
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

    }

    #region create sets by script 
    AIStatusData[] aiStatus = new AIStatusData[2];
    void CreateDefaultSets()
    {
        if(levelData == null) { Debug.LogError("LevelData对象为空");return; }
        foreach (var item in aiStatus) { if (item == null) { Debug.LogError("AIStatusData对象为空"); return; } }

        int aiWaveCount = 6;
        levelData.appearSets = new AppearSetData[aiWaveCount];
        for (int p = 0; p < aiWaveCount; p++) { levelData.appearSets[p] = new AppearSetData(); }     //* must initialize each element
        levelData.activeSets = new ActiveSetData[1];
        for (int p = 0; p < 1; p++) { levelData.activeSets[p] = new ActiveSetData(); }      //* must initialize each element

        for (int i = 0; i < aiWaveCount; i++)
        {
            AppearSetData wave = levelData.appearSets[i];
            wave.time = (i + 1) * 5;
            //Debug.LogError("第"+i+"波: "+wave.time);
            int aiCount = Random.Range(3, 5);
            wave.objectCfgs = new AppearObjectData[aiCount];
            for (int p = 0; p < aiCount; p++) { wave.objectCfgs[p] = new AppearObjectData(); }
            for (int j = 0; j < aiCount; j++)
            {
                AppearObjectData aiCraft = wave.objectCfgs[j];
                aiCraft.objectIndex = 0;
                aiCraft.XAngle = Random.Range(-20, 20);
                aiCraft.YAngle = Random.Range(-5, 5);
                aiCraft.distance = Random.Range(200, 500);
                aiCraft.hp = 100;
                aiCraft.speed = 1;
                aiCraft.destroyTime = 300;
                aiCraft.aiCfg = aiStatus[Random.Range(0, aiStatus.Length-1)];
                //* aircraft status
                //int stateCount = 3;
                //aiCraft.aiCfg.statusLoop = true;
                //aiCraft.aiCfg.statusArray = new AIState[stateCount];
                //for(int p = 0; p < stateCount; p++) { aiCraft.aiCfg.statusArray[p] = new AIState(); }

                //for (int k = 0; k < stateCount; k++)
                //{
                //    AIState state = aiCraft.aiCfg.statusArray[k];
                //    state.action = BaseAI.AIActionEnum.ToPlayer;
                //    state.fire = BaseAI.AIFireEnum.Fire;
                //    state.keepTime = -1;
                //    state.randomNext = false;
                //    state.fireKeepTime = 2 + Random.Range(0, 1);
                //    state.fireCDTime = 2 + Random.Range(0, 1);
                //}
            }
        }

        OnLevelDataLoad();
        //Save();
    }
    #endregion create sets by script 
}
