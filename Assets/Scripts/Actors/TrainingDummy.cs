using System.Collections;
using UnityEngine;
using Alchemy;

public class TrainingDummy : MonoBehaviour {

	public Enemy thisEnemy;
	

	public GameObject sword;
	BoxCollider2D hurtbox;

	public float turnDist = 5;
	public float turnSpeed = 3;

	Path path;

	const float minPathUpdateTime = 0.2f;
	const float pathUpdateThreshold = 0.5f;

	int targetIndex;

	void Start() {
		hurtbox = GetComponent<BoxCollider2D>();
		StartCoroutine(UpdatePath());
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

			Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
			while (path.turnBoundaries[pathIndex].CrossedLine(pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {
				// Probably have to rewrite this
				Vector2 translatePos = path.lookPoints[pathIndex] - transform.position;
				transform.Translate(translatePos.normalized * Time.deltaTime * thisEnemy.speed, Space.World);
			}

			yield return null;
		}
	}

	public void DropHealth(int i, bool crit) {
		if (thisEnemy.baseHP > 0) {
			GameObject damage = (GameObject)Instantiate(Resources.Load("Damage") as Object, this.transform);
			thisEnemy.baseHP -= i;

			if (thisEnemy.baseHP <= 0) {
				StartCoroutine(IncreaseXP());
			}

			damage.GetComponent<DamageIndicator>().setHealth(i, crit);
		}

	}

	IEnumerator IncreaseXP() {
		for (int i = 0; i < thisEnemy.exp; i++) {
			// Animates the increase (because reasons
			GameObject.Find("Player").GetComponent<PlayerController>().currentExperience++;
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos();
		}
	}

}
