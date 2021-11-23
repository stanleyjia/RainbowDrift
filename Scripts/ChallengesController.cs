using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#pragma warning disable 0219
#pragma warning disable 0414
public class ChallengesController : MonoBehaviour {
    GameObject scrollBackground;
    float levelHeight;
    public bool updateFinished = false;
    public bool challengeCompleted = false;
    public bool challengeCompleted2 = false;
    List<Panel> panels = new List<Panel> ();
    bool oneTime2 = true;
    float pheight;
    float pWidth;
    public int coinReward;
    public bool challengeNotif;
    public bool orbAnimation;
    //For game over orbs and coins
    public Panel donePanel;
    public bool updateOrbPos;
    public bool updateCoinPos;
    public Vector3 coinStartPosition;
    //bool newCompleted = false;
    public bool showChallenges = false;
    public bool showChallenges2 = false;
    public bool challengesFinished = false;
    float currentPosition;
    float nextPosition;
    public int lastFinished = 0;
    float oneTimeHeight;
    public bool readyToMove = false;
    #region Init Challenges
    private static Dictionary<string, object> distance1 = new Dictionary<string, object> { { "Name", "Go for distance!" },
        { "Description", "Travel at least 300m in one run" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> coins1 = new Dictionary<string, object> { { "Name", "Pick up those coins!" },
        { "Description", "Pick up 30 coins in one run" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> rainbow1 = new Dictionary<string, object> { { "Name", "Go Rainbow!" },
        { "Description", "Enter Rainbow Mode" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> cars1 = new Dictionary<string, object> { { "Name", "Buy those cars!" },
        { "Description", "Buy a car" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> play1 = new Dictionary<string, object> { { "Name", "Practice makes perfect" },
        { "Description", "Play 10 times" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> drift1 = new Dictionary<string, object> { { "Name", "Skrt Skrt" },
        { "Description", "Drift 5 times in one run" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> refer1 = new Dictionary<string, object> { { "Name", "Invite a friend" },
        { "Description", "Refer 1 friend" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> avatar1 = new Dictionary<string, object> { { "Name", "Show some personality" },
        { "Description", "Change your avatar icon" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> collectCoins1 = new Dictionary<string, object> { { "Name", "Money matters" },
        { "Description", "Collect 1000 coins" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> changeVolume1 = new Dictionary<string, object> { { "Name", "Get on the beat" },
        { "Description", "Change your volume settings" },
        { "Level", 1 },
        { "coinReward", 100 },
    };
    private static Dictionary<string, object> distance2 = new Dictionary<string, object> { { "Name", "Go for distance!" },
        { "Description", "Travel at least 600m in one run" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> coins2 = new Dictionary<string, object> { { "Name", "Pick up those coins!" },
        { "Description", "Pick up 100 coins in one run" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> ownCars2 = new Dictionary<string, object> { { "Name", "How big is your garage?" },
        { "Description", "Own 3 cars" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> drift2 = new Dictionary<string, object> { { "Name", "Skrt Skrt" },
        { "Description", "Drift 25 times in one run" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> orb2 = new Dictionary<string, object> { { "Name", "Ooh, colors" },
        { "Description", "Pick up an orb" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> allcoins2 = new Dictionary<string, object> { { "Name", "Coin Perfection" },
        { "Description", "Pick up all coins for 500m" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    private static Dictionary<string, object> appstore2 = new Dictionary<string, object> { { "Name", "Give us a rating!" },
        { "Description", "Rate us on the App Store" },
        { "Level", 2 },
        { "coinReward", 250 },
    };
    //Level 3
    private static Dictionary<string, object> distance3 = new Dictionary<string, object> { { "Name", "Go for distance!" },
        { "Description", "Travel at least 1000m in one run" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    private static Dictionary<string, object> play3 = new Dictionary<string, object> { { "Name", "Practice makes perfect" },
        { "Description", "Play 300 times" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    private static Dictionary<string, object> coins3 = new Dictionary<string, object> { { "Name", "Pick up those coins!" },
        { "Description", "Pick up 200 coins in one run" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    private static Dictionary<string, object> drift3 = new Dictionary<string, object> { { "Name", "Skrt Skrt" },
        { "Description", "Drift 100 times in one run" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    private static Dictionary<string, object> buyTrails3 = new Dictionary<string, object> { { "Name", "Flex on em" },
        { "Description", "Buy a trail" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    private static Dictionary<string, object> ownCars3 = new Dictionary<string, object> { { "Name", "How big is your garage?" },
        { "Description", "Own 4 cars" },
        { "Level", 3 },
        { "coinReward", 500 },
    };
    //Level 4
    private static Dictionary<string, object> distance4 = new Dictionary<string, object> { { "Name", "Go the distance!" },
        { "Description", "Travel at least 2000m in one run" },
        { "Level", 4 },
        { "coinReward", 1000 },
    };
    private static Dictionary<string, object> play4 = new Dictionary<string, object> { { "Name", "Practice makes perfect" },
        { "Description", "Play 1000 times" },
        { "Level", 4 },
        { "coinReward", 1000 },
    };
    private static Dictionary<string, object> coins4 = new Dictionary<string, object> { { "Name", "Pick up those coins!" },
        { "Description", "Pick up 500 coins in one run" },
        { "Level", 4 },
        { "coinReward", 1000 },
    };
    private static Dictionary<string, object> coinsDistance4 = new Dictionary<string, object> { { "Name", "Coin Perfection" },
        { "Description", "Pick up all coins for 750m" },
        { "Level", 4 },
        { "coinReward", 1000 },
    };
    private static Dictionary<string, object> distanceTimes4 = new Dictionary<string, object> { { "Name", "Experienced Traveler" },
        { "Description", "Travel more than 1000m 10 times" },
        { "Level", 4 },
        { "coinReward", 1000 },
    };
    //Level 5
    private static Dictionary<string, object> ownCars5 = new Dictionary<string, object> { { "Name", "How big is your garage?" },
        { "Description", "Own 5 cars" },
        { "Level", 5 },
        { "coinReward", 5000 },
    };
    private static Dictionary<string, object> distance5 = new Dictionary<string, object> { { "Name", "Go for distance!" },
        { "Description", "Travel at least 5000m in one run" },
        { "Level", 5 },
        { "coinReward", 5000 },
    };
    private static Dictionary<string, object> play5 = new Dictionary<string, object> { { "Name", "Practice makes perfect" },
        { "Description", "Play 5000 times" },
        { "Level", 5 },
        { "coinReward", 5000 },
    };
    private static Dictionary<string, object> coinsDistance5 = new Dictionary<string, object> { { "Name", "Coin Perfection" },
        { "Description", "Pick up all coins for 1000m" },
        { "Level", 5 },
        { "coinReward", 5000 },
    };
    private static Dictionary<string, object> collectorb5 = new Dictionary<string, object> { { "Name", "Get Points" },
        { "Description", "Collect 50 orbs" },
        { "Level", 5 },
        { "coinReward", 5000 },
    };
    public static Dictionary<int, object> challenges = new Dictionary<int, object> { { 0, distance1 },
        { 1, coins1 },
        { 2, rainbow1 },
        { 3, cars1 },
        { 4, play1 },
        { 5, drift1 },
        { 6, refer1 },
        { 7, avatar1 },
        { 8, collectCoins1 },
        { 9, changeVolume1 },
        { 10, distance2 },
        { 11, coins2 },
        { 12, ownCars2 },
        { 13, drift2 },
        { 14, orb2 },
        { 15, allcoins2 },
        //  { 16, appstore2 },
        { 17, distance3 },
        { 18, play3 },
        { 19, coins3 },
        { 20, drift3 },
        { 21, buyTrails3 },
        { 22, ownCars3 },
        { 23, distance4 },
        { 24, play4 },
        { 25, coins4 },
        { 26, coinsDistance4 },
        // { 27, distanceTimes4 },
        { 28, ownCars5 },
        { 29, distance5 },
        { 30, play5 }, //
        { 31, coinsDistance5 },
        { 32, collectorb5 },
    };
    #endregion
    static int SortByLevel (int l1, int l2) {
        return l1.CompareTo (l2);
    }
    public static ChallengesController instance;
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad (this);
        instance = this;
    }
    public static Dictionary<string, object> GetChallenge (int index) {
        try {
            if (challenges[index] != null) {
                return (challenges[index] as Dictionary<string, object>);
            }
        } catch { }
        return new Dictionary<string, object> ();
    }
    // Update is called once per frame
    void Update () {
        if (SceneManager.GetActiveScene ().name == "GameScene") {
            if (updateCoinPos == true) {
                coinStartPosition = donePanel.gameOb.transform.Find ("Rewards").Find ("CoinReward").Find ("UICoins").position;
            }
            if (CarVariables.instance.gameOn == false) {
                if (oneTime2) {
                    if (DataEntry.instance.challengesToFinish.Count > 0) {
                        SetGameOverChallengesPanel ();
                    }
                    oneTime2 = false;
                }
            } else {
                oneTime2 = true;
            }
        }
        // }
    }
    public void SetGameOverChallengesPanel () {
        lastFinished = 0;
        panels.Clear ();
        scrollBackground = GameObject.FindWithTag ("PanelContainer");
        List<int> challengesToFinish = DataEntry.instance.challengesToFinish;
        challengesToFinish.Sort (SortByLevel);
        for (int i = 0; i < challengesToFinish.Count; i++) {
            Panel panel = new Panel ();
            panel.challenge = challengesToFinish[i];
            panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/GameChallengesPanel"));
            panel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().challengeIndex = challengesToFinish[i];
            panel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().displayIndex = i;
            pheight = panel.gameOb.GetComponent<RectTransform> ().rect.height;
            pWidth = panel.gameOb.GetComponent<RectTransform> ().rect.width;
            panel.gameOb.transform.SetParent (scrollBackground.transform, false);
            panel.chalDesc = panel.gameOb.transform.Find ("ChallengeDesc").GetComponent<Text> ();
            panel.chalDesc.text = GetChallenge (challengesToFinish[i]) ["Description"].ToString ().ToUpper ();
            panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(pheight / 2) - (pheight * i), 0);
            panel.chalName = panel.gameOb.transform.Find ("Header").Find ("ChallengeName").GetComponent<Text> ();
            panel.chalLevel = panel.gameOb.transform.Find ("Header").Find ("ChallengeLevel").GetComponent<Text> ();
            panel.chalName.text = GetChallenge (challengesToFinish[i]) ["Name"].ToString ().ToUpper ();
            panel.chalLevel.text = "LEVEL " + GetChallenge (challengesToFinish[i]) ["Level"].ToString ().ToUpper ();
            panel.coinReward = panel.gameOb.transform.Find ("Rewards").Find ("CoinReward").GetComponentInChildren<Text> ();
            panel.coinReward.text = GetChallenge (challengesToFinish[i]) ["coinReward"].ToString ();
            panel.clickText = panel.gameOb.transform.Find ("Click").Find ("ClickLabel").gameObject;
            if (i == 0) {
                panel.clickText.SetActive (true);
            } else {
                panel.clickText.SetActive (false);
            }
            panel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").gameObject.SetActive (false);
            panel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").gameObject.SetActive (true);
            panel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").GetComponent<ParticleSystem> ().Stop ();
            panels.Add (panel);
        }
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (pWidth, pheight * (challengesToFinish.Count));
        oneTimeHeight = -pheight * challengesToFinish.Count / 2f;
        scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, oneTimeHeight, 0);
        scrollBackground.transform.parent.GetComponent<ScrollRect> ().vertical = false;
        oneTimeHeight = -pheight * challengesToFinish.Count / 2f;
    }
    public static void AddCheck (int index) {
        ChallengesController.instance.readyToMove = false;
        //index is real index
        if (DataEntry.instance.challengesToFinish.Contains (index) == true) {
            DataEntry.instance.challengesToFinish.Remove (index);
        }
        if (DataEntry.instance.challengesCompleted.Contains (index) == false) {
            DataEntry.instance.challengesCompleted.Add (index);
        }
        // DataEntry.instance.Save ();
        for (int i = 0; i < instance.panels.Count; i++) {
            if (instance.panels[i].challenge == index) {
                instance.updateOrbPos = true;
                instance.updateCoinPos = true;
                instance.donePanel = instance.panels[i];
                instance.coinStartPosition = instance.panels[i].gameOb.transform.Find ("Rewards").Find ("CoinReward").Find ("UICoins").position;
                instance.coinReward = (int) GetChallenge (index) ["coinReward"];
                instance.panels[i].gameOb.transform.Find ("Checkbox").Find ("Checkmark").gameObject.SetActive (true);
                instance.panels[i].gameOb.transform.Find ("Click").gameObject.SetActive (false);
                instance.panels[i].gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").gameObject.SetActive (true);
                instance.panels[i].gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").GetComponent<ParticleSystem> ().Play ();
                if (i == instance.panels.Count - 1) {
                    instance.challengesFinished = true;
                } else {
                    MySingleton.Instance.StartCoroutine (instance.ChallengePanelMove (i));
                }
                if ((i + 1) < instance.panels.Count) {
                    instance.panels[i + 1].gameOb.transform.Find ("Click").Find ("ClickLabel").gameObject.SetActive (true);
                }
                break;
            }
        }
        FlyingCoinsGenerator.instance.GenerateChallengeReward (instance.coinReward);
        instance.readyToMove = false;
        //Add challenge to completed
        PlayFabClientAPI.GetUserData (new GetUserDataRequest {
            Keys = new List<string> {
                "ChallengesCompleted",
                "ChallengesFinished"
            }
        }, (GetUserDataResult res) => {
            List<int> newCompletedList = new List<int> ();
            List<int> newFinishedList = new List<int> ();
            string newChallengesCompleted;
            if ((res.Data["ChallengesCompleted"].Value.ToString ().Length == 1) && (res.Data["ChallengesCompleted"].Value[0].ToString () == "#")) {
                newChallengesCompleted = index.ToString ();
            } else {
                List<string> newList = ChallengesDecode.TurnIntoList (res.Data["ChallengesCompleted"].Value.ToString ());
                newList.Add (index.ToString ());
                newCompletedList = ChallengesDecode.TurnIntoIntList (newList);
                newChallengesCompleted = ChallengesDecode.TurnIntoString (newList);
            }
            string newChallengesFinished;
            if (ChallengesDecode.TurnIntoList (res.Data["ChallengesFinished"].Value.ToString ()).Count == 1) {
                newChallengesFinished = "#";
            } else {
                List<string> newList = ChallengesDecode.TurnIntoList (res.Data["ChallengesFinished"].Value.ToString ());
                newList.Remove (index.ToString ());
                newFinishedList = ChallengesDecode.TurnIntoIntList (newList);
                newChallengesFinished = ChallengesDecode.TurnIntoString (newList);
            }
            PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                    Data = new Dictionary<string, string> {
                        {
                            "ChallengesCompleted",
                            newChallengesCompleted
                        },
                        {
                            "ChallengesFinished",
                            newChallengesFinished
                        }
                    }
                },
                (UpdateUserDataResult ress) => {
                    //print ("added new challenge done");
                    DataEntry.instance.UpdateChallengesCompleted (newCompletedList);
                    DataEntry.instance.UpdateChallengesToFinish (newFinishedList);
                    DataEntry.instance.Save ();
                },
                instance.CallFailure
            );
        }, instance.CallFailure);
        //Add coins
        DataEntry.instance.totalCoins += instance.coinReward;
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
                        instance.coinReward.ToString ()
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            if (Debug.isDebugBuild) {
                for (int k = 0; k < res.Logs.Count; k++) {
                    //print (res.Logs[k].Message);
                }
            }
        }, instance.CallFailure);
    }
    IEnumerator ChallengePanelMove (int index) {
        while (readyToMove == false) {
            yield return null;
        }
        currentPosition = scrollBackground.GetComponent<RectTransform> ().anchoredPosition.y;
        nextPosition = (oneTimeHeight) + ((index + 1) * pheight);
        for (float i = 0; i <= 1; i += Time.deltaTime * 2.5f) {
            scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, Mathf.Lerp (currentPosition, nextPosition, i), 0);
            yield return null;
        }
        scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, nextPosition, 0);
        readyToMove = false;
    }
    public static void CompleteChallenge (int index) {
        if (ChallengeDone (index)) {
            if ((SceneManager.GetActiveScene ().name == "GameScene") || (SceneManager.GetActiveScene ().name == "CombinedScene")) {
                DataEntry.instance.challengesToFinish.Add (index);
                DataEntry.instance.Save ();
                instance.updateFinished = true;
                instance.challengeCompleted = true;
                instance.challengeCompleted2 = true;
                instance.showChallenges = true;
                instance.showChallenges2 = true;
                instance.challengeNotif = true;
                //Add challenge to finished
                PlayFabClientAPI.GetUserData (new GetUserDataRequest {
                    Keys = new List<string> {
                        "ChallengesFinished",
                        "ChallengesCompleted"
                    }
                }, (GetUserDataResult res) => {
                    List<string> completedChallengesList = ChallengesDecode.TurnIntoList (res.Data["ChallengesFinished"].Value.ToString ());
                    List<string> newList = ChallengesDecode.TurnIntoList (res.Data["ChallengesFinished"].Value.ToString ());
                    if ((newList.Contains (index.ToString ()) == false) && (completedChallengesList.Contains (index.ToString ()) == false)) {
                        string newChallengesFinished;
                        if ((newList.Count == 1) && (newList[0] == "#")) {
                            newChallengesFinished = index.ToString ();
                        } else {
                            newList.Add (index.ToString ());
                            newChallengesFinished = ChallengesDecode.TurnIntoString (newList);
                        }
                        PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                                Data = new Dictionary<string, string> {
                                    {
                                        "ChallengesFinished",
                                        newChallengesFinished
                                    }
                                }
                            },
                            (UpdateUserDataResult ress) => {
                                UserData.instance.UpdateInfo ();
                                ////print ("added new challenge to finished");
                            },
                            instance.CallFailure
                        );
                    } else {
                        if (Debug.isDebugBuild) {
                            //print ("ChallengesFinished already contains challenge index: " + index);
                        }
                    }
                }, instance.CallFailure);
            }
        } else {
            if (Debug.isDebugBuild) {
                //print ("Challenge already done");
            }
        }
    }
    public static bool ChallengeDone (int index) {
        if ((DataEntry.instance.challengesToFinish.Contains (index) == false) && DataEntry.instance.challengesCompleted.Contains (index) == false) {
            return true;
        } else {
            return false;
        }
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
}
public class Panel {
    public int challenge;
    public GameObject gameOb;
    public Text chalName;
    public Text chalLevel;
    public Text chalDesc;
    public Text coinReward;
    public GameObject clickText;
}