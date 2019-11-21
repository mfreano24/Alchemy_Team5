using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour {
	public int x, y;
	public bool active;

	public void RecolorChildren(Color c) {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).transform.GetComponent<SpriteRenderer>().color = c;
		}
	}
}
