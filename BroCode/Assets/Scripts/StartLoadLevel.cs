using UnityEngine;
using System.Collections;

public class StartLoadLevel : MonoBehaviour {


	public string levelToLoad;

	public Transform Button;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	void OnMouseDown(){
		Application.LoadLevel (levelToLoad);
	}
}
