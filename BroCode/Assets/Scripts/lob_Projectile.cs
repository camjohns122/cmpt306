using UnityEngine;
using System.Collections;

public class lob_Projectile : MonoBehaviour {

	public float launchAngle = 70; //I think 65 would be best

	public float speed = 2f;

	//Player Variables
	private Controller Hero;

	// Make sure to attach confidence.
	public GameObject confidence;

	// The amount of confidence you lose when you get hit by a chicken.
	public float confidenceToLose;

	// Use this for initialization
	void Start () {
		//Player grabbed
		Hero = FindObjectOfType<Controller>();

		GetComponent<Rigidbody2D> ().velocity = LaunchPath (Hero, launchAngle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//velocity method, calculates the path to take to hit the player
	//Personally I think it flies too fast, and will shoot too low if player close. 

	Vector3 LaunchPath(Controller Hero, float angle){
		Vector3 direction = Hero.transform.position - this.transform.position;

		float heightDiff = direction.y;

		direction.y = 0;

		float distance = direction.magnitude;

		float a = angle * Mathf.Deg2Rad;

		direction.y = distance * Mathf.Tan (a);

		//distance += heightDiff / Mathf.Tan (a);

		float vel = Mathf.Sqrt (distance * Physics2D.gravity.magnitude / Mathf.Sin(2*a));

		return vel * direction.normalized;
	}


	// Destroy the projectile when it makes contact with something.
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the Egg hits the player, the player loses confidence.
		if (col.gameObject.tag == "Player")
		{
			confidence.GetComponent<Confidence>().loseConfidence (confidenceToLose);
			Destroy (gameObject);
		}

		// If the Egg gets hit by the player's projectile, destroys both.
		if (col.gameObject.tag == "Projectile")
		{
			Destroy (gameObject);
		}

		// If the Egg hits ground, or platform, destroys itself.
		if (col.gameObject.tag == "Platform")
		{
			Destroy (gameObject);
		}
	}
}
