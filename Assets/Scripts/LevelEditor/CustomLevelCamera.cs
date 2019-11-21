using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLevelCamera : MonoBehaviour {

	private void Start() {
		GameObject.Find("CustomLevelManager").GetComponent<StageManager>().GenerateWorld();
	}

}
