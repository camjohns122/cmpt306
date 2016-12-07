using UnityEngine;
using System.Collections;

public class EggBomber : MonoBehaviour {


	//Player Variables
	private Controller Hero;

	public LayerMask isPlayer;
	public float HeroDistance;
	public bool HeroNear;

	//Shooting Variables
	public Transform launchPoint; 		// The starting point where the projectile is fired from.
	public GameObject lob_projectile; 		// The item that the enemy shoots.

	public float fireRate = 7f;
	public float nextfire;
    private Animator myAnimator;


	// Use this for initialization
	void Start () {
		//Player grabbed
		Hero = FindObjectOfType<Controller>();

        // Initializing the animator.
        myAnimator = GetComponent<Animator>();

		nextfire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        myAnimator.SetBool("throw", false);

		//check if Hero is in radius
		HeroNear = Physics2D.OverlapCircle(transform.position, HeroDistance, isPlayer);

		//if hero is near, chase else patrol
		if(HeroNear && Time.time > nextfire){
			nextfire = Time.time + fireRate;
			Shoot();
            myAnimator.SetBool("throw", true);

		}



		//Add in Idle Animation Here
		//Idle();
	}


	public void Shoot(){

		var clone = Instantiate (lob_projectile, launchPoint.position, launchPoint.rotation);

	}
}
