using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : InteractiveScneeGameObject
{
    public BaseGun[] emitterArray;
     public float maxDistance = 40;
     public int rushPower = 10;
    protected float fireTime = 0;
    protected int[] actionSync = new int[3];
    protected byte fireStatus = 0;
    public AIStatusData cfg { get; set; }
    AIState curState;
    float curStateTime = 0;
    int curStateIndex = 0;
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
    /// <summary>
    /// 初始化AI数据
    /// </summary>
    public void InitAIData(AppearObjectData cfg)
    {
       this.cfg = cfg.aiCfg;
       moveSpeed = cfg.speed;
       hp = cfg.hp;
       maxLifeTime = cfg.destroyTime;
    }
    protected override void OnStart()
    {
        //初始化发射器
        if (emitterArray != null)
        {
            for (int i = 0; i < emitterArray.Length; i++)
            {
                emitterArray[i].SetTag(TagCfg.SHIP, this);
            }
        }
    }
    protected override void OnUpdate()
    {
        //主机更新逻辑
        if (Connection.GetInstance().isHost)
        {
            UpdateAILogic();
            CheckLiftTime();
        }
        //设置开火状态
        Fire(fireStatus);
    }

    protected override void OnSetSync(SyncType type)
    {
        base.OnSetSync(type);
        actionSync[0] = sync.serverID;
        actionSync[1] = SceneObjectActionCfg.FIRE_STATUS;
        actionSync[2] = 0;
        lifeTime = 0;
        if (Connection.GetInstance().isHost)
        {
            NextAIState();
        }
    }

    /// <summary>
    /// 设置同步行为
    /// </summary>
    /// <param name="intArray"></param>
    public override void OnGetAction(int[] intArray)
    {
        if (intArray[1] == SceneObjectActionCfg.FIRE_STATUS)
        {
            fireStatus = (byte)intArray[1];
        }
    }
    /// <summary>
    /// 触发状态
    /// </summary>
    /// <param name="state"></param>
    protected virtual void OnEnterStatus(AIState state)
    {
        fireTime = 0;
        switch (curState.fire)
        {
            case AIFireEnum.Fire:
                if (actionSync[2] != 1)
                {
                    actionSync[2] = 1;
                    RqSyncAction(actionSync);
                }
                break;
            case AIFireEnum.None:
                if (actionSync[2] != 0)
                {
                    actionSync[2] = 0;
                    RqSyncAction(actionSync);
                }
                break;
        }
    }
  /// <summary>
  /// AI更新状态行为
  /// </summary>
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
        fireTime += Time.deltaTime;
        if (actionSync[2] == 1 && curState.fireKeepTime > 0)
        {
            if (fireTime >= curState.fireKeepTime)
            {
                actionSync[2] = 0;
                fireTime = 0;
                RqSyncAction(actionSync);
            }
        }
        else if (actionSync[2] == 0 && curState.fireCDTime > 0)
        {
            if (fireTime >= curState.fireCDTime)
            {
                actionSync[2] = 0;
                fireTime = 1;
                RqSyncAction(actionSync);
            }
        }

    }
    /// <summary>
    /// 退出状态时
    /// </summary>
    /// <param name="state"></param>
    protected virtual void OnExitStatus(AIState state)
    {

    }
    //切换下个状态
    protected virtual void NextAIState()
    {
        int nextIndex = 0;
        if((curState != null))
        {
            OnExitStatus(curState);
            //随机状态
            if (curState.randomNext)
            {
                nextIndex = Random.Range(0, cfg.statusArray.Length);
            }
            else
            {
                nextIndex=curStateIndex+1;
                if (nextIndex >= cfg.statusArray.Length)
                {
                    //循环状态
                    if (cfg.statusLoop)
                        nextIndex = 0;
                    else
                    {
                        //状态执行完
                        curState = null;
                        return;
                    }
                }
            }
        }

        curState = cfg.statusArray[nextIndex];
        curStateIndex = nextIndex;
        curStateTime = 0;
        fireTime = 0;
        OnEnterStatus(curState);
    }
   
    /// <summary>
    /// 开火
    /// </summary>
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
  
    protected override void OnMoveToPlayerFinish()
    {
        if (curState.keepTime == -1)
            NextAIState();
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
    protected override void OnDestroyed()
    {
        base.OnDestroyed();
        sceneModel.UnRegisterAI(this);
    }

   
    /// <summary>
    /// 受伤表现
    /// </summary>
    public override void OnGetDamage(int damage, Vector3 point)
    {
        if(onHitEvent != null){
            onHitEvent.Invoke();
        }
        //* destroy
        hp -= Random.Range(35, 60);
        if(hp <= 0 && onDestroyEvent != null)
        {
            onDestroyEvent.Invoke();
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagCfg.SHIP))
        {
            SceneGameObject sgo = other.gameObject.GetComponent<SceneGameObject>();
            if (sgo != null)
            {
                sgo.SetDamage(rushPower, transform.position,this);
                OnRush();
            }
        }
    }
}
