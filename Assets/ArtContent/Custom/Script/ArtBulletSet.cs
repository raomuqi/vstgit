using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneralEmitter))]
public class ArtBulletSet : MonoBehaviour
{
    private GeneralEmitter emitter;
    [SerializeField] int cdTime = 3;
    private float interTime = 0;
    private float curTime = 0;
    bool isEmpty;
    [SerializeField] int bulletsOneClip = 50;
    int curBullets;
    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<GeneralEmitter>();
        interTime = emitter.fireInterval;
        curBullets = bulletsOneClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmpty)
        {
            curTime += Time.deltaTime;
            if(curTime > cdTime) {
                BulletReset();
            }
        }
    }

    public void BulletDecrease()
    {
        curBullets--;
        if(curBullets <= 0)
        {
            emitter.fireInterval = cdTime;
            isEmpty = true;
        }
    }

    public void BulletReset()
    {
        isEmpty = false;
        curTime = 0;
        emitter.fireInterval = interTime;
        curBullets = bulletsOneClip; 
    }
}
