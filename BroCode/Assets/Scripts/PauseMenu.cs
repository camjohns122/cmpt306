using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Specify what level to load in the Inspector
	public string levelToLoad;

	// Checks if game is paused or not
	public bool isPaused;

	public GameObject pauseMenuCanvas;
	
	// Update is called once per frame
	void Update () {

		// Canvas is set active if paused, not active otherwise
		if (isPaused) {
			pauseMenuCanvas.SetActive (true);

			// when paused, set the time to 0
			Time.timeScale = 0f;
		} 
		else {
			pauseMenuCanvas.SetActive (false);

			// when unpaused, resets the time to normal
			Time.timeScale = 1f;
		}

		// If Esc key is pressed, it will either pause or resume
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown ("Cancel")) {
			isPaused = !isPaused;
		}
	}

	// Resume playing the game
	public void Resume () {
		isPaused = false;
	}

	// Quit game, go to main menu
	public void Quit () {
		Application.LoadLevel (levelToLoad);
	}
}
