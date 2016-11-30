using UnityEngine;
using System.Collections;

public class Boss3AI : MonoBehaviour
{
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = false;		// Check if the enemy is on a platform.
	private bool jump = false;				// Jump is active.

	public float speed = 3f;				// Running speed.
	public float acceleration = 2f;			// Acceleration.
	public float jumpSpeed = 14f;			// Velocity for the highest jump.

	private float timeToJump = 3f;			// To tell the AI when to jump.
	private float timeToShoot = 2f;			// To tell the AI when to shoot.
	private float randomTime = 2f;			// A random time value.
	private float randomJumpHeight = 0.75f; // A random jump height.

	public GameObject player;				// Make sure to attach the player.

	private Animator myAnimator;			// Animator variable that is needed.

	public GameObject confidence;			// Make sure to attach confidence.

	public Transform firePoint; 			// The starting point where the projectile is fired from. (Right of boss)
	public Transform firePoint2;			// The starting point where the projectile is fired from. (Left of boss)
	public GameObject projectile; 			// The item that the enemy shoots.

	private float yDirection;               // used to detect when player begins falling


	void Start()
	{
		// Initializing the animator.
		myAnimator = GetComponent<Animator>();

		// Make changes based on confidence
		speed = speed + confidence.GetComponent<Confidence> ().getConfidence ()/50f;
		myAnimator.SetBool("falling", false);

	}

	void Update ()
	{
		// If the player is to the left of the enemy, he will go left.
		// If the player is to the right of the enemy, he will go right.
		if (player.transform.position.x < transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) <= 8)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (player.transform.position.x > transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) <= 8)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (player.transform.position.x < transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) > 8)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (player.transform.position.x > transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) > 8)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}

		// The enemy will jump every 1-5 seconds (dependent on confidence).
		if (Time.time > timeToJump)
		{
			timeToJump += randomTime;

			if (isGrounded)
			{
				jump = true;
			}
		}

		// The enemy will shoot every 1-5 seconds (dependent on confidence).
		if (Time.time > timeToShoot)
		{
			timeToShoot += randomTime;

			// Made an array of GameObjects to check how many Boss Projectiles are on the screen.
			GameObject[] array;
			array = GameObject.FindGameObjectsWithTag("Boss Projectile");

			// Make the projectile if there aren't too many boss projectiles on the screen.
			if (array.Length < 3)
			{
				var clone = Instantiate (projectile, firePoint.position, firePoint.rotation);
				var clone2 = Instantiate (projectile, firePoint2.position, firePoint2.rotation);
			}
		}
		if (transform.position.y - yDirection < 0)
		{
			myAnimator.SetBool("falling", true);
		}
		yDirection = transform.position.y;
	}

	// The enemey can only jump when he is grounded.
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = true;

			// The random time has a value between 1 and 3.
			// The random jump height is a value between 0.5 and 1.
			// If confidence is high, the boss will jump every 1 second.
			// If the confidence is low, the boss will jump every 1 to 3 seconds.
			randomTime = Random.Range (1f, 5f - confidence.GetComponent<Confidence> ().getConfidence ()/25f);
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