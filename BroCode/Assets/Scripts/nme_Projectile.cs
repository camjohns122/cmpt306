using UnityEngine;
using System.Collections;

public class nme_Projectile : MonoBehaviour {

	public float speed;					// Speed of projectile.
	public BigChicken bigChicken;			// Projectile direction is based on the Player's direction.

	// Make sure to attach confidence.
	public GameObject confidence;

	// The amount of confidence you lose when you get hit by a chicken.
	public float confidenceToLose;

	// Use this for initialization
	void Start () 
	{
		// Finds the Object that has a Controller script attached to it (Player)
		bigChicken = FindObjectOfType<BigChicken> ();
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
		}

		// If the Egg gets hit by the player's projectile, destroys both.
		if (col.gameObject.tag == "Projectile")
		{
			Destroy (gameObject);
		}
	}
}
