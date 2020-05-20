using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour {

	public Level currentStage;
	public bool isCustomLevel;

	public List<TileBase> tiles;
	public List<int> wallVisLayer = new List<int>() { 77, 79, 80, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95 };
	GlobalVars gv;

	public GameObject spawner;

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
		isCustomLevel = false;

		// Debugging
		// currentStage = new Level("Admin", "Stage 1", "68999", "{{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, 8, 8, 8, 8, 8, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, -1, 0, }{-1, -1, 8, 8, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, 0, }{-1, -1, 8, 8, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, 0, }{-1, -1, 8, 8, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, 0, }{-1, -1, 8, 8, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, 0, }{-1, -1, 8, 8, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, 0, }{-1, -1, -1, 8, 8, 8, 8, 8, 8, 8, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, 8, 8, 8, 8, 8, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, }{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, }}", "{{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 79, 80, 80, 80, 80, 80, 80, 76, 89, 89, 89, 0, }{89, 89, 89, 79, 86, -1, -1, -1, -1, 99, 81, 94, 76, 89, 89, 0, }{89, 89, 79, 86, -1, -1, -1, -1, -1, -1, 99, 81, 94, 76, 89, 0, }{89, 89, 85, -1, -1, -1, -1, -1, -1, -1, -1, 99, 81, 77, 89, 0, }{89, 89, 85, -1, -1, -1, -1, -1, -1, -1, -1, 99, 81, 77, 89, 0, }{89, 89, 85, -1, -1, -1, -1, -1, -1, -1, -1, 99, 81, 77, 89, 0, }{89, 89, 85, -1, -1, -1, -1, -1, -1, -1, -1, 99, 81, 77, 89, 0, }{89, 89, 85, -1, -1, -1, -1, -1, -1, -1, -1, 99, 81, 77, 89, 0, }{89, 89, 83, 84, -1, -1, -1, -1, -1, -1, 99, 81, 92, 78, 89, 0, }{89, 89, 89, 83, 84, -1, -1, -1, -1, 99, 81, 92, 78, 89, 89, 0, }{89, 89, 89, 89, 83, 82, 82, 82, 82, 82, 82, 78, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 89, 0, }{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, }}", "{{0, -3, -3}{1, 2, -3}{2, 2, 2}{3, -3, 2}}");

	}

	public void Create() {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Random") {
			
		} else if (isCustomLevel) {
			// Clearing the room
			GameObject.Find("Floor").GetComponent<Tilemap>().ClearAllTiles();
			GameObject.Find("WalVis").GetComponent<Tilemap>().ClearAllTiles();
			GameObject.Find("WallCol").GetComponent<Tilemap>().ClearAllTiles();

			// Destroying spawners
			foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Spawner")) {
				Destroy(spawner);
			}
			GenerateWorld();
		}
	}

	public void GenerateWorld() {
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();

		for (int i = 0; i < 19; i++) {
			for (int j = 0; j < 15; j++) {
				if (currentStage.Floor[i, j] != -1) {
					GameObject.Find("Floor").GetComponent<Tilemap>().SetTile(new Vector3Int(i - gv.worldSizeX, j - gv.worldSizeY, 0), tiles[currentStage.Floor[i, j]]);
				}

				if (currentStage.Walls[i, j] == -1) {
				} else if (wallVisLayer.Contains(currentStage.Walls[i, j])) {
					GameObject.Find("WalVis").GetComponent<Tilemap>().SetTile(new Vector3Int(i - gv.worldSizeX, j - gv.worldSizeY, 0), tiles[currentStage.Walls[i, j]]);
					GameObject.Find("WallCol").GetComponent<Tilemap>().SetTile(new Vector3Int(i - gv.worldSizeX, j - gv.worldSizeY, 0), tiles[currentStage.Walls[i, j]]);
				} else {
					GameObject.Find("WallCol").GetComponent<Tilemap>().SetTile(new Vector3Int(i - gv.worldSizeX, j - gv.worldSizeY, 0), tiles[currentStage.Walls[i, j]]);
				}
			}
		}

		for (int i = 0; i < 4; i++) {
			GameObject newSpawner = (GameObject)Instantiate(spawner, new Vector3(currentStage.Spawners[i].x, currentStage.Spawners[i].y, 0), Quaternion.identity);

			// Set up spawner name
			newSpawner.name = "Spawner_" + i.ToString();
		}
	}

}
