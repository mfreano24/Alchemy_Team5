using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
		GetComponent<RectTransform>().localScale = Vector3.one * 1.2f;
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
		GetComponent<RectTransform>().localScale = Vector3.one;
	}
}
