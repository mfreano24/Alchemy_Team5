﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alchemy;

public class PlayerController : MonoBehaviour {
	public float playerSpeed;
	Vector3 moveDirection;
	CharacterController cc;
	BoxCollider2D sw;
	int endlag = 0;
	float face_Front_x = 0.0f;
	float face_Front_y = 0.0f;
	KeyCode last_Pressed;
	//for reference purposes
	public GameObject player;
	public GameObject sword;
	//for instance purposes
	private GameObject sword_inst;

	public Potion selectedPotion;
	public GameObject potionPrefab;

	public float maxHealth;
	public float currentHealth;
	public float maxExperience;
	public float currentExperience;

	void Start() {

	    cc = GetComponent<CharacterController>();
		sw = sword.GetComponent<BoxCollider2D>();
	    face_Front_x = 0;
	    face_Front_y = -1;

		// DEBUGGING PURPOSES ONLY
		selectedPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[0];
	}

	void Update ()  {
	    //Move First
	    Move();
		//Then Basic Attack
		//Then Potion

		// UI Update
		if (currentHealth < 0) {
			currentHealth = 0;
		}

		if (currentExperience > maxExperience) {
			currentExperience = maxExperience;
			// Level up!
		}

		if (currentExperience < 0) {
			currentExperience = 0;
		}

		GameObject.Find("Health").GetComponent<Image>().fillAmount = currentHealth / maxHealth;
		
		if (currentHealth > maxHealth) {
			// Golden apple effect!
			GameObject.Find("Health").GetComponent<Image>().color = new Color(177f / 255f, 0, 1);
		} else if (currentHealth > 2 * maxHealth / 3) {
			GameObject.Find("Health").GetComponent<Image>().color = new Color(0, 1, 0);
		} else if (currentHealth > maxHealth / 4) {
			GameObject.Find("Health").GetComponent<Image>().color = new Color(1, 1, 0);
		} else {
			GameObject.Find("Health").GetComponent<Image>().color = new Color(1, 0, 0);
		}

		if (currentHealth <= 0) {
			// Game Over
		}

		GameObject.Find("EXP").GetComponent<Image>().fillAmount = currentExperience / maxExperience;

		if (currentExperience >= maxExperience) {
			// Level up!
		}

	}
	void Move() {
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ) {
			//NEED A CONDITION THAT WORKS BETTER HERE
			Assign_LastDirection();
		}
		if (endlag == 0) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0.0f);
			moveDirection *= playerSpeed;
			cc.Move(moveDirection * Time.deltaTime);
			Attack();
		} else {
	        endlag -= 1; //frame countdown
	        if (endlag == 0) {
	            Destroy(sword_inst);
	        }
	    }
	}

/*ABILITIES */

	void Attack(){
		//ISSUE WITH SWORD FACINGS: there is a good 20-30 frame window where switching between A and D / W and S where the input.getaxis function returns 0 instead of +-1.
		//Possile remedies?
		if (Input.GetButtonDown("Fire1")) {
			sword_inst = Instantiate(sword);
			sword_inst.transform.position = new Vector3(player.transform.position.x + 1.25f * face_Front_x, player.transform.position.y + 1.25f * face_Front_y, 0.0f);
			//rotation
			if (face_Front_x == -1) {
				sword_inst.transform.eulerAngles = new Vector3(0, 0, 90 - face_Front_y * 45);
			} else if (face_Front_x == 0) {
				sword_inst.transform.eulerAngles = new Vector3(0, 0, 90 - 90 * face_Front_y);
			} else if (face_Front_x == 1) {
				sword_inst.transform.eulerAngles = new Vector3(0, 0, -90 + face_Front_y * 45);
			}

			endlag += 15;
		}
		if (Input.GetButtonDown("Fire2")) {
			usePotion();
		}
	}

	void usePotion() {
		if (GameObject.FindGameObjectsWithTag("Potion").Length < 2) {
			GameObject newpotion = (GameObject)Instantiate(potionPrefab, this.gameObject.transform.localPosition, Quaternion.identity);
			newpotion.GetComponent<PotionInstance>().thisPotion = selectedPotion;
			StartCoroutine(newpotion.transform.GetComponent<PotionInstance>().DropPotion());
			endlag += 3;
		}
	}

  /*MOVEMENT BASED FUNCTIONS */
	void Assign_LastDirection() {
		face_Front_x = Mathf.Ceil(Input.GetAxis("Horizontal"));
		face_Front_y = Mathf.Ceil(Input.GetAxis("Vertical")); 
	}
}
