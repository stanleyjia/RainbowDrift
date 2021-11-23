using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoginResult = PlayFab.ClientModels.LoginResult;
public class TutorialController : MonoBehaviour {
    public static TutorialController instance;
    public bool start = false;
    public int stage = 0;
    public Animator welcome;
    public Animator instructions;
    public Animator coin;
    public Animator distance;
    public Animator rainbow;
    public Animator joystick;
    public Animator try1;
    public Animator startShader;
    public Animator skipButton;
    GameObject scorePointer;
    GameObject rainbowPointer;
    GameObject coinPointer;
    GameObject joystickPointer;
    List<Animator> stages = new List<Animator> ();
    bool hidePressed = false;
    int lastStage;
    public bool tryAgainOn = false;
    // Use this for initialization
    void Start () {
        startShader.gameObject.SetActive (true);
        startShader.SetTrigger ("FadeOut");
        //0
        stages.Add (welcome);
        //1
        stages.Add (instructions);
        //2
        stages.Add (distance);
        //3
        stages.Add (coin);
        //4
        stages.Add (rainbow);
        //5
        stages.Add (joystick);
        //6
        stages.Add (try1);
        //stage = UserData.instance.tutorialStage;
        instance = this;
        lastStage = 0;
        stage = UserData.instance.tutorialStage;
        joystickPointer = GameObject.FindGameObjectWithTag ("JoystickPointer");
        scorePointer = GameObject.FindGameObjectWithTag ("ScorePointer");
        rainbowPointer = GameObject.FindGameObjectWithTag ("RainbowPointer");
        coinPointer = GameObject.FindGameObjectWithTag ("CoinPointer");
        joystickPointer.SetActive (false);
        scorePointer.SetActive (false);
        coinPointer.SetActive (false);
        rainbowPointer.SetActive (false);
        if (stage == 0) {
            StartCoroutine (Stage0 ());
        }
    }
    private void Awake () {
        //FB.Init(OnFacebookInitialized);
        //stage = UserData.instance.tutorialStage;
        //print (stage);
    }
    private void OnFacebookInitialized () {
        // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
        if (FB.IsLoggedIn) {
            FB.LogOut ();
        }
        // We invoke basic login procedure and pass in the callback to process the result
        //FB.Log
    }
    // Update is called once per frame
    void Update () {
        if (lastStage != stage) {
            //print (stage);
            switch (stage) {
                case 1:
                    StartCoroutine (Stage1 ());
                    break;
                case 2:
                    StartCoroutine (Stage2 ());
                    break;
                case 3:
                    StartCoroutine (Stage3 ());
                    break;
                case 4:
                    StartCoroutine (Stage4 ());
                    break;
                case 5:
                    StartCoroutine (Stage5 ());
                    break;
                case 6:
                    HideStage (5);
                    joystickPointer.SetActive (false);
                    Time.timeScale = 1;
                    StartCoroutine (WaitThenGo ());
                    break;
            }
            lastStage = stage;
        }
    }
    public void EndTutorial () {
        StartCoroutine (ShowRegister ());
    }
    void HideAllPanels () {
        for (int i = 0; i < stages.Count; i++) {
            stages[i].Play ("Off");
        }
    }
    IEnumerator ShowRegister () {
        yield return new WaitForSeconds (2f);
        // yield return null;
        if (GameController.instance.previousScene == "Persistent") {
            GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
            PlayFabLogin.instance.registerMode = 1;
            ChangeScene.instance.changeScene ("RegisterScene");
        } else if (GameController.instance.previousScene == "CombinedScene") {
            GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
            UserData.instance.combinedIndex = 0;
            ChangeScene.instance.changeScene ("CombinedScene");
        } else {
            if (UserData.instance.tutorialFromLogin) {
                GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
                PlayFabLogin.instance.registerMode = 1;
                ChangeScene.instance.changeScene ("RegisterScene");
            } else {
                GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
                UserData.instance.combinedIndex = 0;
                ChangeScene.instance.changeScene ("CombinedScene");
            }
        }
        // registerPanels[0].SetActive(true);
    }
    IEnumerator WaitThenJoystick () {
        yield return new WaitForSeconds (3);
        stage++;
    }
    #region Stages
    IEnumerator Stage0 () {
        //Welcome
        yield return new WaitForSeconds (1f);
        stages[0].SetTrigger ("Show");
        yield return new WaitForSeconds (4f);
        stages[0].SetTrigger ("Hide");
        stage = 1;
    }
    IEnumerator Stage1 () {
        //Instructions
        yield return new WaitForSeconds (0.5f);
        stages[1].SetTrigger ("Show");
        yield return new WaitForSeconds (0.5f);
        //skipButton.SetTrigger ("Show");
        yield return new WaitForSeconds (3f);
        stages[1].SetTrigger ("Hide");
        if ((hidePressed == false) && (tryAgainOn == false)) {
            stage = 2;
        }
    }
    IEnumerator Stage2 () {
        //Distance
        yield return new WaitForSeconds (1f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            stages[2].SetTrigger ("Show");
            scorePointer.SetActive (true);
        }
        yield return new WaitForSeconds (3f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            scorePointer.SetActive (false);
            stages[2].SetTrigger ("Hide");
        }
        if ((hidePressed == false) && (tryAgainOn == false)) {
            stage = 3;
        }
    }
    IEnumerator Stage3 () {
        //Coin
        yield return new WaitForSeconds (1f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            stages[3].SetTrigger ("Show");
            coinPointer.SetActive (true);
        }
        yield return new WaitForSeconds (3f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            coinPointer.SetActive (false);
            stages[3].SetTrigger ("Hide");
        }
        if ((hidePressed == false) && (tryAgainOn == false)) {
            stage = 4;
        }
    }
    IEnumerator Stage4 () {
        //Rainbow
        yield return new WaitForSeconds (1f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            rainbowPointer.SetActive (true);
            stages[4].SetTrigger ("Show");
        }
        yield return new WaitForSeconds (3f);
        if ((hidePressed == false) && (tryAgainOn == false)) {
            rainbowPointer.SetActive (false);
            stages[4].SetTrigger ("Hide");
        }
        stage = 5;
    }
    IEnumerator Stage5 () {
        //Joystick
        yield return new WaitForSeconds (0.5f);
        skipButton.SetTrigger ("Hide");
        yield return new WaitForSeconds (1f);
        joystickPointer.SetActive (true);
        Time.timeScale = 0;
        AIController.instance.AIControllerOn = false;
        TutorialCar.instance.gameObject.GetComponent<Rigidbody> ().drag = 0.5f;
        stages[5].SetTrigger ("Show");
    }
    #endregion
    public void HideStage (int sta) {
        try {
            stages[sta].SetTrigger ("Hide");
        } catch { }
    }
    public void Skip () {
        StartCoroutine (SkipCoroutine ());
    }
    IEnumerator SkipCoroutine () {
        hidePressed = true;
        //yield return new WaitForSeconds (0.2f);
        skipButton.SetTrigger ("Hide");
        yield return new WaitForSeconds (1f);
        HideStage (stage);
        scorePointer.SetActive (false);
        coinPointer.SetActive (false);
        rainbowPointer.SetActive (false);
        stage = 5;
    }
    IEnumerator WaitThenGo () {
        yield return new WaitForSecondsRealtime (1);
        // Time.timeScale = 1;
        HideStage (stage);
        stage++;
    }
    public void TryAgain () {
        //print ("OOPS");
        //stage = 8;
        tryAgainOn = true;
        HideAllPanels ();
        // AIController.instance.Reset ();
        UserData.instance.tutorialStage = stage;
        startShader.SetTrigger ("FadeOut");
        try1.SetTrigger ("Show");
        //skipButton.SetTrigger ("Hide");
        skipButton.Play ("off");
        StartCoroutine (WaitThenRestart ());
        // ChangeScene.instance.changeScene ("TutorialScene");
    }
    IEnumerator WaitThenRestart () {
        yield return new WaitForSecondsRealtime (2);
        ChangeScene.instance.changeScene ("TutorialScene");
    }
    public void TryPressed () {
        HideStage (stage);
        stage = 7;
    }
}