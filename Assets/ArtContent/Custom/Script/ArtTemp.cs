using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//* for testing
public class ArtTemp : MonoBehaviour
{
    [Header("For temporary test")]
    // Start is called before the first frame update
    public UnityEvent onEnable;
    public GameObject explosionGO;
    public float delayDestroy = 3;
    //* tween state
    private MeshRenderer meshRender;
    MaterialPropertyBlock matBlock;
    private Tweener tweenState;
    private float stateIns = 0.0f;
    void Start(){
        meshRender = GetComponentInChildren<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        tweenState = DOTween.To(() => stateIns, x => stateIns = x, 1, 1.2f).SetAutoKill(false)/*.SetEase(EaseFactory.StopMotion(5, Ease.OutFlash))*/.Pause();
    }    
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

    public void ActiveState()
    {
        if (meshRender == null) { Debug.LogError("mesh renderer missing"); return; }
        tweenState.Rewind(false);
           tweenState.Play().OnUpdate(delegate {
               //Debug.LogWarning("stateIns");
               matBlock.SetFloat("_StateIntensity", stateIns);
               if(meshRender != null)meshRender.SetPropertyBlock(matBlock);
           }).OnComplete(delegate {
               tweenState.Rewind();
           });
    }

}
