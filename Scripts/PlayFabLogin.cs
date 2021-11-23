using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.SceneManagement;
public class PlayFabLogin : MonoBehaviour {
    bool newAccount;
    List<int> ownedCars;
    public static PlayFabLogin instance;
    public int registerMode = 0;
    //0 is full registration w/ name setting
    //1 is only linking facebook account
    public bool loggedIn = false;
    int currencyReady = 0;
    private string FBToken;
    bool accountInfoCreated = false;
    bool referralsCreated = false;
    bool trailsReady = false;
    bool firstTime = true;
    Dictionary<string, string> defaultStats = new Dictionary<string, string> () { { "HighScoresToday", "0" }, { "ActualHighScoresToday", "0" }, { "DistanceTraveled", "0" }, { "DistanceDrifted", "0" }, { "CarsOwned", "1" }, { "TrailsOwned", "1" }, { "PlayersReferred", "0" }, { "CoinsCollected", "0" }, { "OrbsCollected", "0" }, { "ChallengesCompleted", "0" }, { "GamesPlayed", "0" },
    };
    bool statisticSet = false;
    public void Start () {
        DontDestroyOnLoad (this);
        instance = this;
        PlayFabSettings.TitleId = "16B7";
        if (firstTime) {
            if (FB.IsInitialized == false) {
                FB.Init (OnFacebookInitialized);
            } else {
                OnFacebookInitialized ();
            }
            firstTime = false;
        }
    }
    private void Update () {
        if (accountInfoCreated) {
            if (trailsReady) {
                if (referralsCreated) {
                    if (statisticSet) {
                        if (currencyReady == 3) {
                            statisticSet = false;
                            accountInfoCreated = false;
                            trailsReady = false;
                            currencyReady = 0;
                            referralsCreated = false;
                            if (DataEntry.instance.firstTimeOnDevice) {
                                DataEntry.instance.firstTimeOnDevice = false;
                                DataEntry.instance.Save ();
                                UserData.instance.UpdateForFirstTime (true);
                            } else {
                                UserData.instance.UpdateForFirstTime ();
                            }
                        }
                    }
                }
            }
        }
    }
    void OnFacebookInitialized () {
        //LoadingController.instance.AddProgress ();
        // //print ("fb init worked");
        FBToken = DataEntry.instance.FBTokenString;
        if (FBToken.Length > 1) {
            if (Debug.isDebugBuild) {
                //print ("Facebook Token Found");
            }
            PlayFabClientAPI.LoginWithFacebook (new LoginWithFacebookRequest { CreateAccount = true, AccessToken = FBToken }, OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        } else {
            //No Token Saved
            if (Debug.isDebugBuild) {
                //print ("No Facebook Token Found");
            }
#if UNITY_IOS
            var request = new LoginWithIOSDeviceIDRequest { CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, OS = Device.systemVersion, DeviceModel = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
#if UNITY_ANDROID
            var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, OS = SystemInfo.operatingSystem, AndroidDevice = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
            ////print ("Request Sent");
        }
    }
    void OnFacebookLoggedIn (ILoginResult result) {
        if (result == null || string.IsNullOrEmpty (result.Error)) {
            // No error while logging into Facebook
            DataEntry.instance.UpdateFBToken (AccessToken.CurrentAccessToken.TokenString);
            DataEntry.instance.Save ();
            PlayFabClientAPI.LoginWithFacebook (new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        } else {
            if (Debug.isDebugBuild) {
                //print ("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
            }
            //Logging in with iOS Device ID
#if UNITY_IOS
            var request = new LoginWithIOSDeviceIDRequest { CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, OS = Device.systemVersion, DeviceModel = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
#if UNITY_ANDROID
            var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, OS = SystemInfo.operatingSystem, AndroidDevice = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
        }
    }
    private void OnPlayfabFacebookAuthComplete (PlayFab.ClientModels.LoginResult result) {
        //print ("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        UserData.instance.UpdateInfo ();
        if (result.NewlyCreated == false) {
            loggedIn = true;
            //print ("fb account found");
            //UserData.instance.UpdateInfo ();
            PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest (), GetAccountInfoSuccess, OnPlayfabFacebookAuthFailed);
        } else {
            //No Facebook Account Found
            //print ("no fb account found");
            PlayFabClientAPI.ForgetAllCredentials ();
#if UNITY_IOS
            var request = new LoginWithIOSDeviceIDRequest { CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, OS = Device.systemVersion, DeviceModel = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
#if UNITY_ANDROID
            var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, OS = SystemInfo.operatingSystem, AndroidDevice = SystemInfo.deviceModel };
            PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
        }
    }
    void SuccessfulAccountInfo (GetAccountInfoResult result) {
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync ("CombinedScene");
    }
    private void OnPlayfabFacebookAuthFailed (PlayFabError error) {
        if (Debug.isDebugBuild) {
            //print ("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport ());
        }
        PlayFabClientAPI.ForgetAllCredentials ();
        DataEntry.instance.UpdateFBToken ("");
        DataEntry.instance.Save ();
        //print(error);
        //print ("no fb account found");
        PlayFabClientAPI.ForgetAllCredentials ();
#if UNITY_IOS
        var request = new LoginWithIOSDeviceIDRequest { CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, OS = Device.systemVersion, DeviceModel = SystemInfo.deviceModel };
        PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
#if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, OS = SystemInfo.operatingSystem, AndroidDevice = SystemInfo.deviceModel };
        PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSuccess, OnLoginFailure);
#endif
    }
    // //Debug.logWarning("Something went wrong with your link API call.  :(");
    ////Debug.log("Here's some debug information:");
    // //Debug.log(error.GenerateErrorReport());
    private void OnLoginSuccess (PlayFab.ClientModels.LoginResult result) {
        loggedIn = true;
        if (Debug.isDebugBuild) {
            //Debug.log ("Newly Created Account: " + result.NewlyCreated);
        }
        newAccount = result.NewlyCreated;
        //DEBUG
        if (Debug.isDebugBuild) {
            ////Debug.log ("Newly Create Accounted Set To True");
            //newAccount = true;
        }
        if (newAccount) {
            //New Account
            //Creates user info
            CreateGuestAccount ();
        } else {
            //Not New Account
            //Gets user info
            PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest (), GetAccountInfoSuccess, GetAccountInfoFailure);
        }
    }
    public void CreateGuestAccount (bool newUsername = true) {
        referralsCreated = false;
        accountInfoCreated = false;
        currencyReady = 0;
        trailsReady = false;
        string guestUsername = "Guest";
        string guestID = UnityEngine.Random.Range (0, 999999).ToString ("000000");
        ////print(guestID);
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = guestUsername + guestID
        };
        if (!newUsername) {
            try {
                request.DisplayName = UserData.instance.displayName;
            } catch {
                request.DisplayName = "";
            }
        }
        PlayFabClientAPI.UpdateUserTitleDisplayName (request, (UpdateUserTitleDisplayNameResult titleResult) => {
            PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                Data = FillMissingController.instance.UserDataDefaultValues
            }, (UpdateUserDataResult tresult) => {
                //print ("User succesfully created");
                PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (),
                    (GetUserInventoryResult result) => {
                        ////print ("Get User Inventory: ");
                        //print (result.Inventory.Count);
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
                                    }, (ExecuteCloudScriptResult res) => {
                                        if (Debug.isDebugBuild) {
                                            for (int k = 0; k < res.Logs.Count; k++) {
                                                //print (res.Logs[k].Message);
                                            }
                                        }
                                    }, (PlayFabError error) => {
                                        if (Debug.isDebugBuild) {
                                            //print (error);
                                        }
                                    });
                                } else {
                                    if (carExists == false) {
                                        carExists = true;
                                    } else {
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
                                        }, (PlayFabError error) => {
                                            if (Debug.isDebugBuild) {
                                                //print (error);
                                            }
                                        });
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
                                        for (int k = 0; k < res.Logs.Count; k++) {
                                            //print (res.Logs[k].Message);
                                        }
                                    }, (PlayFabError error) => {
                                        //print (error);
                                    });
                                } else {
                                    if (Debug.isDebugBuild) {
                                        //print ("trail found");
                                    }
                                    if (trailExists == false) {
                                        trailExists = true;
                                    } else {
                                        if (Debug.isDebugBuild) {
                                            //print ("extra trail1");
                                        }
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
                                        }, (PlayFabError error) => {
                                            if (Debug.isDebugBuild) {
                                                //print (error);
                                            }
                                        });
                                    }
                                }
                            }
                        }
                        if (trailExists == false) {
                            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                                ItemId = "T0001",
                                    Price = 0,
                                    VirtualCurrency = "OB",
                            }, (PurchaseItemResult purchRes) => {
                                trailsReady = true;
                            }, (PlayFabError error) => {
                                if (Debug.isDebugBuild) {
                                    //print (error);
                                }
                            });
                        } else {
                            trailsReady = true;
                        }
                        if (carExists == false) {
                            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                                ItemId = "C0002",
                                    Price = 0,
                                    VirtualCurrency = "CN",
                            }, CreateAccountInfoSuccess, (PlayFabError error) => {
                                if (Debug.isDebugBuild) {
                                    //print (error);
                                }
                            });
                        } else {
                            accountInfoCreated = true;
                        }
                        Dictionary<string, List<string>> data = new Dictionary<string, List<string>> { { "Referrals", new List<string> () }
                        };
                        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                            FunctionName = "CreateReadOnlyData",
                                FunctionParameter = new Dictionary<string, Dictionary<string, List<string>>> {
                                    {
                                        "Data",
                                        data
                                    }
                                }
                        }, (ExecuteCloudScriptResult res) => {
                            referralsCreated = true;
                            /*
                            for (int k = 0; k < res.Logs.Count; k++) {
                               //print (res.Logs[k].Message);
                            }*/
                        }, (PlayFabError error) => {
                            if (Debug.isDebugBuild) {
                                error.GenerateErrorReport ();
                            }
                        });
                        PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                            FunctionName = "SetStatistics",
                                FunctionParameter = defaultStats
                        }, (ExecuteCloudScriptResult res) => {
                            // //print ("Distance Traveled");
                            statisticSet = true;
                            if (Debug.isDebugBuild) {
                                for (int k = 0; k < res.Logs.Count; k++) {
                                    //print (res.Logs[k].Message);
                                }
                            }
                        }, (PlayFabError error) => {
                            if (Debug.isDebugBuild) {
                                //print (error);
                            }
                        });
                    }, (PlayFabError error) => {
                        if (Debug.isDebugBuild) {
                            //print (error);
                        }
                    });
            }, CreateAccountInfoFailure);
        }, (PlayFabError error) => {
            if (Debug.isDebugBuild) {
                //print (error);
            }
            switch (error.Error) {
                case PlayFabErrorCode.NameNotAvailable:
                    CreateGuestAccount ();
                    break;
            }
        });
        //Reset Currency
        PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (), (GetUserInventoryResult result) => {
                if (result.VirtualCurrency["CN"] > 0) {
                    PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                        VirtualCurrency = "CN",
                            Amount = result.VirtualCurrency["CN"]
                    }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                        ////print ("coins reset");
                        currencyReady++;
                    }, (PlayFabError error) => {
                        if (Debug.isDebugBuild) {
                            //print (error);
                        }
                        currencyReady++;
                    });
                } else {
                    currencyReady++;
                }
                if (result.VirtualCurrency["BL"] > 0) {
                    PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                        VirtualCurrency = "BL",
                            Amount = result.VirtualCurrency["BL"]
                    }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                        ////print ("balance reset");
                        currencyReady++;
                    }, (PlayFabError error) => {
                        //print (error);
                        currencyReady++;
                    });
                } else {
                    currencyReady++;
                }
                if (result.VirtualCurrency["OB"] > 0) {
                    PlayFabClientAPI.SubtractUserVirtualCurrency (new SubtractUserVirtualCurrencyRequest {
                        VirtualCurrency = "OB",
                            Amount = result.VirtualCurrency["OB"]
                    }, (ModifyUserVirtualCurrencyResult modifyResult) => {
                        //  //print ("orbs reset");
                        currencyReady++;
                    }, (PlayFabError error) => {
                        //print (error);
                        currencyReady++;
                    });
                } else {
                    currencyReady++;
                }
            },
            (PlayFabError error) => {
                if (Debug.isDebugBuild) {
                    //print (error);
                }
            });
    }
    void CreateAccountInfoSuccess (PurchaseItemResult res) {
        accountInfoCreated = true;
    }
    void CreateAccountInfoFailure (PlayFabError error) {
        if (Debug.isDebugBuild) {
            //print (error);
        }
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync ("TutorialScene");
    }
    void GetAccountInfoSuccess (GetAccountInfoResult result) {
        //LoadingController.instance.AddProgress ();
        //print ("GetAccountInfoSuccess Run");
        UserData.instance.UpdateForFirstTime ();
    }
    void GetAccountInfoFailure (PlayFabError error) {
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync ("RegisterScene");
        if (Debug.isDebugBuild) {
            //print ("failure");
            //print (error);
        }
    }
    private void OnLoginFailure (PlayFabError error) {
        if (Debug.isDebugBuild) {
            //print ("Login Error");
        }
        //  //Debug.logWarning ("Something went wrong with your login  :(");
        // //Debug.log ("Here's some debug information:");
        ////Debug.log (error.GenerateErrorReport ());
        if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
            //no connection
            ConnectionController.instance.noConnectionDetected ();
        } else {
            if (Debug.isDebugBuild) {
                //print (error);
            }
        }
    }
    /*IEnumerator NoConnection () {
        CoroutineWithData cd = new CoroutineWithData (this, WWWPic (kpr.Value));
        yield return cd.coroutine;
    }*/
}