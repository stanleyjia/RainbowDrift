using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UserData : MonoBehaviour {
  public static UserData instance;
  public int coinCount;
  public int orbCount;
  string[] strArray;
  public int balance;
  public int highScore;
  List<string> ownedCars = new List<string> ();
  List<string> ownedTrails = new List<string> ();
  List<string> thisownedCars = new List<string> ();
  List<string> thisownedTrails = new List<string> ();
  List<int> challengesFinished = new List<int> ();
  List<int> challengesCompleted = new List<int> ();
  public Dictionary<string, string> requestedBystring = new Dictionary<string, string> ();
  public Dictionary<string, string> tempRequestedBystring = new Dictionary<string, string> ();
  public List<string> requestedById = new List<string> ();
  List<string> tempRequestedById = new List<string> ();
  public List<string> requestingById = new List<string> ();
  public string currentCar;
  public string currentTrail;
  bool missingUserData;
  private int lastCoinCount;
  private int lastOrbCount;
  public float volume;
  public string playfabId;
  // bool updated = false;
  bool firstTime = true;
  string lastScene = "";
  List<string> keys = new List<string> ();
  public bool FacebookAccountLinked = false;
  public string FBFullName = "";
  public string AvatarUrl = "";
  public int AvatarInd = 0;
  public bool UsernameSet = false;
  public string displayName;
  int updateLoaded = 0;
  bool updateStarted = false;
  bool firstUpdate = false;
  bool needToUpdate = false;
  public int leaderboardPosition = 0;
  public int dailyLeaderboardPosition = 0;
  public int friendLeaderboardPosition = 0;
  public int dailyFriendLeaderboardPosition = 0;
  public List<string> referrals = new List<string> ();
  public List<Dictionary<string, object>> FriendsList = new List<Dictionary<string, object>> ();
  public List<Dictionary<string, object>> FriendsLeaderboard = new List<Dictionary<string, object>> ();
  public List<Dictionary<string, object>> DailyFriendsLeaderboard = new List<Dictionary<string, object>> ();
  public List<Dictionary<string, object>> Leaderboards = new List<Dictionary<string, object>> ();
  public List<Dictionary<string, object>> DailyLeaderboards = new List<Dictionary<string, object>> ();
  public List<Dictionary<string, object>> CloseLeaderboards = new List<Dictionary<string, object>> ();
  public Dictionary<string, int> Statistics = new Dictionary<string, int> ();
  public List<Dictionary<string, object>> StoreItems = new List<Dictionary<string, object>> ();
  bool chal6;
  bool chal8;
  List<string> returnString;
  bool chal32;
  public int combinedIndex = 1;
  public GameObject loadingPanel;
  public List<string> tags;
  bool updating = false;
  public bool updateScene = true;
  bool updateScene2 = false;
  public int tutorialStage = 0;
  public bool tutorialFromLogin;
  List<string> thischallengesFinished;
  List<string> thischallengesCompleted;
  bool firstTimeEver = false;
  AsyncOperation asyncLoad;
  GetPlayerTagsRequest tagsRequest;
  PlayerProfileViewConstraints profConstraints = new PlayerProfileViewConstraints () {
    ShowTags = true, ShowDisplayName = true, ShowLastLogin = true, ShowAvatarUrl = true
  };
  GetLeaderboardAroundPlayerRequest aroundLeaderboardReq1;
  GetFriendsListRequest friendsListRequest;
  GetFriendLeaderboardRequest getFriendLeaderboardReq;
  GetFriendLeaderboardRequest getDailyFriendLeaderboardReq;
  GetFriendLeaderboardAroundPlayerRequest aroundFriendLeaderboardReq1;
  GetFriendLeaderboardAroundPlayerRequest aroundFriendLeaderboardReq;
  GetLeaderboardRequest getLeaderboardReq;
  GetLeaderboardAroundPlayerRequest aroundLeaderboardReq;
  GetLeaderboardRequest getLeaderboardReq1;
  WaitForSeconds wait = new WaitForSeconds (0.25f);
  // Use this for initialization
  void Start () {
    instance = this;
    tagsRequest = new GetPlayerTagsRequest { };
    DontDestroyOnLoad (this);
    // Application.backgroundLoadingPriority = ThreadPriority.Low;
    loadingPanel = GameObject.FindGameObjectWithTag ("Loading");
    //InvokeRepeating("UpdateInfo", 0, 2f);List<string> keys = new List<string> ();
    keys.Add ("ChallengesFinished");
    keys.Add ("ChallengesCompleted");
    keys.Add ("AvatarUrl");
    keys.Add ("AvatarInd");
    keys.Add ("UsernameSet");
    keys.Add ("CurrentCar");
    keys.Add ("CurrentTrail");
    keys.Add ("HighScore");
    keys.Add ("HighScoreCarUsed");
    keys.Add ("HighScoreTrailUsed");
    UpdateChallenges ();
    //InvokeRepeating ("UpdateInfo", 0, 2f);
    lastScene = SceneManager.GetActiveScene ().name;
    aroundLeaderboardReq1 = new GetLeaderboardAroundPlayerRequest {
      MaxResultsCount = 1,
      StatisticName = "ActualHighScoresToday",
      ProfileConstraints = profConstraints
    };
    friendsListRequest = new GetFriendsListRequest {
      IncludeFacebookFriends = false,
      IncludeSteamFriends = false,
      ProfileConstraints = profConstraints
    };
    getFriendLeaderboardReq = new GetFriendLeaderboardRequest {
      MaxResultsCount = 25,
      StartPosition = 0,
      StatisticName = "HighScoresToday",
      ProfileConstraints = profConstraints,
      IncludeFacebookFriends = false,
      IncludeSteamFriends = false
    };
    getDailyFriendLeaderboardReq = new GetFriendLeaderboardRequest {
      MaxResultsCount = 25,
      StartPosition = 0,
      StatisticName = "ActualHighScoresToday",
      ProfileConstraints = profConstraints,
      IncludeFacebookFriends = false,
      IncludeSteamFriends = false
    };
    aroundFriendLeaderboardReq1 = new GetFriendLeaderboardAroundPlayerRequest {
      MaxResultsCount = 1,
      StatisticName = "ActualHighScoresToday",
      ProfileConstraints = profConstraints
    };
    aroundFriendLeaderboardReq = new GetFriendLeaderboardAroundPlayerRequest {
      MaxResultsCount = 1,
      StatisticName = "HighScoresToday",
      ProfileConstraints = profConstraints
    };
    getLeaderboardReq = new GetLeaderboardRequest {
      MaxResultsCount = 25,
      StartPosition = 0,
      StatisticName = "HighScoresToday",
      ProfileConstraints = profConstraints
    };
    aroundLeaderboardReq = new GetLeaderboardAroundPlayerRequest {
      MaxResultsCount = 1,
      StatisticName = "HighScoresToday",
      ProfileConstraints = profConstraints
    };
    getLeaderboardReq1 = new GetLeaderboardRequest {
      MaxResultsCount = 25,
      StartPosition = 0,
      StatisticName = "ActualHighScoresToday",
      ProfileConstraints = profConstraints
    };
  }
  // Update is called once per frame
  void UpdateChallenges () {
    chal6 = ChallengesController.ChallengeDone (6);
    chal8 = ChallengesController.ChallengeDone (8);
    chal32 = ChallengesController.ChallengeDone (32);
  }
  void IncrementUpdateLoaded () {
    //called 17 times
    LoadingController.instance.AddProgress ();
    updateLoaded++;
    //print (updateLoaded);
  }
  void Update () {
    if (Time.frameCount % 20 == 0) {
      System.GC.Collect ();
    }
    if (firstTime == true) {
      if (PlayFabLogin.instance.loggedIn) {
        InvokeRepeating ("UpdateInfo", 8f, 5f);
        firstTime = false;
      }
    }
    if (PlayFabLogin.instance.loggedIn) {
      if (SceneManager.GetActiveScene ().name != lastScene) {
        lastScene = SceneManager.GetActiveScene ().name;
        if (SceneManager.GetActiveScene ().name != "GameScene") {
          if (updating == true) {
            UpdateChallenges ();
            UpdateInfo ();
          } else {
            updating = true;
          }
        }
      }
      if (updateScene && updateScene2) {
        updateScene2 = false;
        if (loadingPanel.activeInHierarchy) {
          loadingPanel.SetActive (false);
        }
        asyncLoad.allowSceneActivation = true;
        ////print ("scene activated");
      }
    }
    if (firstUpdate) {
      if (updateStarted) {
        //print (updateLoaded);
        if (updateLoaded >= 17) {
          LoadingController.instance.SetTo (1.0f);
          //Update is complete
          //print ("Update2 complete");
          firstUpdate = false;
          updateScene2 = true;
          GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
          UserData.instance.combinedIndex = 1;
          if (firstTimeEver) {
            asyncLoad = SceneManager.LoadSceneAsync ("TutorialScene");
            tutorialFromLogin = true;
          } else {
            asyncLoad = SceneManager.LoadSceneAsync ("CombinedScene");
          }
          asyncLoad.allowSceneActivation = false;
          System.GC.Collect ();
          updateStarted = false;
          updateLoaded = 0;
        }
      }
    }
  }
  public void UpdateForFirstTime (bool firstLogin = false) {
    // //print ("first update");'
    firstTimeEver = firstLogin;
    firstUpdate = true;
    DataEntry.instance.SetFromFile ();
    UpdateInfo ();
  }
  IEnumerator UpdateInfoCoroutine () {
    updateStarted = true;
    updateLoaded = 0;
    //print ("--------start of update");
    //Updating user info every 2 seconds
    PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (), (GetUserInventoryResult result) => {
      //print ("user inventory");
      IncrementUpdateLoaded ();
      if (result.VirtualCurrency != null) {
        try {
          coinCount = result.VirtualCurrency["CN"];
          orbCount = result.VirtualCurrency["OB"];
          balance = result.VirtualCurrency["BL"];
        } catch {
          PlayFabLogin.instance.CreateGuestAccount ();
        }
        if (coinCount != DataEntry.instance.totalCoins) {
          DataEntry.instance.UpdateCoins (coinCount);
          needToUpdate = true;
          //print ("need to update");
        }
        if (orbCount != DataEntry.instance.orbCount) {
          DataEntry.instance.UpdateOrbs (orbCount);
          //print ("need to update");
          needToUpdate = true;
        }
        if (balance != DataEntry.instance.balance) {
          DataEntry.instance.UpdateBalance (balance);
          needToUpdate = true;
          //print ("need to update");
        }
      }
      if (result.Inventory != null) {
        if (result.Inventory.Count > 0) {
          if (DataEntry.instance.carsOwned.Count + DataEntry.instance.trailsOwned.Count != result.Inventory.Count) {
            thisownedCars.Clear ();
            thisownedTrails.Clear ();
            for (int i = 0; i < result.Inventory.Count; i++) {
              if (result.Inventory[i].ItemId.Substring (0, 1) == "C") {
                //Car
                if (thisownedCars.Contains (result.Inventory[i].ItemId) == false) {
                  thisownedCars.Add (result.Inventory[i].ItemId);
                }
              } else if (result.Inventory[i].ItemId.Substring (0, 1) == "T") {
                //Trail
                if (thisownedTrails.Contains (result.Inventory[i].ItemId) == false) {
                  thisownedTrails.Add (result.Inventory[i].ItemId);
                }
              }
            }
            ownedCars = thisownedCars;
            ownedTrails = thisownedTrails;
            //print (DataEntry.instance.carsOwned[0].Length);
            //print (ownedCars[0].Length);
            if (ownedCars != DataEntry.instance.carsOwned) {
              DataEntry.instance.UpdateCarsOwned (ownedCars);
              DataEntry.instance.UpdateTrailsOwned (ownedTrails);
              needToUpdate = true;
              //print ("need to update");
            }
          }
        }
      }
    }, CallFailure);
    yield return null;
    PlayFabClientAPI.GetUserReadOnlyData (new GetUserDataRequest (), (GetUserDataResult dataonlyresult) => {
      try {
        referrals = SplitReferralsString (dataonlyresult.Data["Referrals"].Value);
      } catch {
        FillMissingController.instance.SetData ("UserReadOnlyData");
        referrals.Clear ();
      }
      if (chal6) {
        if (referrals.Count >= 1) {
          ChallengesController.CompleteChallenge (6);
          chal6 = false;
        }
      }
      PlayfabStatisticsController.instance.UpdateStats ();
      //print ("user read only");
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return null;
    //Update Challenges To Finish
    PlayFabClientAPI.GetUserData (new GetUserDataRequest {
      Keys = keys
    }, (GetUserDataResult userdata) => {
      missingUserData = false;
      //print ("user data");
      IncrementUpdateLoaded ();
      ////print ("get user data")
      try {
        if (userdata.Data["CurrentCar"].Value != "") {
          currentCar = userdata.Data["CurrentCar"].Value;
        } else {
          missingUserData = true;
          FillMissingController.instance.MissingUserData.Add ("CurrentCar");
          currentCar = "C0002";
        }
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          //print (ex);
        }
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("CurrentCar");
        currentCar = "C0002";
      }
      try {
        if (userdata.Data["CurrentTrail"].Value != "") {
          currentTrail = userdata.Data["CurrentTrail"].Value;
        } else {
          currentTrail = "T0001";
          missingUserData = true;
          FillMissingController.instance.MissingUserData.Add ("CurrentTrail");
        }
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          //print (ex);
        }
        currentTrail = "T0001";
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("CurrentTrail");
      }
      try {
        if (currentTrail != DataEntry.instance.trailUsing) {
          DataEntry.instance.UpdateTrailUsing (currentTrail);
          needToUpdate = true;
          //print ("need to update");
        }
        if (currentCar != DataEntry.instance.carUsing) {
          DataEntry.instance.UpdateCarUsing (currentCar);
          needToUpdate = true;
          //print ("need to update");
        }
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          //print (ex);
        }
      }
      try {
        //Version from PlayFab, should be just # inside
        thischallengesFinished = ChallengesDecode.TurnIntoList (userdata.Data["ChallengesFinished"].Value);
        if (thischallengesFinished.Contains ("#") == false) {
          if (challengesFinished.Count != thischallengesFinished.Count) {
            challengesFinished = ChallengesDecode.TurnIntoIntList (thischallengesFinished);
            if (challengesFinished.Count != DataEntry.instance.challengesToFinish.Count) {
              DataEntry.instance.UpdateChallengesToFinish (challengesFinished);
              //print ("need to update");
              needToUpdate = true;
            }
          }
        } else {
          if (DataEntry.instance.challengesToFinish.Count != 0) {
            DataEntry.instance.UpdateChallengesToFinish (new List<int> ());
            needToUpdate = true;
            //print ("need to update");
          }
        }
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          print (ex);
        }
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("ChallengesFinished");
        if (DataEntry.instance.challengesToFinish.Count != 0) {
          DataEntry.instance.UpdateChallengesToFinish (new List<int> ());
          needToUpdate = true;
          //print ("need to update");
        }
      }
      try {
        thischallengesCompleted = ChallengesDecode.TurnIntoList (userdata.Data["ChallengesCompleted"].Value);
        if (thischallengesCompleted.Contains ("#") == false) {
          if (challengesCompleted.Count != thischallengesCompleted.Count) {
            challengesCompleted = ChallengesDecode.TurnIntoIntList (thischallengesCompleted);
            if (challengesCompleted.Count != DataEntry.instance.challengesCompleted.Count) {
              DataEntry.instance.UpdateChallengesCompleted (challengesCompleted);
              needToUpdate = true;
              //print ("need to update");
              //Update stats
              PlayfabStatisticsController.instance.UpdateStats ();
              PlayfabStatisticsController.instance.statistics["ChallengesCompleted"] = challengesCompleted.Count;
              PlayfabStatisticsController.instance.SetStatistics ();
            }
          }
        } else {
          if (DataEntry.instance.challengesCompleted.Count != 0) {
            DataEntry.instance.UpdateChallengesCompleted (new List<int> ());
            needToUpdate = true;
            //print ("need to update");
          }
        }
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          //print (ex);
        }
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("ChallengesCompleted");
        if (DataEntry.instance.challengesCompleted.Count != 0) {
          DataEntry.instance.UpdateChallengesCompleted (new List<int> ());
          needToUpdate = true;
          //print ("need to update");
        }
      }
      try {
        AvatarUrl = userdata.Data["AvatarUrl"].Value;
        AvatarInd = Convert.ToInt32 (userdata.Data["AvatarInd"].Value);
      } catch (Exception ex) {
        if (Debug.isDebugBuild) {
          //print (ex);
        }
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("AvatarUrl");
        FillMissingController.instance.MissingUserData.Add ("AvatarInd");
        if (Debug.isDebugBuild) {
          //Debug.Log ("Avatar URL Error");
        }
        AvatarUrl = "https://res.cloudinary.com/moosepark/image/upload/v1534403658/fox.png";
        AvatarInd = 0;
      }
      //           //print (AvatarUrl);
      //print (DataEntry.instance.AvatarUrl);
      //print (DataEntry.instance.AvatarInd);
      if (AvatarUrl != DataEntry.instance.AvatarUrl || AvatarInd != DataEntry.instance.AvatarInd) {
        DataEntry.instance.UpdateAvatarUrlInd (AvatarUrl, AvatarInd);
        needToUpdate = true;
        //print (AvatarUrl);
        //print (AvatarInd);
        //print ("need to update");
      }
      try {
        highScore = int.Parse (userdata.Data["HighScore"].Value);
      } catch (KeyNotFoundException) {
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("HighScore");
        highScore = 0;
      }
      if (highScore != DataEntry.instance.highScore) {
        DataEntry.instance.UpdateHighScore (highScore);
        needToUpdate = true;
        //print ("need to update");
      }
      try {
        if (userdata.Data["UsernameSet"].Value == "False") {
          UsernameSet = false;
        } else if (userdata.Data["UsernameSet"].Value == "True") {
          UsernameSet = true;
        } else {
          if (Debug.isDebugBuild) {
            //print ("Username Set error");
          }
        }
        if (UsernameSet != DataEntry.instance.UsernameSet) {
          DataEntry.instance.UpdateUsernameSet (UsernameSet);
          //print ("need to update");
          needToUpdate = true;
        }
      } catch (KeyNotFoundException exception) {
        missingUserData = true;
        FillMissingController.instance.MissingUserData.Add ("UsernameSet");
        if (Debug.isDebugBuild) {
          //print (exception);
        }
      }
      if (missingUserData) {
        FillMissingController.instance.SetData ("UserData");
        missingUserData = false;
      }
    }, CallFailure);
    yield return null;
    PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest (), (GetAccountInfoResult res) => {
      IncrementUpdateLoaded ();
      ////print ("get account info");
      try {
        //Facebook Linked
        displayName = res.AccountInfo.TitleInfo.DisplayName;
        FBFullName = res.AccountInfo.FacebookInfo.FullName;
        FacebookAccountLinked = true;
      } catch {
        //Facebook Not Linked
        // displayName = "GUEST";
        FacebookAccountLinked = false;
        FBFullName = "";
      }
      if (displayName != DataEntry.instance.displayName) {
        DataEntry.instance.UpdateDisplayName (displayName);
        needToUpdate = true;
        //print ("need to update");
      }
      if (FacebookAccountLinked != DataEntry.instance.FBLinked || FBFullName != DataEntry.instance.FBName) {
        DataEntry.instance.UpdateFacebookLinked (FacebookAccountLinked, FBFullName);
        needToUpdate = true;
        //print ("need to update");
      }
      playfabId = res.AccountInfo.PlayFabId;
      tagsRequest.PlayFabId = res.AccountInfo.PlayFabId;
      PlayFabClientAPI.GetPlayerTags (tagsRequest, (GetPlayerTagsResult result) => {
        //print ("user tag");
        IncrementUpdateLoaded ();
        for (int i = 0; i < result.Tags.Count; i++) {
          if (tags.Contains (ActualTag (result.Tags[i])) == false) {
            tags.Add (ActualTag (result.Tags[i]));
          }
        }
        for (int i = 0; i < tags.Count; i++) {
          if (result.Tags.Contains (InverseTag (tags[i])) == false) {
            tags.Remove (tags[i]);
          }
        }
        requestingById.Clear ();
        tempRequestedById.Clear ();
        for (int i = 0; i < tags.Count; i++) {
          if (tags[i].Contains ("requestedby:")) {
            string temp = "";
            temp = tags[i].Replace ("requestedby:", "");
            strArray = temp.Split (':');
            for (int w = 0; w < strArray.Length; w++) {
              if (strArray[w].ToLower () != playfabId.ToLower ()) {
                if (strArray[w] != "") {
                  tempRequestedById.Add (strArray[w]);
                }
              }
            }
          } else if (tags[i].Contains ("requesting:")) {
            string temp = "";
            temp = tags[i].Replace ("requesting:", "");
            strArray = temp.Split (':');
            for (int w = 0; w < strArray.Length; w++) {
              if (strArray[w].ToLower () != playfabId.ToLower ()) {
                if (strArray[w] != "") {
                  requestingById.Add (strArray[w]);
                }
              }
            }
          }
        }
        //print ("Requested by: " + requestedById.Count + " people");
        if (tempRequestedById.Count > 0) {
          if (tempRequestedById.Count != requestedById.Count) {
            requestedById = tempRequestedById;
            StartCoroutine (GetRequestedByInfo ());
          } else {
            IncrementUpdateLoaded ();
          }
        } else {
          IncrementUpdateLoaded ();
        }
      }, CallFailure);
    }, CallFailure);
    yield return null;
    StartCoroutine (UpdateFriends ());
    yield return null;
    StartCoroutine (UpdateLeaderboards ());
    yield return null;
    UpdateStoreItems ();
    yield return null;
    UpdateStatistics ();
    yield return null;
    CheckToUpdate ();
  }
  IEnumerator UpdateFriends () {
    yield return null;
    PlayFabClientAPI.GetFriendsList (
      friendsListRequest, (GetFriendsListResult res) => {
        if (res.Friends.Count != FriendsList.Count) {
          FriendsList.Clear ();
          for (int i = 0; i < res.Friends.Count; i++) {
            //print (res.Friends[i].Profile.LastLogin);
            if (ParseRequested (res.Friends[i].Profile.Tags) == false) {
              FriendsList.Add (new Dictionary<string, object> { { "DisplayName", res.Friends[i].Profile.DisplayName },
                { "PlayFabID", res.Friends[i].FriendPlayFabId },
                { "LastLogin", res.Friends[i].Profile.LastLogin },
                { "AvatarURL", ParseAvatar (res.Friends[i].Profile.Tags) }
              });
            }
          }
        }
        try {
          FriendsController.instance.UpdateFriendsList ();
        } catch { }
        IncrementUpdateLoaded ();
      }, CallFailure
    );
  }
  string ActualTag (string input) {
    return (input.Replace ("title.16B7.", ""));
  }
  string InverseTag (string input) {
    return ("title.16B7." + input);
  }
  public void UpdateInfo () {
    if (PlayFabLogin.instance.loggedIn) {
      if (SceneManager.GetActiveScene ().name == "GameScene") {
        if (CarVariables.instance.gameOn == false) {
          StartCoroutine (UpdateInfoCoroutine ());
        }
      } else {
        StartCoroutine (UpdateInfoCoroutine ());
      }
    } else {
      if (Debug.isDebugBuild) {
        //print ("Playfab not logged in yet");
      }
    }
  }
  public void UpdateStatistics () {
    PlayFabClientAPI.GetPlayerStatistics (new GetPlayerStatisticsRequest (), (GetPlayerStatisticsResult res) => {
      //Statistics.Clear ();
      try {
        for (int i = 0; i < res.Statistics.Count; i++) {
          switch (res.Statistics[i].StatisticName) {
            case "CoinsCollected":
              if (chal8) {
                if (res.Statistics[i].Value > 1000) {
                  ChallengesController.CompleteChallenge (8);
                  chal8 = false;
                }
              }
              break;
            case "OrbsCollected":
              if (chal32) {
                if (res.Statistics[i].Value > 50) {
                  ChallengesController.CompleteChallenge (32);
                  chal32 = false;
                }
              }
              break;
          }
          Statistics[res.Statistics[i].StatisticName] = (int) res.Statistics[i].Value;
        }
        DataEntry.instance.UpdateUserStatistics (Statistics);
      } catch {
        if (Debug.isDebugBuild) {
          Debug.Log ("Statistics Value not found");
        }
      }
    }, CallFailure);
  }
  IEnumerator UpdateLeaderboards () {
    //Update Friends Leaderboard, all time
    PlayFabClientAPI.GetFriendLeaderboard (getFriendLeaderboardReq, (GetLeaderboardResult res) => {
      try {
        FriendsLeaderboard.Clear ();
        for (int i = 0; i < res.Leaderboard.Count; i++) {
          FriendsLeaderboard.Add (new Dictionary<string, object> { { "DisplayName", res.Leaderboard[i].DisplayName },
            { "PlayFabID", res.Leaderboard[i].PlayFabId },
            { "StatValue", res.Leaderboard[i].StatValue },
            { "Position", res.Leaderboard[i].Position + 1 },
            { "Car", ParseCar (res.Leaderboard[i].Profile.Tags) },
            { "Trail", ParseTrail (res.Leaderboard[i].Profile.Tags) }
          });
        }
      } catch {
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 1 Value not found");
        }
      }
      System.GC.Collect ();
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return wait;
    //Update Friends Leaderboard, daily
    PlayFabClientAPI.GetFriendLeaderboard (getDailyFriendLeaderboardReq, (GetLeaderboardResult res) => {
      try {
        DailyFriendsLeaderboard.Clear ();
        for (int i = 0; i < res.Leaderboard.Count; i++) {
          DailyFriendsLeaderboard.Add (new Dictionary<string, object> { { "DisplayName", res.Leaderboard[i].DisplayName },
            { "PlayFabID", res.Leaderboard[i].PlayFabId },
            { "StatValue", res.Leaderboard[i].StatValue },
            { "Position", res.Leaderboard[i].Position + 1 },
            { "Car", ParseCar (res.Leaderboard[i].Profile.Tags) },
            { "Trail", ParseTrail (res.Leaderboard[i].Profile.Tags) }
          });
        }
      } catch {
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 1 Value not found");
        }
      }
      System.GC.Collect ();
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return wait;
    PlayFabClientAPI.GetFriendLeaderboardAroundPlayer (aroundFriendLeaderboardReq1, (GetFriendLeaderboardAroundPlayerResult res) => {
      try {
        //print (res.Leaderboard[0].Position + 1);
        dailyFriendLeaderboardPosition = res.Leaderboard[0].Position;
      } catch {
        dailyFriendLeaderboardPosition = 1000;
        if (Debug.isDebugBuild) {
          //print ("Leaderboard 2 Value not found");
        }
      }
      IncrementUpdateLoaded ();
      //print ("heee");
    }, CallFailure);
    yield return wait;
    PlayFabClientAPI.GetFriendLeaderboardAroundPlayer (aroundFriendLeaderboardReq, (GetFriendLeaderboardAroundPlayerResult res) => {
      try {
        //print (res.Leaderboard[0].Position + 1);
        friendLeaderboardPosition = res.Leaderboard[0].Position;
      } catch {
        friendLeaderboardPosition = 1000;
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 2 Value not found");
        }
      }
      IncrementUpdateLoaded ();
    }, CallFailure);
    //Update Leaderboard info, all time
    yield return wait;
    PlayFabClientAPI.GetLeaderboard (getLeaderboardReq, (GetLeaderboardResult res) => {
      try {
        Leaderboards.Clear ();
        for (int i = 0; i < res.Leaderboard.Count; i++) {
          Leaderboards.Add (new Dictionary<string, object> { { "DisplayName", res.Leaderboard[i].DisplayName },
            { "PlayFabID", res.Leaderboard[i].PlayFabId },
            { "StatValue", res.Leaderboard[i].StatValue },
            { "Position", res.Leaderboard[i].Position + 1 },
            { "Car", ParseCar (res.Leaderboard[i].Profile.Tags) },
            { "Trail", ParseTrail (res.Leaderboard[i].Profile.Tags) }
          });
        }
      } catch {
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 1 Value not found");
        }
      }
      System.GC.Collect ();
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return wait;
    //Close Leaderboards
    PlayFabClientAPI.GetLeaderboardAroundPlayer (aroundLeaderboardReq, (GetLeaderboardAroundPlayerResult res) => {
      try {
        //print (res.Leaderboard[0].Position + 1);
        leaderboardPosition = res.Leaderboard[0].Position;
      } catch {
        leaderboardPosition = 1000;
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 2 Value not found");
        }
      }
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return wait;
    //Update Leaderboard info, all time
    PlayFabClientAPI.GetLeaderboard (getLeaderboardReq1, (GetLeaderboardResult res) => {
      try {
        DailyLeaderboards.Clear ();
        for (int i = 0; i < res.Leaderboard.Count; i++) {
          DailyLeaderboards.Add (new Dictionary<string, object> { { "DisplayName", res.Leaderboard[i].DisplayName },
            { "PlayFabID", res.Leaderboard[i].PlayFabId },
            { "StatValue", res.Leaderboard[i].StatValue },
            { "Position", res.Leaderboard[i].Position + 1 },
            { "Car", ParseCar (res.Leaderboard[i].Profile.Tags) },
            { "Trail", ParseTrail (res.Leaderboard[i].Profile.Tags) }
          });
          //print (DailyLeaderboards.Count);
        }
        System.GC.Collect ();
      } catch {
        //print ("Daily Leaderboards Error");
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 1 Value not found");
        }
      }
      IncrementUpdateLoaded ();
    }, CallFailure);
    yield return wait;
    PlayFabClientAPI.GetLeaderboardAroundPlayer (aroundLeaderboardReq1, (GetLeaderboardAroundPlayerResult res) => {
      try {
        //print (res.Leaderboard[0].Position + 1);
        dailyLeaderboardPosition = res.Leaderboard[0].Position;
      } catch {
        dailyLeaderboardPosition = 1000;
        if (Debug.isDebugBuild) {
          //Debug.logError ("Leaderboard 2 Value not found");
        }
      }
      IncrementUpdateLoaded ();
    }, CallFailure);
    try {
      if (LeaderboardModeController.instance.mode == 1) {
        //print ("Updated all-time");
        LeaderboardController.instance.UpdateLeaderboard (false);
      } else if (LeaderboardModeController.instance.mode == 0) {
        //print ("Updated daily");
        LeaderboardController.instance.UpdateLeaderboard (true);
      }
    } catch { }
    yield return null;
    try {
      if (FriendLeaderboardMode.instance.mode == 1) {
        //print ("Updated all-time");
        FriendsController.instance.UpdateLeaderboard (false);
      } else if (FriendLeaderboardMode.instance.mode == 0) {
        //print ("Updated daily");
        FriendsController.instance.UpdateLeaderboard (true);
      }
    } catch { }
  }
  string ParseCar (List<TagModel> list) {
    for (int i = 0; i < list.Count; i++) {
      if (ActualTag (list[i].TagValue.ToString ()) [0] == 'C') {
        return (ActualTag (list[i].TagValue.ToString ()));
      }
    }
    return "C0002";
  }
  string ParseTrail (List<TagModel> list) {
    for (int i = 0; i < list.Count; i++) {
      if (ActualTag (list[i].TagValue.ToString ()) [0] == 'T') {
        return (ActualTag (list[i].TagValue.ToString ()));
      }
    }
    return "T0001";
  }
  string ParseAvatar (List<TagModel> list) {
    for (int i = 0; i < list.Count; i++) {
      if (ActualTag (list[i].TagValue.ToString ()).Contains ("avatar:")) {
        return (ActualTag (list[i].TagValue.ToString ()).Replace ("avatar:", ""));
      }
    }
    return "fox";
  }
  bool ParseRequested (List<TagModel> list) {
    bool requested = false;
    for (int i = 0; i < list.Count; i++) {
      if (ActualTag (list[i].TagValue.ToString ()).Contains ("requestedBy:" + playfabId)) {
        requested = true;
      }
    }
    return (requested);
  }
  IEnumerator GetRequestedByInfo () {
    tempRequestedBystring.Clear ();
    for (int i = 0; i < requestedById.Count; i++) {
      if (requestedById[i].ToLower () != playfabId.ToLower ()) {
        PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest { PlayFabId = requestedById[i] }, (GetAccountInfoResult res) => {
          try {
            tempRequestedBystring.Add (res.AccountInfo.PlayFabId, res.AccountInfo.TitleInfo.DisplayName);
          } catch (Exception ex) {
            print (ex);
          }
        }, CallFailure);
        //yield return new WaitForSeconds (1);
        yield return null;
      }
    }
    requestedBystring = tempRequestedBystring;
    IncrementUpdateLoaded ();
    yield return null;
  }
  public void UpdateStoreItems () {
    PlayFabClientAPI.GetCatalogItems (new GetCatalogItemsRequest (), (GetCatalogItemsResult res) => {
      // //print ("Store Updated");
      StoreItems.Clear ();
      try {
        for (int i = 0; i < res.Catalog.Count; i++) {
          StoreItems.Add (new Dictionary<string, object> { { "ItemId", res.Catalog[i].ItemId },
            { "ItemClass", res.Catalog[i].ItemClass },
            { "CatalogVersion", res.Catalog[i].CatalogVersion },
            { "DisplayName", res.Catalog[i].DisplayName },
            { "Description", res.Catalog[i].Description },
            { "VirtualCurrencyPrices", res.Catalog[i].VirtualCurrencyPrices },
            { "RealCurrencyPrices", res.Catalog[i].RealCurrencyPrices },
            { "Bundle", res.Catalog[i].Bundle },
            { "Tags", res.Catalog[i].Tags },
          });
        }
      } catch {
        if (Debug.isDebugBuild) {
          //Debug.logError ("Store Catalog Item not found");
        }
      }
      IncrementUpdateLoaded ();
    }, CallFailure);
  }
  void CallFailure (PlayFabError error) {
    print ("Failure");
    if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
      //no connection
      ConnectionController.instance.noConnectionDetected ();
    } else {
      IncrementUpdateLoaded ();
      if (Debug.isDebugBuild) {
        print (error);
      }
    }
  }
  List<string> SplitReferralsString (string str) {
    returnString.Clear ();
    string[] newString = str.Split ('"');
    for (int i = 0; i < newString.Length; i++) {
      if (newString[i].Length == 16) {
        returnString.Add (newString[i]);
      }
    }
    return (returnString);
  }
  void CheckToUpdate () {
    if (needToUpdate) {
      //print (needToUpdate);
      //print ("need to update");
      DataEntry.instance.Save ();
      //DataEntry.instance.SetFromFile ();
      StartCoroutine (DelayedSetFromFile ());
      needToUpdate = false;
    }
    IncrementUpdateLoaded ();
  }
  IEnumerator DelayedSetFromFile () {
    yield return null;
    yield return null;
    DataEntry.instance.SetFromFile ();
  }
}