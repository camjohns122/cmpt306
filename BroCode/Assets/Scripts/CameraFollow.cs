using UnityEngine;
using System.Collections;

// This script is to handle the simple movement of the camera.
// It follows the character horizontally. NOT vertically.
public class CameraFollow : MonoBehaviour
{
	// Make sure to attach the player to the camera.
	public GameObject player;

	void Start ()
	{
		// The position of the camera.
		// Follows the player's x-axis.
		// Always y-axis of 0.
		// Always z-axis of -10.
		transform.position = new Vector3(player.transform.position.x, 0f, -10f);
	}

	void Update()
	{
		// The position of the camera.
		// Follows the player's x-axis.
		// Always y-axis of 0.
		// Always z-axis of -10.
		transform.position = new Vector3(player.transform.position.x, 0f, -10f);
	}
}
