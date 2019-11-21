using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	int selectedIndex;
	int MAX_OBJECTS = 2;
	GlobalVars gv;
	bool moved = false;

	private void Start() {
		selectedIndex = 0;
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
	}

	private void Update() {
		if (Input.GetAxis("Vertical") < 0 && !moved) {
			selectedIndex++;
			selectedIndex++;
			moved = true;
			if (MAX_OBJECTS < selectedIndex) {
				selectedIndex = 0;
			}
		}
		if (Input.GetAxis("Vertical") > 0 && !moved) {
			selectedIndex--;
			selectedIndex--;
			moved = true;
			if (selectedIndex < 0) {
				selectedIndex = MAX_OBJECTS;
			}
		}
		if (Input.GetAxis("Vertical") == 0 && moved) {
			moved = false;
		}

		if (Input.GetButtonDown("Fire2")) {
			SelectObject();
			return;
		}
		UpdatePos();
	}

	void UpdatePos() {
		GameObject.Find("Cursor").transform.localPosition = new Vector2(-150, 200 - selectedIndex * 75);
	}

	void SelectObject() {
		switch (selectedIndex) {
			case 0:
				BackToGame();
				break;
			case 1:
				Options();
				break;
			case 2:
				Quit();
				break;
		}
	}

	void BackToGame() {
		gv.playing = true;
		GameObject.Find("PauseScreen").SetActive(false);
	}

	void Options() {

	}

	private void Quit() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
	}

}
