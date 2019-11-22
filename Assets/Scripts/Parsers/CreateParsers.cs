using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateParsers : MonoBehaviour {

	string[] potionDoc = new string[] {"-----",
		"Sulfur",
		"None",
		"Explosion",
		"1",
		"1000",
		"25",
		"DOT",
		"-----",
		"Greater Sulfur",
		"Sulfur, Sulfur",
		"Explosion",
		"3",
		"1000",
		"60",
		"DOT",
		"-----",
		"Nitrogen",
		"None",
		"Fire",
		"5",
		"1000",
		"0",
		"Slowing",
		"-----",
		"Greater Nitrogen",
		"Nitrogen, Nitrogen",
		"Fire",
		"7",
		"800",
		"0",
		"Slowing",
		"-----",
		"Oxygen",
		"None",
		"Wind",
		"4",
		"500",
		"0",
		"Windbox",
		"-----",
		"Greater Oxygen",
		"Oxygen, Oxygen",
		"Wind",
		"7",
		"250",
		"0",
		"Windbox",
		"-----",
		"Explosion",
		"Sulfur, Nitrogen",
		"Explosion",
		"2",
		"1000",
		"650",
		"Damage",
		"-----",
		"Time Warp",
		"Oxygen, Nitrogen",
		"Cyclone",
		"5",
		"750",
		"0",
		"Wind",
		"-----",
		"Volcano",
		"Sulfur, Oxygen",
		"Explosion",
		"10",
		"450",
		"35",
		"Damage"
	};

	string[] enemyDoc = new string[] {
		"-----",
		"Basic_Slime",
		"200",
		"3",
		"1",
		"1",
		"None",
		"25",
		"-----",
		"Fire_Slime",
		"350",
		"5",
		"1",
		"1",
		"Sulfur",
		"40",
		"-----",
		"Nitroshroom",
		"500",
		"8",
		"1",
		"1",
		"Nitrogen",
		"65",
		"-----"
	};

	string[] spawnDoc = new string[] {
		"0 3 3",
		"1 -2 5",
		"2 -2 -7",
		"3 3 -4"
	};

	string[][] waveDoc = new string[][] {
		new string[] {"Basic_Slime 4", "Basic_Slime 6", "Basic_Slime 8"},
		new string[] {"Fire_Slime 8", "Basic_Slime 4", "Basic_Slime 6" },
		new string[] {"Fire_Slime 8", "Basic_Slime 42", "Basic_Slime 48", "Basic_Slime 62", "Basic_Slime 68"},
		new string[] {"Basic_Slime 2", "Basic_Slime 4", "Basic_Slime 6", "Nitroshroom 8"},
		new string[] {"Nitroshroom 8", "Fire_Slime 2", "Basic_Slime 4", "Basic_Slime 6"},
		new string[] {"Fire_Slime 8", "Nitroshroom 7", "Nitroshroom 9", "Basic_Slime 4", "Basic_Slime 6"},
		new string[] {"Fire_Slime 84", "Fire_Slime 86", "Nitroshroom 7", "Nitroshroom 9", "Basic_Slime 2", "Basic_Slime 4", "Basic_Slime 6"},
		new string[] {"Fire_Slime 85", "Fire_Slime 45", "Fire_Slime 65", "Nitroshroom 2", "Basic_Slime 42", "Basic_Slime 48", "Basic_Slime 62", "Basic_Slime 68"},
		new string[] {"Fire_Slime 2", "Fire_Slime 8", "Nitroshroom 4", "Nitroshroom 6", "Basic_Slime 1", "Basic_Slime 3", "Basic_Slime 7", "Basic_Slime 9", "Basic_Slime 84", "Basic_Slime 86"}
	};

	private void Start() {
		//using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Application.) ) {}
	}

}
