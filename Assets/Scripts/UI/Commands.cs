using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commands : MonoBehaviour {

	Text inputText;
	GameObject es;
	GameObject inputObject;

	private void Awake() {
		inputText = GameObject.Find("TerminalInput").GetComponent<InputField>().textComponent;
		inputObject = GameObject.Find("Terminal");
		es = GameObject.Find("EventSystem");
	}

	public void EnterCommand() {
		string input = inputText.text;

		string[] chain = input.Split(' ');

		if (chain[0] == "/level") {
			LevelCommand(chain);
		}


		inputText.text = "";
		inputObject.SetActive(false);
		es.GetComponent<GlobalVars>().playing = true;
	}

	void LevelCommand(string[] items) {
		if (items[1] == "set") {
			es.GetComponent<EnemyManager>().wave = System.Convert.ToInt32(items[2]);
			Debug.Log("Level set to " + items[2]);
		} else if (items[1] == "add") {
			es.GetComponent<EnemyManager>().wave += System.Convert.ToInt32(items[2]);
			Debug.Log("Level increased by " + items[2]);
		} else {
			// Invalid
			throw new System.Exception("Invalid command entered!");
		}
	}

}
