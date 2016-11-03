using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	//variables to affect players confidence meter
	public float confidenceLost;
	public float confidenceGiven;
	public GameObject confidence;

	public float health;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		//if enemys health reaches 0 it is destroyed and player gains confidence
		if (health <= 0) {
			confidence.GetComponent<Confidence> ().giveConfidence (confidenceGiven);
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {

		//if enemy collides with the player decrement the players confidence meter
		if (col.gameObject.tag == "Player") {
			confidence.GetComponent<Confidence>().loseConfidence (confidenceLost);
		}

		//if the gameObject is collided with by a projectile reduce the health by 1
		if (col.gameObject.tag == "Projectile") {
			health = health - 1;
		}
	}

}
