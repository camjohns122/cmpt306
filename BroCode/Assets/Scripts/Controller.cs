﻿using UnityEngine;
using System.Collections;

// This is a script that will be used to control the player.
public class Controller : MonoBehaviour
{
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = false;		// Check if the player is on a platform.
	private bool jump = false;				// Jump is held.
	private bool jumpCancel = false;		// Jump is released.
	private float jumpTimer;				// Makes the above possible.
		
	public float speed = 8f;				// Running speed.
	public float acceleration = 8f;			// Acceleration.
	public float jumpSpeed = 14f;			// Velocity for the highest jump.
	public float jumpLeeway = 0.15f;		// The amount of time a player can still jump after falling.

	// Update is called once per frame
	void Update ()
	{
		// Assigns leftOrRight if player is going left or right.
		if( Input.GetAxisRaw("Horizontal") < 0 )
		{
			// Flips the sprite if the player goes left.
			// Assigns -1 if going left.
			leftOrRight = -1;
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		}
		else if( Input.GetAxisRaw("Horizontal") > 0 )
		{
			// Flips the sprite if the player goes right.
			// Assings 1 if going right.
			leftOrRight = 1;
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
		}
		else
		{
			// If standing still.
			leftOrRight = 0;
		}

		// Assigns jump if player holds the jump button.
		if (Input.GetButtonDown ("Jump") && isGrounded)
		{
			jump = true;
		}

		// Assings jumpCancel if player releases the jump button.
		if (Input.GetButtonUp ("Jump"))
		{
			jumpCancel = true;
		}
	}

	// Function to tell if the player is on a platform
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			// Player is touching the ground.
			isGrounded = true;
		}
	}

	// Function to tell if the player has left a platform
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			// Player leaves the ground.
			isGrounded = false;
		}
	}

	void FixedUpdate ()
	{
		// jumpTimer only updates when the player is on the ground.
		if(isGrounded)
		{
			jumpTimer = Time.time;
		}

		// If the player falls off a platform, they still have a fraction of a second where a jump is still possible.
		if(!isGrounded && Input.GetButtonDown("Jump") && ((Time.time - jumpTimer) < jumpLeeway))
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpSpeed);
			jump = false;
			jumpTimer = 0f;
		}

		// This moves the player left or right.
		// This will make it so that there is a max speed in the left or right.
		GetComponent<Rigidbody2D>().AddForce(new Vector2(((leftOrRight * speed) - GetComponent<Rigidbody2D>().velocity.x) * acceleration, 0));

		// If the player is stationary, he will stop dead in his tracks.
		if (leftOrRight == 0)
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		}

		if (jump)
		{
			// This makes it so the player has a max jump height.
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpSpeed);
			jump = false;
		}

		// Cancel the jump when the button is no longer pressed.
		if (jumpCancel)
		{
			if ( GetComponent<Rigidbody2D>().velocity.y > 0 )
			{
				// This makes it so the player has a min jump height.
				// This will also ensure that the player will immediately drop as soon as the player releases the jump button.
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
			}

			jumpCancel = false;
			jumpTimer = 0f;
		}
	}
}
