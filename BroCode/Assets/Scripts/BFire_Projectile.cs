using UnityEngine;
using System.Collections;

public class BFire_Projectile : MonoBehaviour {

	public float speed;					// Speed of projectile.
	public DrunkAI drunk;				// Projectile direction is based on the Boss's direction.

	// Make sure to attach confidence.
	public GameObject confidence;

	// The amount of confidence you lose when you get hit by a chicken.
	public float confidenceToLose;

	// Use this for initialization
	void Start () 
	{
		// Finds the Object that has a BossOneAI script attached to it (Boss)
		drunk = FindObjectOfType<DrunkAI> ();

		// If the boss is facing left, the projectile will shoot left. Otherwise, the projectile will shoot right.
		if (drunk.transform.localScale.x < 0)
		{
			speed = -speed;
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		else
		{
			GetComponent<SpriteRenderer> ().flipX = true;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Make the projectile move horizontally.
		GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, GetComponent<Rigidbody2D>().velocity.y);﻿
	}

	// Destroy the projectile when it makes contact with something.
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the Egg hits the player, the player loses confidence.
		if (col.gameObject.tag == "Player")
		{
			confidence.GetComponent<Confidence>().loseConfidence (confidenceToLose);
			Destroy (gameObject);
		}

		// If the Egg gets hit by the player's projectile, destroys both.
		if (col.gameObject.tag == "Projectile")
		{
			Destroy (gameObject);
		}

		// If the Egg hits ground, or platform, destroys itself.
		if (col.gameObject.tag == "Platform")
		{
			Destroy (gameObject);
		}
	}
}

