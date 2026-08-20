using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour {

	public Transform player;
	public Text saveText;

	public void SaveState() {
		BinaryFormatter bf = new BinaryFormatter ();
		List<PlayerData> data = new List<PlayerData>();

		if (System.IO.File.Exists (Application.persistentDataPath + "/PlayerData.dat")) {
			FileStream _file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			data = bf.Deserialize (_file) as List<PlayerData>;
			_file.Close ();
		}

		FileStream file = File.Create (Application.persistentDataPath + "/PlayerData.dat");
		PlayerData player = new PlayerData();
		player.enemyXP = MainGameManager.getInstance ().getEnemyXP ();
		player.saveName = saveText.text + "-" +  System.DateTime.Now.Date.Day + "-" + System.DateTime.Now.Date.Month + "-" + System.DateTime.Now.Date.Year + "-" + System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes;
		player.health = MainGameManager.getInstance ().getHealth ();
		player.maximumLevel = MainMenuManager.chosenLevel;
		player.tankIndex = MainMenuManager.chosenTankIndex;

		data.Add (player);
		bf.Serialize (file, data);
		file.Close ();
	}

}