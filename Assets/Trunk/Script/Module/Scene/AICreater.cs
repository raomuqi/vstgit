using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreater : MonoBehaviour
{
    bool start = false;
    float invTime = 0;
    void Awake()
    {
        EventsMgr.AddEvent(EventName.START_GAME, OnGameStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (start && Connection.GetInstance().isHost)
        {
            invTime += Time.deltaTime;
            if (invTime >= 5)
            {
                invTime = 0;
                CreateAI();
            }
        }
    }
    void OnGameStart(EventArgs args)
    {
        invTime = 0;
        start = true;
    }
    void CreateAI()
    {
        SyncObject[] list = new SyncObject[2];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = new SyncObject();
            list[i].objectIndex = 0;
            list[i].SetPos(GetPosByRadian(transform.position,i==0? -10:10,30, 10));
          //  list[i].SetPos(GetPosByRadian(transform.position, 180, 10));
        }
        var t = new EventObjectArgs();
        t.t = list;
        SceneController.instance.SendNetMsg(ProtoIDCfg.CREATE_OBJECTS, t);

    }
    public  Vector3 GetPosByRadian(Vector3 pos, float angleX, float r,float angleY=0)
    {
        angleX = (float)(angleX / 180 *System.Math.PI);
        angleY = (float)(angleY / 180 * System.Math.PI);
        float x = Mathf.Sin(angleX) * r ;
        float y= Mathf.Sin(angleY) * r ;
        float z = Mathf.Cos(angleX) * r ;
        return transform.localToWorldMatrix.MultiplyPoint(new  Vector3(x, y, z));
    }
}
