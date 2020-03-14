using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AAA");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//* testing
[CreateAssetMenu(fileName = "NewArtLevelData", menuName = "创建/美术自定义关卡配置")]
public class CustomLevelData : LevelData
{
    public static int waves = 5;
    [Header("对象生成配置")]
    public AppearSetData[] appearSets =  new AppearSetData[waves];
 //   for (int i = 0; i<waves; i++)
	//{

	//}
    [Header("对象激活出现")]
    public ActiveSetData[] activeSets;
}
