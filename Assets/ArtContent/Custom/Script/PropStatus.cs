using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropStatus : MonoBehaviour
{
    //* tween
    MeshRenderer meshRender;
    MaterialPropertyBlock matBlock;
    private Tweener tweenFlash;
    private float flashIns = 0.0f;
    [SerializeField] float flashDuration = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        StartFlash();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        meshRender = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        flashDuration += Random.Range(-0.5f, 0.5f);
        tweenFlash = DOTween.To(() => flashIns, x => flashIns = x, 1, flashDuration)
            .SetAutoKill(false).SetEase(EaseFactory.StopMotion(5, Ease.InOutCubic))
            .SetLoops(-1, LoopType.Yoyo).Pause();
    }
    
    void StartFlash()
    {
        if(meshRender  == null) { Debug.LogError("mesh renderer missing"); return; }
        tweenFlash.Rewind(false);
        tweenFlash.Play().OnUpdate(delegate
        {
            matBlock.SetFloat("_StateIntensity", flashIns);
            if (meshRender != null) meshRender.SetPropertyBlock(matBlock);
        });
    }
}
