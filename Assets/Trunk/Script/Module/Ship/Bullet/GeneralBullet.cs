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

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagetTag))
        {
            SceneGameObject sgo = other.gameObject.GetComponent<SceneGameObject>();
            if (sgo != null)
            {
                sgo.SetDamage(power,transform.position);
            }
            Recycle();
        }
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
