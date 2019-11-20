using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	GameObject title;
	AudioSource cur;

	int selectedIndex = 0;

	int MAX_OBJECTS = 2;

	bool moved = false;

	private void Start() {
		cur = GetComponent<AudioSource>();
		cur.volume = 0.5f;
		title = GameObject.Find("Title");
		StartCoroutine(Pulse());
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Submit")) {
			
		}

		if (Input.GetAxis("Vertical") < 0 && !moved) {
			cur.Play();
			moved = true;
			selectedIndex++;
			if (MAX_OBJECTS < selectedIndex) {
				selectedIndex = 0;
			}
		}
		if (Input.GetAxis("Vertical") > 0 && !moved) {
			cur.Play();
			moved = true;
			selectedIndex--;
			if (selectedIndex < 0) {
				selectedIndex = MAX_OBJECTS;
			}
		}

		if (Input.GetAxis("Vertical") == 0 && moved) {
			moved = false;
		}

		if (Input.GetButtonDown("Submit")) {
			cur.Play();
			SelectObject();
			return;
		}
		UpdatePos();
	}

	private void UpdatePos() {
		GameObject.Find("Cursor").transform.localPosition = new Vector3(-75, -150 * selectedIndex, 0);
	}

	private void SelectObject() {
		switch (selectedIndex) {

			case 0:
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainRoom");
				break;
			case 1:
				Options();
				break;
			case 2:
				Application.Quit();
				break;
		}
	}

	private void Options() {
		throw new NotImplementedException();
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