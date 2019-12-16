using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alchemy;

public class PlayerController : MonoBehaviour {
	Vector2 moveDirection;
	Rigidbody2D rb;
	BoxCollider2D sw;
	GlobalVars gv;
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
	public GameObject menu;
	public GameObject gameOver;
	public GameObject terminalObject;
	public bool invincibility;

	public int invincibilityFrames = 0;

	public float currentHealth = 100;
	public float maxExperience;
	public float currentExperience;
	int level = 1;

	public List<InventorySlot> inventory;
	int BASE_COUNT = 3;

	public Animator anim;

	// UPGRADEABLE DATA
	public int MAX_ITEMS; // Maximum number of each element carried
	public float maxHealth; // Maximum player health
	public int HEAL_FACTOR; // Amount player heals between waves
	public int playerSpeed; // Current speed of the player

	AudioSource[] asc;
	AudioSource curr;

	void Start() {
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
		rb = GetComponent<Rigidbody2D>();
		sw = sword.GetComponent<BoxCollider2D>();
		terminalObject = GameObject.Find("Terminal");
		menu = GameObject.Find("PauseScreen");
		gameOver = GameObject.Find("GameOver");
		terminalObject.SetActive(false);
		gameOver.SetActive(false);
		menu.SetActive(false);
		face_Front_x = 0;
		face_Front_y = -1;

		anim = this.gameObject.GetComponent<Animator>();
		asc = GetComponents<AudioSource>();

		// DEBUGGING PURPOSES ONLY
		inventory = new List<InventorySlot>();
		inventory.Add(new InventorySlot(GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[0], 5));
		inventory.Add(new InventorySlot(GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[2], 1));
		inventory.Add(new InventorySlot(GameObject.Find("EventSystem").GetComponent<PotionManager>().potions[4], 0));
		selectedPotion = inventory[0];
		invincibility = false;

		GameObject.Find("CustomLevelManager").GetComponent<StageManager>().Create();
	}

	void Update() {
		if (gv.playing) {
			UpdateUI();
			PickupItems();
			CheckWall();
			anim.SetFloat("Speed", moveDirection.magnitude);
			anim.SetInteger("Direction", MapDirection(face_Front_x, face_Front_y));
			if (invincibility) {
				invincibilityFrames++;
			}

			if (invincibilityFrames > 60) {
				GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
				invincibility = false;
				invincibilityFrames = 0;
			}

			if (Input.GetButtonDown("Pause") && gv.playing) {
				gv.playing = false;
				menu.SetActive(true);
			}

			if (Input.GetButtonDown("Pause") && !gv.playing) {
				gv.playing = true;
				menu.SetActive(false);
				terminalObject.SetActive(false);
			}

			if (Input.GetButtonDown("Terminal")) {
				gv.playing = false;
				terminalObject.SetActive(true);
			}
		}
	}

	void CheckWall() {
		int maxX = gv.worldSizeX;
		int maxY = gv.worldSizeY;

		if (transform.position.x > gv.worldCenterX + maxX + 3) {
			this.transform.position = new Vector2(gv.worldCenterX + maxX, transform.position.y);
		} else if (transform.position.x < gv.worldCenterX - maxX - 3) {
			this.transform.position = new Vector2(gv.worldCenterX - maxX, transform.position.y);
		}

		if (transform.position.y > gv.worldCenterY + maxY + 3) {
			transform.position = new Vector2(transform.position.x, gv.worldCenterY + maxY);
		} else if (transform.position.y < gv.worldCenterY - maxY - 3) {
			transform.position = new Vector2(transform.position.x, gv.worldCenterY - maxY);
		}
	}

