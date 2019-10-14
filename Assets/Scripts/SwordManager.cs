using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider sw;
    public GameObject player;
    Rigidbody rbp;
    
    void Start()
    {
        sw = GetComponent<BoxCollider>();
        player = GameObject.Find("Player");
        rbp = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Damageable")){
            Debug.Log("Hit!");
        }
    }


}
