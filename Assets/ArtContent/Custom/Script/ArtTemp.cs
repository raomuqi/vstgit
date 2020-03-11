using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//* 临时用来测试
public class ArtTemp : MonoBehaviour
{
    [Header("这个脚本是临时测试使用")]
    // Start is called before the first frame update
    public UnityEvent onEnable;
    public GameObject explosionGO;
    public float delayDestroy = 3;
    void OnEnable()
    {
        if(onEnable != null) onEnable.Invoke();
    }

    public void SpawnExplosion(Vector3 pos){
        if(explosionGO != null)
            Instantiate(explosionGO, transform.position, Quaternion.identity);
        gameObject.SetActive(false);    
    }
    public void Destroy(){
        Destroy(gameObject, delayDestroy); 
    }
}
