using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierManager : MonoBehaviour {

	private Transform destination;
	public GameObject Bullet;

	public Transform BulletEmiter;
	private Transform enemy;
	private GameObject TemporaryBulletHandler;

	private Vector3 firstControlPoint;
	private Vector3 secondControlPoint;
	private Vector3 thirdControlPoint;
	private Vector3 fourthControlPoint;

	private float bulletSpeed = 10f;
	private int numberOfProjectiles = 3;
	private int fireProjectile = 0;
	private bool shootProjectile = false;
	private float Damping = 6f;

	void Awake() {
		destination = GameObject.FindGameObjectWithTag ("Player").transform;
		enemy = this.transform;
		BulletEmiter = this.transform.GetChild(0).transform;
	}

	public int getNumberOfProjectiles() {
		return numberOfProjectiles; 
	}

	void Update() {
		Quaternion rotation = Quaternion.LookRotation (destination.position - enemy.position);
		enemy.rotation = rotation;//Quaternion.Slerp (enemy.rotation, rotation, Time.deltaTime * Damping);
		if (fireProjectile == 0 && numberOfProjectiles > 0) {
			fireProjectile = 1;
			numberOfProjectiles--;
			TemporaryBulletHandler = Instantiate (Bullet, BulletEmiter.position, BulletEmiter.rotation) as GameObject;
			//TemporaryBulletHandler.transform.Rotate (Vector3.left * 90);
		}
		if (fireProjectile == 1) {
			FireRocket ();
		}
	}

	void FireRocket() {
		RaycastHit hit;
		bool foundDestination = false;

		float step = bulletSpeed * Time.deltaTime;
		Vector3 direction = destination.position - TemporaryBulletHandler.transform.position;

		firstControlPoint = TemporaryBulletHandler.transform.position;
		secondControlPoint = Vector3.MoveTowards (firstControlPoint, destination.position, step);
		thirdControlPoint = Vector3.MoveTowards (secondControlPoint, destination.position, step);
		fourthControlPoint = Vector3.MoveTowards (thirdControlPoint, destination.position, step);

		bool foundBlock = false;

		if (Physics.Raycast (firstControlPoint, direction, out hit, step)) {
			if (hit.transform.position == destination.position) {
				foundDestination = true;
				float distance = Vector3.Distance (TemporaryBulletHandler.transform.position, hit.transform.position);
				TemporaryBulletHandler.transform.position = Vector3.MoveTowards (TemporaryBulletHandler.transform.position, destination.position, distance);
				TemporaryBulletHandler.transform.rotation = Quaternion.LookRotation (direction);
				//Un Sphere cu damage
			} else {
				secondControlPoint = Vector3.MoveTowards (secondControlPoint, direction + new Vector3(0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
				thirdControlPoint = Vector3.MoveTowards (thirdControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
				fourthControlPoint = Vector3.MoveTowards (fourthControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
				foundBlock = true;
			}
		}

		if (foundDestination == false) {
			if (Physics.Raycast (secondControlPoint, direction, out hit, step)) {
				if (hit.transform.position == destination.position) {
					foundDestination = true;
					float distance = Vector3.Distance (TemporaryBulletHandler.transform.position, hit.transform.position);
					TemporaryBulletHandler.transform.position = Vector3.MoveTowards (TemporaryBulletHandler.transform.position, destination.position, distance);
					TemporaryBulletHandler.transform.rotation = Quaternion.LookRotation (direction);
				} else {
					secondControlPoint = Vector3.MoveTowards (secondControlPoint, direction + new Vector3(0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
					thirdControlPoint = Vector3.MoveTowards (thirdControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
					fourthControlPoint = Vector3.MoveTowards (fourthControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
					foundBlock = true;
				}
			}

			if (foundDestination == false) {
				if (Physics.Raycast (thirdControlPoint, direction, out hit, step)) {
					if (hit.transform.position == destination.position) {
						foundDestination = true;
						float distance = Vector3.Distance (TemporaryBulletHandler.transform.position, hit.transform.position);
						TemporaryBulletHandler.transform.position = Vector3.MoveTowards (TemporaryBulletHandler.transform.position, destination.position, distance);
						TemporaryBulletHandler.transform.rotation = Quaternion.LookRotation (direction);
					} else {
						thirdControlPoint = Vector3.MoveTowards (thirdControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
						fourthControlPoint = Vector3.MoveTowards (fourthControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
						foundBlock = true;
					}
				}
			}
		}

		if (foundDestination == true) {
			fireProjectile = 0;
		} else {
			// pregatim terenul pentru urmatorul punct
			if (foundBlock == false) {
				if (Physics.Raycast (fourthControlPoint, direction, out hit, step)) {
					if (hit.transform.position == destination.position) {
						fourthControlPoint = Vector3.MoveTowards (fourthControlPoint, direction + new Vector3 (0, 90, 0), 200.0f * hit.transform.lossyScale.y * Time.deltaTime);
					}
				}
			}
			BezierPath bezierPath = new BezierPath();
			List<Vector3> points = new List<Vector3>();
			points.Add (firstControlPoint);
			points.Add (secondControlPoint);
			points.Add (thirdControlPoint);
			points.Add (fourthControlPoint);
			bezierPath.SetControlPoints(points);
			List<Vector3> drawingPoints = bezierPath.GetDrawingPoints ();
			DrawBezierPoints(drawingPoints, step);
		}

	}

	private void DrawBezierPoints(List<Vector3> drawingPoints, float step) {
		RaycastHit hit;
		float miniStep = step / drawingPoints.Count;

		for (int i = 0; i < drawingPoints.Count; i++) {
			if (Physics.Raycast (TemporaryBulletHandler.transform.position, drawingPoints [i] - TemporaryBulletHandler.transform.position, out hit, miniStep)) {
				float distance = Vector3.Distance (TemporaryBulletHandler.transform.position, hit.transform.position);
				TemporaryBulletHandler.transform.position = Vector3.MoveTowards (TemporaryBulletHandler.transform.position, destination.position, distance);
				/*if (drawingPoints [i] - TemporaryBulletHandler.transform.position == new Vector3 (0, 0, 0)) {
					TemporaryBulletHandler.transform.rotation = Quaternion.LookRotation (drawingPoints [i] - TemporaryBulletHandler.transform.position);
				}*/
				fireProjectile = 0;
				//Un Sphere cu damage
				return;
			} else {
				TemporaryBulletHandler.transform.position = Vector3.MoveTowards (TemporaryBulletHandler.transform.position, drawingPoints[i], miniStep);
				/*if (drawingPoints [i] - TemporaryBulletHandler.transform.position == new Vector3 (0, 0, 0)) {
					TemporaryBulletHandler.transform.rotation = Quaternion.LookRotation (TemporaryBulletHandler.transform.position - drawingPoints[i]);			
				}*/
			}
		}
	}

}