using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PotionDisplay : MonoBehaviour {

	PlayerController pc;
	GlobalVars gv;

    Text current_potion;
    int iterator;
    void Start() {
		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
		current_potion = GetComponent<Text>();
        iterator = 0;
    }
    void Update() {
        PotionMenu();
    }

    void PotionMenu() {
        /*PSEUDO-
        the header of this script will declare an iterator i = 0 and an array size 6
        Checks for iterator switches:
        when RT is pressed: i++ UNLESS i++ == 6, in which case set i to 0.
        when LT is pressed: i-- UNLESS i-- == -1, in which case set i to 5.
        
        After any checks for iterator switches -> UI displays potion (text for now) in top left.*/

        //TODO: Skipping system for elements we dont have any of?
		// TO-DO: SET THESE AS GLOBAL INPUTS
        if(Input.GetButtonDown("Next") && gv.playing) {
            iterator++;
            if(iterator == pc.inventory.Count) {
                iterator = 0;
            }
        }
        if(Input.GetButtonDown("Previous") && gv.playing) {
            iterator--;
            if(iterator == -1){
                iterator = pc.inventory.Count - 1;
            }

        }

        current_potion.text = pc.inventory[iterator].item.name;
		GameObject.Find("ElementCount").GetComponent<Text>().text = "x" + pc.inventory[iterator].count.ToString();
		pc.selectedPotion = pc.inventory[pc.FindInventorySlot(pc.inventory[iterator].item)];
		if (Resources.Load<Sprite>("UIIcons/UI" + pc.inventory[iterator].item.name.ToString().Replace(" ", "")) != null) {
			GameObject.Find("PotionImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UIIcons/UI" + pc.inventory[iterator].item.name.ToString().Replace(" ", ""));
		}else {
		GameObject.Find("PotionImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UIIcons/UIOther");
        } 
    }

	public void PotionUsed() {
		iterator--;
	}
}
