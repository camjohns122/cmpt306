using UnityEngine;
using System.Collections;

// This script is used to manage the player's health
public class PlayerHealth : MonoBehaviour
{
	// The player's total health.
	public float playerHealth;

	// Type in the inspector which level you want to load.
	public string levelToLoad;

	// Update is called once per frame
	void Update ()
	{
		// When the player dies, reload the appropriate level.
		if (playerHealth <= 0f)
		{
			Application.LoadLevel (levelToLoad);
		}	
	}

	// When the player gets hit by something, subtract the appropriate health.
	public void giveDamage(float damageToGive)
	{
		playerHealth = playerHealth - damageToGive;
	}

	// A function used to get the players health outside of this script.
	public float getHealth()
	{
		return playerHealth;
	}
}
