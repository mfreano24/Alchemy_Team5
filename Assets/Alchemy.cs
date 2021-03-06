﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemy {

	[System.Serializable]
	public class Potion {

		// Attributes
		[SerializeField]
		public string name { get; set; }
		[SerializeField]
		public List<string> combination { get; set; }
		[SerializeField]
		public Sprite sprite { get; set; }
		[SerializeField]
		public int size { get; set; }
		[SerializeField]
		public float time { get; set; }
		[SerializeField]
		public int damage { get; set; }
		[SerializeField]
		public string effect { get; set; }

		// Detailed constructor
		public Potion(string name, List<string> combination, Sprite sprite, int size, int time, int damage, string effect) {
			this.name = name;
			this.combination = combination;
			this.sprite = sprite;
			this.size = size;
			this.time = time;
			this.damage = damage;
			this.effect = effect;
		}

		// Template constructor
		public Potion() {
			this.name = "";
			this.combination = new List<string>();
			this.sprite = null;
			this.size = 2;
			this.time = 0;
			this.damage = 0;
			this.effect = "";
		}

		// Overriding the string
		public override string ToString() {
			string text = "";

			text += "Potion Name: " + name;
			text += "\nPotion Combination:\n";
			foreach (string i in combination) {
				text += "-" + i + "\n";
			}

			text += "Collision Sprite: " + /*sprite.ToString()*/ "Does not exist";
			text += "\nBlast Radius: " + size.ToString();
			text += "\nLasting Time: " + time.ToString();
			text += "\nDamage Output: " + damage.ToString();

			text += "\nPotion Effect: " + effect;
			text += "\n";

			return text;
		}

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

		// These stats get updated based on the user's level
		// Possibly
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


