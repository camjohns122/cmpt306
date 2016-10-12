using UnityEngine;
using System.Collections;

public class ShooterController : MonoBehaviour {

	public Transform firePoint; 		// The starting point where the projectile is fired from.
	public GameObject projectile; 		// The item that the player shoots.

	// Update is called once per frame
	void Update () 
	{
		// If the Return key is pressed, the projectile will fire in the same direction as the Player.
		if (Input.GetKeyDown(KeyCode.Return)) 
		{
			Instantiate (projectile, firePoint.position, firePoint.rotation);
		}
	}
}
