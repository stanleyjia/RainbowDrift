using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BonusTextController : MonoBehaviour {
	Animator animator;
	Text text;
	bool lastRainbowMode = false;
	int lastColorMode = 0;
	int number = 0;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		text = GetComponent<Text> ();
		lastRainbowMode = ColorBarController.instance.rainbowMode;
		lastColorMode = ColorBarController.instance.colorMode;
	}
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 3 == 1) {
			if (lastRainbowMode != ColorBarController.instance.rainbowMode) {
				lastRainbowMode = ColorBarController.instance.rainbowMode;
				if (ColorBarController.instance.rainbowMode == true) {
					number = ((ColorBarController.instance.colorMode + 2));
					if (animator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Hidden") {
						//animator.SetTrigger ("Show");
					}
					animator.SetTrigger ("Show");
				} else {
					number = ((ColorBarController.instance.colorMode + 1));
				}
				text.text = "COLLECT " + number + "X COINS";
			}
		}
		if (Time.frameCount % 3 == 2) {
			if (lastColorMode != ColorBarController.instance.colorMode) {
				lastColorMode = ColorBarController.instance.colorMode;
				if (ColorBarController.instance.rainbowMode == true) {
					number = ((ColorBarController.instance.colorMode + 2));
				} else {
					number = (ColorBarController.instance.colorMode + 1);
				}
				text.text = "COLLECT " + number + "X COINS";
				if (animator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Hidden") {
					//animator.SetTrigger ("Show");
				}
				animator.SetTrigger ("Show");
			}
		}
	}
}