using UnityEngine;
using System.Collections;

public class BigChicken : MonoBehaviour {

	//Patrol Variables
	private Vector3 PointA;

	private Vector3 PointB;

	[SerializeField]
	private Transform Point1;

	[SerializeField]
	private Transform Point2;

	private Vector3 next;

	[SerializeField]
	private float moveSpeed;



	//Player Variables
	private Controller Hero;

	public LayerMask isPlayer;
	public float HeroDistance;
	public bool HeroNear;


	//Shooting Variables
	public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject nme_projectile; 		// The item that the enemy shoots.

	//Enemy Fire Rate
	public float fireRate = 4f;
	public float nextfire = 0.0F;

	// Use this for initialization
	void Start () {
		//Patrol Points initialized
		PointA = Point1.localPosition;
		PointB = Point2.localPosition;
		next = PointB;

		//Player grabbed
		Hero = FindObjectOfType<Controller>();
	}

	// Update is called once per frame
	void Update () {

		//check if Hero is in radius
		HeroNear = Physics2D.OverlapCircle(transform.position, HeroDistance, isPlayer);

		//if hero is near, chase else patrol
		if(HeroNear && Time.time > nextfire){
			nextfire = Time.time + fireRate;
			Shoot();
		}

			Patrol();


	}

	//Move Enemy back and forth between two points
	private void Patrol()
	{
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, next, moveSpeed * Time.deltaTime);

		if (Vector3.Distance(transform.localPosition, next) <= 0.1)
		{
			ChangeDestination();
		}
	}

	public void Shoot(){

			var clone = Instantiate (nme_projectile, firePoint.position, firePoint.rotation);
			
			// Destroy the Projectile after x seconds
			Destroy (clone, 2f);

	
	}

	//Cycle through patrol points
	private void ChangeDestination()
	{
		next = next != PointA ? PointA : PointB;
	}

}
