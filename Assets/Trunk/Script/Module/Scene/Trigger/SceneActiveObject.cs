using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景激活物体脚本 //
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class SceneActiveObject : MonoBehaviour
{
    [Header("激活的物体, 默认激活所有的子物体")]
    public GameObject mActiveGameObject;
    /// <summary>
    /// 实际激活的物体列表 //
    /// </summary>
    private GameObject[] _mActiveGameObjects;

    public enum TriggerOpportunity
    {
        Enter,
        Exit,
    }
    [Header("触发时机")]
    public TriggerOpportunity eTriggerOpportunity = TriggerOpportunity.Enter;

    [Header("延迟关闭激活时间")]
    public float mDelayDeactiveTime = 2f;

    /// <summary>
    /// 是否已经激活 //
    /// </summary>
    private bool _bHasActived;

    private void Awake()
    {
        if(mActiveGameObject == null || gameObject == mActiveGameObject)
        {
            // 如果没有指定物体或者指定的物体为本身, 则默认获取所有子物体 //
            _mActiveGameObjects = new GameObject[transform.childCount];
            for(int i=0; i<_mActiveGameObjects.Length; i++)
            {
                _mActiveGameObjects[i] = transform.GetChild(i).gameObject;
            }

            if(_mActiveGameObjects.Length == 0)
            {
                Debug.LogErrorFormat("错误!!! 没有获取到有效的激活物体");
            }
        }
        else
        {
            _mActiveGameObjects = new GameObject[] { mActiveGameObject };
        }

        // 把当前物体改为触发器 //
        GetComponent<BoxCollider>().isTrigger = true;
        // 关闭当前物体的Renderer //
        Renderer tempRenderer = GetComponent<Renderer>();
        if(tempRenderer != null)
        {
            tempRenderer.enabled = false;
        }
        // 先把需要激活的物体给关闭 //
        for (int i = 0; i < _mActiveGameObjects.Length; i++)
        {
            _mActiveGameObjects[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActiveObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ActiveObject(other);
    }

    void ActiveObject(Collider other)
    {
        if(!_bHasActived)
        {
            if(other.tag == "Ship")
            {
                for (int i = 0; i < _mActiveGameObjects.Length; i++)
                {
                    _mActiveGameObjects[i].SetActive(true);
                }
                _bHasActived = true;
                StartCoroutine(IEnumerator_DelayDeactiveObject());
            }
        }
    }

    IEnumerator IEnumerator_DelayDeactiveObject()
    {
        yield return new WaitForSeconds(mDelayDeactiveTime);

        gameObject.SetActive(false);
    }
}