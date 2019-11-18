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


	/// <summary>
	/// 
	/// These variables are dedicated to explaining the world size.
	/// This will be used to check if enemies (or the player) are out of bounds.
	/// They will be used to find the next closest entry point in which the
	/// actor will be teleported. This will prevent the object from being out
	/// of bounds. (Think of it like Mario's wall system).
	/// 
	/// </summary>
	public int worldSizeX;
	public int worldSizeY;

	/// <summary>
	/// 
	/// These variables are dedicated to explaining the center of the
	/// world. This is in case the world is offset, this will allow
	/// for the "world zone" to be properly implemented.
	/// 
	/// </summary>
	public int worldCenterX;
	public int worldCenterY;

	/// <summary>
	/// 
	/// Set up the target framerate of the game.
	/// Probably shouldn't edit this.
	/// Default and main target should be 60.
	/// 
	/// </summary>
	public int targetFrameRate;
		
	private void Start() {
		// Initialize all vars
		playing = true;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFrameRate;
	}
}
