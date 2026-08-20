using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GoldManager : MonoBehaviour {

	private float playerGold;

	void Update () {
		playerGold = MainMenuManager.gold;

		if (this.GetComponent<Text> () != null) {
			this.GetComponent<Text>().text = playerGold + "";
		}
	}

}