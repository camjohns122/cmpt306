using UnityEngine;
using System.Collections;

public class b2_Projectile : MonoBehaviour {

	public float speed;					// Speed of projectile.
	public Boss2AI boss;				// Projectile direction is based on the Boss's direction.

	// Use this for initialization
	void Start () 
	{
		// Finds the Object that has a BossOneAI script attached to it (Boss)
		boss = FindObjectOfType<Boss2AI> ();

		// If the boss is facing left, the projectile will shoot left. Otherwise, the projectile will shoot right.
		if (boss.transform.localScale.x < 0)
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
		Destroy (gameObject);
	}
}
