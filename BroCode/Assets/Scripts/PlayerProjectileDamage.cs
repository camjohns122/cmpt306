﻿using UnityEngine;
using System.Collections;

// This script is used to damage the player when he touches a boss or an enemy.
public class PlayerProjectileDamage : MonoBehaviour
{
	// This is the amount of damage to give to the player.
	public float damageToGive;

	// When the object hits the player, subtract the apprioriate health from the player.
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			// If boss one exists, give damage to him.
			if (col.GetComponent<BossOneHealth> () != null)
			{
				col.GetComponent<BossOneHealth> ().giveDamage (damageToGive);
			}
		}
	}
}