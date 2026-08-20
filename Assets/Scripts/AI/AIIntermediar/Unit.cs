using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Transform target;
	public float speed = 5f;
	Vector3[] path;
	int targetIndex = 0;

	float Distance;
	public float lookAtDistance = 20f;

	private float attackTime;

	int attackRepeatTime = 1;

	public float moveSpeed = 5f;
	public float Damping = 6f;
	public GameObject BulletEmiter;
	public float Bullet_Forward_Force = 1000f;

	void Start() {
		attackTime = Time.time;
	}

	void Update () {

		Distance = Vector3.Distance (target.position, transform.position);

		if (Distance > lookAtDistance) {
			findHim ();
		} else {
			attack ();
		}
	}

	private void findHim() {
		path = null;
		targetIndex = 0;
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
	}

	private void attack() {
		StopCoroutine ("FollowPath");
		int targetIndex = 0;
		path = null;
		lookAt ();
		chase ();
		attackHim ();
	}

	private void lookAt() {
		Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * Damping);
	}

	private void chase() {
		transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

	private void attackHim() {
		if (Time.time > attackTime) {
			Debug.Log ("Attack Here");
			attackTime = Time.time + attackRepeatTime;

			GameObject obj = ObjectPoolerScript.current.GetPooledObject ();
			if (obj == null) return;
			obj.transform.position = BulletEmiter.transform.position;
			obj.transform.rotation = BulletEmiter.transform.rotation;
			obj.transform.Rotate (Vector3.left * 90);
			obj.SetActive (true);
			GameObject TemporaryBulletHandler = obj;
			Rigidbody Temporary_Rigidbody;
			Temporary_Rigidbody = TemporaryBulletHandler.GetComponent<Rigidbody> ();
			Temporary_Rigidbody.AddForce (BulletEmiter.transform.forward * Bullet_Forward_Force);

		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path [0];

		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex++;
				if (targetIndex >= path.Length) {
					path = null;
					targetIndex = 0;
					PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
					yield break;
				}
				currentWaypoint = path [targetIndex];
			}

			transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}

	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube (path [i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine (transform.position, path [i]);
				} else {
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		}
	}

}
