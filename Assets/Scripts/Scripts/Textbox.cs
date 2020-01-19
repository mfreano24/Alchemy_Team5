using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {

	public GameObject textBox;

	int currentIndex;
	int currentSubString;

	// Text to be said by player
	string[] text = new string[] {
		"Another dungeon to explore! What's in store for me today?",
		"Well, what do we have here? It's a slime! If I can get close enough with the WASD Keys, I should be able to use a potion with a Right Click to defeat it! I just have to make sure to move out of the way, or my own potions will hurt me!",
		"That was too easy!",
		"Oww! That hurt! ... Hey, isn't that a fire slime? If I recall correctly, a fire slime will explode immediately upon impact with a nitrogen potion. If I scroll over with Q or E, I should be able to make quick work of it.",
		"Hah, that was tiring! But at least I leveled up! I can choose one of four upgrades! Either increase my maximum health, increase how much I get after each wave, increase how many of a single potion I can carry, or increase my movement speed.",
		"Good choice! Oh hey, that slime dropped some elements. They can help me craft stronger potions!",
		"If I scroll to the potions, I can add them to a crafting station with Left Shift, and complete the craft with Space! I should create a Volcano Potion using one Sulfur and one Oxygen.",
		"Sweet! That worked like a charm!",
		"That's a lot of enemies! I should be able to use the Volcano potion to defeat them all.",
		"Finally! That went well, I think I'm ready to take on the rest of the dungeon!"
	};

	private void Update() {
		if (Input.GetButtonDown("Fire2") && currentSubString == text[currentIndex].Length && GameObject.Find("Textbox(Clone)")) {
			// Destroy the textbox
			Destroy(GameObject.Find("Textbox(Clone)"));
			GetComponent<TutorialManager>().pc.currentExperience += 5;
			GetComponent<TutorialManager>().gv.playing = true;
			currentSubString = 0;
		}

		if (Input.GetButtonDown("Fire2") && currentSubString != text[currentIndex].Length) {
			// Show the whole textbox
			currentSubString = text[currentIndex].Length;
			GameObject.Find("Textbox(Clone)").transform.GetComponentInChildren<Text>().text = text[currentIndex];
		}
	}

	public IEnumerator scrollText(int index) {
		// Instantiate the textbox
		GameObject box = (GameObject)Instantiate(textBox, GameObject.Find("UI").transform);
		box.transform.localPosition = new Vector3(0, -247.5f, 0); // TO-DO: Make this show up in the right place

		// Set "global" variable
		currentIndex = index;

		for (int i = 0; i <= text[index].Length; i++) {

			GetComponent<TutorialManager>().gv.playing = false;
			// Scroll through text
			if (currentSubString == text[index].Length) {
				break;
			}
			currentSubString = i;
			box.transform.GetChild(0).GetComponent<Text>().text = text[index].Substring(0, currentSubString);
			yield return new WaitForSeconds(0.01f);
		}

	}

}
