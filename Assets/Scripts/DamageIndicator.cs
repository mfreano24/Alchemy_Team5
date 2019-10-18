using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	int health;

	public void setHealth(int i, bool crit) {
		health = i;
		if (crit) {
			this.transform.GetChild(0).transform.GetComponent<Text>().color = Color.red;
		}
		this.transform.GetChild(0).transform.GetComponent<Text>().text = "-" + health.ToString();
		StartCoroutine(Animate());
	}

	IEnumerator Animate() {
		for (int i = 0; i < 25; i++) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.08f, this.transform.localPosition.z);
			yield return new WaitForSeconds(0.0001f);
		}

		if (this.transform.GetComponentInParent<TrainingDummy>().health <= 0) {
			Destroy(this.transform.parent.gameObject);
		}

		Destroy(this.gameObject);

	}

}
