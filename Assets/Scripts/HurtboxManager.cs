using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public GameObject player;
    void Awake(){//Frame 0
    }

    void Update(){
       transform.position = new Vector3(player.transform.position.x, player.transform.position.y+0.25f, player.transform.position.z); 
    }
}
