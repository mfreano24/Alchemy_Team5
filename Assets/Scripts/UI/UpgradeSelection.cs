using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelection : MonoBehaviour {

	PlayerController pc;
	GlobalVars gv;

	public void Start() {
		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
	}

	public IEnumerator OnUpgradeBegin() {
		gv.playing = false;
		for (int i = 0; i < 56; i++) {
			transform.localPosition = new Vector2(0, (float)f(i));
			yield return new WaitForEndOfFrame();
		}
	}

	public void IncreaseMaxHealth() {
		if (!gv.playing) {
			gv.playing = true;
			float ratio = pc.currentHealth / pc.maxHealth;
			pc.maxHealth += 50;
			pc.currentHealth = pc.maxHealth * ratio;
			StartCoroutine(OnUpgradeComplete());
		}
	}

	public void IncreaseHealingFactor() {
		if (!gv.playing) {
			gv.playing = true;
			pc.HEAL_FACTOR += 15;
			StartCoroutine(OnUpgradeComplete());
		}
	}

	public void IncreaseSpeed() {
		if (!gv.playing) {
			gv.playing = true;
			pc.playerSpeed += 2;
			StartCoroutine(OnUpgradeComplete());
		}
	}

	public void IncreaseCapacity() {
		if (!gv.playing) {
			gv.playing = true;
			pc.MAX_ITEMS += 2;
			StartCoroutine(OnUpgradeComplete());
		}
	}

	public IEnumerator OnUpgradeComplete() {
		for (int i = 55; i > -1; i--) {
			transform.localPosition = new Vector2(0, (float)f(i));
			yield return new WaitForEndOfFrame();
		}
	}

	double f(int x) {
		return -750 - 58.5 * x + 4.6 * x * x - 0.09 * x * x * x + 0.00055 * x * x * x * x;
	}
}
