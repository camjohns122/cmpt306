using UnityEngine;
using System.Collections;

public class ShooterController : MonoBehaviour {

	public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject projectile; 		// The item that the player shoots.

	// Update is called once per frame
	void Update () 
	{
		// Made an array of GameObjects to check how many Projectiles are on the screen.
		GameObject[] arr;
		arr = GameObject.FindGameObjectsWithTag("Projectile");

		// If the number of Projectiles on screen is < 2, then the player can fire.
		if (arr.Length < 2) 
		{
			// If the Return key is pressed, the projectile will fire.
			if (Input.GetKeyDown (KeyCode.Return)) {
				// Make a clone of the Projectile prefab
				var clone = Instantiate (projectile, firePoint.position, firePoint.rotation);

				// Destroy the Projectile after x seconds
				Destroy (clone, 0.5f);
			}
		}
	}
}
