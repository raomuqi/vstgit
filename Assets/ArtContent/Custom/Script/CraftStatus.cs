using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class CraftStatus : MonoBehaviour
{
    public enum CraftState
    {
        attack,
        isHit
    }
    //* tween state
    MeshRenderer meshRender;
    MaterialPropertyBlock matBlock;
    private Tweener tweenState;
    private float stateIns = 0.0f;
    [SerializeField] [ColorUsage(false, true)] private Color attackStateColor;
    [SerializeField] [ColorUsage(false, true)] private Color isHitStateColor;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        meshRender = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        tweenState = DOTween.To(() => stateIns, x => stateIns = x, 1, 2.0f).SetAutoKill(false).SetEase(EaseFactory.StopMotion(5, Ease.OutFlash)).Pause();
    }

    #region STATE
    public void ActiveFireState()
    {
        ActiveState(CraftState.attack);
    }
    public void ActiveHitState()
    {
        ActiveState(CraftState.isHit);
    }
    void ActiveState(CraftState state)
    {
        if (meshRender == null) { Debug.LogError("mesh renderer missing"); return; }
        tweenState.Rewind(false);
        tweenState.Play().OnPlay(delegate {
            switch (state)
            {
                case CraftState.attack:
                    matBlock.SetColor("_StateColor", attackStateColor);
                    break;
                case CraftState.isHit:
                    matBlock.SetColor("_StateColor", isHitStateColor);
                    break;
                default:
                    break;
            }
        }).OnUpdate(delegate {
            //Debug.LogWarning("stateIns");
            matBlock.SetFloat("_StateIntensity", stateIns);
            if (meshRender != null) meshRender.SetPropertyBlock(matBlock);
        }).OnComplete(delegate {
            tweenState.Rewind();
        });
    }
    #endregion
}
