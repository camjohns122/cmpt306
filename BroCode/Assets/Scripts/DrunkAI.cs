using UnityEngine;
using System.Collections;

public class DrunkAI : MonoBehaviour 
{
	public DecisionTree tree;				//Decision Tree for AI

	private Animator myAnimator;			// Animator variable that is needed.

	//Drunk Variables
	Rigidbody2D drunkBody;
	private int leftOrRight = 0;			// Input for left, right, and stationary.
	private bool isGrounded = false;		// Check if the enemy is on a platform.
	private bool jump = false;				// Jump is active.
	public float speed = 3f;				// Running speed.
	private float jumpForce = 100f;		//Big ol jump force for boss to jump to highest 
	public float acceleration = 2f;			// Acceleration.
	private float yDirection;               // used to detect when player begins falling


	//Drunk Attack Variables
	public Transform firePoint; 			// The starting point where the projectile is fired from.
	public GameObject BFire_Projectile; 			// The item that the enemy shoots.
	public bool shoot = true;
	public Transform lobPoint; 			// The starting point where the projectile is lobbed from.
	public GameObject BLob_Projectile; 			// The item that the enemy lobs.
	public bool lob = false;
	public float nextAttack = 0.0f;			//keeps track of when we can use rage attack again
	public float attackRate = 1f;

	//Rage Variables
	public float nextRage = 4.0f;			//keeps track of when we can use rage attack again
	public float rageCD = 4f;				//Time we want rage on cooldown

	public float chargeTime = 1.75f;		//time to stand still and charge before attack
	public float rageCharged = 0.0f;		//time when the rage will be charged

	public float rageSpacing = 0.5f;       //Spacing for rage attacks to fire
	public float nextRageFire = 0.0f;		//keeps track of when the next rage projectile spawned

	public float rageDone = 0.0f;
	public float rageDuration = 2f;

	public bool raged = true;
	public bool charged = true;

	//doing thing Cooldown
	public float doRate = 2f;
	public float nextDo = 0.0F;




	//Player Variables
	//----To detect Hero's Location and Distance from Drunk
	private Controller Hero;
	public LayerMask isPlayer;
	public float HeroNearDistance;
	public bool HeroNear;				// Make sure to attach the player.

	public float HeroRangeDistance;
	public bool HeroRange;				// Make sure to attach the player.



	// Use this for initialization
	void Start () {

		// Initializing the animator.
		myAnimator = GetComponent<Animator>();
		Hero = FindObjectOfType<Controller>();
		myAnimator.SetBool("falling", false);

		//Initialize the Tree and call for it to build
		tree = new DecisionTree ();
		BuildDecisionTree ();


		//Grab Hero
		Hero = FindObjectOfType<Controller>();
		//Grab RGB2D
		drunkBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		//Animation Checks
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

		if (transform.position.y - yDirection < 0)
		{
			myAnimator.SetBool("falling", true);
		}
		yDirection = transform.position.y;

		tree.Search ();

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

	//Check if Rage is off CD
	public bool rageOffCD(){
		if (Time.time > nextRage) {
			if (raged == true) {
				rageCharged = Time.time + chargeTime;
				raged = false;
			}
			Debug.Log ("rage ready");
			return true;
		} else {
			Debug.Log ("rage not ready");
			return false;
		}
	}

	//Check if Rage is done charging
	public bool rageCharging(){
		if (Time.time > rageCharged) {
			if(charged == true){
				nextRageFire = Time.time + rageSpacing;
				rageDone = Time.time + rageDuration;
				charged = false;
			}
			return true;
		} else {
			//Play charging animation
			return false;
		}
	}

	//Detect whether the Hero is within a decided radius
	public bool heroNear(){
		HeroNear = Physics2D.OverlapCircle(transform.position, HeroNearDistance, isPlayer);

		if (HeroNear) {
			return true;
		} else {
			return false;
		}

	}

	//Detect whether the Hero is within a decided radius
	public bool heroInRange(){
		HeroRange = Physics2D.OverlapCircle(transform.position, HeroRangeDistance, isPlayer);

		if (HeroRange) {
			return true;
		} else {
			return false;
		}

	}


	/*
	*
	***********************		Action Methods    ***************************************
	*
	*/

	//Rage attack that shoots and lobs two bottle alternatively
	public void Rage(){

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

		drunkBody.velocity = new Vector3 (0,0,0);

		if (Time.time > nextRageFire && shoot == true) {
			nextRageFire = Time.time + rageSpacing;
			Shoot ();
			shoot = false;
			lob = true;
		} else if (Time.time > nextRageFire && lob == true) {
			nextRageFire = Time.time + rageSpacing;
			Lob ();
			shoot = true;
			lob = false;		
		}

		if(Time.time > rageDone){
			nextRage = Time.time + rageCD;
			raged = true;
			charged = true;
		}

	}

	//Charging before let loose the rage 
	public void Charge(){
		drunkBody.velocity = new Vector3 (0,0,0);
		//Play charging animation
	}
	//Not really needing but would be used to play charging animation



	//Run at hero and throw a bottle straight forward
	public void fireAttack(){
		

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
				drunkBody.AddForce (transform.up * (jumpForce/2f));
			}
		}

