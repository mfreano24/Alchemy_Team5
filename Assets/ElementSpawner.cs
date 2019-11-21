using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour {
	public int timer;

	PotionManager pm;

	public void Start() {
		pm = GameObject.Find("EventSystem").GetComponent<PotionManager>();
		StartCoroutine(Spawner());
	}

	IEnumerator Spawner() {
		while (true) {

			timer = Random.Range(900, 2400);

			for (int i = 0; i < timer; i++) {
				yield return new WaitForEndOfFrame();
			}

			GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, transform.position, Quaternion.identity);
			enemyDrop.GetComponent<PotionInstance>().thisPotion = pm.potions[0];
			enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
			StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());
		}
	}
}
