using UnityEngine;
using System.Collections;

public class Boss2AI : MonoBehaviour
{
	public DecisionTree tree;





	private Animator myAnimator;			// Animator variable that is needed.

	public GameObject confidence;			// Make sure to attach confidence.

	//Player Variables
	private Controller Hero;

	public LayerMask isPlayer;
	public float HeroNearDistance;
	public bool HeroNear;

	// Boss Variables
	Rigidbody2D bossBody;
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = true;		// Check if the enemy is on a platform.
	private bool jump;				// Jump is active.
	public float bossHealth;
	private float pushForce = 500f;
	private float jumpForce = 1200f;
	public float acceleration = 5f;
	public float moveSpeed = 6f;

	//Shooting Variables
	public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject b2_projectile; 		// The item that the enemy shoots.

	//Enemy Fire Rate
	public float fireRate = 4f;
	public float nextfire = 0.0F;

	//Enemy Healing
	public bool healing = false;
	public float healRate = 3f;
	public float healAmount = 2f;
	public float nextHeal = 0.0F;

	//doing thing Cooldown
	public float doRate = 2f;
	public float nextDo = 0.0F;

	//Bomber Variables
	public GameObject bomber;

	//Construct Locations
	public bool constructing = false;
	public GameObject[] jumpLoc;
	public Transform leftJumpPoint;
	public Transform rightJumpPoint;
	public Transform mirrorPoint; 

	//Constructing Flags
	private bool step1 = false;
	private bool step2 = false;
	private bool step3 = false;
	private bool step4 = false;
	private bool step5 = false;


	void Start () {

		// Initializing the animator.
		myAnimator = GetComponent<Animator>();
		tree = new DecisionTree ();
		BuildDecisionTree ();
		Hero = FindObjectOfType<Controller>();
		bossBody = GetComponent<Rigidbody2D>();
		jumpLoc = GameObject.FindGameObjectsWithTag ("JumpLoc");
		moveSpeed = moveSpeed + confidence.GetComponent<Confidence> ().getConfidence ()/50f;
	}


