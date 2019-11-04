using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {
	public Potion thisPotion;
	public bool isEnemyDrop = false;
	const int _critChance = 20;

	public IEnumerator DropPotion() {
		// Delay the explosion
		if (!isEnemyDrop) {
			yield return new WaitForSeconds(thisPotion.time / 1000f);

			if (thisPotion.name == "Sulfur") {
				Debug.Log("Dropped Sulfur!");
				for (int i = 0; i < 5; i++) {
					foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
						if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
							int roll = Random.Range(0, _critChance);
							bool crit = (roll == 0);
							// Critical hit!
							enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);

						}
					}

					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
						if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
							player.GetComponent<PlayerController>().currentHealth -= 0.0125f * player.GetComponent<PlayerController>().maxHealth;
						}
					}
					yield return new WaitForSeconds(0.5f);
				}
			} else if (thisPotion.name == "Nitrogen") {
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					//enemy.GetComponent<TrainingDummy>().speed /= 1.75f;
				}
				yield return new WaitForSeconds(3f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					//enemy.GetComponent<TrainingDummy>().speed *= 1.75f;
				}
			} else if (thisPotion.name == "Oxygen") {
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					/*Get the (hopefully) rigidbody component of the enemy and add a force according to a
					Vector2 defined by the position of the enemy minus the position of the potion on the map */
				}

			} else if (thisPotion.name == "Greater Sulfur") {
				for (int i = 0; i < 10; i++) {
					foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
						if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
							int roll = Random.Range(0, _critChance);
							bool crit = (roll == 0);
							// Critical hit!
							enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);

						}
					}
					foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
						if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
							player.GetComponent<PlayerController>().currentHealth -= 0.0125f * player.GetComponent<PlayerController>().maxHealth;
						}
					}

					yield return new WaitForSeconds(0.3f);
				}

			} else if (thisPotion.name == "Greater Nitrogen") {
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					//enemy.GetComponent<TrainingDummy>().speed /= 2.25f;
				}
				yield return new WaitForSeconds(3f);
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					//enemy.GetComponent<TrainingDummy>().speed *= 2.25f;
				}
			} else if (thisPotion.name == "Greater Oxygen") {
				//force will just be stronger

			} else if (thisPotion.name == "Explosion") {
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
						int roll = Random.Range(0, _critChance);
						bool crit = (roll == 0);
						// Critical hit!
						enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
						player.GetComponent<PlayerController>().takeDamage(20f);
						player.GetComponent<PlayerController>().Knockback(transform.position, player.transform.position);
					}

				}
			} else if (thisPotion.name == "(Nitrogen-OxygenMix)") {
			} else if (thisPotion.name == "(Sulfur-OxygenMix)") {

			}

			// Delete the game object
			Destroy(this.gameObject);
		}
		
	}

}