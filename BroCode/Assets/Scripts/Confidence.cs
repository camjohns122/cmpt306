using UnityEngine;
using System.Collections;

// This script is used to manage the player's health
public class Confidence : MonoBehaviour
{
	// The player's confidence.
	public static float confidence;

	// Update is called once per frame
	void Update ()
	{
		// The maximum confidence is 100.
		if (confidence >= 100f)
		{
			confidence = 100f;
		}	

		// The minimum confidence is 0.
		if (confidence <= 0)
		{
			confidence = 0f;
		}	
	}

	// When the player does something good, add the appropriate amount of confidence.
	public void giveConfidence(float confidenceToGive)
	{
		confidence = confidence + confidenceToGive;
	}

	// When the player does something bad, subtract the appropriate amount of confidence.
	public void loseConfidence(float confidenceToLose)
	{
		confidence = confidence - confidenceToLose;
	}

	// A function used to get the player's confidence outside of this script.
	public float getConfidence()
	{
		return confidence;
	}
}
