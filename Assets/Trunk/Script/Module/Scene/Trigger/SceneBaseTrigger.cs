using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBaseTrigger: MonoBehaviour
{
    public GameObject[] prefabs;
    public bool triggered = false;
    public void CreatePrefab()
    {
        if (prefabs == null) return;
        int[] t = new int[prefabs.Length];
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = SyncCreater.instance.GetIndex(prefabs[i]);
        }
        EventIntArrayArgs e = new EventIntArrayArgs();
        e.t = t;
        SceneController.instance.SendNetMsg(ProtoIDCfg.ACTIVE_OBJECTS, e);
    }
}
