using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralEmitter : BaseEmitter
{
    public GameObject bulletPrefab;
    public float fireInterval = 0.5f;
    float curFireTime = 0;
    bool fireCD = false;
    public UnityEvent onFireEvent;
    protected override void OnFire(byte fireStatue,Vector3 dir)
    {
        if (fireStatue == 1 && !fireCD)
        {
            fireCD = true;
            GameObject bulletGo = ObjectPool.goPool.GetObj(bulletPrefab.GetInstanceID());

            if (bulletGo == null)
                bulletGo = Instantiate(bulletPrefab) as GameObject;

            bulletGo.transform.position = transform.position;
            bulletGo.transform.rotation = transform.rotation;

            BaseBullet bullet = bulletGo.GetComponent<BaseBullet>();
            bullet.ResetBullet();
            bullet.tagetTag = tagetTag;
            bullet.pookKey = bulletPrefab.GetInstanceID();
            if (bullet != null)
            {
                bullet.SetDir(dir);

            }
            bulletGo.SetActive(true);
            OnFire();

        }
    }
    protected virtual void OnFire()
    {
        if(onFireEvent != null){
            onFireEvent.Invoke();
        }
    }
    protected override void OnUpdate()
    {
        if (fireCD)
        {
            curFireTime += Time.deltaTime;
            if (curFireTime >= fireInterval)
            {
                fireCD = false;
                curFireTime = 0;
            }
        }
    }
}
