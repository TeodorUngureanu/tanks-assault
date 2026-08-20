using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public Canvas mainMenu;
	public Canvas loadMenu;
	public Canvas tanksMenu;
	public Canvas optionsMenu;
	public Button returnToMainMenu;
	public GameObject areYouSure;
	public Text usernameToAdd;

	public Text levelDropDownLabel;

	public static string username = "";
	public static float gold = 0f;
	public static int chosenLevel = 0;
	public static List<Tank> tanks = new List<Tank>();
	public static int chosenTankIndex = 0;

	public enum StartGameChoice {
		ContinueGame,
		NewGame,
		LoadGame
	};
	public static StartGameChoice choice = StartGameChoice.NewGame;
	public static string recentSave = "";

	void Awake() {
		if (tanks.Count == 0) {
			OpenFile ();
			CheckUsername ();
		}

		mainMenu = mainMenu.GetComponent<Canvas> ();
		loadMenu = loadMenu.GetComponent<Canvas> ();
		optionsMenu = optionsMenu.GetComponent<Canvas> ();
		tanksMenu = tanksMenu.GetComponent<Canvas> ();
		returnToMainMenu = returnToMainMenu.GetComponent<Button> ();
		usernameToAdd = usernameToAdd.GetComponent<Text> ();
	}

	public void OpenFile() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/PlayerProfile.dat", FileMode.Open);
		PlayerProfile data = bf.Deserialize (file) as PlayerProfile;
		file.Close ();

		username = data.username;
		gold = data.gold;
		chosenLevel = data.chosenLevel;
		tanks = data.tanks;
		chosenTankIndex = data.chosenTankIndex;
	}

	public void CheckUsername() {
		AbstractUser user = UserFactory.getUser (username);
		username = user.getName();
	}

	void Start () {
		mainMenu.gameObject.SetActive (true);
		loadMenu.gameObject.SetActive (false);
		optionsMenu.gameObject.SetActive (false);
		tanksMenu.gameObject.SetActive (false);
		returnToMainMenu.gameObject.SetActive (false);
		areYouSure.SetActive (false);

		usernameToAdd.text = username;
	}

	void Update() {
		usernameToAdd.text = username;
	}

	public void ContinueGame() {
		choice = StartGameChoice.ContinueGame;
		GoToChapter1 ();
	}

	public void NewGame() {
		choice = StartGameChoice.NewGame;
		GoToChapter1 ();
	}
		
	public void LoadMenu() {
		mainMenu.gameObject.SetActive (false);
		loadMenu.gameObject.SetActive (true);
		optionsMenu.gameObject.SetActive (false);
		tanksMenu.gameObject.SetActive (false);
		returnToMainMenu.gameObject.SetActive (true);
	}

	public void OptionsMenu() {
		mainMenu.gameObject.SetActive (false);
		loadMenu.gameObject.SetActive (false);
		optionsMenu.gameObject.SetActive (true);
		tanksMenu.gameObject.SetActive (false);
		returnToMainMenu.gameObject.SetActive (true);
	}

	public void TanksMenu() {
		mainMenu.gameObject.SetActive (false);
		loadMenu.gameObject.SetActive (false);
		optionsMenu.gameObject.SetActive (false);
		tanksMenu.gameObject.SetActive (true);
		returnToMainMenu.gameObject.SetActive (true);
	}

	public void Training() {
		GoToTraining ();
	}

	public void ReturnToMainMenu() {
		if (optionsMenu.isActiveAndEnabled) {
			ChangeLevel (levelDropDownLabel.text);
		}
		if (tanksMenu.isActiveAndEnabled) {
			UpdateTanksInFile ();
		}

		mainMenu.gameObject.SetActive (true);
		loadMenu.gameObject.SetActive (false);
		optionsMenu.gameObject.SetActive (false);
		tanksMenu.gameObject.SetActive (false);
		returnToMainMenu.gameObject.SetActive (false);
	}

	public void ChangeLevel(string newLevel) {
		switch(newLevel) {
			case "Easy": {
					chosenLevel = 0;
					break;
				}
			case "Intermediate": {
					chosenLevel = 1;
					break;
				}
			case "Advanced": {
					chosenLevel = 2;
					break;
				}
			}
	}

	public void ExitGame() {
		Application.Quit ();
	}

	public void LoadAreYouSure(Button pressedButton) {
		areYouSure.SetActive (true);
		recentSave = pressedButton.gameObject.GetComponentInChildren<Text> ().text;
	}

	public void LoadAreYouSureYes() {
		choice = StartGameChoice.LoadGame;
		GoToChapter1 ();
	}

	public void LoadAreYouSureNo() {
		areYouSure.SetActive (false);
	}

	public void GoToChapter1() {
		UpdateTanksInFile ();
		SceneManager.LoadScene (2);
	}

	public void GoToTraining() {
		UpdateTanksInFile ();
		SceneManager.LoadScene (3);
	}

	public void EquipDefault() {
		chosenTankIndex = 0;
	}

	public void EquipPack1() {
		chosenTankIndex = 1;
	}

	public void EquipPack2() {
		chosenTankIndex = 2;
	}

	public void BuyPack1() {
		gold -= TankManager.goldPack1;
		Tank tank1 = new Tank (TankManager.healthPack1, TankManager.speedPack1, TankManager.goldPack1, 1);
		tanks.Add (tank1);
	}

	public void BuyPack2() {
		gold -= TankManager.goldPack2;
		Tank tank2 = new Tank (TankManager.healthPack2, TankManager.speedPack2, TankManager.goldPack2, 2);
		tanks.Add (tank2);
	}

	void UpdateTanksInFile() {
		BinaryFormatter bf = new BinaryFormatter ();

		PlayerProfile data = new PlayerProfile ();
		data.gold = gold;
		data.username = username;
		data.chosenLevel = chosenLevel;
		data.tanks = tanks;
		data.chosenTankIndex = chosenTankIndex;

		FileStream file = File.Create (Application.persistentDataPath + "/PlayerProfile.dat");
		bf.Serialize (file, data);
		file.Close ();
	}
}