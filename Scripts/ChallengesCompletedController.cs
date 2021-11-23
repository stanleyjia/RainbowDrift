using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChallengesCompletedController : MonoBehaviour {
    public GameObject mask;
    Image image;
    Transform panelContainer;
    public GameObject header;
    //  public GameObject completed;
    public GameObject completedText;
    public Animator challengesShine;
    float alpha = 0.9f;
    public float speed = 3f;
    public bool disappear = false;
    public float fadeInTime = 0.6f;
    float fadeOutTime = 0.4f;
    public static ChallengesCompletedController instance;
    // Use this for initialization
    void Start () {
        instance = this;
        panelContainer = transform.Find ("PanelContainer");
        image = GetComponent<Image> ();
        image.color = new Color (image.color.r, image.color.g, image.color.b, 1f);
        header.GetComponent<Image> ().color = new Color (header.GetComponent<Image> ().color.r, header.GetComponent<Image> ().color.g, header.GetComponent<Image> ().color.b, 1f);
        //completed.GetComponent<Image> ().color = new Color (completed.GetComponent<Image> ().color.r, completed.GetComponent<Image> ().color.g, completed.GetComponent<Image> ().color.b, 1f);
        completedText.GetComponent<Text> ().color = new Color (completedText.GetComponent<Text> ().color.r, completedText.GetComponent<Text> ().color.g, completedText.GetComponent<Text> ().color.b, 1f);
        mask.SetActive (false);
    }
    public void ActivateMask (bool state) {
        mask.SetActive (state);
    }
    public void ShowChallenges () {
        //StartCoroutine(FadeIn());
        //  StartCoroutine (Fade (gameObject, alpha, fadeInTime));
        // StartCoroutine (Fade (header, 1, fadeInTime));
        //StartCoroutine (Fade (completed, 1, fadeInTime));
        //StartCoroutine (Fade (completedText, 1, fadeInTime));
    }
    public void FadeOutFunction () {
        StartCoroutine (FadeOut ());
    }
    public void ChallengesShine () {
        challengesShine.SetTrigger ("Go");
    }
    public void PlayRewards () {
        FlyingCoinsGenerator.instance.GenerateNormalReward ();
    }
    IEnumerator FadeIn () {
        //Alpha
        for (float i = 0; i <= alpha; i += Time.deltaTime / fadeInTime) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }
        image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
    }
    IEnumerator Fade (GameObject obj, float opacity, float time) {
        if (obj.GetComponent<Text> () != null) {
            Text img = obj.GetComponent<Text> ();
            float startAlpha = img.color.a;
            for (float i = 0; i <= 1; i += Time.deltaTime / time) {
                img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, opacity, i));
                yield return null;
            }
            img.color = new Color (img.color.r, img.color.g, img.color.b, opacity);
        } else if (obj.GetComponent<Image> () != null) {
            Image img = obj.GetComponent<Image> ();
            float startAlpha = img.color.a;
            // yield return new WaitForSeconds(1f);
            for (float i = 0; i <= 1; i += Time.deltaTime / time) {
                img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, opacity, i));
                yield return null;
            }
            img.color = new Color (img.color.r, img.color.g, img.color.b, opacity);
        }
    }
    IEnumerator FadeOut () {
        //Alpha
        // child fade out
        for (int i = 0; i < panelContainer.childCount; i++) {
            if (panelContainer.GetChild (i).gameObject.activeSelf == true) {
                panelContainer.GetChild (i).GetComponent<ChallengePanel> ().FadeOut ();
            }
        }
        //  StartCoroutine (Fade (this, 0.00f, fadeOutTime));
        StartCoroutine (Fade (gameObject, 0.00f, fadeOutTime));
        StartCoroutine (Fade (header, 0.00f, fadeOutTime));
        // StartCoroutine (Fade (completed, 0.05f, fadeOutTime));
        StartCoroutine (Fade (completedText, 0.00f, fadeOutTime));
        while (disappear == false) {
            yield return null;
        }
        /*
               header.GetComponent<Image> ().color = new Color (header.GetComponent<Image> ().color.r, header.GetComponent<Image> ().color.g, header.GetComponent<Image> ().color.b, 0f);
               // completed.GetComponent<Image> ().color = new Color (completed.GetComponent<Image> ().color.r, completed.GetComponent<Image> ().color.g, completed.GetComponent<Image> ().color.b, 0f);
               completedText.GetComponent<Text> ().color = new Color (completedText.GetComponent<Text> ().color.r, completedText.GetComponent<Text> ().color.g, completedText.GetComponent<Text> ().color.b, 0f);
               image.color = new Color (image.color.r, image.color.g, image.color.b, 0f);*/
        //   mask.SetActive(false);
        PlayerController.instance.updateOrb = true;
        PlayerController.instance.updateCoin = true;
        disappear = false;
    }
}