using UnityEngine;
using System.Collections;

public class BossOneAI : MonoBehaviour
{
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = false;		// Check if the enemy is on a platform.
	private bool jump = false;				// Jump is active.

	public float speed = 4f;				// Running speed.
	public float acceleration = 2f;			// Acceleration.
	public float jumpSpeed = 14f;			// Velocity for the highest jump.

	private float timeToJump = 3f;			// To tell the AI when to jump.
	private float randomTime = 2f;			// A random time value.
	private float randomJumpHeight = 0.75f; // A random jump height.

	public GameObject player;				// Make sure to attach the player.

	private Animator myAnimator;			// Animator variable that is needed.

	void Start()
	{
		// Initializing the animator.
		myAnimator = GetComponent<Animator>();
	}
	
	void Update ()
	{
		// If the player is to the left of the enemy, he will go left.
		// If the player is to the right of the enemy, he will go right.
		if (player.transform.position.x < transform.position.x)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (player.transform.position.x > transform.position.x)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			leftOrRight = 0;
		}

		// The enemy will jump every 1-3 seconds.
		if (Time.time > timeToJump)
		{
			timeToJump += randomTime;

			if (isGrounded)
			{
				jump = true;
			}
		}
	}

	// The enemey can only jump when he is grounded.
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = true;

			// The random time has a value between 1 and 3.
			// The random jump height is a value between 0.5 and 1.
			randomTime = Random.Range (1f, 3f);
			randomJumpHeight = Random.Range (0.5f, 1f);
		}
	}

	// The enemy can only jump when he is grounded.
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = false;
		}
	}

	void FixedUpdate()
	{
		// Movement for going left and right.
		GetComponent<Rigidbody2D>().AddForce(new Vector2(((leftOrRight * speed) - GetComponent<Rigidbody2D>().velocity.x) * acceleration, 0));

		// This is the running animation.
		myAnimator.SetFloat("speed", Mathf.Abs(leftOrRight));

		// Movement for the jump.
		if (jump)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpSpeed * randomJumpHeight);
			jump = false;
		}

		// Code for the jump animations.
		if (isGrounded)
		{
			// The jump animation is set to false when on the ground.
			myAnimator.SetBool ("jump", false);
		}
		else
		{
			// The jump animation is set to true when in the air.
			myAnimator.SetBool("jump", true);
		}
	}

}
