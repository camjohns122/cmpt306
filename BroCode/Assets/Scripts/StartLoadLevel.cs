﻿using UnityEngine;
using System.Collections;

public class StartLoadLevel : MonoBehaviour {


	public string levelToLoad;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			Application.LoadLevel (levelToLoad);
		}
	}
		
}
