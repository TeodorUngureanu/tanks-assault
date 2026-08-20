using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ChapterMinus1Manager : MonoBehaviour {

	public Text inputFieldUsername;
	private string username;
	private float defaultPlayerGold = 0f;
	private int defaultPlayerLevel = 0;
	private Tank defaultTank = new Tank(100, 50f, 5000f, 0);
	private int defaultTankIndex = 0;

	void Awake() {
		if (System.IO.File.Exists (Application.persistentDataPath + "/PlayerProfile.dat")) {
			GoToChapter0 ();
		} else {
			inputFieldUsername = inputFieldUsername.GetComponent<Text> ();
		}
	}

	void Start() {
		username = getInformationFromInputField ();
		initializeUser (username);
	}

	public void GoToChapter0 () {
		username = getInformationFromInputField ();
		initializeUser (username);
		SceneManager.LoadScene (1);
	}

	public string getInformationFromInputField() {
		return inputFieldUsername.text;
	}

	public void initializeUser(string username) {
		createFileForUser (username);
	}

	public void createFileForUser(string username) {
		BinaryFormatter bf = new BinaryFormatter ();

		PlayerProfile data = new PlayerProfile ();
		data.gold = defaultPlayerGold;
		data.username = username;
		data.chosenLevel = defaultPlayerLevel;
		List<Tank> _tanks = new List<Tank> ();
		_tanks.Add (defaultTank);
		data.tanks = _tanks;
		data.chosenTankIndex = defaultTankIndex;

		FileStream file = File.Create (Application.persistentDataPath + "/PlayerProfile.dat");
		bf.Serialize (file, data);
		file.Close ();
	}

}