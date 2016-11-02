using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This script is used to show a visual representation of the health for the player.
public class PlayerHealthBar : MonoBehaviour
{
	// Make sure to attach the slider.
	public Slider slider;

	// Make sure to attach the player.
	public GameObject player;

	// At the start of the level, set the health to full.
	void Start ()
	{
		slider.value = 1f;
	}

	void Update ()
	{
		// As long as the player exists, divide the player's health by their total health to properly display it.
		if (player != null)
		{
			slider.value = player.GetComponent<PlayerHealth> ().getHealth () / 100f;
		}
	}
}
