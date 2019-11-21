using System.Collections;
using UnityEngine;
using Alchemy;

public class TrainingDummy : MonoBehaviour {

	public Enemy thisEnemy;

	GlobalVars gv;

	public GameObject sword;
	BoxCollider2D hurtbox;
	Rigidbody2D rb;

	public Vector2 translatePos;
	public float turnDist = 5;
	public float turnSpeed = 3;

	Path path;

	const float minPathUpdateTime = 0.1f;
	const float pathUpdateThreshold = 0.1f;
	/*public Animation fire_slime;
	public Animation basic_slime;
	public Animation nitroshroom;*/
	int targetIndex;
	Animator anim;
	void Start() {
		hurtbox = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(UpdatePath());
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
		anim = GetComponent<Animator>();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(thisEnemy.name + "_Walk");
	}

	public void OnPathFound(Vector3[] waypoints, bool success) {
		if (success) {
			path = new Path(waypoints, transform.position, turnDist);
			StopCoroutine(Follow());
			StartCoroutine(Follow());
		}
	}

	IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < 0.3f) {
			yield return new WaitForSeconds(0.3f);
		}
		PathRequestManager.RequestPath(new PathRequest(transform.position, GameObject.Find("Player").transform.position, OnPathFound));

		float sqrMoveThreshold = pathUpdateThreshold * pathUpdateThreshold;
		Vector3 targetPosOld = GameObject.Find("Player").transform.position;

		while (true) {
			yield return new WaitForSeconds(minPathUpdateTime);
			if ((GameObject.Find("Player").transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath(new PathRequest(transform.position, GameObject.Find("Player").transform.position, OnPathFound));
				targetPosOld = GameObject.Find("Player").transform.position;
			}
		}
	}

	IEnumerator Follow() {
		bool followingPath = true;
		int pathIndex = 0;

		while (followingPath) {
			Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
			while (path.turnBoundaries[pathIndex].CrossedLine(pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath && gv.playing) {
				translatePos = path.lookPoints[pathIndex] - transform.position;
				transform.Translate(translatePos.normalized * Time.deltaTime * thisEnemy.speed, Space.World);
			}

			yield return null;

		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Player") && gv.playing) {
			collision.GetComponent<PlayerController>().takeDamage(thisEnemy.baseATK);
			collision.GetComponent<PlayerController>().Knockback(0.2f, 0.5f, this.transform);
		}
	}

	public void DropHealth(int i, bool crit) {
		if (thisEnemy.baseHP > 0) {
			GameObject damage = (GameObject)Instantiate(Resources.Load("Damage") as Object, transform.position, Quaternion.identity);
			// <If possible> damage.transform.GetChild(0).transform.localScale = new Vector3(0.8f / transform.localScale.x, 0.46f / transform.localScale.y, 1); 
			damage.transform.SetParent(transform);
			thisEnemy.baseHP -= i;

			if (thisEnemy.baseHP <= 0) {
				GameObject.Find("EventSystem").GetComponent<WaveManager>().currentEnemies.Remove(this.gameObject);
				StartCoroutine(IncreaseXP());
				// Drop the item
				if (thisEnemy.type != "None") {

					int probability = Random.Range(1, 10);
					int items = 1;

					if (probability <= 5) {
						// 50% chance of 1 item
						items = 1;
					} else if (probability <= 9) {
						// 40% chance of 2 items
						items = 2;
					} else {
						// 10% chance of 3 items
						items = 3;
						
					}

					if (items == 1) {
						GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, transform.position, Quaternion.identity);
						enemyDrop.GetComponent<PotionInstance>().thisPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().FindByName(thisEnemy.type);
						enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
						StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());
					} else if (items == 2) {
						for (int j = 0; j < 2; j++) {
							GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, transform.position + Mathf.Pow(-1, j) * Vector3.right * 0.3f, Quaternion.identity);
							enemyDrop.GetComponent<PotionInstance>().thisPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().FindByName(thisEnemy.type);
							enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
							StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());
						}
					} else {
						for (int j = 0; j < 3; j++) {
							float pi = Mathf.PI;
							GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, transform.position + 0.6f * new Vector3(Mathf.Cos(pi / 2 + 2 * pi * j / 3f), Mathf.Sin(pi / 2 + 2 * pi * j / 3f), 0), Quaternion.identity);
							enemyDrop.GetComponent<PotionInstance>().thisPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().FindByName(thisEnemy.type);
							enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
							StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());
						}
					}

					if (Random.Range(1, 10) == 1) {
						GameObject enemyDrop = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<PlayerController>().potionPrefab, transform.position + 0.6f * new Vector3(0, 0, -0.3f), Quaternion.identity);
						enemyDrop.GetComponent<PotionInstance>().thisPotion = GameObject.Find("EventSystem").GetComponent<PotionManager>().FindByName("Oxygen");
						enemyDrop.GetComponent<PotionInstance>().isEnemyDrop = true;
						StartCoroutine(enemyDrop.GetComponent<PotionInstance>().DropPotion());
					}
				}
			}

			damage.GetComponent<DamageIndicator>().setHealth(i, crit);
		}

	}

	public IEnumerator IncreaseXP() {
		for (int i = 0; i < thisEnemy.exp; i++) {
			// Animates the increase (because reasons
			GameObject.Find("Player").GetComponent<PlayerController>().currentExperience++;
			yield return new WaitForSeconds(0.01f);
		}
	}


	public void CallKB(float duration, float pow, Transform other) {
		StartCoroutine(Knockback(duration, pow, other));
	}

	public IEnumerator Knockback(float duration, float pow, Transform other) {
		float time = 0;
		while (duration > time) {
			time += Time.deltaTime;
			Vector2 direction = (other.transform.position - transform.position).normalized;
			rb.AddForce(-direction * pow);
		}
		yield return 0;
	}


	public void CallSlowDown(float duration, int strength) {
		StartCoroutine(SlowdownDebuff(duration, strength));
	}
	public IEnumerator SlowdownDebuff(float duration, int strength) {
		float tempSpeed = thisEnemy.speed;
		thisEnemy.speed -= strength;
		yield return new WaitForSeconds(duration);
		thisEnemy.speed = tempSpeed;
		yield return 0;
	}

	//public void OnDrawGizmos() {
	//	if (path != null) {
	//		path.DrawWithGizmos();
	//	}
	//}

}


