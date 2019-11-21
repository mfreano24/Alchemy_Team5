using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour {

	bool tryAgain;
	bool moved = false;

	private void Start() {
		tryAgain = true;
	}

	private void Update() {
		if (Input.GetAxis("Vertical") > 0 && !moved) {
			tryAgain = !tryAgain;
			moved = true;
			GameObject.Find("Cursor").transform.localPosition = new Vector3(-100, tryAgain ? -50 : -150);
		}

		if (Input.GetAxis("Vertical") == 0 && moved) {
			moved = false;
		}

		if (Input.GetButton("Fire2")) {
			if (tryAgain) {
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
			} else {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
			}
		}
	}

}