		drunkBody.AddForce(new Vector2(((leftOrRight * speed) - GetComponent<Rigidbody2D>().velocity.x) * acceleration, 0));

		// This is the running animation.
		myAnimator.SetFloat("speed", Mathf.Abs(leftOrRight));

		if(Time.time > nextAttack){
			nextAttack = Time.time + attackRate;
			Shoot ();	
		}
	}


	//back away from hero and lob a bottle at him
	public void lobAttack(){

		if (Hero.transform.position.x < transform.position.x) {
			leftOrRight = -1;
			transform.localScale = new Vector3 (-1f, 1f, 1f);
		} else if (Hero.transform.position.x > transform.position.x) {
			leftOrRight = 1;
			transform.localScale = new Vector3 (1f, 1f, 1f);
		}

		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (((leftOrRight * speed) - GetComponent<Rigidbody2D> ().velocity.x) * acceleration, 0));
		// This is the running animation.
		myAnimator.SetFloat ("speed", Mathf.Abs (leftOrRight));


		if (Time.time > nextAttack) {
			nextAttack = Time.time + attackRate;
			Lob ();	
		}

	}

	//jus do nothing if hero is too far away
	public void Chill(){
		drunkBody.velocity = new Vector3 (0,0,0);	
	}


	//Secondary Action Functions
	public void Shoot(){

		var clone = Instantiate (BFire_Projectile, firePoint.position, firePoint.rotation);

	}

	public void Lob(){

		var clone = Instantiate (BLob_Projectile, lobPoint.position, lobPoint.rotation);

	}



	//BuildDecisionTree
	public void BuildDecisionTree(){

		DecisionTree a1 = new DecisionTree ();
		DecisionTree a2 = new DecisionTree ();
		DecisionTree b1 = new DecisionTree ();
		DecisionTree b2 = new DecisionTree ();
		DecisionTree c1 = new DecisionTree ();
		DecisionTree c2 = new DecisionTree ();
		DecisionTree c3 = new DecisionTree ();
		DecisionTree c4 = new DecisionTree ();

		c1.act = Rage;
		c2.act = Charge;
		c3.act = fireAttack;
		c4.act = lobAttack;

		b1.dec = rageCharging;
		b1.setLeft (c1);
		b1.setRight (c2);

		b2.dec = heroNear;
		b2.setLeft (c3);
		b2.setRight (c4);

		a1.dec = rageOffCD;
		a1.setLeft (b1);
		a1.setRight (b2);

		a2.act = Chill;

		tree.dec = heroInRange;
		tree.setLeft (a1);
		tree.setRight (a2);


	}

}

