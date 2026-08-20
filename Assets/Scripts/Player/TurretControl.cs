using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour {

	public float speed = 0.5f;

	void Update () 
	{
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

		float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
		transform.rotation =  Quaternion.Euler (new Vector3(0f, 0f, angle));
	}

	float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x)* Mathf.Rad2Deg * speed;
	}

}