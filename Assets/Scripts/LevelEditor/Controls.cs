using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

	public Tilemap floors;
	public Tilemap wallsCol;
	public Tilemap wallsVis;

	public List<TileBase> tiles;
	public int iterator;

	public GameObject cross;
	GameObject crossedWith;

	public string mode;
	public bool submitted = false;
	public bool succeeeded = false;

	GameObject success;
	Text successText;

	int FIRST_WALL = 76;

	Vector3Int gridPoint;
	Vector3Int oldPoint;

	public List<Vector3> spawners = new List<Vector3>();

	int[,] floor = new int[20, 16];
	int[,] walls = new int[20, 16];

	List<Vector2> activeSpawners;

	// Start is called before the first frame update
	void Awake() {
		success = GameObject.Find("Success");
		successText = GameObject.Find("SuccessText").GetComponent<Text>();
		success.SetActive(false);

		gridPoint = new Vector3Int(0, 0, 0);
		oldPoint = new Vector3Int(0, 0, 0);
		mode = "Edit";
		iterator = 0;

		activeSpawners = new List<Vector2>();

		// Instantiate Crosses
		for (int i = -9; i <= 9; i++) {
			for (int j = -7; j <= 7; j++) {
				GameObject newCross = (GameObject)Instantiate(cross, new Vector2(i, j), Quaternion.identity);
				newCross.transform.localScale.Set(0.2f, 0.2f, 0.2f);
				newCross.GetComponent<Cross>().x = i;
				newCross.GetComponent<Cross>().y = j;
				newCross.GetComponent<Cross>().active = false;
				newCross.layer = SortingLayer.NameToID("UI");
				floor[i + 9, j + 7] = -1;
				walls[i + 9, j + 7] = -1;
			}
		}
	}

	// Update is called once per frame
	void Update() {
		GameObject.Find("Cursor").transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * 10;

		if (Mathf.Abs(GameObject.Find("Cursor").transform.position.x) < 8.5f && Mathf.Abs(GameObject.Find("Cursor").transform.position.y) < 6.5f) {
			if (crossedWith != null) {
				Debug.Log(crossedWith.name);
			}
			// Swap through the floor if needed
			if (Input.GetButtonDown("Next")) {
				iterator++;
				if (iterator >= tiles.Count) {
					iterator = 0;
				}
			}

			if (Input.GetButtonDown("Previous")) {
				iterator--;
				if (iterator < 0) {
					iterator = tiles.Count - 1;
				}
			}

			if (oldPoint != gridPoint && (floor[oldPoint.x + 9, oldPoint.y + 7] != iterator && walls[oldPoint.x + 9, oldPoint.y + 7] != iterator)) {
				if (floor[oldPoint.x + 9, oldPoint.y + 7] == -1) {
					// There was nothing to begin with.
					floors.SetTile(oldPoint, null);
				} else {
					floors.SetTile(oldPoint, tiles[floor[oldPoint.x + 9, oldPoint.y + 7]]);
				}

				if (walls[oldPoint.x + 9, oldPoint.y + 7] == -1) {
					// There was nothing to begin with.
					wallsCol.SetTile(oldPoint, null);
				} else {
					wallsCol.SetTile(oldPoint, tiles[walls[oldPoint.x + 9, oldPoint.y + 7]]);
				}
			}

			oldPoint = gridPoint;
			Vector3 point = GameObject.Find("Cursor").transform.position + new Vector3(-0.45f, 0.45f, 0);
			gridPoint = new Vector3Int((int)Mathf.Floor(point.x), (int)Mathf.Floor(point.y), 0);

			// Overlay the preview
			if (mode == "Edit") {
				if (tiles[iterator].name.StartsWith("Floor")) {
					floors.SetTile(gridPoint, tiles[iterator]);
				} else if (tiles[iterator].name.StartsWith("Wall")) {
					wallsCol.SetTile(gridPoint, tiles[iterator]);
				}
			} else if (mode == "Erase") {
				floors.SetTile(gridPoint, null);
				wallsCol.SetTile(gridPoint, null);
			}


			if (Input.GetButton("Submit")) {
				if (mode == "Edit") {
					if (iterator < FIRST_WALL) {
						floor[gridPoint.x + 9, gridPoint.y + 7] = iterator;
					} else {
						walls[gridPoint.x + 9, gridPoint.y + 7] = iterator;
					}
				} else if (mode == "Erase") {
					floor[gridPoint.x + 9, gridPoint.y + 7] = -1;
					walls[gridPoint.x + 9, gridPoint.y + 7] = -1;
				}
			}

			if (Input.GetButtonDown("Submit")) {
				if (mode == "Spawn") {
					if (crossedWith != null && !crossedWith.GetComponent<Cross>().active && spawners.Count < 4) {
						crossedWith.GetComponent<Cross>().active = true;
						spawners.Add(crossedWith.transform.position);
						crossedWith.GetComponent<Cross>().RecolorChildren(Color.yellow);
						Debug.Log("Spawner added!");
					} else if (crossedWith != null && crossedWith.GetComponent<Cross>().active) {
						spawners.Remove(crossedWith.transform.position);
						crossedWith.GetComponent<Cross>().active = false;
						crossedWith.GetComponent<Cross>().RecolorChildren(Color.white);
						Debug.Log("Spawner removed!");
					}
				}
			}

			if (Input.GetButtonDown("AddCraft")) {
				if (mode == "Edit") {
					mode = "Erase";
				} else if (mode == "Erase") {
					mode = "Spawn";
				} else if (mode == "Spawn") {
					mode = "Edit";
				}
			}
		}

		if (GameObject.Find("Cursor").transform.position.x < -8.5f) {
			if (Input.GetButtonDown("Submit")) {
				if (!submitted) {
					if (GameObject.Find("Cursor").transform.position.x < -9.8f && GameObject.Find("Cursor").transform.position.y > 5.4f) {
						UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
					}

					if (GameObject.Find("Cursor").transform.position.x < -9.8f && GameObject.Find("Cursor").transform.position.y < -5.8f) {
						if (spawners.Count < 4) {
							Debug.Log("Not enough spawners!");
						} else {
							string floorJson = CreateFloor();
							string wallJson = CreateWalls();
							string spawnJson = CreateSpawners();
							Debug.Log(floorJson);
							Debug.Log(wallJson);
							Debug.Log(CreateSpawners());
							string output = (GameObject.Find("EventSystem").GetComponent<DatabaseConnection>().UploadMap("Admin", "Stage " + PlayerPrefs.GetInt("Levels Created").ToString(), Random.Range(1000, 99999).ToString(), CreateFloor(), CreateWalls(), CreateSpawners()));
							submitted = true;
							if (output == "1") {
								succeeeded = true;
							}

							if (!succeeeded) {
								successText.text = "There was a problem uploading your level. Try again another time.\n(Space/Start to return to menu)";
							}
						}
					}
				}
			}
		}

		if (submitted) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
		}
	}

	private void OnTriggerStay(Collider other) {
		if (mode == "Spawn") {
			crossedWith = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other) {
		crossedWith = null;
	}

	public string CreateFloor() {

		string json = "";

		json += "{";

		for (int i = 0; i < 20; i++) {
			json += "{";
			for (int j = 0; j < 16; j++) {
				json += floor[i, j].ToString();
				if (j != 19) {
					json += ", ";
				}
			}
			json += "}";
		}

		json += "}";

		return json;
	}

	public string CreateWalls() {

		string json = "";

		json += "{";

		for (int i = 0; i < 20; i++) {
			json += "{";
			for (int j = 0; j < 16; j++) {
				if (walls[i, j] == -1 && floor[i, j] == -1) {
					json += "89";
				} else {
					json += walls[i, j].ToString();
				}
				if (j != 19) {
					json += ", ";
				}
			}
			json += "}";
		}

		json += "}";

		return json;
	}

	public string CreateSpawners() {
		string json = "";

		json += "{";

		for (int i = 0; i < spawners.Count; i++) {
			json += "{" + i.ToString();
			json += ", " + spawners[i].x + ", " + spawners[i].y + "}";
		}

		json += "}";

		return json;
	}
}

// There are 102 tiles