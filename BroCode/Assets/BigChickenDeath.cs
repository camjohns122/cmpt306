using UnityEngine;
using System.Collections;

public class BigChickenDeath : MonoBehaviour
{
	// The amount of confidence you lose when you get hit by a chicken.
	public float confidenceToLose;

	// The amount of confidence you gain when you kill a chicken.
	public float confidenceToGive;

	// The big chicken's health.
	public float health;

	// Make sure to attach confidence.
	public GameObject confidence;

	void OnTriggerEnter2D(Collider2D col)
	{
		// If the chicken hits the player, the player loses confidence.
		if (col.gameObject.tag == "Player")
		{
			confidence.GetComponent<Confidence>().loseConfidence (confidenceToLose);
		}

		// If the chicken gets hit by the player's projectile, the player gains confidence.
		if (col.gameObject.tag == "Projectile")
		{
			health = health - 1;
			if (health <= 0)
			{
				confidence.GetComponent<Confidence> ().giveConfidence (confidenceToGive);
				Destroy (gameObject);
			}
		}
	}
}
