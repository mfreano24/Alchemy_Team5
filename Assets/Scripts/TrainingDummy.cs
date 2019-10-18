using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDummy : MonoBehaviour {
    public int health;
    public GameObject sword;
    BoxCollider2D hurtbox;

    void Start() {
        health = 5000;
        hurtbox = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        health-=5;
        Debug.Log("Hit- Dummy Health is now "+health);
    }

	public void DropHealth(int i, bool crit) {

		GameObject damage = (GameObject)Instantiate(Resources.Load("Damage") as Object, this.transform);
		damage.GetComponent<DamageIndicator>().setHealth(i, crit);
		health -= i;
	}

}
