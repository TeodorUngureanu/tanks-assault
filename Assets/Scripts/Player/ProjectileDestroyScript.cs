using System.Collections;
using UnityEngine;

public class ProjectileDestroyScript : MonoBehaviour {

		void OnEnable() {
			Invoke ("Destroy", 10f);
		}

		void Destroy() {
			gameObject.SetActive (false);
		}

		void OnDisable() {
			CancelInvoke ();	
		}

}