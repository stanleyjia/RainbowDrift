using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeaderboardsResizer : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		gameObject.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, gameObject.GetComponent<RectTransform> ().offsetMin.y);
		gameObject.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, gameObject.GetComponent<RectTransform> ().offsetMax.y);
	}
	void Start () {
		gameObject.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, gameObject.GetComponent<RectTransform> ().offsetMin.y);
		gameObject.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, gameObject.GetComponent<RectTransform> ().offsetMax.y);
	}
	// Update is called once per frame
	void Update () { }
}