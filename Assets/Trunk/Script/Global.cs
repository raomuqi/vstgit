using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {
       // Use this for initialization
       void Awake () {
        EventsMgr.AddEvent(EventName.INPUT_BTN1_DOWN,TestInput);
        GameObject.DontDestroyOnLoad(gameObject);
        InputController.instance.InitModule();
	}
	
	// Update is called once per frame
	void Update ()
    {
        InputController.instance.FireCommand(InputCommand.UPDATE_INPUT);
    }

    public void TestInput(EventArgs args)
    {
        Debug.Log("|");
    }
 
}
