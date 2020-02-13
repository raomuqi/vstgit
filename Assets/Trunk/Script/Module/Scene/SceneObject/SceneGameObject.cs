using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameObject : MonoBehaviour
{
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }
    public SceneObject sceneObject;

    public SceneGameObject()
    {
        sceneObject = new SceneObject(this);
    }
    private void Awake()
    {
        OnAwake();
    }
    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
}
