using UnityEngine;
using System.Collections;

public class Boss2AI : MonoBehaviour
{
	public DecisionTree tree;				//Decision Tree for AI

	private Animator myAnimator;			// Animator variable that is needed.

	public GameObject confidence;			// Make sure to attach confidence.

	//Player Variables
	//----To detect Hero's Location and Distance from Boss
	private Controller Hero;
	public LayerMask isPlayer;
	public float HeroNearDistance;
	public bool HeroNear;

	// Boss Variables
	Rigidbody2D bossBody;
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = true;		// Check if the enemy is on a platform.
	private bool jump;				// Jump is active.
	public float bossHealth;				//Health
	private float pushForce = 400f;			//used simply to push boss back down off highest platform
	private float jumpForce = 1200f;		//Big ol jump force for boss to jump to highest 
	public float acceleration = 2f;	
	public float moveSpeed = 3.5f;
	private float yDirection;

    private float yDirection2;          // to determine when to switch from jump animation
    private float xDirection;           // to determine when to switch from run to idle animation





    //Shooting Variables
    public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject b2_projectile; 		// The item that the enemy shoots.

	//Enemy Fire Rate
	public float fireRate = 1f;
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

		//Initialize the Tree and call for it to build
		tree = new DecisionTree ();
		BuildDecisionTree ();

		//Grab Hero
		Hero = FindObjectOfType<Controller>();
		//Grab RGB2D
		bossBody = GetComponent<Rigidbody2D>();

		//Grab an Array of the JumpPoint
		jumpLoc = GameObject.FindGameObjectsWithTag ("JumpLoc");

		//Multiply moveSpeed by Confidence factor
		moveSpeed = moveSpeed + confidence.GetComponent<Confidence> ().getConfidence ()/50f;
		myAnimator.SetBool("falling", false);


        xDirection = transform.position.x;
        yDirection2 = transform.position.y;

