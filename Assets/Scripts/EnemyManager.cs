using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Alchemy;

public class EnemyManager : MonoBehaviour {

	List<Enemy> enemies = new List<Enemy>();

    void Start() {
		// List of elements
		string path = Directory.GetCurrentDirectory() + "\\enemies.txt";

		using (var reader = new StreamReader(path)) {
			// Get reader input
			string input = "hi";

			while (input != "" && input != null) {
				// Creating the items
				// Create a new potion
				Enemy newEnemy = new Enemy();
				for (int i = -1; i < 7; i++) {

					// Read actual input
					input = reader.ReadLine();

					if (input == "" || input == null) {
						break;
					}

					Debug.Log("i = " + i.ToString() + ", input = " + input);

					switch (i) {
						case (0):
							// Setting the element name
							newEnemy.name = input;
							break;
						case (1):
							// Create the combination
							newEnemy.baseHP = System.Convert.ToInt32(input);
							break;
						case (2):
							// Set up the sprite used
							newEnemy.baseATK = System.Convert.ToInt32(input);
							break;
						case (3):
							// Set up the maximum size of the explosion
							newEnemy.baseDEF = System.Convert.ToInt32(input);
							break;
						case (4):
							// Set up the time it lasts
							newEnemy.speed = System.Convert.ToInt32(input);
							break;
						case (5):
							// Set up the damage it'll do
							newEnemy.type = input;
							break;
						default:
							// Just for the -----
							break;
					}
				} // End creation

				enemies.Add(newEnemy);

			} // End of while (input != "" && input != null)
		} // End of File IO

		// Removes an empty potion
		enemies.RemoveAt(enemies.Count - 1);

		foreach (Enemy i in enemies) {
			Debug.Log(i.ToString());
		}
	}
}
