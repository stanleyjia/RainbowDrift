using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetRenderTexture : MonoBehaviour {
	Camera cam;
	RawImage image;
	RenderTexture CarDrift1024;
	RenderTexture CarDrift512;
	// Use this for initialization
	void Start () {
		//print (gameObject.name);
		try {
			cam = GetComponent<Camera> ();
		} catch {
			print ("Camera not found");
		}
		try {
			image = GameObject.FindGameObjectWithTag ("DriftRender").GetComponent<RawImage> ();
		} catch {
			print ("Image not found");
		}
		try {
			CarDrift1024 = Resources.Load ("RenderTextures/CarDrift1024") as RenderTexture;
		} catch {
			CarDrift1024 = new RenderTexture (1024, 1024, 24);
		}
		try {
			CarDrift512 = Resources.Load ("RenderTextures/CarDrift512") as RenderTexture;
		} catch {
			CarDrift1024 = new RenderTexture (512, 512, 24);
		}
		if (Screen.width < 1024) {
			cam.targetTexture = CarDrift512;
			image.texture = CarDrift512;
		} else {
			cam.targetTexture = CarDrift1024;
			image.texture = CarDrift1024;
		}
	}
	// Update is called once per frame
	void Update () { }
}