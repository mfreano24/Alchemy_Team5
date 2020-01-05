using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

	RectTransform rect;

	private void Start() {
		rect = GetComponent<RectTransform>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		return;
	}

	public void OnDrag(PointerEventData eventData) {
		// Set the initial position of the pointer
		float posX = eventData.position.x - transform.parent.position.x;
		float posY = eventData.position.y - transform.parent.position.y;

		float angle = Mathf.Atan(posY / posX);

		posX = Mathf.Clamp(posX, -65, 65);
		posY = Mathf.Clamp(posY, -65, 65);

		rect.anchoredPosition = new Vector2(posX, posY);
	}

	public void OnEndDrag(PointerEventData eventData) {
		rect.anchoredPosition = Vector3.zero;
	}

	public void OnPointerDown(PointerEventData eventData) {
		return;
	}

}
