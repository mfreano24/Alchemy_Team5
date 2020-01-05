using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy;
using System.IO;

[System.Serializable]
public class PotionManager : MonoBehaviour {

	[SerializeField]
	public List<Potion> potions = new List<Potion>();

	// Initializes all potions from file
	private void Awake() {

		// List of elements
		string path = Application.streamingAssetsPath + "\\potions.txt";

		//if (Application.platform == RuntimePlatform.Android) {
		WWW dat = new WWW(path);
		while (!dat.isDone) { }
		string info = dat.text;
		//}

		for (int j = 0; j < info.Split('\n').Length; j += 8) {

			Potion newPotion = new Potion();
			for (int i = 0; i < 8; i++) {

				// Read actual input
				string input = info.Split('\n')[j + i].TrimEnd('\r');

				if (input == "" || input == null) {
					break;
				}

				switch (i) {
					case (1):
						// Setting the element name
						newPotion.name = input;
						break;
					case (2):
						// Create the combination
						newPotion.combination = new List<string>();
						string[] combo = input.Split(new string[] { ", " }, System.StringSplitOptions.None);
						foreach (string k in combo) {
							newPotion.combination.Add(k);
						}
						break;
					case (3):
						// Set up the sprite used
						newPotion.sprite = (Sprite)Resources.Load(input);
						break;
					case (4):
						// Set up the maximum size of the explosion
						newPotion.size = System.Convert.ToInt32(input);
						break;
					case (5):
						// Set up the time it lasts
						newPotion.time = System.Convert.ToInt32(input);
						break;
					case (6):
						// Set up the damage it'll do
						newPotion.damage = System.Convert.ToInt32(input);
						break;
					case (7):
						// Set up the Effect
						newPotion.effect = input;
						break;
					default:
						// Just for the -----
						break;
				}
			} // End creation

			potions.Add(newPotion);

		} // End of File IO

		// Removes an empty potion
		potions.RemoveAt(potions.Count - 1);

	}// End of Start

	public Potion FindByName(string s) {
		foreach (Potion p in potions) {
			if (p.name == s) {
				return p;
			}
		}

		return null;

	}

	public List<Potion> BaseList(List<Potion> pList) {
		List<Potion> newList = new List<Potion>();
		bool simplified = true;
		foreach (Potion p in pList) {
			if (p.combination[0] == "None") {
				newList.Add(p);
			} else {
				foreach (string s in p.combination) {
					simplified = false;
					newList.Add(FindByName(s));
				}
			}
		}
		if (!simplified) {
			return BaseList(newList);
		}

		return newList;
	}
}
