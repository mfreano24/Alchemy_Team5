﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;
using System.IO;

[System.Serializable]
public class WaveManager : MonoBehaviour {

	public GameObject spawner;

	public int WAVE_COUNT;

	[SerializeField]
	public List<Wave> waves = new List<Wave>();
	
	// Start is called before the first frame update
    void Start() {
		CreateIndices();
    }

	public void StartWave (int i) {
		Wave current = waves[i - 1];

		for (int j = 0; j < current.waveEnemies.Count; j++) {
			GameObject newEnemy = (GameObject)Instantiate(this.transform.GetComponent<EnemyManager>().EnemyPrefab, GameObject.Find("Spawner_" + current.spawnIndices[j].ToString()).transform.position, Quaternion.identity);
			newEnemy.GetComponent<TrainingDummy>().thisEnemy = new Enemy(current.waveEnemies[j]);
		}

	}

	void CreateIndices() {
		// Find the spawn index data
		string path = Directory.GetCurrentDirectory() + "\\waves\\spawn_indices.txt";
		using (var reader = new StreamReader(path)) {
			// Initial data
			string input = "hello there!";

			// Create empty spawner
			input = reader.ReadLine();

			// While more data exist
			while (input != null && input != "") {

				string[] data = input.Split(' ');

				GameObject newSpawner = (GameObject)Instantiate(spawner, new Vector3(System.Convert.ToInt32(data[1]), System.Convert.ToInt32(data[2]), 0), Quaternion.identity);

				// Set up spawner name
				newSpawner.name = "Spawner_" + data[0].ToString();

				// Create empty spawner
				input = reader.ReadLine();
			}
		}

		GenerateWaves();

	}

	void GenerateWaves() {
		for (int i = 1; i <= WAVE_COUNT; i++) {
			string path = Directory.GetCurrentDirectory() + "\\waves\\Wave" + i.ToString() + ".txt";
			using (var reader = new StreamReader(path)) {
				// Initial data
				string input = reader.ReadLine();

				Wave newWave = new Wave();

				// While more data exist
				while (input != null && input != "") {

					string[] fragments = input.Split(' ');

					Enemy e = this.gameObject.GetComponent<EnemyManager>().FindByName(fragments[0]);
					newWave.waveEnemies.Add(e);
					newWave.spawnIndices.Add(System.Convert.ToInt32(fragments[1]));

					input = reader.ReadLine();
				}

				waves.Add(newWave);
			}
		}
	}
	
}