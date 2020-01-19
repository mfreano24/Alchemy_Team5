using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class TutorialManager : MonoBehaviour {

	public int lastFlag = 0;
	public int lastUsed = -1;
	public GlobalVars gv;

	public PlayerController pc;

	private void Start() {
		gv = GetComponent<GlobalVars>();
		pc = GameObject.Find("Player").GetComponent<PlayerController>();

		gv.playing = false;

		StartCoroutine(GetComponent<Textbox>().scrollText(0));
	}

	private void Update() {
		//pc.currentExperience
		if (pc.level == 1) {

			switch (pc.currentExperience) {
				case (5):
					pc.currentExperience++;
					SpawnEnemy(GetComponent<EnemyManager>().FindByName("Basic_Slime"), GameObject.Find("Player").transform.position + new Vector3(5, 0));
					break;
				case (6):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(1));
					break;
				case (22):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(2));
					break;
				case (35):
					pc.currentExperience++;
					break;
				case (36):
					pc.currentExperience++;
					SpawnEnemy(GetComponent<EnemyManager>().FindByName("Fire_Slime"), GameObject.Find("Player").transform.position + new Vector3(0, 3));
					StartCoroutine(AnimateEnemy());
					break;
				case (54):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(3));
					break;
				case (100):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(4));
					break;
				case (106):
					pc.currentExperience++;
					pc.Levelup();
					break;
			}
		}
		
		if (pc.level == 2) {
			switch (pc.currentExperience) {
				case (8):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(5));
					break;
				case (15):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(6));
					break;
				case (22):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(7));
					break;
				case (28):
					pc.currentExperience++;
					Vector2 center = GameObject.Find("Player").transform.position + Vector3.right * 8;
					for (int i = 0; i < 8; i++) {
						SpawnEnemy(GetComponent<EnemyManager>().FindByName("Basic_Slime"), center + Random.insideUnitCircle * 3);
					}
					break;
				case (29):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(8));
					break;
				case (235):
					pc.Levelup();
					break;
			}
		}

		if (pc.level == 3) {
			switch (pc.currentExperience) {
				case (35):
					pc.currentExperience++;
					StartCoroutine(GetComponent<Textbox>().scrollText(9));
					break;
				case (41):
					UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
					break;
			}
		}

	}

	void SpawnEnemy(Enemy e, Vector3 pos) {
		GameObject newEnemy = (GameObject)Instantiate(this.transform.GetComponent<EnemyManager>().EnemyPrefab, pos, Quaternion.identity);
		newEnemy.GetComponent<TrainingDummy>().thisEnemy = e;
	}

	IEnumerator AnimateEnemy() {
		while (Vector3.Distance(GameObject.Find("Debug Dummy(Clone)").transform.position, GameObject.Find("Player").transform.position) >= 0.3f) {
			yield return new WaitForSeconds(0.01f);
			GameObject.Find("Debug Dummy(Clone)").GetComponent<Rigidbody2D>().MovePosition(Vector3.down * 0.1f);
		}

		for (int i = 0; i < 50; i++) {
			GameObject.Find("Debug Dummy(Clone)").transform.position = GameObject.Find("Player").transform.position + new Vector3(i / 50f, -0.03f * i * ((i - 50) / 10f));
			yield return new WaitForSeconds(0.01f);
		}

		pc.currentExperience = 54;
	}

}