using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;

public class TutorialManager : MonoBehaviour {

	public int lastFlag = 0;
	public int lastUsed = -1;
	public GlobalVars gv;

	public int kills = 0;

	public PlayerController pc;

	private void Start() {
		gv = GetComponent<GlobalVars>();
		pc = GameObject.Find("Player").GetComponent<PlayerController>();

		gv.playing = false;

		StartCoroutine(GetComponent<Textbox>().scrollText(0));
	}

	private void Update() {
		if (lastFlag != lastUsed) {
			switch (lastFlag) {
				case (1):
					SpawnEnemy(GetComponent<EnemyManager>().FindByName("Basic_Slime"), GameObject.Find("Player").transform.position + new Vector3(5, 0));
					StartCoroutine(GetComponent<Textbox>().scrollText(1));
					break;
				case (3):
					StartCoroutine(GetComponent<Textbox>().scrollText(2));
					break;
				case (4):
					pc.currentExperience++;
					SpawnEnemy(GetComponent<EnemyManager>().FindByName("Fire_Slime"), GameObject.Find("Player").transform.position + new Vector3(0, 3));
					StartCoroutine(GetComponent<Textbox>().scrollText(3));
					pc.currentExperience = 99;
					break;
				case (6):
				case (7):
					lastFlag = 7;
					StartCoroutine(GetComponent<Textbox>().scrollText(4));
					break;
				case (10):
					StartCoroutine(GetComponent<Textbox>().scrollText(5));
					break;
				case (11):
					StartCoroutine(GetComponent<Textbox>().scrollText(6));
					break;
				case (13):
					StartCoroutine(GetComponent<Textbox>().scrollText(7));
					break;
				case (14):
					Vector2 center = GameObject.Find("Player").transform.position + Vector3.right * 8;
					for (int i = 0; i < 8; i++) {
						SpawnEnemy(GetComponent<EnemyManager>().FindByName("Basic_Slime"), center + Random.insideUnitCircle * 3);
					}
					StartCoroutine(GetComponent<Textbox>().scrollText(8));
					break;
				case (16):
					StartCoroutine(GetComponent<Textbox>().scrollText(9));
					break;
				case (17):
					UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
					break;
			}
		}

		lastUsed = lastFlag;

		if (lastFlag == 2 && pc.currentExperience > 0) {
			lastFlag++;
		}

		if (pc.currentExperience == 100 && pc.level == 1 && GameObject.Find("Textbox(Clone)") == null) {
			pc.Levelup();
		}

		if (kills >= 8 && lastFlag < 16) {
			Debug.Log("Hi!");
			lastFlag++;
			Debug.Log(lastFlag);
		}
	}

	void SpawnEnemy(Enemy e, Vector3 pos) {
		Enemy toSet = new Enemy(e);
		GameObject newEnemy = (GameObject)Instantiate(this.transform.GetComponent<EnemyManager>().EnemyPrefab, pos, Quaternion.identity);
		newEnemy.GetComponent<TrainingDummy>().thisEnemy = toSet;
	}

}