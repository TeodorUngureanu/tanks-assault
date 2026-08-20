using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile {
	public string username;
	public float gold;
	public int chosenLevel;
	public List<Tank> tanks;
	public int chosenTankIndex;
}

[System.Serializable]
public class Tank {
	public int health;
	public float speed;
	public float price;
	public int index;

	public Tank (int _health, float _speed, float _price, int _index) {
		health = _health;
		speed = _speed;
		price = _price;
		index = _index;
	}
}