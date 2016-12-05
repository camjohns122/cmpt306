using UnityEngine;
using System.Collections;

public class TutorialChicken : MonoBehaviour {

	// Type in the inspector which level you want to load.
	public string levelToLoad;

	public bool loadIt = false;

	// Update is called once per frame
	void Update ()
	{
		if(loadIt){
			Application.LoadLevel (levelToLoad);
		}
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		
		if (col.gameObject.tag == "Projectile")
		{
			loadIt = true;
		}
	}

}
