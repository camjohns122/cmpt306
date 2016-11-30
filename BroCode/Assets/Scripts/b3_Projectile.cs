using UnityEngine;
using System.Collections;

public class b3_Projectile : MonoBehaviour {

	public float speed;					// Speed of projectile.
	public Boss3AI boss;				// Projectile direction is based on the Boss's direction.
	public Transform firePoint;			// The starting point where the projectile is fired from. (Right of boss)
	public Transform firePoint2;		// The starting point where the projectile is fired from. (Left of boss)

	// Use this for initialization
	void Start () 
	{
		// Finds the Object that has a BossOneAI script attached to it (Boss)
		boss = FindObjectOfType<Boss3AI> ();

		// If the boss is facing left, the projectile will shoot left. Otherwise, the projectile will shoot right.
/*		if (boss.transform.localScale.x < 0) {
			speed = -speed;
		}*/
	}

	// Update is called once per frame
	void Update () 
	{
		// Make the projectile move horizontally to the right of the boss.
		if (GetComponent<Rigidbody2D> ().position.x > boss.transform.position.x) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);﻿
		}
		// Make the projectile move horizontally to the left of the boss.
		else if (GetComponent<Rigidbody2D> ().position.x < boss.transform.position.x) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);﻿
		}
		// Make the projectile move vertically above the boss.
		else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, speed);﻿
		}
	}

	// Destroy the projectile when it makes contact with something.
	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy (gameObject);
	}
}
