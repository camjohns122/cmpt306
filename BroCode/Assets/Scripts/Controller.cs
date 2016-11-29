using UnityEngine;
using System.Collections;

// This is a script that will be used to control the player.
public class Controller : MonoBehaviour
{
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = false;		// Check if the player is on a platform.
	private bool jump = false;				// Jump is held.
	private bool jumpCancel = false;		// Jump is released.
		
	public float speed = 8f;				// Running speed.
	public float acceleration = 8f;			// Acceleration.
	public float jumpSpeed = 14f;			// Velocity for the highest jump.

	public float jumpLeeway = 0.15f;		// The amount of time a player can still jump after falling.
	private float jumpTimer;				// Makes the above possible.

	private Animator myAnimator;			// Animator variable that is needed.
	private float horizontal;				// Assists horizontal animations.

    private float yDirection;               // used to detect when player begins falling

    private Rigidbody2D player;             // reference to players Rigidbody

    private float topAngle;
    private float sideAngle;



	void Start()
	{
		// Initializing the animator.
		myAnimator = GetComponent<Animator>();
        myAnimator.SetBool ("falling", false);
        yDirection = transform.position.y;
        player = GetComponent<Rigidbody2D>();   //set references to game objects
        Vector2 size = GetComponent<BoxCollider2D>().size;
        topAngle = Mathf.Atan(size.x / size.y) * Mathf.Rad2Deg;
        sideAngle = 90.0f - topAngle;


	}

	void Update ()
	{
		// Assings horizontal which is needed for animations.
		horizontal = Input.GetAxisRaw ("Horizontal");

		// Assigns leftOrRight if player is going left or right.
		if( Input.GetAxisRaw("Horizontal") < 0 )
		{
			// Flips the sprite if the player goes left.
			// Assigns -1 if going left.
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if( Input.GetAxisRaw("Horizontal") > 0 )
		{
			// Flips the sprite if the player goes right.
			// Assings 1 if going right.
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
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

        if (transform.position.y - yDirection < 0)
        {
            myAnimator.SetBool("falling", true);
        }
        yDirection = transform.position.y;

	}

	// Function to tell if the player is on a platform
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			// Player is touching the ground.
			isGrounded = true;
		}
        
        // knockback if hit by an enemy
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss Projectile")
        {

            Vector3 v = (Vector3)col.contacts[0].point - transform.position;
            if (Vector3.Angle(v, transform.right) <= sideAngle)
            {
                myAnimator.SetBool("hit", true);
                player.AddForce(new Vector2(-800, 0));
            }
            else if (Vector3.Angle(v, -transform.right) <= sideAngle)
            {
                player.AddForce(new Vector2(800, 0));
                myAnimator.SetBool("hit", true);
            }
            else if (leftOrRight == -1)
            {
                myAnimator.SetBool("hit", true);
                player.AddForce(new Vector2(800, 0));
            }
            else
            {
                player.AddForce(new Vector2(-800, 0));
                myAnimator.SetBool("hit", true);
            }
            

        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Boss Projectile")
        {
            player.AddForce(new Vector2(-800, 0));
            myAnimator.SetBool("hit", true);
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
        myAnimator.SetBool("hit", false);

		// jumpTimer only updates when the player is on the ground.
		if (isGrounded)
		{
			jumpTimer = Time.time;

			// The jump animation is set to false when on the ground.
			myAnimator.SetBool ("jump", false);
		}
		else
		{
			// The jump animation is set to true when in the air.
			myAnimator.SetBool("jump", true);
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

		// This is the running animation.
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

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
