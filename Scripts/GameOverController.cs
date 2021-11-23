using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
public class GameOverController : MonoBehaviour {
    public OrbPanelController orbPanel;
    public static bool ready = false;
    public static bool ready1 = false;
    public static bool ready2 = false;
    public static bool ready3 = false;
    public GameObject BlockOutPanel;
    public static bool readyToTransition1 = false;
    bool one = false;
    bool two = false;
    //  bool three = false;
    bool firstTime = true;
    bool firstTime2 = true;
    public static bool firstTime3 = true;
    bool firstTime4 = true;
    public static bool backgroundReady = false;
    public GameObject pauseButton;
    public Animator gameOverPanelAnimator;
    public Animator challengesAnimator;
    public Animator orbPanelAnimator;
    public Animator highScoreShine;
    public Animator challengesShine;
    public Animator orbShine;
    public Animator challengesNoPress;
    bool updateDailyHighScoreCar = false;
    string highScoreCar = "";
    string highScoreTrail = "";
    public static GameOverController instance;
    // Use this for initialization
    void Start () {
        instance = this;
        ready = true;
        ready1 = false;
        ready2 = false;
        orbPanel.gameObject.SetActive (false);
        BlockOutPanel.SetActive (false);
        challengesNoPress.gameObject.SetActive (false);
    }
    // Update is called once per frame
    void Update () {
        if (CarVariables.instance.gameOn == false) {
            if (ready) {
                UserData.instance.UpdateInfo ();
                BlockOutPanel.SetActive (true);
                pauseButton.SetActive (false);
                ScoreController.instance.FadeOutGameOverLabels ();
                gameOverPanelAnimator.SetTrigger ("showPanel");
                if (OrbGeneratorController.instance.orbCount > 0) {
                    ChallengesCompletedController.instance.ActivateMask (true);
                }
                StartCoroutine (ActivateBackgroundShaders ());
                PlayfabStatisticsController.instance.UpdateStats ();
                PlayfabStatisticsController.instance.statistics["DistanceTraveled"] += CarScoreController.instance.score;
                PlayfabStatisticsController.instance.statistics["DistanceDrifted"] += (int) CarVariables.instance.distanceDrifted;
                PlayfabStatisticsController.instance.statistics["CoinsCollected"] += CoinGeneratorController.instance.coinCount;
                PlayfabStatisticsController.instance.statistics["OrbsCollected"] += OrbGeneratorController.instance.orbCount;
                PlayfabStatisticsController.instance.statistics["GamesPlayed"]++;
                if (CarScoreController.instance.score > PlayfabStatisticsController.instance.statistics["HighScoresToday"]) {
                    updateDailyHighScoreCar = true;
                    //debug
                    PlayfabStatisticsController.instance.statistics["HighScoresToday"] = CarScoreController.instance.score;
                    DataEntry.instance.UpdateDailyHighScore (CarScoreController.instance.score);
                }
                if (CarScoreController.instance.score > PlayfabStatisticsController.instance.statistics["ActualHighScoresToday"]) {
                    updateDailyHighScoreCar = true;
                    //debug
                    PlayfabStatisticsController.instance.statistics["ActualHighScoresToday"] = CarScoreController.instance.score;
                    DataEntry.instance.UpdateActualDailyHighScore (CarScoreController.instance.score);
                }
                if ((CarScoreController.instance.score > PlayfabStatisticsController.instance.statistics["HighScoresToday"]) || (CarScoreController.instance.score > PlayfabStatisticsController.instance.statistics["ActualHighScoresToday"])) {
                    DataEntry.instance.Save ();
                }
                PlayfabStatisticsController.instance.SetStatistics ();
                if (CarScoreController.instance.score > DataEntry.instance.highScore) {
                    highScoreCar = DataEntry.instance.carUsing;
                    highScoreTrail = DataEntry.instance.trailUsing;
                    //  StartCoroutine (HighScoreShine ());
                    StartCoroutine (CongratsSound ());
                    InvokeRepeating ("TriggerHighScoreShine", 1f, 4f);
                    PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                        Data = new Dictionary<string, string> { { "HighScore", CarScoreController.instance.score.ToString () },
                        }
                    }, (UpdateUserDataResult updateResult) => {
                        DataEntry.instance.UpdateHighScore (CarScoreController.instance.score);
                        DataEntry.instance.Save ();
                    }, CallFailure);
                }
                if (updateDailyHighScoreCar) {
                    highScoreCar = DataEntry.instance.carUsing;
                    highScoreTrail = DataEntry.instance.trailUsing;
                    DailyHighScoreController.instance.UpdateCarTrailTags (highScoreCar, highScoreTrail);
                    updateDailyHighScoreCar = false;
                }
                ready = false;
            }
        }
        if (backgroundReady == true) {
            if (OrbGeneratorController.instance.orbCount > 0) {
                if (firstTime) {
                    one = true;
                    orbPanel.gameObject.SetActive (true);
                    StartCoroutine (ActivateOrbPanel ());
                    firstTime = false;
                }
            } else {
                one = false;
                ready1 = true;
            }
            backgroundReady = false;
        }
        if (ready1) {
            if (firstTime2) {
                if (one) {
                    // challengesNoPress.SetActive (false);
                    challengesNoPress.SetTrigger ("FadeOut");
                    orbPanel.Deactivate ();
                    ChallengesCompletedController.instance.ActivateMask (false);
                } else {
                    readyToTransition1 = true;
                }
                firstTime2 = false;
            }
            if (DataEntry.instance.challengesToFinish.Count > 0) {
                two = true;
                if (readyToTransition1) {
                    ready1 = false;
                    ChallengesCompletedController.instance.ActivateMask (true);
                    StartCoroutine (ActivateChallengesPanel ());
                }
                ChallengesController.instance.showChallenges2 = false;
            } else {
                ready1 = false;
                ready2 = true;
                two = false;
            }
        }
        if (ready2) {
            if (two) {
                ChallengesCompletedController.instance.ActivateMask (false);
                challengesNoPress.SetTrigger ("FadeOut");
                ChallengesCompletedController.instance.FadeOutFunction ();
                //CompletedHeaderController.instance.FadeOutFunction();
            }
            if (firstTime4) {
                if (CoinGeneratorController.instance.coinCount > 0) {
                    StartCoroutine (PlayCoinRewards (true));
                } else {
                    BlockOutPanel.SetActive (false);
                }
                firstTime4 = false;
            }
            firstTime3 = true;
            ready2 = false;
        }
    }
    IEnumerator CongratsSound () {
        yield return new WaitForSeconds (1f);
        //PlayAudio.PlaySound ("congrats");
    }
    void AddCoin (int coinReward) {
        BlockOutPanel.SetActive (false);
        DataEntry.instance.totalCoins += coinReward;
        DataEntry.instance.Save ();
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "AddCurrency",
                FunctionParameter = new Dictionary<string, string> {
                    {
                        "Currency",
                        "CN"
                    },
                    {
                        "Amount",
                        coinReward.ToString ()
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            UserData.instance.UpdateInfo ();
            StartCoroutine (WaitThenUpdateCoin ());
            if (Debug.isDebugBuild) {
                for (int k = 0; k < res.Logs.Count; k++) {
                    //print (res.Logs[k].Message);
                }
            }
        }, CallFailure);
    }
    IEnumerator WaitThenUpdateCoin () {
        yield return new WaitForSeconds (2.0f);
        GameSceneCoinController.instance.updating = true;
    }
    void AddOrb (int orbReward) {
        DataEntry.instance.orbCount += orbReward;
        DataEntry.instance.Save ();
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "AddCurrency",
                FunctionParameter = new Dictionary<string, string> {
                    {
                        "Currency",
                        "OB"
                    },
                    {
                        "Amount",
                        orbReward.ToString ()
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            BlockOutPanel.SetActive (false);
            UserData.instance.UpdateInfo ();
            StartCoroutine (WaitThenUpdateCoin ());
            for (int k = 0; k < res.Logs.Count; k++) {
                //print (res.Logs[k].Message);
            }
        }, CallFailure);
    }
    IEnumerator PlayCoinRewards (bool wait = true) {
        AddCoin (CoinGeneratorController.instance.coinCount);
        if (wait) {
            yield return new WaitForSeconds (ChallengesCompletedController.instance.fadeInTime);
        }
        yield return new WaitForSeconds (0.5f);
        ChallengesCompletedController.instance.PlayRewards ();
        yield return new WaitForSeconds (GameSceneCoinController.instance.speed);
        //print ("add coin request sent");
    }
    IEnumerator ActivateOrbPanel () {
        yield return new WaitForSeconds (0.3f);
        orbPanel.Activate ();
        orbPanelAnimator.SetTrigger ("panelIn");
        orbShine.SetTrigger ("Go");
        yield return new WaitForSeconds (0.5f);
        challengesNoPress.gameObject.SetActive (true);
        yield return new WaitForSeconds (1f);
        ChallengesController.instance.orbAnimation = true;
        yield return new WaitForSeconds (GameSceneOrbController.instance.speed);
        AddOrb (OrbGeneratorController.instance.orbCount);
        ////print ("add orb request sent");
    }
    IEnumerator ActivateBackgroundShaders () {
        yield return new WaitForSeconds (0.2f);
        GameSceneOrbController.instance.GameOver ();
        GameSceneCoinController.instance.GameOver ();
        // TopShaderController.instance.CallShader ();
        // ScoreController.instance.FadeInGameOverLabels ();
        StartShaderController.instance.CallFade (1);
    }
    IEnumerator HighScoreShine () {
        yield return new WaitForSeconds (1f);
        InvokeRepeating ("TriggerHighScoreShine", 0, 4f);
    }
    void TriggerHighScoreShine () {
        highScoreShine.SetTrigger ("Go");
    }
    IEnumerator ActivateLabels () {
        yield return new WaitForSeconds (0.7f);
        // ScoreController.instance.FadeInGameOverLabels ();
    }
    IEnumerator ActivateChallengesPanel () {
        yield return new WaitForSeconds (0.5f);
        challengesNoPress.gameObject.SetActive (true);
        challengesAnimator.SetTrigger ("showPanel");
        two = true;
        yield return new WaitForSeconds (1f);
        challengesShine.SetTrigger ("Go");
    }
    IEnumerator FadeIn (Image image, float alpha) {
        float startAlpha = image.color.a;
        yield return new WaitForSeconds (0.5f);
        //Alpha
        for (float i = 0; i <= 1; i += Time.deltaTime / 0.8f) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.Lerp (startAlpha, alpha, i));
            yield return null;
        }
        image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
    }
    void CallFailure (PlayFabError error) {
        if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
            //no connection
            ConnectionController.instance.noConnectionDetected ();
        } else {
            if (Debug.isDebugBuild) {
                //print (error);
            }
        }
    }
    public void TurnOffNoTouch () {
        challengesNoPress.gameObject.SetActive (false);
    }
}