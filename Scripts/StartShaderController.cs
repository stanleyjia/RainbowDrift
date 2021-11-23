using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartShaderController : MonoBehaviour {
    public GameObject restartButton;
    public GameObject homeButton;
    public GameObject pauseButton;
    public GameObject StoreButton;
    public GameObject UICoin;
    // public GameObject UIOrb;
    public GameObject GameOverButtons;
    UnityEngine.UI.Image image;
    float startcolor;
    float endcolor;
    bool firstTime = true;
    public static StartShaderController instance;
    void Start () {
        instance = this;
        image = GetComponent<UnityEngine.UI.Image> ();
        startcolor = 0.8f;
        endcolor = 0.1f;
        image.color = new Color (image.color.r, image.color.g, image.color.b, startcolor);
        //  UIOrb.SetActive(false);
        GameOverButtons.SetActive (false);
    }
    void Update () {
        if ((PlayerController.instance.start == false) && (firstTime == true)) {
            StartCoroutine (FadeImage (0));
            firstTime = false;
        }
    }
    public void CallFade (int fadeAway) {
        if (fadeAway < 3) {
            StartCoroutine (FadeImage (fadeAway));
        } else {
            image.color = new Color (image.color.r, image.color.g, image.color.b, 0.4f);
        }
    }
    IEnumerator FadeImage (int fadeAway) {
        if (fadeAway == 0) {
            //fade out at start
            yield return new WaitForSeconds (0.2f);
            for (float i = startcolor; i >= 0; i -= Time.deltaTime / 1.0f) {
                image.color = new Color (image.color.r, image.color.g, image.color.b, i);
                yield return null;
            }
            image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
            pauseButton.SetActive (true);
        } else if (fadeAway == 1) {
            //Game over
            for (float i = 0; i <= endcolor; i += Time.deltaTime / 1.7f) {
                image.color = new Color (image.color.r, image.color.g, image.color.b, i);
                yield return null;
            }
            image.color = new Color (image.color.r, image.color.g, image.color.b, endcolor);
            // restartButton.SetActive(true);
            // homeButton.SetActive(true);
            // StoreButton.SetActive(true);
            GameOverButtons.SetActive (true);
            UICoin.SetActive (true);
            GameOverController.backgroundReady = true;
        } else if (fadeAway == 2) {
            //Pause
            for (float i = 0.4f; i >= 0; i -= Time.deltaTime / 1.2f) {
                image.color = new Color (image.color.r, image.color.g, image.color.b, i);
                yield return null;
            }
            image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
        }
    }
}