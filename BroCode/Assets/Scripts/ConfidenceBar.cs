using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This script is used to show a visual representation of the confidence for the player.
public class ConfidenceBar : MonoBehaviour
{
	// Make sure to attach the slider.
	public Slider slider;

	// Make sure to attach the confidence.
	public GameObject confidence;

	// At the start of the level, set the health to full.
	void Start ()
	{
		slider.value = 1f;
	}

	void Update ()
	{
		// As long as the confidence exists, divide the player's confidence by their total confidence to properly display it.
		if (confidence != null)
		{
			slider.value = confidence.GetComponent<Confidence> ().getConfidence () / 100f;
		}
	}
}
