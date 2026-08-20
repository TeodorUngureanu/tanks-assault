using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class LevelManager : MonoBehaviour {

	public GameObject canvas;
	private bool canvasActive;
	public Canvas saveMenu;
	public Canvas gameOver;
	public static bool setGameOverToTrue = false;

	void Start() {
		canvas.SetActive (false);
		saveMenu.gameObject.SetActive (false);
		gameOver.gameObject.SetActive (false);
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

		if (setGameOverToTrue == true) {
			gameOver.gameObject.SetActive (true);
		} else {
			gameOver.gameObject.SetActive (false);
		}

	}

	public void SetCanvasInactive() {
		canvas.SetActive (false);
		canvasActive = false;
		Time.timeScale = 1.0f;
	}

	public void SavePopUp() {
		saveMenu.gameObject.SetActive (true);
	}

	public void HidePopUp() {
		saveMenu.gameObject.SetActive (false);	
	}

	public void ReturnToChapter0() {
		LevelManager.setGameOverToTrue = false;

		PlayerProfile oldGold = new PlayerProfile();
		BinaryFormatter bf = new BinaryFormatter ();

		if (System.IO.File.Exists (Application.persistentDataPath + "/PlayerProfile.dat")) {
			FileStream _file = File.Open (Application.persistentDataPath + "/PlayerProfile.dat", FileMode.Open);
			oldGold = bf.Deserialize (_file) as PlayerProfile;
			_file.Close ();
		}

		FileStream file = File.Create (Application.persistentDataPath + "/PlayerProfile.dat");
		PlayerProfile player = new PlayerProfile();
		player.gold = oldGold.gold + MainGameManager.getInstance ().getScore ();

		MainMenuManager.gold = player.gold;

		bf.Serialize (file, player);
		file.Close ();

		Time.timeScale = 1.0f;
		SceneManager.LoadScene (1);
	}

	public void Exit() {
		Application.Quit ();
	}
}