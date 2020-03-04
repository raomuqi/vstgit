using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTimerTrigger : SceneBaseTrigger
{
    public float triggerTime = 20;
    float startTime=0;
    bool init = false;
    private void Start()
    {
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
    }
    private void Update()
    {
        if (init == true && Time.time - startTime >= triggerTime)
        {
            CreatePrefab();
        }
    }
    void OnGameStart(EventArgs args)
    {
        startTime = Time.time;
        init = true;
    }
}
