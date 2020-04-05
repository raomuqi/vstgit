using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArtSetting : MonoBehaviour
{
    public GameObject explosionGO;
    public float delayDestroy = 3;
    public UnityEvent onEnableEvent;
    void OnEnable()
    {
        if (onEnableEvent != null) onEnableEvent.Invoke();
    }
    public void SpawnExplosion()
    {
        if (explosionGO != null)
        {
            GameObject go = Instantiate(explosionGO, transform.position, Quaternion.identity);
            //go.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        Destroy(gameObject, delayDestroy);
    }

}

