using UnityEngine;
using System.Collections;

public class speechBubble : MonoBehaviour {

	private float horizontal;

	private float xPos;
	private float yPos;
	private float zPos;

	// Use this for initialization
	void Start () {
		xPos = gameObject.transform.localScale.x;
		yPos = gameObject.transform.localScale.y;
		zPos = gameObject.transform.localScale.z;
	}
	
	// Update is called once per frame
	void Update () {
		// Assings horizontal which is needed for animations.
		horizontal = Input.GetAxisRaw ("Horizontal");

		//checks which direction the parent Player object is facing and flips the child SpeechBubble to stay legible
		if( Input.GetAxisRaw("Horizontal") < 0 )
		{
			// Flips the sprite so it stays readable
			transform.localScale = new Vector3(-xPos, yPos, zPos);
		}
		else if( Input.GetAxisRaw("Horizontal") > 0 )
		{
			// Flips the sprite so it stays readable
			transform.localScale = new Vector3(xPos, yPos, zPos);
		}
	}
}
