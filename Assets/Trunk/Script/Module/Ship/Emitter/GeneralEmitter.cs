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
    public int row = 1;
    public float bulletSpace = 1;
    protected override void OnFire(byte fireStatue,Vector3 dir)
    {
        if (fireStatue == 1 && !fireCD)
        {
            fireCD = true;
            float offset = row % 2 == 1 ? 0 : bulletSpace / 2;
            float start = -Mathf.Floor(row / 2) * bulletSpace + offset;
            for (int i = 0; i < row; i++)
            {
                GameObject bulletGo = ObjectPool.goPool.GetObj(bulletPrefab.GetInstanceID());
                if (bulletGo == null)
                    bulletGo = Instantiate(bulletPrefab) as GameObject;
                float x= start + i * bulletSpace;
                bulletGo.transform.position = transform.localToWorldMatrix.MultiplyPoint(new Vector3(x, 0,0));
                bulletGo.transform.rotation = transform.rotation;

                BaseBullet bullet = bulletGo.GetComponent<BaseBullet>();
                bullet.ResetBullet();
                bullet.tagetTag = tagetTag;
                bullet.master = master;
                bullet.pookKey = bulletPrefab.GetInstanceID();
                if (bullet != null)
                {
                    bullet.SetDir(dir);

                }
                bulletGo.SetActive(true);
            }
            ShowFireEff();
        }
    }

    public override void ChangeBullet(GameObject prefab)
    {
        bulletPrefab = prefab;
    }
    public override void UpGrade(int addValue)
    {
        row += addValue;
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
    protected virtual void ShowFireEff()
    {
        if (onFireEvent != null)
        {
            onFireEvent.Invoke();
        }
    }
}
