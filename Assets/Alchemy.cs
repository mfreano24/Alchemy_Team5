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
		public int speed { get; set; }
		[SerializeField]
		public string type { get; set; }

		// Why the hell not?
		[SerializeField]
		float hue { get; set; }

		public Enemy(string name, int hp, int atk, int def, int sp, string tp) {
			this.name = name;
			this.baseHP = hp;
			this.baseATK = atk;
			this.baseDEF = def;
			this.speed = sp;
			this.type = tp;
			this.hue = 0;
		}

		public Enemy() {
			this.name = "";
			this.baseHP = 0;
			this.baseATK = 0;
			this.baseDEF = 0;
			this.speed = 0;
			this.type = "";
			this.hue = 0;
		}

		// These stats get updated based on the user's level

	}

}


