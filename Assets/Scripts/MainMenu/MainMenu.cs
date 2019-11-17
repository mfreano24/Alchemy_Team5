using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Submit")) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainRoom");
		}
	}
}
