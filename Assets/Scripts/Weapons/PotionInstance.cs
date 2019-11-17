using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {
	public Potion thisPotion;
	public GameObject hitbox;
	GameObject hb_inst;
	public bool isEnemyDrop = false;
	public bool reaction = false;
	const int _critChance = 20;

	public IEnumerator DropPotion() {
		reaction = false;
		// Delay the explosion
		if (!isEnemyDrop) {
			yield return new WaitForSeconds(thisPotion.time / 1000f);

			if (thisPotion.name == "Sulfur") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*3.5f,thisPotion.size*3.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(214f/255f, 143f/255f, 81f/255f, 50f/100f);
				for (int i = 0; i < 5; i++) {
					foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
						if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3.5f) {
							int roll = Random.Range(0, _critChance);
							bool crit = (roll == 0);
							// Critical hit!
							enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
							if(enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Nitro"){
								reaction = true;
								EnemyExplode(enemy, 1);
							}
						}
					}

					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
						if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3.5f) {
							player.GetComponent<PlayerController>().currentHealth -= 0.0125f * player.GetComponent<PlayerController>().maxHealth;
						}
					}
					yield return new WaitForSeconds(0.5f);
				}
				Destroy(hb_inst);
			}


			else if (thisPotion.name == "Nitrogen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(132f/255f, 217f/255f, 119f/255f, 50f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 1.5f) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(1,1);
						if(enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Sulfur"){
							reaction = true;
							EnemyExplode(enemy, 1f);
						}
					}
				}
				foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
					if(Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 1.5f){
						player.GetComponent<PlayerController>().CallSlowDown(1,1);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}
			
			else if (thisPotion.name == "Oxygen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 50f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3){
						enemy.GetComponent<TrainingDummy>().CallKB(0.08f, 0.08f, this.transform);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3){
						player.GetComponent<PlayerController>().CallKB(0.000008f, 1f, this.transform);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}
			
			
			else if (thisPotion.name == "Greater Sulfur") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*2f,thisPotion.size*2f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(214f/255f, 143f/255f, 81f/255f, 75f/100f);
				for (int i = 0; i < 10; i++) {
					foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
						if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 2) {
							int roll = Random.Range(0, _critChance);
							bool crit = (roll == 0);
							// Critical hit!
							enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
							if(enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Nitro"){
								reaction = true;
								EnemyExplode(enemy, 2f);
							}
						}
					}
					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
						if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
							player.GetComponent<PlayerController>().currentHealth -= 0.0125f * player.GetComponent<PlayerController>().maxHealth;
						}
					}
					yield return new WaitForSeconds(0.3f);
				}
				Destroy(hb_inst);
			}
			
			else if (thisPotion.name == "Greater Nitrogen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(132f/255f, 217f/255f, 119f/255f, 75f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 1.5f) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(1.5f,2);
						if(enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Sulfur"){
							reaction = true;
							EnemyExplode(enemy, 2f);
						}
					}
				}
				foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
					if(Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 1.5f){
						player.GetComponent<PlayerController>().CallSlowDown(1.5f,2);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}

			else if (thisPotion.name == "Greater Oxygen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 50f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3){
						enemy.GetComponent<TrainingDummy>().CallKB(0.1f, 0.09f, this.transform);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3){
						player.GetComponent<PlayerController>().CallKB(0.1f, 0.09f, this.transform);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}
			
			else if (thisPotion.name == "Explosion") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 75f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
						int roll = Random.Range(0, _critChance);
						bool crit = (roll == 0);
						// Critical hit!
						enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
						enemy.GetComponent<TrainingDummy>().CallKB(0.2f, 0.8f, this.transform);
						StartCoroutine(enemy.GetComponent<TrainingDummy>().IncreaseXP());
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
						player.GetComponent<PlayerController>().takeDamage(player.GetComponent<PlayerController>().maxHealth/3);
						player.GetComponent<PlayerController>().CallKB(0.2f, 0.8f, this.transform);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}
			
			else if (thisPotion.name == "Time Warp") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(19f/255f, 214f/255f, 133f/255f, 75f/100f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 10) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(3,3);
					}
				}
				foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
					if(Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 10){
						player.GetComponent<PlayerController>().CallSlowDown(2,1);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			}
			
			else if (thisPotion.name == "Volcano") {
				hb_inst = Instantiate(hitbox);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size*1.5f,thisPotion.size*1.5f,0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 75f/100f);
				for (int i = 0; i < 10; i++) {
					foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
						if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 10) {
							int roll = Random.Range(0, _critChance);
							bool crit = (roll == 0);
							// Critical hit!
							enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
							if(enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Nitro"){
								reaction = true;
								EnemyExplode(enemy, 0.025f);
							}
						}
					}

					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
						if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
							player.GetComponent<PlayerController>().currentHealth -= 0.0125f * player.GetComponent<PlayerController>().maxHealth;
						}
					}
					yield return new WaitForSeconds(0.125f);
				}
				Destroy(hb_inst);

			}

			// Delete the game object
			Destroy(this.gameObject);
		} else {
			yield return new WaitForSeconds(5);
			Destroy(this.gameObject);
		}
		
	}

	public void EnemyExplode(GameObject e, float mult){ //e for enemy
	
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if(Vector3.Distance(enemy.transform.position, e.transform.position) < mult*7.5f) {
				int roll = Random.Range(0, _critChance);
				bool crit = (roll == 0);
				enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
				StartCoroutine(enemy.GetComponent<TrainingDummy>().IncreaseXP());
			}
		}

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if(Vector3.Distance(player.transform.position, e.transform.position) < mult*7.5f) {
				int roll = Random.Range(0, _critChance);
				player.GetComponent<PlayerController>().takeDamage(20f);
				player.GetComponent<PlayerController>().CallKB(0.3f, 0.125f, this.transform);
			}
		}
		Destroy(e);
	}

}