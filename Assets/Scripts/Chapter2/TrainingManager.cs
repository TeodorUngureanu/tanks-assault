using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour {

	public GameObject canvas;
	private bool canvasActive;

	void Start() {
		canvas.SetActive (false);
		canvasActive = false;
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (canvasActive == false) {
				Time.timeScale = 0.0f;
				canvas.SetActive (true);
				canvasActive = true;
			} else {
				canvas.SetActive (false);
				Time.timeScale = 1.0f;
				canvasActive = false;
			}
		}

	}

	public void SetCanvasInactive() {
		canvas.SetActive (false);
		canvasActive = false;
		Time.timeScale = 1.0f;
	}

	public void ReturnToChapter0() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (1);
	}
}
