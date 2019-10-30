using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {
	public Potion thisPotion;

	const int _critChance = 20;
	Vector2 force;

	public IEnumerator DropPotion() {
		// Delay the explosion
		yield return new WaitForSeconds(thisPotion.time / 1000f);

		if(thisPotion.name == "Sulfur"){
			Debug.Log("Dropped Sulfur!");
			for(int i = 0 ; i < 5 ; i++){
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size*3) {
						int roll = Random.Range(0, _critChance);
						bool crit = (roll == 0);
						// Critical hit!
						enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
						
					}				
				}
				yield return new WaitForSeconds(0.5f);
			}	
		}

		else if(thisPotion.name == "Nitrogen"){
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")){
				//enemy.speed -= 1;
			}
			yield return new WaitForSeconds(3f);
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")){
				//enemy.speed += 1;
			}
		}

		else if(thisPotion.name == "Oxygen"){
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")){
				//this is definitely not correct but I'm cool with it for now lol
				force = new Vector2(enemy.GetComponent<Rigidbody2D>().position.x,enemy.GetComponent<Rigidbody2D>().position.y);
				enemy.GetComponent<Rigidbody2D>().AddForce(force);
			}

		}
		else if(thisPotion.name == "GreaterSulfur"){
			for(int i = 0 ; i < 10 ; i++){
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size*3) {
						int roll = Random.Range(0, _critChance);
						bool crit = (roll == 0);
						// Critical hit!
						enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
						
					}				
				}
				yield return new WaitForSeconds(0.3f);
			}

		}

		else if(thisPotion.name == "GreaterNitrogen"){
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")){
				//enemy.GetComponent<TrainingDummy>().speed /= 2.25f;
			}
			yield return new WaitForSeconds(3f);
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")){
				//enemy.GetComponent<TrainingDummy>().speed *= 2.25f;
			}
		}

		else if(thisPotion.name == "GreaterOxygen"){
			//force will just be stronger

		}

		else if(thisPotion.name == "Explosion"){
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
				if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size*3) {
					int roll = Random.Range(0, _critChance);
					bool crit = (roll == 0);
					// Critical hit!
					enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
				}
			}
		}

		else if(thisPotion.name == "(Nitrogen-OxygenMix)"){
		}

		else if(thisPotion.name == "(Sulfur-OxygenMix)"){

		}

		// Delete the game object
		Destroy(this.gameObject);
	}

}