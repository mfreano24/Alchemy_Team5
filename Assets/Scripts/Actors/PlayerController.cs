using System.Collections;
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
	float temp_x;
	float temp_y;
	KeyCode last_Pressed;
	//for reference purposes
	public GameObject player;
	public GameObject sword;
	//for instance purposes
	private GameObject sword_inst;
	public InventorySlot selectedPotion;
	public GameObject potionPrefab;

	public float maxHealth;
	public float currentHealth;
	public float maxExperience;
	public float currentExperience;
	int level = 1;

	public List<InventorySlot> inventory;
	int BASE_COUNT = 2;

	void Start() {

	    cc = GetComponent<CharacterController>();
		sw = sword.GetComponent<BoxCollider2D>();
	    face_Front_x = 0;
	    face_Front_y = -1;

		// DEBUGGING PURPOSES ONLY
		inventory = new List<InventorySlot>();
		inventory.Add(new InventorySlot(GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[0], 5));
		inventory.Add(new InventorySlot(GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[1], 5));
		selectedPotion = inventory[0];
	}

	void Update () {
	    //Move First
	    Move();
		//Then Basic Attack
		//Then Potion

		// UI Update
		UpdateUI();

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

	void Attack() {
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
		if (GameObject.FindGameObjectsWithTag("Potion").Length < 2 && selectedPotion.count > 0) {
			GameObject newpotion = (GameObject)Instantiate(potionPrefab, this.gameObject.transform.localPosition, Quaternion.identity);
			newpotion.GetComponent<PotionInstance>().thisPotion = selectedPotion.item;
			selectedPotion.count--;
			StartCoroutine(newpotion.transform.GetComponent<PotionInstance>().DropPotion());
			endlag += 3;
		}

		if (selectedPotion.count == 0 && inventory.IndexOf(selectedPotion) > BASE_COUNT - 1) {
			int index = inventory.IndexOf(selectedPotion) - 1;
			inventory.Remove(selectedPotion);
			selectedPotion = inventory[index];
			GameObject.Find("CurrentPotion").GetComponent<PotionDisplay>().PotionUsed();
		}
	}

	void UpdateUI() {
		if (currentHealth < 0) {
			currentHealth = 0;
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
			Levelup();
		}
	}

	void Levelup() {
		level++;
		currentExperience -= maxExperience;
		maxExperience = LevelToExp(level);
	}

	int LevelToExp(int x) {
		return 100 * x;
	}

  /*MOVEMENT BASED FUNCTIONS */
	void Assign_LastDirection() {
		temp_x = face_Front_x;
		temp_y = face_Front_y;
		face_Front_x = Mathf.Ceil(Input.GetAxis("Horizontal"));
		face_Front_y = Mathf.Ceil(Input.GetAxis("Vertical")); 
		if(face_Front_x == 0 && face_Front_y == 0){
			//frames between directional switch
			face_Front_x = temp_x;
			face_Front_y = temp_y;
		}
	}
}
