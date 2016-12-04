using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public float speed;					// Speed of projectile.
	public Controller player;			// Projectile direction is based on the Player's direction.

	// Use this for initialization
	void Start () 
	{
		// Finds the Object that has a Controller script attached to it (Player)
		player = FindObjectOfType<Controller> ();

		// If the player is facing left, the projectile will shoot left. Otherwise, the projectile will shoot right.
		if (player.transform.localScale.x < 0) {
			speed = -speed;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make the projectile move horizontally.
		GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, GetComponent<Rigidbody2D>().velocity.y);﻿
	}

	// Destroy the projectile when it makes contact with anything except Clouds.
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Cloud")) {
			Destroy (gameObject);
		}
	}
}
