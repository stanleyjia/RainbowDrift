using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDestruction : MonoBehaviour {

	public GameObject trackDestructionPoint;

	 
	void Start () {
		trackDestructionPoint = GameObject.Find ("trackDestroyerPoint");
	}
	
	
	void Update () {

		if (transform.position.y < trackDestructionPoint.transform.position.y) {
			Destroy (gameObject);
		}
		
	}
}
