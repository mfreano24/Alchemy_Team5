using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemy {

	[System.Serializable]
	public class Potion {

		// Base Attributes
		public string name { get; set; }
		public byte type { get; set; }
		public List<string> ingredients { get; set; }
		public float requiredScale { get; set; }		// For the initial image
		public float explosionSize { get; set; }		// For the scale of the explosion
		public int time { get; set; }					// Time in milliseconds for explosion to occure
		public float rangeExtension { get; set; }
		public byte audioSource { get; set; }

		// Damage-Based Attributes
		public float playerDamage { get; set; }
		public int enemyDamage { get; set; }
		public float buffer { get; set; }
		public byte hits { get; set; }

		// Speed-Based Attributes
		public float slowDuration { get; set; }
		public float slowStrength { get; set; }

		// Knockback-Based Attributes
		public float kbDuration { get; set; }
		public float kbPower { get; set; }

		// Combined Attributes
		public float toScale { get { return requiredScale * explosionSize; } }

		public Potion() { ingredients = new List<string>(); }
	}

	[System.Serializable]
	public class Enemy {
		// Attributes of an enemy
		[SerializeField]
		public string name { get; set; }
		[SerializeField]
		public int baseHP { get; set; }
		[SerializeField]
		public int baseATK { get; set; }
		[SerializeField]
		public int baseDEF { get; set; }
		[SerializeField]
		public float speed { get; set; }
		[SerializeField]
		public string type { get; set; }
		[SerializeField]
		public int exp { get; set; }

		// Why the hell not?
		[SerializeField]
		float hue { get; set; }

		// Detailed Constructor
		public Enemy(string name, int hp, int atk, int def, float sp, string tp, int ex) {
			this.name = name;
			this.baseHP = hp;
			this.baseATK = atk;
			this.baseDEF = def;
			this.speed = sp;
			this.type = tp;
			this.hue = 0;
			this.exp = ex;
		}

		// Copy Constructor
		public Enemy(Enemy e) {
			this.name = e.name;
			this.baseHP = e.baseHP;
			this.baseATK = e.baseATK;
			this.baseDEF = e.baseDEF;
			this.speed = e.speed;
			this.type = e.type;
			this.hue = e.hue;
			this.exp = e.exp;
		}

		// Default Constructor
		public Enemy() {
			this.name = "";
			this.baseHP = 0;
			this.baseATK = 0;
			this.baseDEF = 0;
			this.speed = 0;
			this.type = "";
			this.hue = 0;
			this.exp = 0;
		}

		// Scales the enemies based on your level
		public void Scale(int level) {
			this.baseHP += 25 * level;
			this.baseATK += 2 * level;
			this.baseDEF += level;
			this.exp += level;
		}
	}

	public class InventorySlot {
		public Potion item { get; set; }
		public int count { get; set; }

		public InventorySlot() {
			this.item = null;
			this.count = 0;
		}

		public InventorySlot(Potion newItem, int newCount) {
			this.item = newItem;
			this.count = newCount;
		}
	}

	public class Wave {
		public List<Enemy> waveEnemies { get; set; }
		public List<int> spawnIndices { get; set; }

		public Wave() {
			waveEnemies = new List<Enemy>();
			spawnIndices = new List<int>();
		}
	}

	public class Level {

		public string Author { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int[,] Floor { get; set; }
		public int[,] Walls { get; set; }

		public List<Vector2> Spawners { get; set; }

		public Level(string auth, string name, string code, string floor, string wall, string spawn) {
			Author = auth;
			Name = name;
			Code = code;
			Floor = GenerateTiles(floor);
			Walls = GenerateTiles(wall);
			Spawners = GenerateSpawners(spawn);
		}

		private List<Vector2> GenerateSpawners(string spawn) {
			string[] spawners = spawn.Split(new string[] { "}{" }, StringSplitOptions.None);
			spawners[0] = spawners[0].Trim('{');
			spawners[spawners.Length - 1] = spawners[spawners.Length - 1].Trim('}');

			List<Vector2> toReturn = new List<Vector2>();

			foreach (string s in spawners) {
				string[] fragment = s.Split(new string[] { ", " }, StringSplitOptions.None);
				Vector2 toAdd = new Vector2(System.Convert.ToInt32(fragment[1]), System.Convert.ToInt32(fragment[2]));
				toReturn.Add(toAdd);
			}

			return toReturn;
		}

		private int[,] GenerateTiles(string s) {

			int[,] toReturn = new int[20, 16];

			string[] spawners = s.Split(new string[] { ", }{" }, StringSplitOptions.None);
			spawners[0] = spawners[0].TrimStart('{');
			spawners[spawners.Length - 1] = spawners[spawners.Length - 1].TrimEnd(new char[] { '}', ',', ' ' });
			for (int i = 0; i < 20; i++) {
				string[] fragment = spawners[i].Split(new string[] { ", " }, StringSplitOptions.None);
				for (int j = 0; j < 16; j++) {
					int toAdd = System.Convert.ToInt32(fragment[j]);
					toReturn[i, j] = toAdd;
				}
			}

			return toReturn;
		}
	}
}