	void PickupItems() {
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Potion")) {
			if (item.GetComponent<PotionInstance>().isEnemyDrop && Vector3.Distance(this.transform.position, item.transform.position) < 2) {
				int invIndex = FindInventorySlot(item.GetComponent<PotionInstance>().thisPotion);
				if (invIndex == -1) {
					inventory.Add(new InventorySlot(item.GetComponent<PotionInstance>().thisPotion, 1));
					return;
				}
				if (inventory[invIndex].count < MAX_ITEMS) {
					inventory[invIndex].count++;
					Destroy(item);
					return;
				}
			}
		}
	}

	public int FindInventorySlot(Potion p) {
		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].item == p) {
				return i;
			}
		}
		return -1;
	}

	void FixedUpdate() {
		if (gv.playing) {
			if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
				//NEED A CONDITION THAT WORKS BETTER HERE
				Assign_LastDirection();
			}

			if (endlag == 0) {
				Move();
				Attack();
			} else {
				endlag -= 1; //frame countdown
			}
		} else {
			anim.SetFloat("Speed", 0);
		}
	}

	void Move() {
		moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		rb.MovePosition(rb.position + playerSpeed * moveDirection * Time.deltaTime);
	}

	/*ABILITIES */

	void Attack() {
		//ISSUE WITH SWORD FACINGS: there is a good 20-30 frame window where switching between A and D / W and S where the input.getaxis function returns 0 instead of +-1.
		//Possile remedies?
		if (Input.GetButtonDown("Fire2")) {
			usePotion();
		}
	}

	void usePotion() {

		int droppedCount = 0;

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Potion")) {
			if (!go.GetComponent<PotionInstance>().isEnemyDrop) {
				droppedCount++;
			}
		}
		if (droppedCount < 2 && selectedPotion.count > 0) {
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
			gv.playing = false;
			anim.SetFloat("Speed", 0);
			gameOver.SetActive(true);
			curr = asc[1];
			curr.volume = 0.5f;
			curr.Play();

		}

		if (currentExperience < 0) {
			currentExperience = 0;
		}

		GameObject.Find("Health").GetComponent<Image>().fillAmount = currentHealth / maxHealth;
		GameObject.Find("BonusHealth").GetComponent<Image>().fillAmount = (currentHealth - maxHealth) / maxHealth;

		if (currentHealth > 2 * maxHealth / 3) {
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

	public void takeDamage(float d) {
		if (!invincibility) {
			currentHealth -= d;
			curr = asc[0];
			curr.volume = 0.5f;
			curr.Play();
			StartCoroutine(IFrames());
		} else if (d < 0) {
			currentHealth -= d;
			curr = asc[0];
			curr.volume = 0.5f;
		}
	}

	public IEnumerator IFrames() {
		invincibility = true;
		EnemyColliders(false);
		Color temp = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.65f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = temp;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.65f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = temp;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.65f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = temp;
		EnemyColliders(true);
		invincibility = false;
	}

	void EnemyColliders(bool val) {
		foreach (GameObject g in GameObject.Find("EventSystem").GetComponent<WaveManager>().currentEnemies) {
			g.GetComponent<BoxCollider2D>().enabled = val;
		}
	}

	public void CallKB(float duration, float pow, Transform other) {
		StartCoroutine(Knockback(duration, pow, other));
	}

	public IEnumerator Knockback(float duration, float pow, Transform other) {
		float time = 0;
		while (duration > time) {
			time += Time.deltaTime;
			Vector2 direction = (other.transform.position - this.transform.position).normalized;
			rb.AddForce(-direction * pow);
		}
		yield return 0;
	}

	public void CallSlowDown(float duration, int strength) {
		StartCoroutine(SlowdownDebuff(duration, strength));
	}

	public IEnumerator SlowdownDebuff(float duration, int strength) {
		int tempSpeed = playerSpeed;
		playerSpeed -= strength;
		yield return new WaitForSeconds(1f);
		playerSpeed = tempSpeed;
		yield return 0;
	}

	void Levelup() {
		level++;
		currentExperience -= maxExperience;
		maxExperience = LevelToExp(level);
		StartCoroutine(GameObject.Find("Upgrades").GetComponent<UpgradeSelection>().OnUpgradeBegin());
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
		if (face_Front_x == 0 && face_Front_y == 0) {
			//frames between directional switch
			face_Front_x = temp_x;
			face_Front_y = temp_y;
		}
	}

	int MapDirection(float x, float y) {
		return (int)(3 * x + 2 * y);
	}
}
