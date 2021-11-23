using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadingController : MonoBehaviour {
	public static LoadingController instance;
	public Transform barContent;
	float progress = 0f;
	int calls = 0;
	int totalCallCount = 17;
	// Use this for initializa
	float nextPosition = -700;
	float speed;
	bool updating = false;
	bool once = true;
	bool set = false;
	float smoothLerp = 0;
	void Start () {
		instance = this;
		calls = 0;
		barContent.localPosition = new Vector3 (-700, 0, 0);
		StartCoroutine (StartUpdating ());
		UserData.instance.updateScene = false;
	}
	// Update is called once per frame
	void Update () {
		if (updating) {
			if (once) {
				if (Mathf.Abs (barContent.localPosition.x - nextPosition) >= 20f) {
					if (nextPosition > barContent.localPosition.x) {
						if (smoothLerp < progress) {
							smoothLerp += (Time.deltaTime * 0.5f);
						}
						barContent.localPosition = new Vector3 (Mathf.Lerp (-700, 0, smoothLerp), 0, 0);
					}
				} else {
					if (set) {
						StartCoroutine (StartUpdatingScene ());
						once = false;
					}
				}
			}
		}
	}
	IEnumerator StartUpdating () {
		yield return new WaitForSeconds (0.2f);
		updating = true;
	}
	IEnumerator StartUpdatingScene () {
		UserData.instance.updateScene = true;
		yield return null;
	}
	public void SetTo (float percent) {
		set = true;
		if (percent != progress) {
			if (percent > progress) {
				progress = percent;
				nextPosition = -700 + (progress * 700);
			}
		}
	}
	public void AddProgress () {
		calls++;
		CalculatePercentage ();
	}
	void CalculatePercentage () {
		progress = (float) calls / (float) totalCallCount;
		progress = Mathf.Clamp (progress, 0f, 1f);
		nextPosition = -700 + (progress * 700);
	}
	IEnumerator MoveBar () {
		yield return null;
		for (float i = 0; i <= 1; i += Time.deltaTime / 1) {
			transform.localPosition = new Vector3 (Mathf.Lerp (0, -700, i), 0, 0);
			yield return null;
		}
		transform.localPosition = new Vector3 (-700, 0, 0);
	}
}