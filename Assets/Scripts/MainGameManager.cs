using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour {

	private int _currentScore;
	private int _currentXP;
	public static int _currentHealth;
	public static float _currentSpeed;
	public static int _maximumLevel = 0;
	private static MainGameManager instance;
	private string recentSave;

	Tank _currentTank;

	public Text goldText;
	public Text enemyXP;
	public Text speed;
	public Text health;

	private MainGameManager() {}

	void Awake() {
		instance = this;
		_currentScore = 0;

		switch (MainMenuManager.choice) {
			case MainMenuManager.StartGameChoice.ContinueGame:
			{
				startFromContinue ();
				break;
			}
			case MainMenuManager.StartGameChoice.NewGame:
			{
				startFromNewGame ();
				break;
			}
			case MainMenuManager.StartGameChoice.LoadGame:
			{
				startFromLoad ();
				break;
			}
		}

	}
		
	public Tank returnTank(List<Tank> tanks, int tankIndex) {
		for (int i = 0; i < tanks.Count; i++) {
			if (tanks [i].index == tankIndex) {
				return tanks [i];
			}
		}
		return null;
	}

	private void startFromContinue() {
		if (File.Exists (Application.persistentDataPath + "/PlayerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			List<PlayerData> data = bf.Deserialize (file) as List<PlayerData>;
			file.Close ();

			_currentXP = data [data.Count - 1].enemyXP;
			_currentHealth = data [data.Count - 1].health;
			_currentTank = returnTank(MainMenuManager.tanks, data [data.Count - 1].tankIndex);
			_currentSpeed = _currentTank.speed;
			_maximumLevel = data [data.Count - 1].maximumLevel;
		} else {
			startFromNewGame ();
		}
	}

	private void startFromNewGame() {
		_currentTank = returnTank (MainMenuManager.tanks, MainMenuManager.chosenTankIndex);
		_currentXP = 0;
		switch (MainMenuManager.chosenTankIndex) {
			case 0:
			{
				_currentHealth = 100;
				break;
			}
			case 1:
			{
				_currentHealth = 150;
				break;
			}
			case 2:
			{
				_currentHealth = 200;
				break;
			}
		}
		_currentSpeed = _currentTank.speed;
		_maximumLevel = MainMenuManager.chosenLevel;
	}

	private void startFromLoad() {
		recentSave = MainMenuManager.recentSave;

		bool found = false;
		if (File.Exists (Application.persistentDataPath + "/PlayerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			List<PlayerData> data = bf.Deserialize (file) as List<PlayerData>;
			file.Close ();

			for (int i = 0; i < data.Count; i++) {
				if (data [i].saveName == recentSave) {
					
					_currentXP = data[i].enemyXP;
					_currentHealth = data[i].health;
					_currentTank = returnTank(MainMenuManager.tanks, data[i].tankIndex);
					_currentSpeed = _currentTank.speed;
					_maximumLevel = data[i].maximumLevel;

					found = true;
					break;
				}
			}
		}

		if (found == false) {
			startFromNewGame ();
		}
	}

	void Update() {
		goldText.text = "Gold " + _currentScore;
		enemyXP.text = "EnemyXP " + _currentXP;
		speed.text = "SPEED" + '\n' + _currentSpeed;
		health.text = " Health " + _currentHealth;
	}

	public static MainGameManager getInstance() {
		return instance;
	}

	public void AdjustScore(int amount) {
		_currentScore = _currentScore + amount;
	}

	public void AdjustEnemyXP(int amount) {
		_currentXP = _currentXP + amount;
	}

	public void AdjustHealth(int amount) {
		_currentHealth -= amount;
	}

	public int getScore() {
		return _currentScore;
	}

	public void setScore(int score) {
		_currentScore = score;
	}

	public int getEnemyXP() {
		return _currentXP;
	}

	public void setEnemyXP(int enemyXP) {
		_currentXP = enemyXP;
	}

	public int getHealth() {
		return _currentHealth;
	}

	public void setHealth (int health) {
		_currentHealth = health;
	}

}