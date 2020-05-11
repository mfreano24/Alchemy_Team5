using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {

	// Attributes
	public Potion thisPotion;

	// Hitbox objects
	public GameObject hitbox;
	GameObject hb_inst;
	GameObject hb_inst_2;

	// More attributes
	public bool isEnemyDrop;
	public bool reaction = false;
	const int _critChance = 20;

	// Audio and Animation
	public AudioSource source;
	AudioSource[] sf;
	Animator anim;

	// Initilalize the start values
	private void Start() {
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Stable_" + thisPotion.name);
		anim = GetComponent<Animator>();
	}

	private void Update() {
		// The item is not an enemy drop
		if (!isEnemyDrop) {
			// Set the scale of the potion's image
			if (anim.GetInteger("State") == 0 && (thisPotion.name == "Sulfur" || thisPotion.name == "Greater Sulfur" || thisPotion.name == "Volcano")) {
				transform.localScale = Vector3.one;
			} else if (anim.GetInteger("State") == 0 && (thisPotion.name == "Nitrogen" || thisPotion.name == "Greater Nitrogen" ||
			  thisPotion.name == "Time Warp")) {
				transform.localScale = Vector3.one * 3;
			} else if (anim.GetInteger("State") == 0 && (thisPotion.name == "Oxygen" || thisPotion.name == "Greater Oxygen")) {
				transform.localScale = Vector3.one * 3;
			} else if (anim.GetInteger("State") == 0 && (thisPotion.name == "Explosion")) {
				transform.localScale = Vector3.one * 3;
			}

			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(thisPotion.name);
		} else {
			// It is an enemy drop, set up the timer
			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(thisPotion.name + "Drop");
			StartCoroutine(Timer());
		}
	}

	//Destroys the dropped item after 3000 frames.
	public IEnumerator Timer() {
		for (int i = 0; i < 3000; i++) {
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}

	public void Slowdown() {
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if (Vector3.Distance(enemy.transform.position, this.transform.position) <= thisPotion.explosionSize) {
				enemy.GetComponent<TrainingDummy>().CallSlowDown(thisPotion.slowDuration, thisPotion.slowStrength);
				if (enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Sulfur") {
					reaction = true;
					if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial") {
						GameObject.Find("EventSystem").GetComponent<TutorialManager>().lastFlag++;
						Debug.Log("Hello");
					}
					StartCoroutine(EnemyExplode(enemy, 1f));
				}
			}
		}
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (Vector3.Distance(player.transform.position, this.transform.position) <= thisPotion.explosionSize) {
				player.GetComponent<PlayerController>().CallSlowDown(1, 1);
			}
		}
	}

	public void Knockback() {
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.explosionSize * 3) {
				enemy.GetComponent<TrainingDummy>().CallKB(thisPotion.kbDuration, thisPotion.kbPower, this.transform);
			}
		}
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.explosionSize * 3) {
				player.GetComponent<PlayerController>().CallKB(thisPotion.kbDuration, thisPotion.kbPower, this.transform);
			}
		}
	}

	public IEnumerator DropPotion() {
		// Set up inital components of the dropped potion
		sf = GetComponents<AudioSource>(); // SULFUR, NITROGEN, TIMEWARP, EXPLOSION, OXYGEN
		source = sf[0];
		//SFX definitions
		source.volume = 0f;
		reaction = false;

		if (!isEnemyDrop) {
			// This should explode
			yield return new WaitForSeconds(thisPotion.time / 1000f);
			anim.SetInteger("State", 1);
			sf = GetComponents<AudioSource>(); // SULFUR, NITROGEN, TIMEWARP, EXPLOSION, OXYGEN
											   //source = sf[0];

			hb_inst = Instantiate(hitbox);
			hb_inst.tag = "HB";
			hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
			hb_inst.transform.position = this.transform.position;
			hb_inst.transform.localScale = new Vector3(thisPotion.toScale, thisPotion.toScale, 0.0f);
			hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 50f / 100f);
			source = sf[thisPotion.audioSource];
			source.volume = 0.5f;
			source.Play();

			if ((thisPotion.type & 0x1) == 0x1) {   // (0000 0101) & (0000 0001) == (0000 0001)
				StartCoroutine(Damage_Over_Time(thisPotion.playerDamage, thisPotion.rangeExtension, thisPotion.buffer, thisPotion.hits));
			}

			if ((thisPotion.type & 0x2) == 0x2) {   // (0011 0010) & (0000 0010) == (0000 0010)
				Slowdown();
			}

			if ((thisPotion.type & 0x4) == 0x4) {	// (0110 1100) & (0000 0100) == (0000 0100)
				Knockback();
			}

			yield return new WaitForSeconds(2);
			Destroy(hb_inst);

			// Delete the game object
			Destroy(this.gameObject);
		} else {
			// it's an enemy drop
		}

	}

	public IEnumerator EnemyExplode(GameObject e, float mult) { //e for enemy
		hb_inst_2 = Instantiate(hitbox);
		hb_inst_2.tag = "HB";
		hb_inst_2.GetComponent<HitboxInstance>().changeSprite("Explosion");
		hb_inst_2.transform.position = this.transform.position;
		hb_inst_2.transform.localScale = new Vector3(thisPotion.explosionSize * 1.5f, thisPotion.explosionSize * 1.5f, 0.0f);
		hb_inst_2.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 75f / 100f);
		source = sf[3];
		source.volume = 0.5f;
		source.Play();

		GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, e.transform.position, Quaternion.identity);
		enemyDrop.GetComponent<PotionInstance>().thisPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().FindByName(e.GetComponent<TrainingDummy>().thisEnemy.type);
		enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
		StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if (Vector3.Distance(enemy.transform.position, e.transform.position) < mult * 3 && enemy != e) {
				int roll = Random.Range(0, _critChance);
				bool crit = (roll == 0);
				enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.enemyDamage * (crit ? 2 : 1), crit);
			}
		}

		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (Vector3.Distance(player.transform.position, e.transform.position) < mult * 3) {
				int roll = Random.Range(0, _critChance);
				player.GetComponent<PlayerController>().takeDamage(20f);
				player.GetComponent<PlayerController>().CallKB(0.3f, 0.125f / 2f, this.transform);
			}
		}

		StartCoroutine(e.GetComponent<TrainingDummy>().IncreaseXP());

		Destroy(e);

		yield return new WaitForSeconds(1f);
		foreach (GameObject hitbox in GameObject.FindGameObjectsWithTag("HB")) {
			Destroy(hitbox);
		}
	}



	//OPTIMIZING
	public IEnumerator Damage_Over_Time(float damage_mult, float size_mult, float time_between, int reps) {
		for (int i = 0; i < reps; i++) {
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
				if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.explosionSize * size_mult) {
					int roll = Random.Range(0, _critChance);
					bool crit = (roll == 0);
					// Critical hit!
					enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.enemyDamage * (crit ? 2 : 1), crit);
					if (enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Nitrogen") {
						reaction = true;
						StartCoroutine(EnemyExplode(enemy, size_mult / 3.5f));
					}
				}
			}
			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
				if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.explosionSize * size_mult) {
					player.GetComponent<PlayerController>().currentHealth -= damage_mult * player.GetComponent<PlayerController>().maxHealth;
				}
			}
			yield return new WaitForSeconds(time_between);
		}
	}
}