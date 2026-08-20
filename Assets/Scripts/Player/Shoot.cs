using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour {

	public float Bullet_Forward_Force = 1000f;

	public Canvas escapeMenu;

	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			if (!escapeMenu.isActiveAndEnabled) {
				GameObject obj = ObjectPoolerScript.current.GetPooledObject ();
				if (obj == null) return;
				obj.transform.position = transform.position;
				obj.transform.rotation = transform.rotation;
				//obj.transform.Rotate (Vector3.left * 90);
				obj.SetActive (true);
				GameObject TemporaryBulletHandler = obj;
				Rigidbody Temporary_Rigidbody;
				Temporary_Rigidbody = TemporaryBulletHandler.GetComponent<Rigidbody> ();
				Temporary_Rigidbody.AddForce (transform.forward * Bullet_Forward_Force);
			}

		}

	}
}