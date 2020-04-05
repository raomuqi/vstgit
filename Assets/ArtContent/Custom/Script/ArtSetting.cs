using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArtSetting : MonoBehaviour
{
    [Header("# Spawn explosion")]public GameObject explosionGO;
    [Header("# Delay destroy")]public float delayDestroy = 3;
    [Header("# Rotate constrain")] public Transform target;
    public Vector2 xLimit = new Vector2(-45, 45);
    public bool needDamping = true; //是否需要的阻尼
    private float damping = 5;//阻尼 
    private Vector3 angles;
    [Header("# Unity Event")]public UnityEvent onEnableEvent;

    void OnEnable()
    {
        if (onEnableEvent != null) onEnableEvent.Invoke();
    }
    void LateUpdate()
    {
        if (target)
        {
            angles = target.localEulerAngles;
            angles.x = ClampAngle(angles.x, xLimit.x, xLimit.y);
            Quaternion rotation = Quaternion.Euler(angles);
            if (needDamping)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                transform.localRotation = rotation;
            }
        }
    }


    public void Update()
    {
        //Debug.Log(target.localEulerAngles);
    }

    //** If angles over 360 or under 360 degree, then normalize them.
    private float ClampAngle(float angle, float min, float max)
    {
        angle = NormalizeAngle(angle);
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        min = NormalizeAngle(min);
        if (min > 180)
        {
            min -= 360;
        }
        else if (min < -180)
        {
            min += 360;
        }

        max = NormalizeAngle(max);
        if (max > 180)
        {
            max -= 360;
        }
        else if (max < -180)
        {
            max += 360;
        }

        // Aim is, convert angles to -180 until 180.
        return Mathf.Clamp(angle, min, max);
    }
    float NormalizeAngle(float angle)
    {
        while (angle > 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;
        return angle;
    }

    #region OUTSIDE
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
    #endregion
}

