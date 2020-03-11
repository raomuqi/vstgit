using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseAI : SceneGameObject
{
    PlayerShip playerShip;
    public BaseGun[] emitterArray;
    Transform playerShipTransform;
    public float maxLifeTime=20;
    float lifeTime = -1;
    public float maxDistance = 40;
    public int rushPower = 10;
    public UnityEvent onHitEvent;
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
    protected override void OnStart()
    {
        if (emitterArray != null)
        {
            for (int i = 0; i < emitterArray.Length; i++)
            {
                emitterArray[i].SetTag(TagCfg.SHIP);
            }
        }
    }
    protected override void OnSetSync(SyncType type)
    {
        playerShip = sceneModel.GetPlayerShip();
        playerShipTransform = playerShip.transform;
        fireSync[0] = sync.serverID;
        fireSync[1] = 0;
        lifeTime = 0;
        if (type == SyncType.UpLoad)
        {
            NextAIState();
        }
    }
    protected virtual void OnEnterStatus (AIState state)
    {
        switch (curState.fire)
        {
            case AIFireEnum.Fire:
                if (fireSync[1] != 1)
                {
                    fireSync[1] = 1;
                    RqSyncAction(fireSync);
                }
                // Fire(1);
                break;
            case AIFireEnum.None:
                if (fireSync[1] != 0)
                {
                    fireSync[1] = 0;
                    RqSyncAction(fireSync);
                }
                //  Fire(0);
                break;

        }
    }
    protected virtual void OnExitStatus(AIState state)
    {

    }
    int[] fireSync = new int[2];
    byte fireStatus = 0;
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
       
        }



    }
    protected override void OnUpdate()
    {
        if (syncType == SyncType.UpLoad)
        {
            UpdateAILogic();
            CheckLiftTime();
        }
        else if (syncType == SyncType.UpDate)
        {
            
        }
        Fire(fireStatus);
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

    public override void SyncAction(int[] intArray)
    {
        fireStatus = (byte)intArray[1];
    }
    /// <summary>
    /// 撞击回调
    /// </summary>
    protected virtual void OnRush()
    {
        if (Connection.GetInstance().isHost)
        {
            ReqDestroy();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagCfg.SHIP))
        {
            SceneGameObject sgo = other.gameObject.GetComponent<SceneGameObject>();
            if (sgo != null)
            {
                sgo.SetDamage(rushPower,transform.position);
                OnRush();
            }
        }
    }
    /// <summary>
    /// 受伤表现
    /// </summary>
    public override void OnGetDamage(int damage, Vector3 point)
    {
        if(onHitEvent != null){
            onHitEvent.Invoke();
        }
        //* testing
        ArtTemp artTemp = GetComponent<ArtTemp>();
        if(artTemp != null) artTemp.SpawnExplosion(point);
    }
}
