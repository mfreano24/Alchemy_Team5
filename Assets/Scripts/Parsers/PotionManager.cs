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
		string path = Application.streamingAssetsPath + "\\potion_data.csv";

		//if (Application.platform == RuntimePlatform.Android) {
		WWW dat = new WWW(path);
		while (!dat.isDone) { }
		string[] info = dat.text.Split('\n');

		for (int i = 1; i < info.Length - 1; i++) {
			string[] data = info[i].Split(',');
			Potion newPotion = new Potion();
			newPotion.name = data[0];
			newPotion.type = System.Convert.ToByte(data[1]);
			newPotion.ingredients.AddRange(data[2].Split('/'));
			newPotion.requiredScale = (float)System.Convert.ToDouble(data[3]);
			newPotion.explosionSize = (float)System.Convert.ToDouble(data[4]);
			newPotion.time = System.Convert.ToInt32(data[5]);
			newPotion.rangeExtension = (float)System.Convert.ToDouble(data[6]);
			newPotion.audioSource = System.Convert.ToByte(data[7]);
			newPotion.playerDamage = (float)System.Convert.ToDouble(data[8]);
			newPotion.enemyDamage = System.Convert.ToInt32(data[9]);
			newPotion.buffer = (float)System.Convert.ToDouble(data[10]);
			newPotion.hits = System.Convert.ToByte(data[11]);
			newPotion.slowDuration = (float)System.Convert.ToDouble(data[12]);
			newPotion.slowStrength = (float)System.Convert.ToDouble(data[13]);
			newPotion.kbDuration = (float)System.Convert.ToDouble(data[14]);
			newPotion.kbPower = (float)System.Convert.ToDouble(data[15]);
			potions.Add(newPotion);
		}

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
			if (p.ingredients[0] == "None") {
				newList.Add(p);
			} else {
				foreach (string s in p.ingredients) {
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