        InvokeRepeating("velocityCheck", 0.0f, 0.5f);   // change xDirection and yDirection 2 times a second to get a velocity
    }


    //----The Flow of Fixed update is basically this:
    // If not constructing or Healing --> go through decision Tree
    // If constructing keep looping on the steps until they are completed, uninterrupted
    //		- once construction is completed launched back down onto the lower platforms to return to the Tree
    // Healing is less strenuous than constructing. Healing takes 2s to do
    //		- but once that second is up go back to search tree
    void FixedUpdate(){

        //Animation Checks
        if (isGrounded)
        {
            // The jump animation is set to false when on the ground.
            myAnimator.SetBool("jump", false);
        }
        else
        {
            // The jump animation is set to true when in the air.
            //myAnimator.SetBool("jump", true);
        }

        if (transform.position.y - yDirection < 0)
        {
            myAnimator.SetBool("falling", true);
        }
        yDirection = transform.position.y;

        if (Mathf.Abs(transform.position.x - xDirection) > 0.3)
        {
            myAnimator.SetFloat("speed", 1.0f);
        }
        else if (Mathf.Abs(transform.position.x - xDirection) <= 0.3)
        {
            myAnimator.SetFloat("speed", 0.0f);
        }

        if (Mathf.Abs(transform.position.y - yDirection2) <= 0.3)
        {
            myAnimator.SetBool("jump", false);
        }
        else if (Mathf.Abs(transform.position.y - yDirection2) > 0.3)
        {
            myAnimator.SetBool("jump", true);
        }

        //This can get a bit confusing so Ill breakdown above here
        /**
		*So If we want to construct a Bomber we continue to loop in this if statement and only proceed
		*to the next if the step == true (complete)
		*
		***STEP ONE
		*	-Find out which location we are building at
		*		-if bombers == 0, whichever is closer
		*		-if bombers == 1, whichever platform is not preoccupied
		*	-Go to that locations jump spot,
		*	-Once there step one is complete
		*
		***STEP TWO
		*	- JUMP(), large jumpforce to ensure height
		*	-step two complete
		*
		***STEP THREE
		*	- FlipCorrectWay() to make sure we are facing the appropriate direction to build the Bomber
		*	- initialize a time for it to take to "build"
		*	- step three complete
		*
		***STEP FOUR
		*	- once time waited/ "builded" spawn the bomber
		*	- step four complete
		*
		***STEP FIVE
		*	- Flip around
		*	- determine which side of map boss is on
		*	- push him off top platform into middle of map
		*	- reset all flags
		*	- constructing now = false
		*	- step five complete
		*/
        if (constructing == true) {
			if (step1 == false) {				//STEP ONE
				Transform Target;
				Target = GetCloserJumpLoc ();
				if (transform.position.x != Target.position.x) {
					transform.localPosition = Vector3.MoveTowards (transform.localPosition, Target.position, moveSpeed * Time.deltaTime);
                   
                    // check if need to flip for animation
                    if (transform.position.x > Target.position.x && leftOrRight == -1)
                        leftOrRight = -1;
                    else if (transform.position.x < Target.position.x && leftOrRight == 1)
                        leftOrRight = 1;

				} else {
					step1 = true;
				}

			} else if (step2 == false) {		//STEP TWO
				Jump ();
				step2 = true;

			} else if (step3 == false) {		//STEP THREE
				flipCorrectWay ();
				//start countdown for delay
				nextDo = Time.time + doRate;
				step3 = true;

			} else if (step4 == false) {		//STEP FOUR
				//Animation for construction.
				if (Time.time > nextDo) {
					//spawn Bomber
					GameObject clone = (GameObject) Instantiate (bomber, firePoint.position, firePoint.rotation);
                    
                   // clone.AddComponent.<Rigidbody>();

                    if (transform.position.x > 0)
                        clone.transform.localScale = new Vector3(-1f, 1f, 1f);
                    else
                        clone.transform.localScale = new Vector3(1f, 1f, 1f);
                    
					step4 = true;
                }

			} else if (step5 == false) {		//STEP FIVE
				leftOrRight = leftOrRight * -1;
                if (leftOrRight == -1)
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(1f, 1f, 1f);

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

	// When leaving platform, change isGrounded
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = false;
		}
	}
	//
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Platform")
		{
			isGrounded = true;
		}
	}

	/*
	*
	***********************		Decision Methods    ***************************************
	*
	*/

	//Detect whether the Hero is within a decided radius
	public bool heroNear(){
		HeroNear = Physics2D.OverlapCircle(transform.position, HeroNearDistance, isPlayer);

		if (HeroNear) {
			return true;
		} else {
			return false;
		}

	}


	//Detect if health is below a certain threshold
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
	***********************		Action Methods    ***************************************
	*
	*/

	//run away  from player, might add a follow up if cornered try to fight its way out, for balance
	public void getAway(){

		//Determine whether hero is left or right of you

		if (Hero.transform.position.x < transform.position.x)
		{
			leftOrRight = 1;
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (Hero.transform.position.x > transform.position.x)
		{
			leftOrRight = -1;
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}

//*****************************This is where the boss should run away but he's not into it 
		GetComponent<Rigidbody2D>().AddForce(new Vector2(((leftOrRight * moveSpeed) - GetComponent<Rigidbody2D>().velocity.x) * acceleration, 0));
		// This is the running animation.
		//myAnimator.SetFloat("speed", Mathf.Abs(leftOrRight));
	}

	//Run at Player 
	//Shoot projectile, might add alternate shoot and lob projectiles later
	public void Attack(){

		//Determine whether hero is left or right of you

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

		if(Hero.transform.position.y > transform.position.y){
			if(Time.time > nextDo){
				nextDo = Time.time + doRate;
				bossBody.AddForce (transform.up * (jumpForce/2f));
			}
		}
		/**
		 * this is where AddForce is not digging it, for now using transform
		*/
		GetComponent<Rigidbody2D>().AddForce(new Vector2(((leftOrRight * moveSpeed) - GetComponent<Rigidbody2D>().velocity.x) * acceleration, 0));
		//transform.localPosition = Vector3.MoveTowards (transform.localPosition, Hero.transform.position, moveSpeed * Time.deltaTime);
		// This is the running animation.
		//myAnimator.SetFloat("speed", Mathf.Abs(leftOrRight));
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
	//if not, flag constructing
	public void Construct(){

		GameObject[] arr;
		arr = GameObject.FindGameObjectsWithTag("Bomber");

		if (arr.Length < 2) {
			constructing = true;
			bossBody.velocity = new Vector3 (0,0,0);
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
        myAnimator.SetBool("jump", true);
    }

    //This is only ever called if bombers < 2
    //	-if bombers == 1
    //		-find out which side existing bomber is on and pass the opposite jump location
    //	-else (bombers = 0)
    //		-return the closest jump location
    //Finds out which jump point it is closest too.
    public Transform GetCloserJumpLoc(){

		//Grabs an array of the Bombers currently on the map
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
			//Returns the closest jump location
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

    // called twice a second to update variables to check velocity
    void velocityCheck()
    {
        xDirection = transform.position.x;
        yDirection2 = transform.position.y;
    }
}