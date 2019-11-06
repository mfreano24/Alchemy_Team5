using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour {
	/// <summary>
	/// This will be used for the pause screen.
	/// In order to call to this, you can do it in two ways:
	/// 
	/// Assign a variable in the following way:
	/// GlobalVars gv;
	/// 
	/// private void Start() {
	///		gv = GameObject.Find("EventSystem").GetComponent<GlobalVars>();
	///		gv.playing = true;
	/// }
	/// 
	/// 
	/// Just call the item directly:
	/// GameObject.Find("EventSystem").GetComponent<GlobalVars>().playing = true;
	/// 
	/// If you're using a script that needs this a lot, you might want the first method. Saves time and characters.
	/// 
	/// </summary>
	public bool playing;


	private void Start() {
		// Initialize all vars
		playing = true;
	}
}
