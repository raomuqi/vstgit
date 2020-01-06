using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(gameObject);
        InputController.instance.InitModule();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
