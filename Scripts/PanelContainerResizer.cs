using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelContainerResizer : MonoBehaviour {
	RectTransform canvas;
	void Awake () {
		//gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (3 * Screen.width, Screen.height);
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		gameObject.GetComponent<RectTransform> ().offsetMin = new Vector2 (-canvas.rect.width, 0);
		gameObject.GetComponent<RectTransform> ().offsetMax = new Vector2 (canvas.rect.width, 0);
	}
	// Use this for initialization
	void Start () { }
	// Update is called once per frame
	void Update () { }
}