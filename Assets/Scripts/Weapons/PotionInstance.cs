using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class PotionInstance : MonoBehaviour {
	public Potion thisPotion;
	public GameObject hitbox;
	GameObject hb_inst;
	GameObject hb_inst_2;
	public bool isEnemyDrop;
	public bool reaction = false;
	const int _critChance = 20;
	public AudioSource source;
	AudioSource[] sf;
	Animator anim;

	private void Start() {
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Stable_" + thisPotion.name);
		anim = GetComponent<Animator>();
	}

	private void Update() {
		if (!isEnemyDrop) {
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
		}
		//anim.SetInteger("State", anim.GetInteger("State") + 3);
		if (isEnemyDrop) {
			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(thisPotion.name + "Drop");
			StartCoroutine(Timer());
		} else {
			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(thisPotion.name);
		}
	}

	public IEnumerator Timer() {
		for (int i = 0; i < 3000; i++) {
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}

	public IEnumerator DropPotion() {
		sf = GetComponents<AudioSource>(); // SULFUR, NITROGEN, TIMEWARP, EXPLOSION, OXYGEN
		source = sf[0];
		//SFX definitions
		source.volume = 0f;
		reaction = false;
		// Delay the explosion
		if (!isEnemyDrop) {
			yield return new WaitForSeconds(thisPotion.time / 1000f);
			anim.SetInteger("State", 1);
			sf = GetComponents<AudioSource>(); // SULFUR, NITROGEN, TIMEWARP, EXPLOSION, OXYGEN
											   //source = sf[0];
			if (thisPotion.name == "Sulfur") {
				//hitbox
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 7f, thisPotion.size * 7f, 0.0f);
				//SFX
				source = sf[0];
				source.volume = 0.5f;
				source.Play();
				StartCoroutine(Damage_Over_Time(0.0125f, 3.5f, 0.5f, 5));
				yield return new WaitForSeconds(2.5f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Nitrogen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 2, thisPotion.size * 2, 0.0f);
				//SFX
				source = sf[1];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) <= thisPotion.size) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(1, 1);
						if (enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Sulfur") {
							reaction = true;
							StartCoroutine(EnemyExplode(enemy, 1f));
						}
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) <= thisPotion.size) {
						player.GetComponent<PlayerController>().CallSlowDown(1, 1);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Oxygen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 3f, thisPotion.size * 3f, 0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 50f / 100f);
				source = sf[4];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
						enemy.GetComponent<TrainingDummy>().CallKB(0.08f, 0.08f, this.transform);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
						player.GetComponent<PlayerController>().CallKB(0.000008f, 1f, this.transform);
					}
				}
				yield return new WaitForSeconds(0.6f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Greater Sulfur") {//0.0125f, 2, 0.3f, 10
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 2, thisPotion.size * 2, 0.0f);
				source = sf[0];
				source.volume = 0.5f;
				source.Play();
				StartCoroutine(Damage_Over_Time(0.0125f, 1, 0.3f, 10));
				yield return new WaitForSeconds(3f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Greater Nitrogen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 3f, thisPotion.size * 3f, 0.0f);
				source = sf[1];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 1.5f) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(1.5f, 2);
						if (enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Sulfur") {
							reaction = true;
							StartCoroutine(EnemyExplode(enemy, 1f));
						}
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 1.5f) {
						player.GetComponent<PlayerController>().CallSlowDown(1.5f, 2);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Greater Oxygen") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 1.5f, thisPotion.size * 1.5f, 0.0f);
				hb_inst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 50f / 100f);
				source = sf[4];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
						enemy.GetComponent<TrainingDummy>().CallKB(0.1f, 0.09f, this.transform);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
						player.GetComponent<PlayerController>().CallKB(0.1f, 0.09f, this.transform);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Explosion") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 3f, thisPotion.size * 3f, 0.0f);
				source = sf[3];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 3) {
						int roll = Random.Range(0, _critChance);
						bool crit = (roll == 0);
						// Critical hit!
						enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
						enemy.GetComponent<TrainingDummy>().CallKB(0.2f, 0.8f, this.transform);
						StartCoroutine(enemy.GetComponent<TrainingDummy>().IncreaseXP());
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 3) {
						player.GetComponent<PlayerController>().takeDamage(player.GetComponent<PlayerController>().maxHealth / 3);
						player.GetComponent<PlayerController>().CallKB(0.2f, 0.8f, this.transform);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Time Warp") {
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 3f, thisPotion.size * 3f, 0.0f);
				source = sf[2];
				source.volume = 0.5f;
				source.Play();
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
					if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * 10) {
						enemy.GetComponent<TrainingDummy>().CallSlowDown(3, 3);
					}
				}
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * 10) {
						player.GetComponent<PlayerController>().CallSlowDown(2, 1);
					}
				}
				yield return new WaitForSeconds(1f);
				Destroy(hb_inst);
			} else if (thisPotion.name == "Volcano") { //0.0125, 10, 0.125, 10
				hb_inst = Instantiate(hitbox);
				hb_inst.tag = "HB";
				hb_inst.GetComponent<HitboxInstance>().changeSprite(thisPotion.name);
				hb_inst.transform.position = this.transform.position;
				hb_inst.transform.localScale = new Vector3(thisPotion.size * 3f, thisPotion.size * 3f, 0.0f);
				source = sf[0];
				source.volume = 0.5f;
				source.Play();
				StartCoroutine(Damage_Over_Time(0.0375f, 10, 0.125f, 10));
				yield return new WaitForSeconds(1.25f);
				Destroy(hb_inst);

			}

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
		hb_inst_2.transform.localScale = new Vector3(thisPotion.size * 1.5f, thisPotion.size * 1.5f, 0.0f);
		hb_inst_2.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 75f / 100f);
		source = sf[3];
		source.volume = 0.5f;
		source.Play();

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Damageable")) {
			if (Vector3.Distance(enemy.transform.position, e.transform.position) < mult * 3) {
				int roll = Random.Range(0, _critChance);
				bool crit = (roll == 0);
				enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
				StartCoroutine(enemy.GetComponent<TrainingDummy>().IncreaseXP());
			}
		}

		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (Vector3.Distance(player.transform.position, e.transform.position) < mult * 3) {
				int roll = Random.Range(0, _critChance);
				player.GetComponent<PlayerController>().takeDamage(20f);
				player.GetComponent<PlayerController>().CallKB(0.3f, 0.125f / 2f, this.transform);
			}
		}
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
				if (Vector3.Distance(enemy.transform.position, this.transform.position) < thisPotion.size * size_mult) {
					int roll = Random.Range(0, _critChance);
					bool crit = (roll == 0);
					// Critical hit!
					enemy.GetComponent<TrainingDummy>().DropHealth(thisPotion.damage * (crit ? 2 : 1), crit);
					if (enemy.GetComponent<TrainingDummy>().thisEnemy.type == "Nitrogen") {
						reaction = true;
						StartCoroutine(EnemyExplode(enemy, size_mult / 3.5f));
					}
				}
			}
			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
				if (Vector3.Distance(player.transform.position, this.transform.position) < thisPotion.size * size_mult) {
					player.GetComponent<PlayerController>().currentHealth -= damage_mult * player.GetComponent<PlayerController>().maxHealth;
				}
			}
			yield return new WaitForSeconds(time_between);
		}
	}
}