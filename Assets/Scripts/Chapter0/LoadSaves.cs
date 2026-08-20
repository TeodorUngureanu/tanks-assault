using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;

public class LoadSaves : MonoBehaviour {

	public GameObject button;

	void Awake () {
		if (File.Exists (Application.persistentDataPath + "/PlayerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			List<PlayerData> data = bf.Deserialize (file) as List<PlayerData>;
			file.Close ();

			for (int i = 0; i < data.Count; i++) {
				GameObject _button = Instantiate (button, gameObject.transform);
				_button.GetComponentInChildren<Text>().text = data[i].saveName;
			}
		}
	}
		
}
