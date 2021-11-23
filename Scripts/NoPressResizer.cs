using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NoPressResizer : MonoBehaviour {
	RectTransform canvas;
	// Use this for initialization
	RectTransform self;
	public GameObject parent;
	void Start () {
		self = gameObject.GetComponent<RectTransform> ();
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		// Update is called once per frame
		transform.position = parent.transform.position;
		self.sizeDelta = new Vector2 (canvas.rect.width * 1.5f, canvas.rect.height * 1.5f);
	}
}