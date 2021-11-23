using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelResizer : MonoBehaviour {
	RectTransform canvas;
	RectTransform self;
	Vector3[] corners = new Vector3[4];
	// Use this for initialization
	void Awake () {
		self = gameObject.GetComponent<RectTransform> ();
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		self.sizeDelta = new Vector2 (canvas.rect.width, 0);
		// Update is called once per frame
	}
}