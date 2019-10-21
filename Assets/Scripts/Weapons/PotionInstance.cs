using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {
	public Potion thisPotion;

	const int _critChance = 20;

	public IEnumerator DropPotion() {
		// Delay the explosion
		yield return new WaitForSeconds(thisPotion.time / 1000f);

		// Create the explosion
		//GameObject explosion = (GameObject)Instantiate(Resources.Load(thisPotion.sprite.name) as Object, this.transform, true);
		//
		//for (int i = 0; explosion.transform.localScale.magnitude < thisPotion.size * 100; i++) {
		//	explosion.transform.localScale = new Vector2(i / 100, i / 100);
		//}

		// Damage anything in range
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size) {
				
				int roll = Random.Range(0, _critChance);
				bool crit = (roll == 0);
				// Critical hit!
				enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
			}
		}

		// Delete the game object
		Destroy(this.gameObject);
	}

}