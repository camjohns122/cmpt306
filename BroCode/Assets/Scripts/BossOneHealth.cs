using UnityEngine;
using System.Collections;

// This script is used to manage the boss's health
public class BossOneHealth : MonoBehaviour
{
	// The player's total health.
	public float bossHealth;

	// Type in the inspector which level you want to load.
	public string levelToLoad;

	// Update is called once per frame
	void Update ()
	{
		// When the player dies, reload the appropriate level.
		if (bossHealth <= 0f)
		{
			Application.LoadLevel (levelToLoad);
		}	
	}

	// When the boss gets hit by something, subtract the appropriate health.
	public void giveDamage(float damageToGive)
	{
		bossHealth = bossHealth - damageToGive;
	}

	// A function used to get the boss's health outside of this script.
	public float getHealth()
	{
		return bossHealth;
	}
}
