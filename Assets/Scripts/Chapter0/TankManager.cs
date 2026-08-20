using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankManager : MonoBehaviour {

	private Button equipDefault;
	private Button buyDefault;
	public static int healthDefault = 100;
	public static float speedDefault = 50f;
	public static float goldDefault = 5000f;
	public static int indexDefault = 0;

	private Button equipPack1;
	private Button buyPack1;
	public static int healthPack1 = 150;
	public static float speedPack1 = 80f;
	public static float goldPack1 = 100f;
	public static int indexPack1 = 1;

	private Button equipPack2;
	private Button buyPack2;
	public static int healthPack2 = 200;
	public static float speedPack2 = 100f;
	public static float goldPack2 = 500f;
	public static int indexPack2 = 2;

	void Awake () {
		GameObject tankDefault = GameObject.FindGameObjectWithTag("TankDefault");
		equipDefault = tankDefault.GetComponentsInChildren<Button> () [1];
		buyDefault = tankDefault.GetComponentsInChildren<Button> () [2];

		GameObject tankPack1 = GameObject.FindGameObjectWithTag("TankPack1");
		equipPack1 = tankPack1.GetComponentsInChildren<Button> () [1];
		buyPack1 = tankPack1.GetComponentsInChildren<Button> () [2];

		GameObject tankPack2 = GameObject.FindGameObjectWithTag("TankPack2");
		equipPack2 = tankPack2.GetComponentsInChildren<Button> () [1];
		buyPack2 = tankPack2.GetComponentsInChildren<Button> () [2];
	
		buyDefault.interactable = false;
		equipDefault.interactable = false;
		buyPack1.interactable = false;
		equipPack1.interactable = false;
		buyPack2.interactable = false;
		equipPack2.interactable = false;
	}

	void Start () {
		EnableEquipButtons ();
		EnableBuyButtons ();
	}

	public void EnableEquipButtons() {
		equipDefault.interactable = true;
		if (MainMenuManager.tanks.Count > 2) {
			equipPack1.interactable = true;
			equipPack2.interactable = true;
		} else if (MainMenuManager.tanks.Count > 1) {
			if (MainMenuManager.tanks[1].speed == speedPack1){ equipPack1.interactable = true; }
			if (MainMenuManager.tanks[1].speed == speedPack2){ equipPack2.interactable = true; }
		}
	}

	public void EnableBuyButtons() {
		buyDefault.interactable = false;

		if (MainMenuManager.tanks.Count > 2) {
			buyPack1.interactable = false;
			buyPack2.interactable = false;
		} else if (MainMenuManager.tanks.Count > 1) {
			if (MainMenuManager.tanks[1].speed == speedPack1){
				buyPack1.interactable = false;
				if (MainMenuManager.gold >= goldPack2) { buyPack2.interactable = true; }
			}
			if (MainMenuManager.tanks[1].speed == speedPack2) {
				buyPack2.interactable = false;
				if (MainMenuManager.gold >= goldPack1) { buyPack1.interactable = true; }
			}
		} else {
			if (MainMenuManager.gold >= goldPack1) { buyPack1.interactable = true; }
			if (MainMenuManager.gold >= goldPack2) { buyPack2.interactable = true; }
		}
	}

	void Update() {
		EnableEquipButtons ();
		EnableBuyButtons ();

		if (MainMenuManager.gold < goldPack1 && buyPack1.interactable == true) {
			buyPack1.interactable = false;
		}

		if (MainMenuManager.gold >= goldPack1 && buyPack1.interactable == false) {
			buyPack1.interactable = true;
		}

		if (MainMenuManager.gold < goldPack2 && buyPack2.interactable == true) {
			buyPack2.interactable = false;
		}

		if (MainMenuManager.gold >= goldPack2 && buyPack2.interactable == false) {
			buyPack2.interactable = true;
		}
	}

}