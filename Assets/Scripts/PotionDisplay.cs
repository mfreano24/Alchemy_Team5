using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionDisplay : MonoBehaviour
{
    string[] potion_arr = {"Sulfur", "Mercury", "Salt", "Bomb", "Cyclone", "Cloud"}; //These will be Potion.type
    int[] potion_quants = {5, 17, 10, 3, 0, 1};//These will be Potion.quantity
    Text current_potion;
    int iterator;
    void Awake(){
        current_potion = GetComponent<Text>();
        iterator = 0;
    }
    void Update(){
        PotionMenu();
    }

    void PotionMenu(){
        /*PSEUDO-
        the header of this script will declare an iterator i = 0 and an array size 6
        Checks for iterator switches:
        when RT is pressed: i++ UNLESS i++ == 6, in which case set i to 0.
        when LT is pressed: i-- UNLESS i-- == -1, in which case set i to 5.
        
        After any checks for iterator switches -> UI displays potion (text for now) in top left.*/

        //TODO: Skipping system for elements we dont have any of?
        if(Input.GetKeyDown(KeyCode.E)){
            iterator++;
            if(iterator == potion_arr.Length){
                iterator = 0;
            }

        }
        if(Input.GetKeyDown(KeyCode.Q)){
            iterator--;
            if(iterator == -1){
                iterator = 5;
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            potion_quants[iterator] -= 1;
            if(potion_quants[iterator]<0){
                potion_quants[iterator] = 0;
            }
        }
        current_potion.text = potion_arr[iterator];
		GameObject.Find("ElementCount").GetComponent<Text>().text = "x" + potion_quants[iterator].ToString();
    }
}
