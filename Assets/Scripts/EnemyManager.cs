using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Alchemy;

public class EnemyManager : MonoBehaviour {

	List<Enemy> enemies = new List<Enemy>();

	public GameObject EnemyPrefab;

	float arenaRadius = 20;
	float safeDistance = 8;

	int wave = 0;

	int timer = 0;

	int timeBetweenWaves = 300;

	public GameObject waveStatus;

    void Start() {
		waveStatus.SetActive(false);
	}

	private void Update() {
		if (timer > 0) {
			timer--;
			if (timer == 5) {
				wave++;
				StartCoroutine(UpdateWaveStatus("Wave " + wave.ToString() + " starting!"));
				SpawnEnemies(wave);
			}
		} else if (GameObject.FindGameObjectsWithTag("Damageable").Length == 0) {
			if (wave > 0) {
				StartCoroutine(UpdateWaveStatus("Wave " + wave.ToString() + " completed!"));
			}
			timer = timeBetweenWaves;
		}
	}

	public IEnumerator UpdateWaveStatus(string text) {
		waveStatus.transform.localPosition = new Vector3(0, 150, 0);
		waveStatus.GetComponent<Text>().text = text;
		waveStatus.SetActive(true);
		for (int i = 0; i < 50; i++) {
			waveStatus.transform.localPosition = new Vector3(waveStatus.transform.localPosition.x, waveStatus.transform.localPosition.y + 1f, waveStatus.transform.localPosition.z);
			yield return new WaitForSeconds(0.0001f);
		}
		waveStatus.SetActive(false);
	}

	public void SpawnEnemies(int wave) {

		int enemies = WaveToEnemy(wave);

		for (int i = 0; i < enemies; i++) {
			Vector2 pos = Random.insideUnitCircle * arenaRadius;
			while (Vector2.Distance(pos, GameObject.Find("Player").transform.position) < safeDistance) {
				pos = Random.insideUnitCircle * arenaRadius;
			}
			GameObject enemy = (GameObject)Instantiate(EnemyPrefab, pos, Quaternion.identity);
		}
	}

	public int WaveToEnemy(int x) {
		// The function for an ouptut of enemies given an input of wave.
		// (x^(3/4) / 2) + 3
		return (int)Mathf.Ceil(Mathf.Pow(x, 3f / 4f) / 2 + 3);
	}

	// Reading the enemies
	void ReadEnemies() {
		// List of elements
		string path = Directory.GetCurrentDirectory() + "\\enemies.txt";

		using (var reader = new StreamReader(path)) {
			// Get reader input
			string input = "hi";

			while (input != "" && input != null) {
				// Creating the items
				// Create a new potion
				Enemy newEnemy = new Enemy();
				for (int i = -1; i < 7; i++) {

					// Read actual input
					input = reader.ReadLine();

					if (input == "" || input == null) {
						break;
					}

					Debug.Log("i = " + i.ToString() + ", input = " + input);

					switch (i) {
						case (0):
							// Setting the element name
							newEnemy.name = input;
							break;
						case (1):
							// Create the combination
							newEnemy.baseHP = System.Convert.ToInt32(input);
							break;
						case (2):
							// Set up the sprite used
							newEnemy.baseATK = System.Convert.ToInt32(input);
							break;
						case (3):
							// Set up the maximum size of the explosion
							newEnemy.baseDEF = System.Convert.ToInt32(input);
							break;
						case (4):
							// Set up the time it lasts
							newEnemy.speed = System.Convert.ToInt32(input);
							break;
						case (5):
							// Set up the damage it'll do
							newEnemy.type = input;
							break;
						default:
							// Just for the -----
							break;
					}
				} // End creation

				enemies.Add(newEnemy);

			} // End of while (input != "" && input != null)
		} // End of File IO

		// Removes an empty potion
		enemies.RemoveAt(enemies.Count - 1);

		foreach (Enemy i in enemies) {
			Debug.Log(i.ToString());
		}
	}
}