	void FixedUpdate(){
		// This is the running animation.
		myAnimator.SetFloat("speed", Mathf.Abs(leftOrRight));
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



		if (constructing == true) {
			if (step1 == false) {
				Transform Target;
				Target = GetCloserJumpLoc ();
				if (transform.position.x != Target.position.x) {
					transform.localPosition = Vector3.MoveTowards (transform.localPosition, Target.position, moveSpeed * Time.deltaTime);
				} else {
					step1 = true;
				}
			} else if (step2 == false) {
				Jump ();
				step2 = true;
			} else if (step3 == false) {
				flipCorrectWay ();
				//start countdown for delay
				nextDo = Time.time + doRate;
				step3 = true;
			} else if (step4 == false) {
				//Animation for construction.
				if (Time.time > nextDo) {
					//spawn Bomber
					var clone = Instantiate (bomber, firePoint.position, firePoint.rotation);
					step4 = true;
				}
			} else if (step5 == false) {
				leftOrRight = leftOrRight * -1;
				if (mirrorPoint.transform.position.x < this.transform.position.x) {
					bossBody.AddForce (new Vector2 (leftOrRight * pushForce, 0f));
				} else if (mirrorPoint.transform.position.x > this.transform.position.x) {
					bossBody.AddForce (new Vector2 (leftOrRight * pushForce, 0f));
				}
				step1 = false;
				step2 = false;
				step3 = false;
				step4 = false;
				step5 = false;
				constructing = false;
			}

		} else if (healing == true) {
			//healing animation
			if (Time.time > nextDo) {
				GetComponent<BossOneHealth> ().heal(healAmount);
				healing = false;
			}
		}else{
			tree.Search ();		
		}




	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = true;
		}
	}

	/*
	*
	*		Decision Methods
	*
	*/

	public bool heroNear(){
		HeroNear = Physics2D.OverlapCircle(transform.position, HeroNearDistance, isPlayer);

		if (HeroNear) {
			return true;
		} else {
			return false;
		}

	}

	public bool healthLow(){
		bossHealth = GetComponent<BossOneHealth> ().bossHealth;
		if (bossHealth <= 20f)
		{
			return true;
		}	
		else{
			return false;
		}

	}

	/*
	*
	*		Action Methods
	*
	*/

	//run away  from player, might add a follow up if cornered try to fight its way out, for balance
	public void getAway(){

		if (Hero.transform.position.x < transform.position.x)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (Hero.transform.position.x > transform.position.x)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}

		bossBody.AddForce(new Vector2(((leftOrRight * moveSpeed) - bossBody.velocity.x) * acceleration, 0));

	}

	//Run at Player 
	//Shoot projectile, might add alternate shoot and lob projectiles later
	public void Attack(){

		if (Hero.transform.position.x < transform.position.x)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (Hero.transform.position.x > transform.position.x)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}

		bossBody.AddForce(new Vector2(((leftOrRight * moveSpeed) - bossBody.velocity.x) * acceleration, 0));
		Shoot ();

	}


	//A slow ticking healing 
	//Would love an animation for it.
	public void Heal(){
		healing = true;
		nextDo = Time.time + doRate;
	}

	//Construct 
	//I want to check if two constructed,
	//if not, run to jump point, jump
	//face appropriate way, construct,
	//move off top platform
	public void Construct(){

		GameObject[] arr;
		arr = GameObject.FindGameObjectsWithTag("Bomber");

		if (arr.Length < 2) {
			constructing = true;
		} else {

			Shoot ();//I think this is a good time to lob.
		}

	}



	//Secondary Action Functions
	public void Shoot(){

		if(Time.time > nextfire){
			nextfire = Time.time + fireRate;

			var clone = Instantiate (b2_projectile, firePoint.position, firePoint.rotation);
		}

	}

	public void Jump(){
		bossBody.AddForce (transform.up * jumpForce);
		jump = false;
	}

	//Finds out which jump point it is closest too.
	public Transform GetCloserJumpLoc(){
		GameObject[] arr;
		arr = GameObject.FindGameObjectsWithTag("Bomber");

		if (arr.Length == 1) {
			GameObject bomber = arr [0];
			if (mirrorPoint.transform.position.x < bomber.transform.position.x) {
				return leftJumpPoint;
			} else if (mirrorPoint.transform.position.x > bomber.transform.position.x) {
				return rightJumpPoint;
			}
		} else {

			GameObject closest = null;
			float distance = Mathf.Infinity;

			foreach (GameObject jlo in jumpLoc) {
				Vector3 diff = jlo.transform.position - transform.position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					closest = jlo;
					distance = curDistance;
				}
			}

			return closest.transform;
		}

		return rightJumpPoint;
	}

	//flip to face correct way to construct
	public void flipCorrectWay(){

		if (mirrorPoint.transform.position.x < transform.position.x)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (mirrorPoint.transform.position.x > transform.position.x)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}


	//BuildDecisionTree
	void BuildDecisionTree(){

		DecisionTree b1 = new DecisionTree ();
		DecisionTree b2 = new DecisionTree ();
		DecisionTree c1 = new DecisionTree ();
		DecisionTree c2 = new DecisionTree ();
		DecisionTree c3 = new DecisionTree ();
		DecisionTree c4 = new DecisionTree ();

		c1.act = getAway;
		c2.act = Attack;
		c3.act = Heal;
		c4.act = Construct;

		b1.dec = healthLow;
		b1.setLeft (c1);
		b1.setRight (c2);

		b2.dec = healthLow;
		b2.setLeft (c3);
		b2.setRight (c4);

		tree.dec = heroNear;
		tree.setLeft (b1);
		tree.setRight (b2);
	}
}