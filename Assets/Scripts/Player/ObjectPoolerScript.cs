using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerScript : MonoBehaviour {

	public static ObjectPoolerScript current;
	public GameObject pooledObject;
	public int pooledAmount = 5;
	public bool willGrow = true;

	public List<GameObject> pooledObjects;

	void Awake() {
		current = this;
	}

	void Start() {
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = Instantiate (pooledObject) as GameObject;
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}

	public GameObject GetPooledObject() {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		if (willGrow) {
			GameObject obj = Instantiate (pooledObject) as GameObject;
			pooledObjects.Add (obj);
			return obj;
		}

		return null;
	}

}
