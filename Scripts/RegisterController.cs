using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;
using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RegisterController : MonoBehaviour {
    public Animator prevPanel;
    public Animator namePanel;
    public Animator linkPanel;
    public InputField nameInput;
    public Animator referralPanel;
    public InputField referralInput;
    public Animator confirmExitPanel;
    public Animator error;
    public Text errorText;
    public Animator FBLinked;
    public Animator NoPress;
    bool showingError = false;
    List<string> illegalStrings = new List<string> {
        "fuck",
        "shit",
        "ass",
        "pussy",
        "dick",
        "sex",
        "penis",
        "vagina",
        "damn",
        "crap",
        "piss",
        "cock",
        "asshole",
        "fag",
        "bastard",
        "slut",
        "douche",
        "kill",
        "gay",
        "chink",
        "nigga",
        "nigger",
        "faggot",
        "slut",
        "hoe",
        "whore",
        "fag",
        "cunt",
        "cock"
    };
    List<string> legalSymbols = new List<string> {
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "_",
        "-",
        "/",
        ".",
        ",",
    };
    int mode = 0;
    // Use this for initialization
    void Start () {
        mode = PlayFabLogin.instance.registerMode;
        //mode = 1;
        switch (mode) {
            case 0:
                StartCoroutine (DelayedShow (linkPanel));
                //Ask to link Facebook
                break;
            case 1:
                StartCoroutine (DelayedShow (prevPanel));
                //Ask for previous facebook account
                break;
            case 2:
                //Ask for username
                StartCoroutine (DelayedShow (namePanel));
                break;
            default:
                StartCoroutine (DelayedShow (prevPanel));
                break;
        }
        NoPress.gameObject.SetActive (false);
        //Debug
        // StartCoroutine (DelayedShow (prevPanel));
    }
    IEnumerator DelayedShow (Animator animator) {
        yield return new WaitForSeconds (0.4f);
        animator.Play ("Show");
    }
    public void CreateNewAccount () {
        StartCoroutine (HidePrevShowName ());
    }
    public void FirstLoginWithFacebook () {
        //prevPanel.SetActive (false);
        prevPanel.SetTrigger ("Hide");
        FB.LogInWithReadPermissions (null, OnFacebookLoggedInLogin);
    }
    void ShowError (int errorCode) {
        switch (errorCode) {
            case 0:
                //Already Linked
                errorText.text = "ACCOUNT ALREADY LINKED";
                break;
            case 1:
                //Facebook api error
                errorText.text = "FACEBOOK API ERROR";
                break;
            case 2:
                //Facebook token error
                errorText.text = "INVALID FACEBOOK TOKEN";
                break;
            case 3:
                //Facebook not found
                errorText.text = "FACEBOOK ACCOUNT NOT FOUND";
                break;
            case 4:
                //length
                errorText.text = "USERNAME HAS TO BE BETWEEN 5-15 CHARACTERS";
                break;
            case 5:
                //SPACE
                errorText.text = "USERNAME CANNOT CONTAIN SPACES";
                break;
            case 6:
                //SYMBOLS
                errorText.text = "USERNAME CAN ONLY CONTAIN ABCs, NUMBERS, DASHES, AND UNDERSCORES";
                break;
            case 7:
                //SWEAR
                errorText.text = "USERNAME CANNOT CONTAIN SWEAR WORDS";
                break;
            case 8:
                //Taken
                errorText.text = "USERNAME TAKEN";
                break;
            case 9:
                //Taken
                errorText.text = "CODE NOT VALID";
                break;
            case 10:
                //Referring yourself
                errorText.text = "CANNOT REFER YOURSELF";
                break;
        }
        StartCoroutine (ErrorAnim ());
    }
    bool CheckString (string input) {
        if ((input.Length >= 5) && (input.Length <= 15)) {
            if (!input.Contains (" ")) {
                //Check for illegal symbols
                for (int i = 0; i < input.Length; i++) {
                    if (!legalSymbols.Contains (input[i].ToString ())) {
                        //illegal symbols
                        ShowError (6);
                        return false;
                    }
                }
                //Check for illegal strings
                for (int i = 0; i < illegalStrings.Count; i++) {
                    if (input.Contains (illegalStrings[i])) {
                        //illegal strings
                        ShowError (7);
                        return false;
                    }
                }
            } else {
                //Space in name
                ShowError (5);
                return false;
            }
        } else {
            //length
            ShowError (4);
            return false;
        }
        return true;
    }
    IEnumerator ErrorAnim () {
        NoPress.gameObject.SetActive (true);
        showingError = true;
        error.SetTrigger ("Show");
        if (DataEntry.instance.hapticOn) {
            iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 6);
        }
        yield return new WaitForSeconds (1f);
        error.SetTrigger ("Hide");
        NoPress.SetTrigger ("FadeOut");
        yield return new WaitForSeconds (0.5f);
        showingError = false;
    }
    private void OnFacebookLoggedInLink (ILoginResult result) {
        if (result == null || string.IsNullOrEmpty (result.Error)) {
            if (Debug.isDebugBuild) {
                //print ("FB token saved");
            }
            try {
                DataEntry.instance.UpdateFBToken (AccessToken.CurrentAccessToken.TokenString);
                DataEntry.instance.Save ();
                LinkFacebookAccountRequest fbRequest = new LinkFacebookAccountRequest { AccessToken = AccessToken.CurrentAccessToken.TokenString, ForceLink = false };
                // //print(AccessToken.CurrentAccessToken.TokenString);
                PlayFabClientAPI.LinkFacebookAccount (fbRequest, OnLinkSuccess, OnLinkFailure);
            } catch { }
            //Where error is right now
        } else {
            if (Debug.isDebugBuild) {
                //print ("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
            }
            ShowError (1);
            StartCoroutine (ShowLink ());
        }
    }
    private void OnFacebookLoggedInLogin (ILoginResult result) {
        if (result == null || string.IsNullOrEmpty (result.Error)) {
            try {
                DataEntry.instance.UpdateFBToken (AccessToken.CurrentAccessToken.TokenString);
                DataEntry.instance.Save ();
                if (Debug.isDebugBuild) {
                    //print ("FB token saved");
                }
                PlayFabClientAPI.LoginWithFacebook (new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                    OnPlayfabFacebookAuthComplete, CallFailure);
            } catch {
                if (Debug.isDebugBuild) {
                    //print ("Access Token not found");
                }
                StartCoroutine (ShowPrev ());
            }
        } else {
            // StartCoroutine (ShowForSeconds (FBAPIError));
            ShowError (1);
            StartCoroutine (ShowPrev ());
            //StartCoroutine (BackToCombinedScene ());
            //print ("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
        }
    }
    private void OnLinkSuccess (LinkFacebookAccountResult result) {
        //   //Debug.log ("Facebook account linked");
        //Unlink ios
        PlayFabClientAPI.UnlinkIOSDeviceID (new UnlinkIOSDeviceIDRequest (), OnUnlinkSuccess, OnLinkFailure);
    }
    void OnUnlinkSuccess (UnlinkIOSDeviceIDResult result) {
        //   //Debug.log ("iOS Device successfully unlinked from account");
        StartCoroutine (HideLinkShowReferral ());
    }
    IEnumerator BackToCombinedScene () {
        yield return new WaitForSeconds (2.5f);
        if (UserData.instance.tutorialFromLogin) {
            UserData.instance.combinedIndex = 1;
            ChangeScene.instance.changeScene ("CombinedScene");
        } else {
            UserData.instance.combinedIndex = 0;
            ChangeScene.instance.changeScene ("CombinedScene");
        }
        UserData.instance.tutorialFromLogin = false;
    }
    private void OnLinkFailure (PlayFabError error) {
        switch (error.Error) {
            case PlayFabErrorCode.AccountNotLinked:
                if (UserData.instance.tutorialFromLogin) {
                    UserData.instance.combinedIndex = 1;
                    ChangeScene.instance.changeScene ("CombinedScene");
                } else {
                    UserData.instance.combinedIndex = 0;
                    ChangeScene.instance.changeScene ("CombinedScene");
                }
                UserData.instance.tutorialFromLogin = false;
                break;
            case PlayFabErrorCode.AccountAlreadyLinked:
                ShowError (0);
                break;
            case PlayFabErrorCode.LinkedAccountAlreadyClaimed:
                NoPress.gameObject.SetActive (true);
                FBLinked.SetTrigger ("Show");
                break;
            case PlayFabErrorCode.FacebookAPIError:
                ShowError (1);
                break;
            case PlayFabErrorCode.InvalidFacebookToken:
                ShowError (2);
                break;
            case PlayFabErrorCode.ServiceUnavailable:
                //no connection
                ConnectionController.instance.noConnectionDetected ();
                break;
        }
    }
    private void OnPlayfabFacebookAuthComplete (LoginResult result) {
        ////print ("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        UserData.instance.UpdateInfo ();
        if (result.NewlyCreated == false) {
            PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest (), SuccessfulAccountInfo, CallFailure);
        } else {
            //No Facebook Account Found
            ShowError (3);
        }
    }
    void SuccessfulAccountInfo (GetAccountInfoResult result) {
        try {
            //iOS Device Linked ==> Bad
            if (result.AccountInfo.IosDeviceInfo.IosDeviceId != null) {
                //print ("ios device linked");
                PlayFabClientAPI.UnlinkIOSDeviceID (new UnlinkIOSDeviceIDRequest (), OnUnlinkSuccess, OnLinkFailure);
            }
        } catch (NullReferenceException) {
            //No iOS Device Linked ==> Good
            if (Debug.isDebugBuild) {
                //print ("no ios device linked");
            }
            //   ChangeScene.instance.changeScene ("CombinedScene");
        }
        if (UserData.instance.tutorialFromLogin) {
            UserData.instance.combinedIndex = 1;
            ChangeScene.instance.changeScene ("CombinedScene");
        } else {
            UserData.instance.combinedIndex = 0;
            ChangeScene.instance.changeScene ("CombinedScene");
        }
        UserData.instance.tutorialFromLogin = false;
    }
    public void LinkToFacebook () {
        //  //print ("Request to link Facebook sent");
        FB.LogInWithReadPermissions (null, OnFacebookLoggedInLink);
    }
    public void ExitLink () {
        confirmExitPanel.SetTrigger ("Show");
        NoPress.gameObject.SetActive (true);
    }
    public void CancelExit () {
        confirmExitPanel.SetTrigger ("Hide");
        NoPress.SetTrigger ("FadeOut");
    }
    public void CancelLogin () {
        NoPress.SetTrigger ("FadeOut");
        FBLinked.SetTrigger ("Hide");
    }
    public void LoginWithFB () {
        NoPress.SetTrigger ("FadeOut");
        FBLinked.SetTrigger ("Hide");
        FB.LogInWithReadPermissions (null, OnFacebookLoggedInLogin);
    }
    public void ActualExit () {
        if (UserData.instance.tutorialFromLogin) {
            UserData.instance.combinedIndex = 1;
            ChangeScene.instance.changeScene ("CombinedScene");
        } else {
            UserData.instance.combinedIndex = 0;
            ChangeScene.instance.changeScene ("CombinedScene");
        }
        UserData.instance.tutorialFromLogin = false;
    }
    public void NameEntered () {
        if (CheckString (nameInput.text.ToLower ())) {
            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = nameInput.text };
            PlayFabClientAPI.UpdateUserTitleDisplayName (request, OnNameSuccess, CallFailure);
        }
    }
    public void SubmitReferralCode () {
        //ShowError (9);
        if (referralInput.text.Length == 16) {
            if (referralInput.text != UserData.instance.playfabId) {
                PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                    FunctionName = "RedeemReferral",
                        FunctionParameter = new Dictionary<string, string> {
                            {
                                "referralCode",
                                referralInput.text
                            }
                        }
                }, (ExecuteCloudScriptResult res) => {
                    if (UserData.instance.tutorialFromLogin) {
                        UserData.instance.combinedIndex = 1;
                        ChangeScene.instance.changeScene ("CombinedScene");
                    } else {
                        UserData.instance.combinedIndex = 0;
                        ChangeScene.instance.changeScene ("CombinedScene");
                    }
                    UserData.instance.tutorialFromLogin = false;
                }, (PlayFabError error) => {
                    ShowError (9);
                    CallFailure (error);
                });
            } else {
                ShowError (10);
            }
        } else {
            ShowError (9);
        }
    }
    private void OnNameSuccess (UpdateUserTitleDisplayNameResult result) {
        //  //Debug.log ("Congratulations, you have changed your username!");
        PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
            Data = new Dictionary<string, string> { { "UsernameSet", "True" }
            },
        }, (UpdateUserDataResult updateResult) => {
            UserData.instance.UpdateInfo ();
            StartCoroutine (HideNameShowLink ());
        }, (PlayFabError error) => {
            StartCoroutine (HideNameShowLink ());
            CallFailure (error);
        });
    }
    void CallFailure (PlayFabError error) {
        if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
            //no connection
            ConnectionController.instance.noConnectionDetected ();
        } else if (error.Error == PlayFabErrorCode.NameNotAvailable) {
            //Name already taken
            ShowError (8);
        } else {
            if (Debug.isDebugBuild) {
                //print (error);
            }
        }
    }
    /*IEnumerator ShowName () {
        yield return new WaitForSeconds (0.4f);
        namePanel.SetTrigger ("Show");
    }*/
    IEnumerator ShowPrev () {
        yield return new WaitForSeconds (0.4f);
        while (showingError == true) {
            yield return new WaitForSeconds (0.1f);
        }
        prevPanel.SetTrigger ("Show");
    }
    IEnumerator ShowLink () {
        yield return new WaitForSeconds (0.4f);
        while (showingError == true) {
            yield return new WaitForSeconds (0.1f);
        }
        linkPanel.SetTrigger ("Show");
    }
    IEnumerator HideNameShowLink () {
        namePanel.SetTrigger ("Hide");
        yield return new WaitForSeconds (0.7f);
        linkPanel.SetTrigger ("Show");
    }
    IEnumerator HidePrevShowName () {
        prevPanel.SetTrigger ("Hide");
        yield return new WaitForSeconds (0.7f);
        namePanel.SetTrigger ("Show");
    }
    IEnumerator HideLinkShowReferral () {
        linkPanel.SetTrigger ("Hide");
        yield return new WaitForSeconds (0.7f);
        referralPanel.SetTrigger ("Show");
    }
}