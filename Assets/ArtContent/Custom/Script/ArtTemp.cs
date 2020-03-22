using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//* for testing
public class ArtTemp : MonoBehaviour
{
    public enum EnemyState
    {
        attack,
        isHit
    }

    [Header("For temporary test")]
    // Start is called before the first frame update
    public UnityEvent onEnable;
    public GameObject explosionGO;
    public float delayDestroy = 3;
    //* tween state
    [SerializeField] private MeshRenderer meshRender;
    MaterialPropertyBlock matBlock;
    private Tweener tweenState;
    private float stateIns = 0.0f;
    //* ai creater
    private LevelData levelData;
    public AIStatusData aiStatusData;
    private AICreater aiCreater;
    //* state
    [SerializeField][ColorUsage(false, true)] private Color attackStateColor;
    [SerializeField][ColorUsage(false, true)] private Color isHitStateColor;
    private void Awake()
    {
        //* ai creater
        //levelData = ScriptableObject.CreateInstance("LevelData") as LevelData;
        //aiCreater = GetComponent<AICreater>();
        //if(aiCreater != null)
        //{
        //    AutoSpawnAI();
        //}
    }
    void Start(){
        if(meshRender == null) meshRender = GetComponentInChildren<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        tweenState = DOTween.To(() => stateIns, x => stateIns = x, 1, 2.0f).SetAutoKill(false).SetEase(EaseFactory.StopMotion(5, Ease.OutFlash)).Pause();
    }    
    void OnEnable()
    {
        if(onEnable != null) onEnable.Invoke();
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(0, LoadSceneMode.Single); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }
    //* auto spawn ai
    void AutoSpawnAI()
    {
        int aiWaves = 20;
        levelData.appearSets = new AppearSetData[aiWaves];
        for (int p = 0; p < aiWaves; p++){ levelData.appearSets[p] = new AppearSetData(); }     //* must initialize each element
        levelData.activeSets = new ActiveSetData[1];
        for (int p = 0; p < 1; p++) { levelData.activeSets[p] = new ActiveSetData(); }      //* must initialize each element

        for (int i = 0; i < aiWaves; i++)
        {
            AppearSetData wave = levelData.appearSets[i];
            wave.time = (i + 1) * 3;
            //Debug.LogError("µÚ"+i+"²¨: "+wave.time);
            int aiCount = Random.Range(3, 5);
            wave.objectCfgs = new AppearObjectData[aiCount];
            for(int p = 0; p < aiCount; p++) { wave.objectCfgs[p] = new AppearObjectData(); }
            for (int j = 0; j < aiCount; j++)
            {
                AppearObjectData aiCraft = wave.objectCfgs[j];
                aiCraft.objectIndex = 0;
                aiCraft.XAngle      = Random.Range(-20, 20);
                aiCraft.YAngle      = Random.Range(-5, 5);
                aiCraft.distance    = Random.Range(200, 500);
                aiCraft.hp = 100;
                aiCraft.speed = 1;
                aiCraft.destroyTime = 300;
                aiCraft.aiCfg = aiStatusData;
                //* aircraft status
                //int stateCount = 3;
                //aiCraft.aiCfg.statusLoop = true;
                //aiCraft.aiCfg.statusArray = new AIState[stateCount];
                //for(int p = 0; p < stateCount; p++) { aiCraft.aiCfg.statusArray[p] = new AIState(); }

                //for (int k = 0; k < stateCount; k++)
                //{
                //    AIState state = aiCraft.aiCfg.statusArray[k];
                //    state.action = BaseAI.AIActionEnum.ToPlayer;
                //    state.fire = BaseAI.AIFireEnum.Fire;
                //    state.keepTime = -1;
                //    state.randomNext = false;
                //    state.fireKeepTime = 2 + Random.Range(0, 1);
                //    state.fireCDTime = 2 + Random.Range(0, 1);
                //}
            }
        }

        aiCreater.levelData = levelData;
    }

    public void SpawnExplosion(Vector3 pos){
        if(explosionGO != null)
            Instantiate(explosionGO, transform.position, Quaternion.identity);
        gameObject.SetActive(true);    
    }

    public void SpawnHitEffect(Vector3 pos)
    {
        if (explosionGO != null)
            Instantiate(explosionGO, transform.position, Quaternion.identity);
    }
    //* common function
    public void Destroy(){
        Destroy(gameObject, delayDestroy); 
    }

    #region STATE
    public void ActiveFireState()
    {
        ActiveState(EnemyState.attack);
    }
    public void ActiveHitState()
    {
        ActiveState(EnemyState.isHit);
    }
    void ActiveState(EnemyState state)
    {
        if (meshRender == null) { Debug.LogError("mesh renderer missing"); return; }
        tweenState.Rewind(false);
           tweenState.Play().OnPlay(delegate{
               switch (state)
               {
                   case EnemyState.attack:
                       matBlock.SetColor("_StateColor", attackStateColor);
                       break;
                   case EnemyState.isHit:
                       matBlock.SetColor("_StateColor", isHitStateColor);
                       break;
                   default:
                       break;
               }
           }).OnUpdate(delegate {
               //Debug.LogWarning("stateIns");
               matBlock.SetFloat("_StateIntensity", stateIns);
               if(meshRender != null)meshRender.SetPropertyBlock(matBlock);
           }).OnComplete(delegate {
               tweenState.Rewind();
           });
    }
    #endregion
}
