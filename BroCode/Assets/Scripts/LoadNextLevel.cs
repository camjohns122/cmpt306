using UnityEngine;
using System.Collections;

// This script is used to load the next level.
public class LoadNextLevel : MonoBehaviour
{
	// Type in the inspector which level you want to load.
	public string levelToLoad;

	// Function to tell if the player is at the end of the level
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			Application.LoadLevel (levelToLoad);
		}
	}
}
