using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

	public GameObject spawner;
	public GameObject crystalSpawner;

	public StageManager stageManager;
	Tilemap wallCol;
	Tilemap wallVis;
	Tilemap floor;

	// Dimensions of wall
	public int width;
	public int height;

	// To help with randomness
	public string seed;
	public bool useRandomSeed;

	// Initial wall fill (47 seems to be a magical number
	[Range(15, 47)]
	public int randomFillPercent;

	// Booleans for map values (true = wall, false = floor)
	bool[,] map;
	bool[,] tilesChecked;
	int roomCount;
	List<Vector3Int> coordinates;

	int recursions = 0;

	public List<string> seeds;

	void Awake() {
		// Get proper components
		stageManager = GameObject.Find("CustomLevelManager").GetComponent<StageManager>();
		GlobalVars gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
		wallCol = GameObject.Find("WallCol").GetComponent<Tilemap>();
		wallVis = GameObject.Find("WalVis").GetComponent<Tilemap>();
		floor = GameObject.Find("Floor").GetComponent<Tilemap>();

		// Initial generation of map
		int success = LoadMap();


		while (success > 0) {
			if (success == 5) {
				// Map failed after 5 iterations. Generate from pre-determined seed
				success += LoadMap(seeds[Random.Range(0, seeds.Count - 1)]);
			} else {
				// Generate a map 5 times 
				success += LoadMap();
			}
		}

		// Set up size of map
		gv.worldSizeX = width / 2;
		gv.worldSizeY = height / 2;
	}

	int LoadMap(string _seed = "") {
		// Clear tiles and generate map
		wallCol.ClearAllTiles();
		wallVis.ClearAllTiles();
		floor.ClearAllTiles();
		GenerateMap(_seed);

		if (FloorSum() > width / 0.3f && roomCount == 1) {
			// This is a valid room
			SetBlocks();
			PopulateMap();
			return int.MinValue;
		} else {
			// Room failed
			return 1;
		}
	}

	void PopulateMap() {
		// Find top left corner of the level
		Vector2 topleft = new Vector2(-width / 2, height / 2);

		int playerX = Random.Range(3, width - 3);
		int playerY = Random.Range(3, height - 3);

		// Find proper x coordinates
		while (map[playerX, playerY]) {
			playerX = Random.Range(3, width - 3);
			playerY = Random.Range(3, height - 3);
		}

		Vector2 offset = new Vector2(playerX, -playerY);
		
		// Place player and camera
		//  TODO: Fix this algorithm
		GameObject.Find("Player").transform.position = (Vector3)(topleft + offset) + new Vector3(1, 0.5f, 0);
		Camera.main.transform.position = (Vector3)(topleft + offset) + transform.forward * -10 + new Vector3(1, 0.5f, 0);

		for (int i = 1; i < 9; i++) {
			// Create the spawners
			int locX = Random.Range(3, width - 3);
			int locY = Random.Range(3, height - 3);

			// Find proper x coordinates
			while (map[locX, locY]) {
				locX = Random.Range(3, width - 3);
				locY = Random.Range(3, height - 3);
			}

			offset = new Vector2(locX, -locY);

			// Place spawner
			//  TODO: Fix this algorithm
			GameObject goSpawn = (GameObject)Instantiate(spawner, (Vector3)(topleft + offset) + new Vector3(1, 0.5f, 0), Quaternion.identity);
			goSpawn.name = "Spawner_" + i;
		}

		for (int i = 0; i < 4; i++) {
			// Create the Item Droppers
			int locX = Random.Range(3, width - 3);
			int locY = Random.Range(3, height - 3);

			// Find proper x coordinates
			while (map[locX, locY]) {
				locX = Random.Range(3, width - 3);
				locY = Random.Range(3, height - 3);
			}

			offset = new Vector2(locX, -locY);

			// Place crystal
			GameObject goSpawn = (GameObject)Instantiate(crystalSpawner, (Vector3)(topleft + offset) + new Vector3(1, 0.5f, 0), Quaternion.identity);
			goSpawn.name = "Element_" + i;
		}
	}

	int FloorSum() {

		int size = 0;

		// If it's a floor, + 1
		foreach (bool b in map) {
			if (!b) {
				size++;
			}
		}

		// Return number of floor tiles
		return size;
	}

	void GenerateMap(string _seed = "") {
		// Instantiate the map and fill it
		map = new bool[width, height];
		tilesChecked = new bool[width, height];
		RandomFillMap(_seed);

		// Five iterations seems to help smooth the map
		for (int i = 0; i < 5; i++) {
			SmoothMap();
		}

		// In charge of removing any "two-block" walls
		NotTallEnough();

		// Diagonal Checks
		DiagonalFix();

		// Room sizing
		RoomSize();
	}

	void RoomSize() {
		// Reset the list of coordinates
		coordinates = new List<Vector3Int>();

		// Number of rooms in the map
		roomCount = 0;

		for (int i = 1; i < width - 1; i++) {
			for (int j = 1; j < height - 1; j++) {
				// If the tile is a floor and not checked
				if (!map[i, j] && !tilesChecked[i, j]) {

					// Clear the list
					coordinates.Clear();

					// Find the room size and increment the counter
					int roomSize = CalculateSize(i, j);
					roomCount++;

					// The room is too damn small. Remove it
					if (roomSize < width) {
						roomCount--;
						foreach (Vector3Int v in coordinates) {
							map[v.x, v.y] = true;
						}
					}
				}
			}
		}
	}

	int CalculateSize(int x, int y) {
		// Initialize the size
		int size = 0;

		// Stop the coordinate from getting checked
		tilesChecked[x, y] = true;
		coordinates.Add(new Vector3Int(x, y, 0));

		// Check the left tile
		if (x != 0) {
			if (!map[x - 1, y] && !tilesChecked[x - 1, y]) {
				size += CalculateSize(x - 1, y);
			}
		}

		// Check the above tile
		if (y != 0) {
			if (!map[x, y - 1] && !tilesChecked[x, y - 1]) {
				size += CalculateSize(x, y - 1);
			}
		}

		// Check the right tile
		if (x != width - 1) {
			if (!map[x + 1, y] && !tilesChecked[x + 1, y]) {
				size += CalculateSize(x + 1, y);
			}
		}

		// Check the left tile
		if (y != height - 1) {
			if (!map[x, y + 1] && !tilesChecked[x, y + 1]) {
				size += CalculateSize(x, y + 1);
			}
		}

		// Return the map size
		return 1 + size;
	}

	void DiagonalFix() {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (FindFloor(i, j) == 3) {
					// If the tile left and down one is the a wall and has height 3, extend this wall left
					if (FindFloor(i - 1, j + 1) == 3) {
						map[i - 1, j] = true;
					}

					// If the tile left and down one is a wall of height 3, set tile to the right as wall
					if (FindFloor(i + 1, j + 1) == 3) {
						map[i + 1, j] = true;
					}
				}
			}
		}
	}

	void NotTallEnough() {
		// For every column
		for (int i = 0; i < width; i++) {

			// Set initial height to 0
			int current = 0;

			for (int j = 0; j < height; j++) {
				if (map[i, j]) {
					// The current tile is a wall
					current++;
				} else {
					// The current tile is a floor
					if (current < 3) {
						// Fix the other walls
						for (int k = 1; k <= current; k++) {
							map[i, j - k] = false;
						}
					}

					// Reset the height for next time
					current = 0;
				}

				if (i < 2 || i > width - 3 || j < 4 || j > height - 3) {
					map[i, j] = true;
				}
			}
		}
	}

	void RandomFillMap(string _seed = "") {
		if (_seed == "") {
			seed = System.DateTime.Now.ToString("o", CultureInfo.CreateSpecificCulture("de-US"));
			Debug.Log(seed);
		} else {
			seed = _seed;
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x < 2 || x > width - 3 || y < 4 || y > height - 3) {
					map[x, y] = true;
				} else {
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent);
				}
				tilesChecked[x, y] = false;
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				if (neighbourWallTiles > 4)
					map[x, y] = true;
				else if (neighbourWallTiles < 4)
					map[x, y] = false;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX, neighbourY] ? 1 : 0;
					}
				} else {
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	void SetBlocks() {
		// Iterate through the map and set up tiles
		Vector3Int topleft = new Vector3Int(-width / 2, height / 2, 0);
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				Vector3Int offset = new Vector3Int(i, -j, 0);

				int neighbors = NeighborValues(i, j);

				// TODO: Map out the walls properly
				if (map[i, j]) {
					int closest = FindClosestFloor(i, j);
					if (closest >= 3) {
						Tilemap layer;
						try {
							if (!map[i, j - 1]) {
								layer = wallVis;
							} else {
								layer = wallCol;
							}
						} catch (System.Exception e) {
							layer = wallCol;
						}

						if (closest == 3) {
							if (FindFloor(i, j) == 3) {
								layer.SetTile(topleft + offset, stageManager.tiles[77]);
							} else if (FindFloor(i - 1, j) == 3) {
								layer.SetTile(topleft + offset, stageManager.tiles[78]);
							} else if (FindFloor(i + 1, j) == 3) {
								layer.SetTile(topleft + offset, stageManager.tiles[76]);
							}
						}

						if (wallVis.GetTile(topleft + offset) == null && wallCol.GetTile(topleft + offset) == null) {
							layer.SetTile(topleft + offset, stageManager.tiles[89]);
						}

					} else {
						// Still a wall
						int directBelow = FindFloor(i, j);
						if (directBelow == 1) {
							// The floor is directly below
							if ((neighbors & 24) == 0) {
								// Floor on both ends
								wallCol.SetTile(topleft + offset, stageManager.tiles[100]);
							} else if ((neighbors & 24) == 16) {
								// Floor on left
								wallCol.SetTile(topleft + offset, stageManager.tiles[99]);
							} else if ((neighbors & 24) == 8) {
								// Floor on right
								wallCol.SetTile(topleft + offset, stageManager.tiles[101]);
							} else {
								wallCol.SetTile(topleft + offset, stageManager.tiles[100]);
							}
						} else if (directBelow == 2) {
							neighbors = NeighborValues(i, j + 1);
							if ((neighbors & 24) == 0) {
								// Floor on both ends
								wallCol.SetTile(topleft + offset, stageManager.tiles[97]);
							} else if ((neighbors & 24) == 16) {
								// Floor on left
								wallCol.SetTile(topleft + offset, stageManager.tiles[96]);
							} else if ((neighbors & 24) == 8) {
								// Floor on right
								wallCol.SetTile(topleft + offset, stageManager.tiles[98]);
							} else {
								wallCol.SetTile(topleft + offset, stageManager.tiles[97]);
							}
						} else if (directBelow == 3) {
							neighbors = NeighborValues(i, j + 2);
							if ((neighbors & 24) == 0) { // 1100 0111 & 0001 1000 = 0000 0000
								// Floor on both ends
								wallCol.SetTile(topleft + offset, stageManager.tiles[95]);
							} else if ((neighbors & 24) == 16) {
								// Floor on left
								wallCol.SetTile(topleft + offset, stageManager.tiles[92]);
							} else if ((neighbors & 24) == 8) {
								// Floor on right
								wallCol.SetTile(topleft + offset, stageManager.tiles[94]);
							} else {
								wallCol.SetTile(topleft + offset, stageManager.tiles[93]);
							}
						} else {

						}
					}
				} else {
					floor.SetTile(topleft + offset, stageManager.tiles[0]);

					// Find what kind of border exists if below is a wall
					if (map[i, j + 1]) {
						if ((neighbors & 160) == 0) {
							// Both points are floors (11)
							wallVis.SetTile(topleft + offset, stageManager.tiles[87]);
						} else if ((neighbors & 160) == 32) {
							// Left is a floor (8)
							wallVis.SetTile(topleft + offset, stageManager.tiles[86]);
						} else if ((neighbors & 160) == 128) {
							// Right is a floor (10)
							wallVis.SetTile(topleft + offset, stageManager.tiles[84]);
						} else {
							// Neither is a floor (9)
							wallVis.SetTile(topleft + offset, stageManager.tiles[85]);
						}
					}
				}

				if (FindClosestFloor(i, j) > 3) {
					if (AboveIsFloor(i, j)) {
						wallVis.SetTile(topleft + offset, stageManager.tiles[89]);
					} else {
						// Set it on the WallCol layer
						wallCol.SetTile(topleft + offset, stageManager.tiles[89]);
					}
				} else {
					floor.SetTile(topleft + offset, stageManager.tiles[40]);

					
				}
			}
		}

		List<TileBase> tiles = new List<TileBase>();

		tiles.Add(stageManager.tiles[76]);
		tiles.Add(stageManager.tiles[78]);
		tiles.Add(stageManager.tiles[84]);
		tiles.Add(stageManager.tiles[86]);
		tiles.Add(stageManager.tiles[87]);

		List<TileBase> bottoms = new List<TileBase>();

		bottoms.Add(stageManager.tiles[92]);
		bottoms.Add(stageManager.tiles[94]);
		bottoms.Add(stageManager.tiles[95]);

		//76, 78, 84, 86, 87
		for (int i = 0; i < width; i++) {
			byte direction = 0;

			for (int j = 0; j < height; j++) {
				Vector3Int offset = new Vector3Int(i, -j, 0);
				if (tiles.Contains(wallCol.GetTile(topleft + offset))) {
					TileBase tb = null;
					if (tiles.IndexOf(wallCol.GetTile(topleft + offset)) == 0 || tiles.IndexOf(wallCol.GetTile(topleft + offset)) == 3) {
						direction = 1;
						tb = stageManager.tiles[90];
					} else if (tiles.IndexOf(wallCol.GetTile(topleft + offset)) == 1 || tiles.IndexOf(wallCol.GetTile(topleft + offset)) == 2) {
						direction = 3;
						tb = stageManager.tiles[88];
					} else {
						direction = 2;
						tb = stageManager.tiles[91];
					}

					if (tb != null) {
						// Go down a tile
						int k = 1;
						while (wallCol.GetTile(topleft + offset + Vector3Int.down * k) == null && wallVis.GetTile(topleft + offset + Vector3Int.down * k) == null) {
							wallCol.SetTile(topleft + offset + Vector3Int.down * k, tb);
							k++;
						}

						if (bottoms.Contains(wallCol.GetTile(topleft + offset + Vector3Int.down * k)) || bottoms.Contains(wallVis.GetTile(topleft + offset + Vector3Int.down * k))) {
							continue;
						}

						k--;

						if (k == 0) {
							continue;
						}

						if (direction == 3) {
							if (map[i - 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[83]); //83
							}
						} else if (direction == 1) {
							if (map[i + 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[79]); //83
							}
						}
					}
				}

				if (tiles.Contains(wallVis.GetTile(topleft + offset))) {
					TileBase tb = null;
					if (tiles.IndexOf(wallVis.GetTile(topleft + offset)) == 0 || tiles.IndexOf(wallVis.GetTile(topleft + offset)) == 2) {
						direction = 1;
						tb = stageManager.tiles[88];
					} else if (tiles.IndexOf(wallVis.GetTile(topleft + offset)) == 1 || tiles.IndexOf(wallVis.GetTile(topleft + offset)) == 3) {
						direction = 3;
						tb = stageManager.tiles[90];
					} else {
						direction = 2;
						tb = stageManager.tiles[91];
					}

					if (tb != null) {
						// Go down a tile
						int k = 1;
						while (wallCol.GetTile(topleft + offset + Vector3Int.down * k) == null && wallVis.GetTile(topleft + offset + Vector3Int.down * k) == null) {
							wallCol.SetTile(topleft + offset + Vector3Int.down * k, tb);
							k++;
						}

						if (bottoms.Contains(wallCol.GetTile(topleft + offset + Vector3Int.down * k)) || bottoms.Contains(wallVis.GetTile(topleft + offset + Vector3Int.down * k))) {
							continue;
						}

						k--;

						if (k == 0) {
							continue;
						}

						if (direction == 1) {
							if (map[i - 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[83]); //83
							}
						} else if (direction == 3) {
							if (map[i + 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[79]); //83
							}
						} else if (direction == 5) {
							if (map[i - 1, j + k + 1] && map[i + 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[102]);
							} else if (map[i - 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[83]); //83
							} else if (map[i + 1, j + k + 1]) {
								wallCol.SetTile(topleft + offset + Vector3Int.down * k, stageManager.tiles[79]); //83
							}
						}
					}
				}
			}
		}
	}

	bool AboveIsFloor(int x, int y) {
		if (y == 0) {
			return false;
		}

		return !map[x, y - 1];
	}

	int FindClosestFloor(int x, int y) {
		List<int> distances = new List<int>();
		for (int i = -1; i < 2; i++) {
			distances.Add(FindFloor(x + i, y));
		}

		distances.Sort();

		return distances[0];
	}

	int FindFloor(int x, int y) {
		int dist = 0;

		if (x == -1 || x == width) {
			return int.MaxValue;
		}

		while (map[x, y]) {
			y++;
			dist++;

			if (y >= height) {
				return int.MaxValue;
			}
		}

		return dist;

	}

	int NeighborValues(int x, int y) {
		int toReturn = 0;

		try {
			toReturn |= (map[x - 1, y - 1] ? 1 : 0) << 0;
		} catch (System.Exception e) {
			toReturn |= 1 << 0;
		}

		try {
			toReturn |= (map[x, y - 1] ? 1 : 0) << 1;
		} catch (System.Exception e) {
			toReturn |= 1 << 1;
		}

		try {
			toReturn |= (map[x + 1, y - 1] ? 1 : 0) << 2;
		} catch (System.Exception e) {
			toReturn |= 1 << 2;
		}

		try {
			toReturn |= (map[x - 1, y] ? 1 : 0) << 3;
		} catch (System.Exception e) {
			toReturn |= 1 << 3;
		}

		try {
			toReturn |= (map[x + 1, y] ? 1 : 0) << 4;
		} catch (System.Exception e) {
			toReturn |= 1 << 4;
		}

		try {
			toReturn |= (map[x - 1, y + 1] ? 1 : 0) << 5;
		} catch (System.Exception e) {
			toReturn |= 1 << 5;
		}

		try {
			toReturn |= (map[x, y + 1] ? 1 : 0) << 6;
		} catch (System.Exception e) {
			toReturn |= 1 << 6;
		}

		try {
			toReturn |= (map[x + 1, y + 1] ? 1 : 0) << 7;
		} catch (System.Exception e) {
			toReturn |= 1 << 7;
		}

		return toReturn;
	}
}
