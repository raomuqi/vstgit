using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform[] anchors;
    // Start is called before the first frame update
    void Start()
    {
        PlayerModel playerModel = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name);
        int index= playerModel.GetPlayerInfo().pos-1;
        transform.SetParent(anchors[index]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
