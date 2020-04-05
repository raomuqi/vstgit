using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 交互对象
/// </summary>
public class InteractiveScneeGameObject : SceneGameObject
{
    protected PlayerShip playerShip;
    protected Transform playerShipTransform;
    public float maxLifeTime = 20;
    protected float lifeTime = -1;
    public UnityEvent onHitEvent, onDestroyEvent;
    public bool beVisible = false;
  
    protected override void OnSetSync(SyncType type)
    {
        playerShip = sceneModel.GetPlayerShip();
        playerShipTransform = playerShip.transform;
    }
    /// <summary>
    /// 检查销毁时间
    /// </summary>
    protected void CheckLiftTime()
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

    /// <summary>
    /// 想玩家飞船移动
    /// </summary>
    /// <param name="keepDistance"></param>
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
            OnMoveToPlayerFinish();
           
        }
    }
    /// <summary>
    /// 朝向玩家飞船完成
    /// </summary>
    protected virtual void OnMoveToPlayerFinish()
    {
       
    }
    /// <summary>
    /// 朝向玩家飞船
    /// </summary>
    protected virtual void LookAtPlayer()
    {
        transform.LookAt(playerShipTransform);
    }
    /// <summary>
    /// 前进
    /// </summary>
    protected virtual void RunAways()
    {
        Vector3 dir = transform.forward;
        transform.position += dir * moveSpeed * Time.deltaTime * 10;
    }
    public virtual void OnBeVisible(){}
    public virtual void OnBeInVisible(){}
    private void OnBecameVisible()
    {
        beVisible = true;
        OnBeVisible();
    }

    private void OnBecameInvisible()
    {
        beVisible = false;
        OnBeInVisible();
    }
}
