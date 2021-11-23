using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreController : MonoBehaviour {
    int highScore;
    public static ScoreController instance;
    float fadeOutSpeed = 0.6f;
    public Text inGameCoins;
    public Text inGameScore;
    public Text coinReward;
    public Text highScoreLabel;
    public Text gameOverScore;
    public Text gameOverLabel;
    public Image inGameCoinImage;
    public Image gameOverCoinImage;
    bool firstTime = true;
    void Start () {
        instance = this;
        highScore = DataEntry.instance.highScore;
        inGameCoins.color = new Color (inGameCoins.color.r, inGameCoins.color.g, inGameCoins.color.b, 1f);
        inGameScore.color = new Color (inGameScore.color.r, inGameScore.color.g, inGameScore.color.b, 1f);
        coinReward.color = new Color (coinReward.color.r, coinReward.color.g, coinReward.color.b, 1);
        highScoreLabel.color = new Color (highScoreLabel.color.r, highScoreLabel.color.g, highScoreLabel.color.b, 1f);
        gameOverScore.color = new Color (gameOverScore.color.r, gameOverScore.color.g, gameOverScore.color.b, 1);
        gameOverCoinImage.color = new Color (gameOverCoinImage.color.r, gameOverCoinImage.color.g, gameOverCoinImage.color.b, 1);
        gameOverLabel.color = new Color (gameOverLabel.color.r, gameOverLabel.color.g, gameOverLabel.color.b, 1);
    }
    public
    void Update () {
        if (CarVariables.instance.gameOn) {
            inGameCoins.text = CoinGeneratorController.instance.coinCount.ToString ();
        } else {
            if (firstTime) {
                coinReward.text = CoinGeneratorController.instance.coinCount.ToString ();
                gameOverScore.text = Mathf.FloorToInt (CarScoreController.instance.score).ToString ();
                if (CarScoreController.instance.score > highScore) {
                    highScoreLabel.text = "BEST " + CarScoreController.instance.score.ToString ();
                    gameOverLabel.text = "NEW BEST";
                    gameOverLabel.fontSize = 120;
                } else {
                    gameOverLabel.text = "GAME OVER";
                    highScoreLabel.text = "BEST " + highScore.ToString ();
                }
                firstTime = false;
            }
        }
    }
    public void FadeOutGameOverLabels () {
        StartCoroutine (Fade (inGameCoins, 0, fadeOutSpeed));
        StartCoroutine (Fade (inGameScore, 0, fadeOutSpeed));
        StartCoroutine (FadeImage (inGameCoinImage, 0, fadeOutSpeed));
    }
    /*public void FadeInGameOverLabels () {
        coinReward.text = CoinGeneratorController.instance.coinCount.ToString ();
        StartCoroutine (Fade (coinReward, 1, fadeInSpeed));
        gameOverScore.text = Mathf.FloorToInt (CarScoreController.instance.score).ToString ();
        StartCoroutine (Fade (gameOverScore, 1, fadeInSpeed));
        StartCoroutine (Fade (highScoreLabel, 1, fadeInSpeed));
        StartCoroutine (FadeImage (gameOverCoinImage, 1, fadeInSpeed));
        if (CarScoreController.instance.score > highScore) {
            highScoreLabel.text = "Best " + CarScoreController.instance.score.ToString ();
            gameOverLabel.text = "New Best";
            gameOverLabel.fontSize = 120;
        } else {
            highScoreLabel.text = "Best " + highScore.ToString ();
        }
        StartCoroutine (Fade (gameOverLabel, 1, fadeInSpeed));
    }*/
    /*IEnumerator FadeAway(float endAlpha)
    {
        yield return new WaitForSeconds(0.4f);
        startAlpha = text.color.a;
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeOutSpeed)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(startAlpha, endAlpha, i));
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, endAlpha);
    }



    IEnumerator FadeIn(float endAlpha)
    {
        //yield return new WaitForSeconds(1f);
        startAlpha = text.color.a;
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeOutSpeed)
        {
            // set color with i as alpha
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(startAlpha, endAlpha, i));
            gameOverLabelText.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(startAlpha, endAlpha, i));
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, endAlpha);
        gameOverLabelText.color = new Color(text.color.r, text.color.g, text.color.b, endAlpha);
    }*/
    IEnumerator Fade (Text img, float alpha, float time) {
        float startAlpha = img.color.a;
        for (float i = 0; i <= 1; i += Time.deltaTime / time) {
            img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, alpha, i));
            yield return null;
        }
        img.color = new Color (img.color.r, img.color.g, img.color.b, alpha);
    }
    IEnumerator FadeImage (Image img, float alpha, float time) {
        float startAlpha = img.color.a;
        for (float i = 0; i <= 1; i += Time.deltaTime / time) {
            img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, alpha, i));
            yield return null;
        }
        img.color = new Color (img.color.r, img.color.g, img.color.b, alpha);
    }
}