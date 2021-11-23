using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DriftAnimController : MonoBehaviour {
	public Skidmarks skidmarks;
	Transform rightWheel;
	Transform leftWheel;
	int leftSkid;
	int rightSkid;
	float skidOpacity = 0.95f;
	public GameObject car;
	// Use this for initialization
	void Start () {
		rightWheel = car.transform.Find ("RightWheel");
		leftWheel = car.transform.Find ("LeftWheel");
		StartCoroutine (Skid ());
	}
	// Update is called once per frame
	void Update () { }
	IEnumerator Skid () {
		yield return null;
		yield return null;
		for (float i = 0; i < 1f; i += Time.deltaTime / 5f) {
			leftSkid = skidmarks.AddSkidMark (leftWheel.position, Vector3.up, skidOpacity, leftSkid, 1f);
			yield return null;
			rightSkid = skidmarks.AddSkidMark (rightWheel.position, Vector3.up, skidOpacity, rightSkid, 1f);
			yield return null;
		}
	}
}