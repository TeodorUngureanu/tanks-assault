using UnityEngine;
using System.Collections;

public class AIEasy : MonoBehaviour {

	float Distance;
	public Transform Target;
	public float lookAtDistance = 30f;
	public float chaseRange = 20f;
	public float attackRange = 20f;
	public float moveSpeed = 5f;
	public float Damping = 6f;
	public float gravity = 20f;
	int attackRepeatTime = 1;

	public GameObject Bullet;
	public GameObject BulletEmiter;
	public float Bullet_Forward_Force = 1000f;
	public float damage = 10f;
	private float range = 1000f;

	private float attackTime;

	void Start() {
		attackTime = Time.time;
	}

	void Update () {
		
			Distance = Vector3.Distance (Target.position, transform.position);

			if (Distance < lookAtDistance) {
				Debug.Log ("Alerta");
				lookAt ();
			}

			if (Distance > lookAtDistance) {
				Debug.Log ("Ok");
			}

			if (Distance < attackRange) {
				Debug.Log ("Ataca");
				attack ();
			}

			if (Distance < chaseRange) {
				Debug.Log ("Urmareste");
				chase ();
			}

	}

	private void lookAt() {
		Quaternion rotation = Quaternion.LookRotation (Target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * Damping);
	}

	private void chase() {
		transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

	private void attack() {
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
		
}
	
