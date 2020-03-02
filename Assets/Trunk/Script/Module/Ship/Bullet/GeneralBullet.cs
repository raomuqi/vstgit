using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBullet : BaseBullet
{
    public float speed = 1;
    public int power = 1;
    public float maxLifeTime = 10;
     float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

    }
    protected override void OnReset()
    {
        lifeTime = 0;
    }
    protected override void OnUpdate()
    {
        transform.position += dir * speed *Time.deltaTime *60;
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
        {
            Recycle();
        }
    }
}
