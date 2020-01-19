using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alchemy;
using System.Linq;
using System;

public class PotionCrafting : MonoBehaviour {
	// Initial values
	Text ingredients;
	Text preview;

	public List<Potion> crafting = new List<Potion>();
	Potion craftedPotion = null;

	PlayerController pc;
	GlobalVars gv;
	AudioSource[] asc;
	AudioSource curr;

	private void Start() {
		ingredients = GameObject.Find("Ingredients").GetComponent<Text>();
		preview = GameObject.Find("Preview").GetComponent<Text>();

		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();

		ingredients.text = "";
		preview.text = "";

		asc = GetComponents<AudioSource>();

	}

	private void Update() {
		if (gv.playing) {
			if (Input.GetButtonDown("AddCraft") && pc.selectedPotion.count > 0) {
				AddCraft();
			}

			if (Input.GetButtonDown("Submit")) {
				CraftPotion();

			}

			if (Input.GetButtonDown("Cancel")) {
				Cancel();
			}
		}
	}

	public void AddCraft() {
		AddIngredient();
		curr = asc[1];
		curr.Play();
	}

	public void CraftingUpdated() {

		craftedPotion = null;

		ingredients.text = "";

		foreach (Potion i in crafting) {
			ingredients.text += "\n" + i.name;
		}

		List<Potion> simplified = GameObject.Find("EventSystem").GetComponent<PotionManager>().BaseList(crafting);

		List<Potion> sortedPotion = new List<Potion>(simplified.OrderByDescending(l => l.name));
		List<string> sortedNames = new List<string>();
		foreach (Potion i in sortedPotion) {
			sortedNames.Add(i.name);
		}

		sortedNames.OrderBy(name => name);

		foreach (Potion i in GameObject.Find("EventSystem").GetComponent<PotionManager>().potions) {

			if (Enumerable.SequenceEqual(sortedNames.OrderBy(name => name), i.combination.OrderBy(name => name))) {
				craftedPotion = i;
				break;
			}
		}

		if (craftedPotion != null) {
			preview.text = craftedPotion.name;
		} else {
			preview.text = "";
		}
	}

	public void AddIngredient() {
		if (pc.selectedPotion.count > 0) {
			crafting.Add(pc.selectedPotion.item);
			pc.selectedPotion.count--;
		}
		CraftingUpdated();
	}

	public void CraftPotion() {

		if (craftedPotion != null) {

			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial" && craftedPotion.name == "Volcano") {
				pc.currentExperience++;
			}

			curr = asc[0];
			curr.Play();
			foreach (InventorySlot slot in pc.inventory) {
				if (craftedPotion == slot.item) {
					// The item exists already
					if (slot.count == pc.MAX_ITEMS) {
						string created = preview.text;
						Cancel();
						ingredients.text = "Maximum capacity for " + created + " achieved!";
						return;
					}
					slot.count++;
					craftedPotion = null;
					crafting = new List<Potion>();
					CraftingUpdated();
					return;
				}
			}

			// The item doesn't exist
			pc.inventory.Add(new InventorySlot(craftedPotion, 1));
			craftedPotion = null;
			crafting = new List<Potion>();
			CraftingUpdated();
		}

	}

	public void Cancel() {

		for (int i = 0; i < crafting.Count; i++) {
			foreach (InventorySlot slot in pc.inventory) {
				if (crafting[i] == slot.item) {
					// Subtract the count from your inventory
					slot.count++;
				}
			}
		}

		craftedPotion = null;
		crafting = new List<Potion>();
		CraftingUpdated();
	}

}
