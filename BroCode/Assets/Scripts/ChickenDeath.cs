using UnityEngine;
using System.Collections;

public class ChickenDeath : MonoBehaviour
{
	// The amount of confidence you lose when you get hit by a chicken.
	public float confidenceToLose;

	// The amount of confidence you gain when you kill a chicken.
	public float confidenceToGive;

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
			confidence.GetComponent<Confidence>().giveConfidence (confidenceToGive);
			Destroy (gameObject);
		}
	}
}
