using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public bool simulatePlayer;

	GameObject title;
	GameObject cursor;
	AudioSource cur;

	int selectedIndex = 0;

	public int selectedLevel = -1;

	int MAX_OBJECTS = 2;

	bool moved = false;

	bool MainScreen = true;
	bool StageSelect = false;
	bool tutorialRead = false;
	bool tutorialPresent = false;

	int direction = 0;

	GameObject warning;
	GameObject levelSelect;
	GameObject howToPlay;

	public GameObject optionPrefab;

	List<Level> levels = new List<Level>();

	List<GameObject> options;

	GameObject mobileUI;

	private void Start() {
		cursor = GameObject.Find("Cursor");
		options = new List<GameObject>();
		options.Add(GameObject.Find("Default"));
		warning = GameObject.Find("Warning");
		howToPlay = GameObject.Find("HowToPlay");
		howToPlay.SetActive(false);
		warning.SetActive(false);
		levelSelect = GameObject.Find("Levels");
		levelSelect.SetActive(false);
		cur = GetComponent<AudioSource>();
		cur.volume = 0.5f;
		title = GameObject.Find("Title");
		StartCoroutine(Pulse());
		mobileUI = GameObject.Find("MobileUI");

		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer || simulatePlayer) {
			cursor.SetActive(false);
		}

		if (Input.GetJoystickNames().Length == 0  && (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)) {
			cursor.SetActive(false);
		}

		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {
			mobileUI.SetActive(false);
		}
	}

	public void UpSelected() {
		direction = 1;
	}

	public void DownSelected() {
		direction = -1;
	}

	public void SelectOption() {
		direction = 2;
	}

	// Update is called once per frame
	void Update() {
		if (cursor.activeInHierarchy) {
			if (!tutorialPresent) {
				if ((Input.GetAxis("Vertical") < 0 || direction == -1) && !moved && MainScreen) {
					cur.Play();
					moved = true;
					selectedIndex++;
					if (MAX_OBJECTS < selectedIndex) {
						selectedIndex = 0;
					}
				}
				if ((Input.GetAxis("Vertical") > 0 || direction == 1) && !moved && MainScreen) {
					cur.Play();
					moved = true;
					selectedIndex--;
					if (selectedIndex < 0) {
						selectedIndex = MAX_OBJECTS;
					}
				}

				if ((Input.GetAxis("Vertical") == 0) && moved && MainScreen) {
					moved = false;
					direction = 0;
				}

				if ((Input.GetAxis("Vertical") < 0 || direction == -1) && !moved && StageSelect) {
					cur.Play();
					moved = true;
					selectedLevel++;
					if (options.Count - 1 <= selectedLevel) {
						selectedLevel = options.Count - 2;
					}
					UpdateColor();
				}

				if ((Input.GetAxis("Vertical") > 0 || direction == 1) && !moved && StageSelect) {
					cur.Play();
					moved = true;
					selectedLevel--;
					if (selectedLevel < -1) {
						selectedLevel = -1;
					}
					UpdateColor();
				}

				if (Input.GetAxis("Vertical") == 0 && moved && StageSelect) {
					moved = false;
					direction = 0;
				}

				if (!StageSelect) {
					UpdatePos();
				}
			}

			if (Input.GetButtonDown("Fire2") || direction == 2) {
				direction = 0;
				cur.Play();
				SelectObject();
				return;
			}
		}
	}

	private void UpdateColor() {
		foreach (GameObject ob in options) {
			ob.GetComponent<Image>().color = Color.black;
		}

		options[selectedLevel + 1].GetComponent<Image>().color = Color.green;
	}

	private void UpdatePos() {
		GameObject.Find("Cursor").transform.localPosition = new Vector3(-75, -300 * selectedIndex / MAX_OBJECTS, 0);
	}

	private void SelectObject() {
		if (MainScreen) {
			switch (selectedIndex) {

				case 0:
					StageSelect = true;
					MainScreen = false;
					levelSelect.SetActive(true);
					if (GameObject.Find("EventSystem").GetComponent<DatabaseConnection>().InternetConnectionFound()) {
						string json = GameObject.Find("EventSystem").GetComponent<DatabaseConnection>().DownloadMaps();
						if (json.StartsWith("[[\"")) {
							Debug.Log(json);
							GenerateLevels(json);
						} else {
							warning.GetComponent<Text>().text = "Error connecting to database!";
							warning.SetActive(true);
						}
					} else {
						warning.SetActive(true);
					}
					break;
				case 1:
					LevelEditor();
					break;
				case 2:
					Application.Quit();
					break;
			}
		} else {
			howToPlay.SetActive(true);
			tutorialPresent = true;
			mobileUI.SetActive(false);
			if (!tutorialRead) {
				GameObject.Find("Levels").SetActive(false);
				GameObject.Find("RegularTitle").SetActive(false);
			}
			if (tutorialRead) {
				if (selectedLevel == -1) {
					UnityEngine.SceneManagement.SceneManager.LoadScene("MainRoom");
				} else {
					GameObject.Find("CustomLevelManager").GetComponent<StageManager>().currentStage = levels[selectedLevel];
					GameObject.Find("CustomLevelManager").GetComponent<StageManager>().isCustomLevel = true;
					UnityEngine.SceneManagement.SceneManager.LoadScene("MainRoom");
				}
			}
			tutorialRead = true;
		}
	}

	private void GenerateLevels(string s) {
		// Remove ends
		s = s.TrimStart('[', '"').TrimEnd(']', '"');

		// Split levels individually
		string[] getLevel = s.Split(new string[] { "\"],[\"" }, StringSplitOptions.None);
		foreach (string level in getLevel) {
			string[] fragments = level.Split(new string[] { "\",\"" }, StringSplitOptions.None);

			Level newLevel = new Level(fragments[0], fragments[1], fragments[2], fragments[3], fragments[4], fragments[5]);

			GameObject choice = (GameObject)Instantiate(optionPrefab, GameObject.Find("LevelHoster").transform, true);
			choice.transform.localPosition = new Vector3(0, 172 - levels.Count * 100, 0);

			choice.GetComponent<LevelChoice>().optionLevel = newLevel;

			levels.Add(newLevel);

			choice.GetComponent<LevelChoice>().optionNumber = levels.Count;
			choice.transform.GetChild(0).GetComponent<Text>().text = newLevel.Name;
			choice.transform.GetChild(1).GetComponent<Text>().text = newLevel.Author;
			choice.transform.GetChild(2).GetComponent<Text>().text = newLevel.Code;
			options.Add(choice);
		}
	}

	private void Options() {
		// Do something
	}

	private void LevelEditor() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor");
	}

	float f(int x) {
		return 0.3f * Mathf.Sin(Time.frameCount * Mathf.PI / 180) + 1;
	}

	IEnumerator Pulse() {
		int x = 0;
		while (true) {
			title.transform.localScale = Vector3.one * (0.1f * Mathf.Sin(x / 30f) + 0.85f);
			x++;
			yield return new WaitForSeconds(0.01f);
		}
	}
}