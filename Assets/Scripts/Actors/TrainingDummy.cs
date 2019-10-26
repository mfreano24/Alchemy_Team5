using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TrainingDummy : MonoBehaviour {
    public int health;
    public GameObject sword;
    BoxCollider2D hurtbox;

	public int experienceDrop;

	public int speed;

	void Start() {
		health = 5000;
		experienceDrop = 20;
		hurtbox = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
        health-=5;
        Debug.Log("Hit- Dummy Health is now "+health);
    }

	public void DropHealth(int i, bool crit) {
		if (health > 0) {
			GameObject damage = (GameObject)Instantiate(Resources.Load("Damage") as Object, this.transform);
			health -= i;

			if (health <= 0) {
				StartCoroutine(IncreaseXP());	
			}

			damage.GetComponent<DamageIndicator>().setHealth(i, crit);
		}
	
	}

	IEnumerator IncreaseXP() {
		for (int i = 0; i < experienceDrop; i++) {
			// Animates the increase (because reasons
			GameObject.Find("Player").GetComponent<PlayerController>().currentExperience++;
			yield return new WaitForSeconds(0.01f);
		}
	}

}
