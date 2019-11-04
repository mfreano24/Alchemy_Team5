using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Alchemy;

public class EnemyManager : MonoBehaviour {

	List<Enemy> enemies = new List<Enemy>();

	public GameObject EnemyPrefab;

	int wave = 0;

	int timer = 0;

	int timeBetweenWaves = 300;

	public GameObject waveTimer;
	public GameObject waveStatus;

    void Awake() {
		waveTimer.SetActive(false);
		waveStatus.SetActive(false);
		ReadEnemies();
	}

	private void Update() {
		if (timer > 0) {
			timer--;
			waveTimer.GetComponent<Text>().text = "Next wave in " + (timer / 30 + 1).ToString() + " seconds!\nPress F to skip timer!";
			if (timer == 5) {
				wave++;
				StartCoroutine(UpdateWaveStatus("Wave " + wave.ToString() + " starting!"));
				GameObject.Find("EventSystem").GetComponent<WaveManager>().StartWave(wave);
				waveTimer.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.F)) {
				timer = 10;
			}

		} else if (GameObject.FindGameObjectsWithTag("Damageable").Length == 0) {
			if (wave > 0) {
				StartCoroutine(UpdateWaveStatus("Wave " + wave.ToString() + " completed!"));
			}
			timer = timeBetweenWaves;
			waveTimer.GetComponent<Text>().text = "Next wave in " + (timer / 60 + 1).ToString() + " seconds!\nPress F to skip timer!";
			waveTimer.SetActive(true);
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
				for (int i = -1; i < 8; i++) {

					// Read actual input
					input = reader.ReadLine();

					if (input == "" || input == null) {
						break;
					}

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
						case (6):
							// Set up the experience gain
							newEnemy.exp = System.Convert.ToInt32(input);
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
	}

	public Enemy FindByName(string s) {
		foreach (Enemy e in enemies) {
			if (e.name == s) {
				return e;
			}
		}

		return null;
	}

}
