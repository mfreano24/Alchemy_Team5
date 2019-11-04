﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alchemy;
using System.Linq;

public class PotionCrafting : MonoBehaviour {
	// Initial values
	Text ingredients;
	Text preview;

	public List<Potion> crafting = new List<Potion>();
	Potion craftedPotion = null;

	PlayerController pc;

	private void Start() {
		ingredients = GameObject.Find("Ingredients").GetComponent<Text>();
		preview = GameObject.Find("Preview").GetComponent<Text>();

		pc = GameObject.Find("Player").GetComponent<PlayerController>();

		ingredients.text = "";
		preview.text = "";
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.LeftShift) && pc.selectedPotion.count > 0) {
			AddIngredient();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			CraftPotion();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cancel();
		}
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
			foreach (InventorySlot slot in pc.inventory) {
				if (craftedPotion == slot.item) {
					// The item exists already
					slot.count++;
					break;
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
