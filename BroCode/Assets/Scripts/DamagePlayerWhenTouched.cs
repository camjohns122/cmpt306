using UnityEngine;
using System.Collections;

// This script is used to damage the player when he touches a boss or an enemy.
public class DamagePlayerWhenTouched : MonoBehaviour
{
	// This is the amount of damage to give to the player.
	public float damageToGive;

	// Make sure to attach the confidence.
	public GameObject confidence;

	// When the object hits the player, subtract the apprioriate health from the player.
	void OnTriggerEnter2D(Collider2D col)
	{
		// When the player has high confidence, the boss does high damage.
		// When the player has low confidence, the boss does low damage.
		if (col.gameObject.tag == "Player")
		{
			col.GetComponent<PlayerHealth> ().giveDamage (1 + damageToGive * confidence.GetComponent<Confidence> ().getConfidence ()/25);
		}
	}
}
