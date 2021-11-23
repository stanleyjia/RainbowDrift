using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoreModeToggle : MonoBehaviour {
    public RectTransform sliderShadow;
    public Animator sliderAnimator;
    public static StoreModeToggle instance;
    // Use this for initialization
    void Start () {
        instance = this;
        GoToLeft ();
    }
    // Update is called once per frame
    void Update () { }
    /* public void GoToRight () {
        if (sliderAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Left") {
            sliderAnimator.SetTrigger ("goRight");
        }
    }
    public void GoToLeft () {
        if (sliderAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Right") {
            sliderAnimator.SetTrigger ("goLeft");
        }
    }*/
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