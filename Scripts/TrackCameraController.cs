using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TrackCameraController : MonoBehaviour {
	GameObject car;
	public Pathmaker pathmaker;
	List<Vector3> points = new List<Vector3> ();
	float pointDistance;
	int index;
	bool firstTime = true;
	Vector3 trackDirection;
	int frames;
	int closePointIndex;
	int cameraIndex;
	Vector3 vertical = new Vector3 (0, 0, -11);
	Vector3 offset = new Vector3 (0, 0, -12);
	Quaternion rotR = Quaternion.AngleAxis (65, Vector3.right);
	float distanceToCar;
	Vector3 lastPosition;
	public float height = 11;
	public float angle = 40;
	float pointsZ = 0;
	Vector3 positionNoZ;
	Quaternion rotU;
	Quaternion rotS;
	// Use this for initialization
	void Start () {
		car = GameObject.FindGameObjectWithTag ("Player");
		vertical = new Vector3 (0, 0, -height);
		rotR = Quaternion.AngleAxis (angle, Vector3.right);
		//	transform.rotation = Quaternion.LookRotation (car.transform.position - transform.position, Vector3.back) * rotR;
		transform.position = car.transform.position + new Vector3 (-10f * car.transform.up.x, -10f * car.transform.up.y, -height);
		transform.rotation = Quaternion.LookRotation (car.transform.position - transform.position, Vector3.back) * Quaternion.AngleAxis (-4, Vector3.right);
	}
	// Update is called once per frame
	/*void Update () {
		points = pathmaker.totalPoints;
	}*/
	private void FixedUpdate () {
		points = pathmaker.totalPoints;
		if (points.Count > 1) {
			if (car.transform.position.z <= -0.15f) {
				UpdateDirections ();
				rotU = Quaternion.LookRotation (trackDirection, Vector3.back) * rotR;
				transform.position = Vector3.Lerp (transform.position, points[cameraIndex] + vertical, 5 * Time.deltaTime);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotU, 2f * Time.deltaTime);
			} else {
				//Game over
				if (firstTime) {
					firstTime = false;
					rotU = Quaternion.LookRotation (car.transform.position - transform.position, Vector3.back) * Quaternion.AngleAxis (25, Vector3.right);
				}
				transform.position = Vector3.Lerp (transform.position, car.transform.position + offset, Time.deltaTime * 10);
				//	rotU = Quaternion.LookRotation (car.transform.position - transform.position, Vector3.back) * rotR;
				transform.rotation = Quaternion.Slerp (transform.rotation, rotU, 10 * Time.deltaTime);
			}
		}
	}
	int FindClosestPoint (List<Vector3> pos, Vector3 po) {
		pointDistance = 30f;
		index = 0;
		for (int i = 0; i < pos.Count - 1; i++) {
			if (Vector3.Distance (pos[i], po) < pointDistance) {
				pointDistance = Vector3.Distance (pos[i], po);
				index = i;
			}
		}
		return (index);
	}
	void UpdateDirections () {
		distanceToCar = 0;
		positionNoZ = new Vector3 (car.transform.position.x, car.transform.position.y, pointsZ);
		closePointIndex = FindClosestPoint (points, positionNoZ);
		//		print (closePointIndex);
		lastPosition = points[closePointIndex];
		for (int i = closePointIndex; i > 0; i--) {
			distanceToCar += Vector3.Distance (points[i], lastPosition);
			lastPosition = points[i];
			if (distanceToCar > 6.5f) {
				cameraIndex = i;
				break;
			}
		}
		if (points.Count > closePointIndex + 30) {
			trackDirection = Vector3.Normalize (points[closePointIndex + 30] - points[cameraIndex]);
		} else {
			trackDirection = Vector3.Normalize (points[closePointIndex] - points[cameraIndex]);
		}
	}
}