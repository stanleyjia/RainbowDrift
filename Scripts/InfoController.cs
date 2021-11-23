using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InfoController : MonoBehaviour {
    //public Text FacebookAccountName;
    public Text AccountBalance;
    public Text PlayFabUsername;
    public bool saveVolume = false;
    public bool infoLoaded = false;
    public static InfoController instance;
    public string FBFullName;
    public Button FacebookButton;
    public Text FacebookButtonText;
    public Text LoginText;
    bool newAccount;
    float balance;
    List<string> keys;
    //public GameObject HomeButton;
    public Animator confirmLogout;
    public Animator noPress;
    public Animator moreInfo;
    public Animator comingSoon;
    GameObject loadingPanel;
    int currencyReady = 0;
    bool accountInfoCreated = false;
    bool friendsRemoved = false;
    bool referralsCreated = false;
    bool statisticSet = false;
    bool trailsReady = false;
    public Text referralCodeText;
    List<string> tags;
    public ScrollRect combinedScrollRect;
    int tagCount;
    Dictionary<string, string> defaultStats = new Dictionary<string, string> () { { "HighScoresToday", "0" }, { "ActualHighScoresToday", "0" }, { "DistanceTraveled", "0" }, { "DistanceDrifted", "0" }, { "CarsOwned", "1" }, { "TrailsOwned", "1" }, { "PlayersReferred", "0" }, { "CoinsCollected", "0" }, { "OrbsCollected", "0" }, { "ChallengesCompleted", "0" }, { "GamesPlayed", "0" },
    };
    bool tokenSent;
    // Use this for initialization
    void Start () {
        loadingPanel = UserData.instance.loadingPanel;
        noPress.gameObject.SetActive (false);
        instance = this;
        SetInfo ();
        // HomeButton.SetActive (true);
        AccountBalance.text = "$" + DataEntry.instance.balance.ToString ("F2");
        tokenSent = false;
        UnityEngine.iOS.NotificationServices.RegisterForNotifications (UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound, true);
    }
    void SetFacebookButtonText (bool active) {
        if (active) {
            FacebookButtonText.color = new Color (FacebookButtonText.color.r, FacebookButtonText.color.g, FacebookButtonText.color.b, 1f);
        } else {
            FacebookButtonText.color = new Color (FacebookButtonText.color.r, FacebookButtonText.color.g, FacebookButtonText.color.b, 0.5f);
        }
    }
    void SetInfo () {
        if (DataEntry.instance.UsernameSet) {
            if (DataEntry.instance.FBLinked) {
                FacebookButtonText.text = "UNLINK FACEBOOK";
                SetFacebookButtonText (false);
                FBFullName = UserData.instance.FBFullName;
                referralCodeText.text = UserData.instance.playfabId;
            } else {
                FacebookButtonText.text = "LINK FACEBOOK";
                SetFacebookButtonText (true);
                referralCodeText.text = "LINK FACEBOOK TO REFER";
            }
        } else {
            FacebookButtonText.text = "CHOOSE USERNAME";
            SetFacebookButtonText (true);
            referralCodeText.text = "CHOOSE USERNAME TO REFER";
        }
        PlayFabUsername.text = DataEntry.instance.displayName;
        //AvatarController.instance.SetChoosePics (DataEntry.instance.AvatarInd);
    }
    public void LinkFacebookAccount () {
        if (DataEntry.instance.FBLinked) {
            //Unlink facebook
            PlayFabClientAPI.UnlinkFacebookAccount (new UnlinkFacebookAccountRequest (), (UnlinkFacebookAccountResult res) => {
                var request = new LinkIOSDeviceIDRequest { DeviceId = SystemInfo.deviceUniqueIdentifier, ForceLink = true };
                PlayFabClientAPI.LinkIOSDeviceID (request, (LinkIOSDeviceIDResult result) => {
                    if (Debug.isDebugBuild) {
                        //print ("Unlink Success");
                    }
                    loadingPanel.SetActive (true);
                    LoadingTextAnim.instance.StartAnimate ();
                    UserData.instance.UpdateForFirstTime ();
                }, CallFailure);
            }, CallFailure);
        } else {
            if (DataEntry.instance.UsernameSet) {
                //print ("Username Set: " + DataEntry.instance.UsernameSet);
                PlayFabLogin.instance.registerMode = 0;
            } else {
                PlayFabLogin.instance.registerMode = 2;
            }
            GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
            ChangeScene.instance.changeScene ("RegisterScene");
        }
    }
    void CallFailure (PlayFabError error) {
        if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
            //no connection
            ConnectionController.instance.noConnectionDetected ();
        } else {
            if (Debug.isDebugBuild) {
                print (error);
            }
        }
    }
    // Update is called once per frame
    void Update () {
        if (!tokenSent) {
            byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
            if (token != null) {
                print ("Sent once_");
                tokenSent = true;
                RegisterForIOSPushNotificationRequest request = new RegisterForIOSPushNotificationRequest ();
                request.DeviceToken = System.BitConverter.ToString (token).Replace ("-", "").ToLower ();
                PlayFabClientAPI.RegisterForIOSPushNotification (request, (RegisterForIOSPushNotificationResult result) => {
                    Debug.Log ("Push Registration Successful");
                }, CallFailure);
            }
        }
        if (accountInfoCreated) {
            if (trailsReady) {
                if (referralsCreated) {
                    if (statisticSet) {
                        if (friendsRemoved) {
                            if (currencyReady == 3) {
                                accountInfoCreated = false;
                                statisticSet = false;
                                currencyReady = 0;
                                UserData.instance.UpdateForFirstTime ();
                            }
                        }
                    }
                }
            }
        }
    }
    IEnumerator WaitUntilLoadScene (string sceneName, float amount) {
        yield
        return new WaitForSeconds (amount);
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync (sceneName);
    }
    public void ViewTutorial () {
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        //SceneManager.LoadSceneAsync ("TutorialScene");
        ChangeScene.instance.changeScene ("TutorialScene");
    }
    public void LogOut () {
        if (DataEntry.instance.FBLinked) {
            loadingPanel.SetActive (true);
            LoadingTextAnim.instance.StartAnimate ();
            PlayFabClientAPI.ForgetAllCredentials ();
            DataEntry.instance.UpdateFBToken ("");
            DataEntry.instance.UpdateFacebookLinked (false);
            DataEntry.instance.Save ();
            if (DataEntry.instance.hapticOn) {
                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
            }
#if UNITY_IOS
            var request = new LoginWithIOSDeviceIDRequest { CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, OS = Device.systemVersion, DeviceModel = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
#if UNITY_ANDROID
            var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, OS = SystemInfo.operatingSystem, AndroidDevice = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
            // FB.LogOut();
        } else {
            ////print("Warning: Account not linked");
            if (DataEntry.instance.hapticOn) {
                //print("Account will be lost");
                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 5);
                confirmLogout.SetTrigger ("Show");
            }
            noPress.gameObject.SetActive (true);
            combinedScrollRect.horizontal = false;
        }
    }
    public void CancelLogout () {
        confirmLogout.SetTrigger ("Hide");
        noPress.SetTrigger ("FadeOut");
        combinedScrollRect.horizontal = true;
    }
    void CreateGuestAccount () {
        friendsRemoved = false;
        referralsCreated = false;
        accountInfoCreated = false;
        //confirmLogout.SetTrigger ("Hide");
        //confirmLogout.Play ("Off");
        // noPress.SetTrigger ("FadeOut");
        combinedScrollRect.horizontal = false;
        currencyReady = 0;
        trailsReady = false;
        string guestUsername = "Guest";
        string guestID = UnityEngine.Random.Range (0, 999999).ToString ("000000");
        ////print(guestID);
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = guestUsername + guestID
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName (request, (UpdateUserTitleDisplayNameResult titleResult) => {
            PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                Data = FillMissingController.instance.UserDataDefaultValues
            }, (UpdateUserDataResult tresult) => {
                //print ("Info succesfully created");
                PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (),
                    (GetUserInventoryResult result) => {
                        StartCoroutine (HandleUserInventory (result));
                    }, CallFailure);
            }, CreateAccountInfoFailure);
            ReadData thisData = new ReadData ();
            thisData.Referrals = "[]";
            //print (JsonUtility.ToJson (thisData));
            //print ("cloud run");
            PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "CreateReadOnlyData",
                    FunctionParameter = new Dictionary<string, string> {
                        {
                            "Data",
                            JsonUtility.ToJson (thisData)
                        }
                    }
            }, (ExecuteCloudScriptResult res) => {
                referralsCreated = true;
                if (Debug.isDebugBuild) {
                    for (int k = 0; k < res.Logs.Count; k++) {
                        //   //print (res.Logs[k].Message);
                    }
                }
            }, CallFailure);
            PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "SetStatistics",
                    FunctionParameter = defaultStats
            }, (ExecuteCloudScriptResult res) => {
                statisticSet = true;
            }, CallFailure);
        }, (PlayFabError error) => {
            // //print (error);
            switch (error.Error) {
                case PlayFabErrorCode.NameNotAvailable:
                    CreateGuestAccount ();
                    break;
            }
            CallFailure (error);
        });
        var req = new GetPlayerTagsRequest {
            PlayFabId = UserData.instance.playfabId
        };
        PlayFabClientAPI.GetPlayerTags (req, (GetPlayerTagsResult result) => {
            tags = result.Tags;
            if (result.Tags.Count > 0) {
                tagCount = result.Tags.Count;
                StartCoroutine (RemoveTags (tagCount));
            } else {
                StartCoroutine (AddTags ());
            }
        }, CallFailure);
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "RemoveAllFriends"
        }, (ExecuteCloudScriptResult res) => {
            friendsRemoved = true;
            if (Debug.isDebugBuild) {
                for (int k = 0; k < res.Logs.Count; k++) {
                    print (res.Logs[k].Message);
                }
            }
        }, CallFailure);
    }
    IEnumerator AddTags () {
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "AddPlayerTag",
                FunctionParameter = new Dictionary<string, string> {
                    {
                        "tagName",
                        "C0002"
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            for (int k = 0; k < res.Logs.Count; k++) {
                //print (res.Logs[k].Message);
            }
        }, CallFailure);
        yield return new WaitForSeconds (1f);
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "AddPlayerTag",
                FunctionParameter = new Dictionary<string, string> {
                    {
                        "tagName",
                        "T0001"
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            if (Debug.isDebugBuild) {
                for (int k = 0; k < res.Logs.Count; k++) {
                    //print (res.Logs[k].Message);
                }
            }
        }, CallFailure);
        yield return new WaitForSeconds (1f);
        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
            FunctionName = "AddPlayerTag",
                FunctionParameter = new Dictionary<string, string> {
                    {
                        "tagName",
                        "avatar:fox"
                    }
                }
        }, (ExecuteCloudScriptResult res) => {
            if (Debug.isDebugBuild) {
                for (int k = 0; k < res.Logs.Count; k++) {
                    //print (res.Logs[k].Message);
                }
            }
        }, CallFailure);
        ////print ("tags added");
    }
    IEnumerator RemoveTags (int count) {
        for (int i = 0; i < count; i++) {
            PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "RemovePlayerTag",
                    FunctionParameter = new Dictionary<string, string> {
                        {
                            "tagName",
                            tags[i]
                        }
                    }
            }, (ExecuteCloudScriptResult res) => {
                if (Debug.isDebugBuild) {
                    for (int k = 0; k < res.Logs.Count; k++) {
                        //print (res.Logs[k].Message);
                    }
                }
            }, CallFailure);
            yield return new WaitForSeconds (1f);
        }
        StartCoroutine (AddTags ());
    }
    public void LogoutForReal () {
        confirmLogout.Play ("Off");
        noPress.gameObject.SetActive (false);
        loadingPanel.SetActive (true);
        LoadingTextAnim.instance.StartAnimate ();
        PlayFabClientAPI.UnlinkIOSDeviceID (new UnlinkIOSDeviceIDRequest (), (UnlinkIOSDeviceIDResult res) => {
            CreateGuestAccount ();
        }, (PlayFabError error) => {
            switch (error.Error) {
                case PlayFabErrorCode.AccountNotLinked:
                    //UserData.instance.UpdateForFirstTime ();
                    CreateGuestAccount ();
                    break;
                case PlayFabErrorCode.ServiceUnavailable:
                    //no connection
                    ConnectionController.instance.noConnectionDetected ();
                    break;
                default:
                    if (Debug.isDebugBuild) {
                        //print (error);
                    }
                    break;
            }
        });
    }
    private void OnLoginSuccess (PlayFab.ClientModels.LoginResult result) {
        //  HomeButton.SetActive (false);
        CreateGuestAccount ();
    }
    private void OnLoginFailure (PlayFabError error) {
        //Debug.logWarning ("Something went wrong with your login  :(");
        //Debug.log ("Here's some debug information:");
        //Debug.log (error.GenerateErrorReport ());
    }
    void CreateAccountInfoFailure (PlayFabError error) {
        if (Debug.isDebugBuild) {
            //print (error);
        }
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync ("TutorialScene");
    }
    void CreateAccountInfoSuccess (PurchaseItemResult result) {
        // //print ("First Car Bought!");
        UserData.instance.UpdateInfo ();
        accountInfoCreated = true;
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        //SceneManager.LoadSceneAsync("TutorialScene");
        SceneManager.LoadSceneAsync ("CombinedScene");
    }
    IEnumerator HandleUserInventory (GetUserInventoryResult result) {
        bool carExists = false;
        bool trailExists = false;
        for (int i = 0; i < result.Inventory.Count; i++) {
            if (result.Inventory[i].ItemId.Substring (0, 1) == "C") {
                //Car
                if (result.Inventory[i].ItemId != "C0002") {
                    PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                        FunctionName = "revokeInventoryItem",
                            FunctionParameter = new Dictionary<string, string> {
                                {
                                    "ItemInstanceId",
                                    result.Inventory[i].ItemInstanceId
                                }
                            }
                    }, (ExecuteCloudScriptResult res) => { }, CallFailure);
                } else {
                    // //print ("car1 found");
                    if (carExists == false) {
                        carExists = true;
                    } else {
                        // //print ("extra car1");
                        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                            FunctionName = "revokeInventoryItem",
                                FunctionParameter = new Dictionary<string, string> {
                                    {
                                        "ItemInstanceId",
                                        result.Inventory[i].ItemInstanceId
                                    }
                                }
                        }, (ExecuteCloudScriptResult res) => {
                            if (Debug.isDebugBuild) {
                                for (int k = 0; k < res.Logs.Count; k++) {
                                    //print (res.Logs[k].Message);
                                }
                            }
                        }, CallFailure);
                    }
                }
            } else if (result.Inventory[i].ItemId.Substring (0, 1) == "T") {
                if (result.Inventory[i].ItemId != "T0001") {
                    PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                        FunctionName = "revokeInventoryItem",
                            FunctionParameter = new Dictionary<string, string> {
                                {
                                    "ItemInstanceId",
                                    result.Inventory[i].ItemInstanceId
                                }
                            }
                    }, (ExecuteCloudScriptResult res) => {
                        if (Debug.isDebugBuild) {
                            for (int k = 0; k < res.Logs.Count; k++) {
                                //print (res.Logs[k].Message);
                            }
                        }
                    }, CallFailure);
                } else {
                    ////print ("trail found");
                    if (trailExists == false) {
                        trailExists = true;
                    } else {
                        //print ("extra trail1");
                        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                            FunctionName = "revokeInventoryItem",
                                FunctionParameter = new Dictionary<string, string> {
                                    {
                                        "ItemInstanceId",
                                        result.Inventory[i].ItemInstanceId
                                    }
                                }
                        }, (ExecuteCloudScriptResult res) => {
                            if (Debug.isDebugBuild) {
                                for (int k = 0; k < res.Logs.Count; k++) {
                                    //print (res.Logs[k].Message);
                                }
                            }
                        }, CallFailure);
                    }
                }
            }
            yield return new WaitForSeconds (1f);
        }
        if (trailExists == false) {
            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                ItemId = "T0001",
                    Price = 0,
                    VirtualCurrency = "OB",
            }, (PurchaseItemResult purchRes) => {
                trailsReady = true;
            }, CallFailure);
        } else {
            trailsReady = true;
        }
        yield return new WaitForSeconds (0.5f);
        if (carExists == false) {
            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                ItemId = "C0002",
                    Price = 0,
                    VirtualCurrency = "CN",
            }, CreateAccountInfoSuccess, CallFailure);
        } else {
            accountInfoCreated = true;
        }
        yield return new WaitForSeconds (1f);
        if (result.VirtualCurrency["CN"] > 0) {
            PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                VirtualCurrency = "CN",
                    Amount = result.VirtualCurrency["CN"]
            }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                //print ("coins reset");
                currencyReady++;
            }, (PlayFabError error) => {
                CallFailure (error);
                currencyReady++;
            });
        } else {
            currencyReady++;
        }
        yield return new WaitForSeconds (1f);
        if (result.VirtualCurrency["BL"] > 0) {
            PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                VirtualCurrency = "BL",
                    Amount = result.VirtualCurrency["BL"]
            }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                //print ("balance reset");
                currencyReady++;
            }, (PlayFabError error) => {
                CallFailure (error);
                currencyReady++;
            });
        } else {
            currencyReady++;
        }
        yield return new WaitForSeconds (1f);
        if (result.VirtualCurrency["OB"] > 0) {
            PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                VirtualCurrency = "OB",
                    Amount = result.VirtualCurrency["OB"]
            }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                ////print ("orbs reset");
                currencyReady++;
            }, (PlayFabError error) => {
                CallFailure (error);
                if (Debug.isDebugBuild) {
                    //print (error);
                }
                currencyReady++;
            });
        } else {
            currencyReady++;
        }
    }
    public void Cashout () {
        PlayAudio.PlaySound ("error");
        StartCoroutine (Warning ());
    }
    IEnumerator Warning () {
        comingSoon.SetTrigger ("Show");
        yield return new WaitForSeconds (0.5f);
        yield return new WaitForSeconds (0.5f);
        comingSoon.SetTrigger ("Hide");
    }
    public void SetAvatarUrl (int index, string url) {
        ////print("avatarurl set");
        string indexString = index.ToString ();
        PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                Data = new Dictionary<string, string> {
                    {
                        "AvatarUrl",
                        url
                    },
                    {
                        "AvatarInd",
                        indexString
                    }
                }
            },
            (UpdateUserDataResult res) => {
                UserData.instance.UpdateInfo ();
            },
            CallFailure);
    }
    public void ViewWebsite () {
        Application.OpenURL ("https://mooseparkstudios.com");
    }
    public void SendEmail () {
        Application.OpenURL ("mailto:admin@mooseparkstudios.com?subject=&body=");
    }
    public void ViewPrivacyPolicy () {
        Application.OpenURL ("https://mooseparkstudios.com/privacy.html");
    }
}
[Serializable]
public class ReadData {
    // public string Referrals;
    public string Referrals;
}