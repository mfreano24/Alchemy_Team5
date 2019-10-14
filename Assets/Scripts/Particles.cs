using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

	int life;

	public int lifeMin;
	public int lifeMax;

	public int speedCap;

	private void Start() {
		life = Random.Range(lifeMin, lifeMax);
		this.GetComponent<Rigidbody>().velocity = new Vector3(SpeedParameter(), SpeedParameter(), SpeedParameter());

		StartCoroutine(LifeCycle());
	}

	float SpeedParameter() {
		float speed;

		speed = Random.Range(-speedCap, speedCap) / 1000f;

		return speed;
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.transform.name != "Particle(Clone)") {

		}
	}

	IEnumerator LifeCycle() {
		yield return new WaitForSeconds(life / 10);
		Destroy(this.gameObject);
	}


}
