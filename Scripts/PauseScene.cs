using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseScene : MonoBehaviour {
    public GameObject restartButton;
    public GameObject homeButton;
    public GameObject pauseButton;
    public GameObject playButton;
    public Animator pauseMenuAnimator;
    public CountdownController countdown;
    public StartShaderController pauseShader;
    public bool paused;
    public bool readyToPause = true;
    public static PauseScene instance;
    public void Start () {
        instance = this;
    }
    public void pauseScene (bool pause) {
        if (pause) {
            if (CarVariables.instance.gameOn) {
                PlayAudio.pause = true;
                pauseMenuAnimator.SetTrigger ("showMenu");
                AudioListener.pause = true;
                paused = true;
                pauseButton.SetActive (false);
                Time.timeScale = 0;
                /*playButton.SetActive (true);
                restartButton.SetActive (true);
                homeButton.SetActive (true);*/
                pauseShader.CallFade (3);
            }
        } else {
            PlayAudio.pause = false;
            pauseMenuAnimator.SetTrigger ("hideMenu");
            paused = false;
            //playButton.SetActive (false);
            //restartButton.SetActive (false);
            //homeButton.SetActive (false);
            pauseShader.CallFade (2);
            countdown.gameObject.SetActive (true);
            countdown.StartCountdown ();
        }
    }
    void Update () {
        if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer)) {
            if ((Input.GetKey (KeyCode.P)) && (readyToPause)) {
                readyToPause = false;
                StartCoroutine (WaitOneSecond ());
                if (paused == false) {
                    pauseScene (true);
                } else {
                    pauseScene (false);
                }
            }
        }
    }
    IEnumerator WaitOneSecond () {
        yield return new WaitForSecondsRealtime (0.5f);
        readyToPause = true;
    }
}