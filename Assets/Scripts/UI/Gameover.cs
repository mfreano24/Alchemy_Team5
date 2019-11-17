using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour {

	bool tryAgain;

	private void Start() {
		tryAgain = true;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
			tryAgain = !tryAgain;
			GameObject.Find("Cursor").transform.localPosition = new Vector3(-100, tryAgain ? -50 : -150);
		}

		if (Input.GetButton("Submit")) {
			if (tryAgain) {
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainRoom");
			} else {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
			}
		}
	}

}
