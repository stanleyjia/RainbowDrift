using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreShaderController : MonoBehaviour {
    float highScore;
    Image image;
    bool firstTime = true;
    float greenAlpha = 0.4f;
    float orangeAlpha = 194f / 255f;
    Color greenColor;
    Color orangeColor;
    float endColor;
    float startColor;
    public static HighScoreShaderController instance;
    public Text highScoreText;
    // Use this for initialization
    void Start () {
        instance = this;
        image = GetComponent<Image> ();
        highScore = (int) DataEntry.instance.highScore;
        //greenColor = new Color (144f / 255f, 177f / 255f, 183f / 255f, 0.95f);
        greenColor = new Color (182f / 255f, 91f / 255f, 51f / 255f, 0.95f);
        orangeColor = new Color (225f / 255f, 178f / 255f, 95f / 255f, 0.95f);
        image.color = greenColor;
    }
    // Update is called once per frame
    void Update () {
        if ((CarVariables.instance.gameOn == false) && (firstTime == true)) {
            if (CarScoreController.instance.score > highScore) {
                image.color = orangeColor;
                highScoreText.color = Color.white;
            } else {
                image.color = greenColor;
                highScoreText.color = orangeColor;
            }
            firstTime = false;
        }
    }
    /*public void CallFade (float time) {
        StartCoroutine (FadeImage (time));
    }*/
    IEnumerator FadeImage (float time) {
        if (CarScoreController.instance.score > highScore) {
            image.color = orangeColor;
            endColor = orangeAlpha;
        } else {
            image.color = greenColor;
            endColor = greenAlpha;
        }
        //Alpha
        startColor = image.color.a;
        for (float i = 0; i <= 1; i += Time.deltaTime / time) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.Lerp (startColor, endColor, i));
            yield return null;
        }
        image.color = new Color (image.color.r, image.color.g, image.color.b, endColor);
    }
}