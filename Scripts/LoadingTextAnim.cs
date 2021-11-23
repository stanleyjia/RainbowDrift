using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingTextAnim : MonoBehaviour {
	public static LoadingTextAnim instance;
	Text text;
	string zero = "LOADING";
	string one = "LOADING.";
	string two = "LOADING..";
	string three = "LOADING...";
	int index = 0;
	bool running = true;
	void Start () {
		instance = this;
		text = GetComponent<Text> ();
		StartCoroutine (Animate ());
	}
	void OnAwake () {
		instance = this;
		text = GetComponent<Text> ();
		StartCoroutine (Animate ());
	}
	public void StartAnimate () {
		StartCoroutine (Animate ());
	}
	IEnumerator Animate () {
		text.text = one;
		index = 0;
		while (running == true) {
			switch (index) {
				case 0:
					text.text = zero;
					break;
				case 1:
					text.text = one;
					break;
				case 2:
					text.text = two;
					break;
				case 3:
					text.text = three;
					break;
			}
			index++;
			if (index == 4) {
				index = 0;
			}
			yield return new WaitForSecondsRealtime (0.4f);
		}
	}
}