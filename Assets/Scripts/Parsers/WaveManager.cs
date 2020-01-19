using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;
using System.IO;

[System.Serializable]
public class WaveManager : MonoBehaviour {

	public GameObject spawner;

	public int WAVE_COUNT;
	public int curr_wave;
	[SerializeField]
	public List<Wave> waves = new List<Wave>();

	public List<GameObject> currentEnemies = new List<GameObject>();

	// Start is called before the first frame update
	void Start() {
		CreateIndices();
	}

	public void StartWave(int i) {

		curr_wave = i;

		if (i > waves.Count) {
			// Add a random wave if all the manually-implemented waves have been completed.
			Wave customWave = new Wave();
			for (int j = 0; j < 9; j++) {
				customWave.spawnIndices.Add(j);
				customWave.waveEnemies.Add(this.gameObject.GetComponent<EnemyManager>().enemies[Random.Range(0, GameObject.Find("EventSystem").GetComponent<EnemyManager>().enemies.Count - 1)]);
			}

			while (waves.Count != i) {
				waves.Add(new Wave());
			}

			waves.Add(customWave);
		}

		Wave current = waves[i - 1];

		for (int j = 0; j < current.waveEnemies.Count; j++) {
			if (!GameObject.Find("CustomLevelManager").GetComponent<StageManager>().isCustomLevel && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial") {
				GameObject newEnemy = (GameObject)Instantiate(this.transform.GetComponent<EnemyManager>().EnemyPrefab, GameObject.Find("Spawner_" + (current.spawnIndices[j]).ToString()).transform.position, Quaternion.identity);
				newEnemy.GetComponent<TrainingDummy>().thisEnemy = new Enemy(current.waveEnemies[j]);
				newEnemy.GetComponent<TrainingDummy>().thisEnemy.Scale(i);
				currentEnemies.Add(newEnemy);
			} else if (GameObject.Find("CustomLevelManager").GetComponent<StageManager>().isCustomLevel) {
				GameObject newEnemy = (GameObject)Instantiate(this.transform.GetComponent<EnemyManager>().EnemyPrefab, GameObject.Find("Spawner_" + (current.spawnIndices[j] % 4).ToString()).transform.position, Quaternion.identity);
				newEnemy.GetComponent<TrainingDummy>().thisEnemy = new Enemy(current.waveEnemies[j]);
				newEnemy.GetComponent<TrainingDummy>().thisEnemy.Scale(i);
				currentEnemies.Add(newEnemy);
			}
		}

	}

	void CreateIndices() {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainRoom") {
			// Find the spawn index data
			string path = Application.streamingAssetsPath + "\\waves\\spawn_indices.txt";

			//if (Application.platform == RuntimePlatform.Android) {
				WWW dat = new WWW(path);
				while (!dat.isDone) { }
				string info = dat.text;
			//}

			for (int i = 0; i < info.Split('\n').Length; i++) {
				// Create empty spawner
				string input = info.Split('\n')[i];


				string[] data = input.Split(' ');

				GameObject newSpawner = (GameObject)Instantiate(spawner, new Vector3(System.Convert.ToInt32(data[1]), System.Convert.ToInt32(data[2]), 0), Quaternion.identity);

				// Set up spawner name
				newSpawner.name = "Spawner_" + data[0].ToString();
			}
		}

		GenerateWaves();

	}

	void GenerateWaves() {
		for (int i = 1; i <= WAVE_COUNT; i++) {
			string path = Application.streamingAssetsPath + "\\waves\\Wave" + i.ToString() + ".txt";

			//if (Application.platform == RuntimePlatform.Android) {
			WWW dat = new WWW(path);
			while (!dat.isDone) { }
			string info = dat.text;
			//}

			Wave newWave = new Wave();

			for (int j = 0; j < info.Split('\n').Length; j++) {
				// Initial data
				string input = info.Split('\n')[j];

				string[] fragments = input.Split(' ');

				Enemy e = gameObject.GetComponent<EnemyManager>().FindByName(fragments[0]);

				newWave.waveEnemies.Add(e);
				newWave.spawnIndices.Add(System.Convert.ToInt32(fragments[1]));

			}

			waves.Add(newWave);
		}
	}

}
