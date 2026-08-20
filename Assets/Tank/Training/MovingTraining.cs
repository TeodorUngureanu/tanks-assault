using UnityEngine;
using System.Collections;

public class MovingTraining : MonoBehaviour {

	public float moveSpeed = 8f;
	public float turnSpeed = 45f;

	private string movementAxisName;
	private string turnAxisName;
	private float movementInputValue;
	private float turnInputValue;
	private Rigidbody rigidBody;

	private void Awake() {
		rigidBody = GetComponent<Rigidbody> ();
	}

	private void OnEnable() {
		rigidBody.isKinematic = false;
		movementInputValue = 0f;
		turnInputValue = 0f;
	}

	private void OnDisable() {
		rigidBody.isKinematic = true;
	}

	private void Start() {
		movementAxisName = "Vertical";
		turnAxisName = "Horizontal";
		rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		//rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
	}

	private void Update () {
		movementInputValue = Input.GetAxis (movementAxisName);
		turnInputValue = Input.GetAxis (turnAxisName);
	}

	void FixedUpdate () {
		Move ();
		Turn ();
	}

	private void Move() {
		Vector3 movement = transform.forward * movementInputValue * moveSpeed * Time.deltaTime;
		rigidBody.MovePosition (rigidBody.position + movement);
	}

	private void Turn(){
		float turn = turnInputValue * turnSpeed * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		rigidBody.MoveRotation (rigidBody.rotation * turnRotation);
	}
}
