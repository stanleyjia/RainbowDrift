using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour {

	public GameObject theTrack;
	public Transform generationPoint;


	private float trackLength;

	 
	void Start () {
		trackLength = theTrack.GetComponent<BoxCollider2D> ().size.y * 0.6f;
	}
	
	
	void Update () {
		if (transform.position.y < generationPoint.position.y) {
			transform.position = new Vector3(transform.position.x, transform.position.y + trackLength, transform.position.z);

			Instantiate (theTrack, transform.position, transform.rotation);
		}
		
	}
}
