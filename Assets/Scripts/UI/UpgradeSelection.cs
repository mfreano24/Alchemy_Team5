using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelection : MonoBehaviour {

	PlayerController pc;
	GlobalVars gv;

	public int currentSelection;
	public bool upgrading;

	public void Start() {
		upgrading = false;
		currentSelection = -1;
		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
	}

	private void Update() {
		if (upgrading) {
			UpdateLook();
			if (Input.GetKeyDown(KeyCode.A)) {
				currentSelection--;
				if (currentSelection < 0) {
					currentSelection = 3;
				}
			} else if (Input.GetKeyDown(KeyCode.D)) {
				currentSelection++;
				if (currentSelection > 3) {
					currentSelection = 0;
				}
			} else if (Input.GetKeyDown(KeyCode.W)) {
				currentSelection -= 2;
				if (currentSelection < 0) {
					currentSelection += 4;
				}
			} else if (Input.GetKeyDown(KeyCode.S)) {
				currentSelection += 2;
				if (currentSelection > 3) {
					currentSelection -= 4;
				}
			}

			if (Input.GetButtonDown("Submit")) {
				switch (currentSelection) {
					case 0:
						IncreaseMaxHealth();
						break;
					case 1:
						IncreaseHealingFactor();
						break;
					case 2:
						IncreaseCapacity();
						break;
					case 3:
						IncreaseSpeed();
						break;
				}
			}
		}
	}

	void UpdateLook() {

		//Health
		ColorBlock cb = GameObject.Find("HealthButton").GetComponent<Button>().colors;
		cb.normalColor = (currentSelection == 0) ? Color.green : Color.black;
		GameObject.Find("HealthButton").GetComponent<Button>().colors = cb;

		// Factor
		cb = GameObject.Find("FactorButton").GetComponent<Button>().colors;
		cb.normalColor = (currentSelection == 1) ? Color.green : Color.black;
		GameObject.Find("FactorButton").GetComponent<Button>().colors = cb;

		// Capacity
		cb = GameObject.Find("CapacityButton").GetComponent<Button>().colors;
		cb.normalColor = (currentSelection == 2) ? Color.green : Color.black;
		GameObject.Find("CapacityButton").GetComponent<Button>().colors = cb;

		//Sepeed
		cb = GameObject.Find("CapacityButton").GetComponent<Button>().colors;
		cb.normalColor = (currentSelection == 3) ? Color.green : Color.black;
		GameObject.Find("SpeedButton").GetComponent<Button>().colors = cb;
	}

	public IEnumerator OnUpgradeBegin() {
		upgrading = true;
		gv.playing = false;
		for (int i = 0; i < 56; i++) {
			transform.localPosition = new Vector2(0, (float)f(i));
			yield return new WaitForEndOfFrame();
		}
		currentSelection = 0;
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
		upgrading = false;
		currentSelection = -1;
		for (int i = 55; i > -1; i--) {
			transform.localPosition = new Vector2(0, (float)f(i));
			yield return new WaitForEndOfFrame();
		}
	}

	double f(int x) {
		return -750 - 58.5 * x + 4.6 * x * x - 0.09 * x * x * x + 0.00055 * x * x * x * x;
	}
}
