using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoreCanvasScaler : MonoBehaviour {
	RectTransform canvas;
	// Use this for initialization
	void Awake () {
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (canvas.rect.width, 0);
		gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-canvas.rect.width / 2f, 0);
	}
	// Update is called once per frame
	void Update () { }
}