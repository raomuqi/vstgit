using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : SceneGameObject
{
    PlayerShip playerShip;
    public BaseGun[] emitterArray;
    Transform playerShipTransform;
    public float maxLifeTime=20;
    float lifeTime = -1;
    public float maxDistance = 40;
    public enum AIActionEnum
    {
        None,
        ToPlayer,
        Stay,
        RunAway,
        RushPlayer
    }
    public enum AIFireEnum
    {
        None,
        Fire,
    }
    public AIStatusData cfg { get; set; }
    AIState curState;
    float curStateTime = 0;
    int curStateIndex = 0;
    //切换下个状态
    protected virtual void NextAIState()
    {
        int nextIndex = 0;
        if((curState != null))
        {
            OnExitStatus(curState);
            if (curState.randomNext)
            {
                nextIndex = Random.Range(0, cfg.statusArray.Length);
            }
            else
            {
                nextIndex=curStateIndex+1;
                if (nextIndex >= cfg.statusArray.Length)
                {
                    if (cfg.statusLoop)
                        nextIndex = 0;
                    else
                    {
                        curState = null;
                        return;
                    }
                }
            }
        }

        curState = cfg.statusArray[nextIndex];
        curStateIndex = nextIndex;
        curStateTime = 0;

        OnEnterStatus(curState);
    }

    protected override void OnSetSync(SyncType type)
    {
        playerShip = sceneModel.GetPlayerShip();
        playerShipTransform = playerShip.transform;
        lifeTime = 0;
        if (type == SyncType.UpLoad)
        {
            NextAIState();
        }
    }
    protected virtual void OnEnterStatus (AIState state)
    {

    }
    protected virtual void OnExitStatus(AIState state)
    {

    }
    protected virtual void UpdateAILogic()
    {
        if (curState != null)
        {
            curStateTime += Time.deltaTime;
            if (curState.keepTime > 0 && curStateTime > curState.keepTime)
            {
                NextAIState();
                return;
            }
            switch (curState.action)
            {
                case AIActionEnum.ToPlayer:
                    MoveToPlayer(maxDistance);
                    break;
                case AIActionEnum.Stay:
                    LookAtPlayer();
                    break;
                case AIActionEnum.RunAway:
                    RunAways();
                    break;
                case AIActionEnum.RushPlayer:
                    MoveToPlayer(0);
                    break;
            }
            switch (curState.fire)
            {
                case AIFireEnum.Fire:
                    Fire(1);
                    break;
                case AIFireEnum.None:
                    Fire(0);
                    break;

            }
        }



    }
    protected override void OnUpdate()
    {
        if (syncType == SyncType.UpLoad)
        {
            UpdateAILogic();
            CheckLiftTime();
        }
        else if (syncType == SyncType.UpDate){}
    }

     void CheckLiftTime()
    {
        if (lifeTime >= 0)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > maxLifeTime)
            {
                lifeTime = -1;
                ReqDestroy();
            }
        }
    }
    public virtual void Fire(byte open)
    {
        if (emitterArray != null)
        {
            for (int i = 0; i < emitterArray.Length; i++)
            {
                BaseGun gun = emitterArray[i];
                gun.SetFire(open, gun.transform.forward);
            }
        }

    }

  
   
    protected virtual void MoveToPlayer(float keepDistance)
    {
        Vector3 dir = Vector3.Normalize(playerShipTransform.position - transform.position);
   
        float curDistance = Vector3.Distance(playerShipTransform.position, transform.position);
        if (curDistance > keepDistance)
        {
            transform.position += dir * moveSpeed * Time.deltaTime * 10;
            transform.LookAt(playerShipTransform);
        }
        else
        {
           
            if(curState.keepTime==-1)
              NextAIState();
        }
     
    }
    protected virtual void LookAtPlayer()
    {
        transform.LookAt(playerShipTransform);
    }
    protected virtual void RunAways()
    {
        Vector3 dir =  transform.forward;
        transform.position += dir * moveSpeed * Time.deltaTime * 10;
    }
}
