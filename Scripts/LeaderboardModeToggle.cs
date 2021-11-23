using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeaderboardModeToggle : MonoBehaviour {
	public Animator sliderAnimator;
	public RectTransform sliderShadow;
	// Use this for initialization
	void Start () {
		GoToLeft ();
	}
	// Update is called once per frame
	void Update () { }
	public void GoToRight () {
		/* if (sliderAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Left") {
			sliderAnimator.SetTrigger ("goRight");
		}*/
		if (sliderShadow.anchoredPosition.x != 75) {
			sliderShadow.anchoredPosition = new Vector2 (75, sliderShadow.anchoredPosition.y);
		}
	}
	public void GoToLeft () {
		/* if (sliderAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Right") {
			sliderAnimator.SetTrigger ("goLeft");
		}*/
		if (sliderShadow.anchoredPosition.x != -75) {
			sliderShadow.anchoredPosition = new Vector2 (-75, sliderShadow.anchoredPosition.y);
		}
	}
}