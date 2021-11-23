using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownController : MonoBehaviour {
    Text countdowntext;
    public GameObject pauseButton;
    void Start () {
        countdowntext = GetComponent<Text> ();
        countdowntext.text = "";
    }
    public void StartCountdown () {
        StartCoroutine (Countdown ());
    }
    IEnumerator Countdown () {
        yield return new WaitForSecondsRealtime (0.25f);
        countdowntext.color = Color.white;
        countdowntext.text = "3";
        PlayAudio.PlaySound ("countdown");
        yield return new WaitForSecondsRealtime (0.5f);
        countdowntext.text = "2";
        PlayAudio.PlaySound ("countdown");
        yield return new WaitForSecondsRealtime (0.5f);
        countdowntext.text = "1";
        PlayAudio.PlaySound ("countdown");
        yield return new WaitForSecondsRealtime (0.5f);
        countdowntext.text = "0";
        PlayAudio.PlaySound ("lastCountdown");
        yield return new WaitForSecondsRealtime (0.4f);
        pauseButton.SetActive (true);
        gameObject.SetActive (false);
        countdowntext.text = "";
        AudioListener.pause = false;
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime (1f);
        // pauseButton.SetActive (true);
    }
}