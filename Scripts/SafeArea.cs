using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SafeArea : MonoBehaviour {
	RectTransform rect;
	//float yOffset;
	// Use this for initialization
	void Awake () {
		rect = GetComponent<RectTransform> ();
		//print ("Safe " + Screen.safeArea.height);
		//print ("Screen " + Screen.height);
		if (Screen.safeArea.height < Screen.height) {
			rect.offsetMax = new Vector2 (rect.offsetMax.x, -(Screen.height - Screen.safeArea.height) / 2f);
		}
	}
	// Update is called once per frame
	void Update () { }
}