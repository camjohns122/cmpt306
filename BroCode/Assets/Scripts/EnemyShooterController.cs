using UnityEngine;
using System.Collections;

public class EnemyShooterController : MonoBehaviour {

	public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject nme_projectile; 		// The item that the enemy shoots.

	//Enemy Fire Rate
	public float fireRate = 500f;
	public float nextfire = 0.0F;

	// Update is called once per frame
	void Update () 
	{
		// Made an array of GameObjects to check how many Projectiles are on the screen.

		// If the number of Projectiles on screen is < 2, then the player can fire.
		if (Time.time > nextfire) 
		{
			
				// Make a clone of the Projectile prefab
				var clone = Instantiate (nme_projectile, firePoint.position, firePoint.rotation);
				
				nextfire = Time.time + fireRate;
				// Destroy the Projectile after x seconds
				Destroy (clone, 2f);
		}
	}
}
