using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour {
    // Start is called before the first frame update
    BoxCollider sw;
    public GameObject player;
    Rigidbody rbp;

	int swordDamage = 25;
	int _critChance = 20;
    
    void Start() {
        sw = GetComponent<BoxCollider>();
        player = GameObject.Find("Player");
        rbp = player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Damageable")){
            Debug.Log("Hit!");
			int roll = Random.Range(0, _critChance);
			bool crit = (roll == 0);
			other.GetComponent<TrainingDummy>().DropHealth(swordDamage * (crit ? 2 : 1), crit);
		}
    }
}
