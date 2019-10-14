using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticles : MonoBehaviour {

	// Prefab of the particle
	public GameObject particle;
	
	// Data on the burst
	public int delay;
	public int particleCount;
	public string creation;

	// The type of burst of the enemy
	public string type;

	// Locking the position of certain axes.
	public bool x;
	public bool y;
	public bool z;
	
	// When made, figure out what kind of burst is happening.
	private void Start() {
		if (creation == "Continuous") {
			StartCoroutine(Generate());
		} else if (creation == "Discrete") {
			GenerateBlast();
		}
	}

	private void GenerateBlast() {
		for (int i = 0; i < particleCount; i++) {
			GameObject item = (GameObject)Instantiate(particle, this.transform);
			if (!x) { item.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionX; }
			if (!y) { item.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY; }
			if (!z) { item.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionZ; }
		}
	}

	IEnumerator Generate() {

		for (int i = 0; i < particleCount; i++) {
			yield return new WaitForSeconds(delay / 10);
			GameObject item = (GameObject)Instantiate(particle, this.transform);
			if (!x) { item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX; }
			if (!y) { item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; }
			if (!z) { item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ; }
		}

	}

	// For testing purposes
	public void ResetSimulation() {
		Start();
	}

}
