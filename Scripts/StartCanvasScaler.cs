using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartCanvasScaler : MonoBehaviour {
	RectTransform canvas;
	private void Awake () {
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (canvas.rect.width, 0);
	}
	// Use this for initialization
	void Start () { }
	// Update is called once per frame
	void Update () { }
}