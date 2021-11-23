using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
public class FriendsController : MonoBehaviour {
  // Use this for initialization
  bool friendsMode;
  int successIndex = 0;
  public List<Dictionary<string, object>> Leaderboards;
  public List<Dictionary<string, object>> DailyLeaderboards;
  public List<Dictionary<string, object>> TempDailyLeaderboards;
  public List<Dictionary<string, object>> TempLeaderboards;
  List<GameObject> friendRequestPanels = new List<GameObject> ();
  List<GameObject> friendPanels = new List<GameObject> ();
  public static FriendsController instance;
  GameObject panelContainer;
  public Animator playerList;
  public Animator leaderboards;
  public Animator addFriendAnimator;
  public Animator removeFriendAnimator;
  public Text removeFriendName;
  public Animator leaderboardRank;
  public Animator leaderboardModeAnimator;
  public Animator NoPress;
  public Animator RemoveNoPress;
  public Animator ErrorNoPress;
  public InputField friendUsernameInput;
  Color youColor = new Color (0, 0, 0, 0.3f);
  Color clearColor = new Color (0, 0, 0, 0.0f);
  Color backgroundColor = new Color (0, 0, 0, 0.18f);
  GameObject scrollBackground;
  string idToRemove;
  public ScrollRect combinedScrollRect;
  public GameObject ViewContainer;
  public Animator error;
  public Text errorText;
  List<Panel> leaderboardPanels = new List<Panel> ();
  float height;
  float width;
  float leaderHeight;
  float leaderWidth;
  Dictionary<string, string> requestedBy = new Dictionary<string, string> ();
  public List<Dictionary<string, object>> FriendsList;
  public int friendsCount = 0;
  public List<Dictionary<string, object>> TempFriendsList;
  bool errorShowing = false;
  int index = 0;
  float extraHeight = 40f;
  bool dailyComplete;
  bool allTimeComplete;
  public RectTransform leaderboardsWindow;
  public Text rankText;
  void Awake () {
    instance = this;
    leaderboardRank.Play ("On");
    leaderboardModeAnimator.Play ("On");
    leaderboards.Play ("On");
    playerList.Play ("Off");
    addFriendAnimator.Play ("Off");
    removeFriendAnimator.Play ("Off");
    NoPress.gameObject.SetActive (false);
    RemoveNoPress.gameObject.SetActive (false);
    ErrorNoPress.gameObject.SetActive (false);
    friendsMode = true;
    panelContainer = GameObject.FindWithTag ("FriendsPanel");
    rankText.text = "RANK #1000";
    requestedBy = UserData.instance.requestedBystring;
    FriendsList = UserData.instance.FriendsList;
    SetDailyLeaderboardPanel ();
    SetFriendRequests ();
    /* if (requestedBy.Count > 0) {
    	SetFriendRequests ();
    } else {
    	if (FriendsList.Count > 0) {
    		SetFriends ();
    	}
    }*/
  }
  void SetDailyLeaderboardPanel () {
    bool success = true;
    rankText.text = "RANK #" + (UserData.instance.dailyFriendLeaderboardPosition + 1);
    leaderboardPanels.Clear ();
    panelContainer = GameObject.FindWithTag ("FriendsPanel");
    DailyLeaderboards = UserData.instance.DailyFriendsLeaderboard;
    TempDailyLeaderboards = DailyLeaderboards;
    for (int i = 0; i < DailyLeaderboards.Count; i++) {
      if ((UserData.instance.requestingById.Contains (DailyLeaderboards[i]["PlayFabID"].ToString ())) || (UserData.instance.requestedById.Contains (DailyLeaderboards[i]["PlayFabID"].ToString ()))) {
        TempDailyLeaderboards.Remove (DailyLeaderboards[i]);
      }
    }
    DailyLeaderboards = TempDailyLeaderboards;
    //Remove Friends that aren't really friends
    //print(DailyLeaderboards.Count);
    for (int i = 0; i < 25; i++) {
      //print (i);
      Panel panel = new Panel ();
      panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/LeaderboardPanel"));
      panel.gameOb.transform.SetParent (panelContainer.transform, false);
      //panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count), 0);
      leaderHeight = panel.gameOb.GetComponent<RectTransform> ().rect.height;
      leaderWidth = panel.gameOb.GetComponent<RectTransform> ().rect.width;
      panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(leaderHeight / 2) - (leaderHeight * leaderboardPanels.Count), 0);
      if (success == true) {
        try {
          DailyLeaderboards[i] = DailyLeaderboards[i];
          success = true;
        } catch {
          success = false;
          successIndex = i;
        }
      }
      if (success == true) {
        try {
          //Set Text
          panel.gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = DailyLeaderboards[i]["DisplayName"].ToString ();
          if (DailyLeaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
            panel.gameOb.GetComponent<Image> ().color = youColor;
          } else {
            panel.gameOb.GetComponent<Image> ().color = backgroundColor;
          }
          //DailyLeaderboards
        } catch {
          panel.gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "Player";
        }
        panel.gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (DailyLeaderboards[i]["Position"]).ToString ("00");
        panel.gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = DailyLeaderboards[i]["StatValue"].ToString ();
        //Set Image
        foreach (Transform child in panel.gameOb.transform.Find ("Render").Find ("CarRender")) {
          //print (child.name);
          if (child.name != DailyLeaderboards[i]["Car"].ToString ()) {
            child.gameObject.SetActive (false);
          } else {
            child.gameObject.SetActive (true);
          }
        }
        foreach (Transform child in panel.gameOb.transform.Find ("Render").Find ("TrailRender")) {
          if (child.name != DailyLeaderboards[i]["Trail"].ToString ()) {
            child.gameObject.SetActive (false);
          } else {
            child.gameObject.SetActive (true);
          }
        }
      } else {
        panel.gameOb.GetComponent<Image> ().color = clearColor;
      }
      leaderboardPanels.Add (panel);
    }
    if (success) {
      dailyComplete = true;
      if ((leaderHeight * (leaderboardPanels.Count)) < leaderboardsWindow.rect.height) {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (leaderWidth, leaderboardsWindow.rect.height);
      } else {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (leaderWidth, (leaderHeight * (leaderboardPanels.Count)) + extraHeight);
      }
      panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (leaderboardPanels.Count)) + extraHeight) / 2f, 0);
    } else {
      dailyComplete = false;
      if ((leaderHeight * (successIndex)) < leaderboardsWindow.rect.height) {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (leaderWidth, leaderboardsWindow.rect.height);
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (leaderboardPanels.Count)) + extraHeight) / 2f, 0);
      } else {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (leaderWidth, (leaderHeight * (successIndex)) + extraHeight);
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (successIndex)) + extraHeight) / 2f, 0);
      }
    }
  }
  public void ResetLeaderboardPos (bool daily) {
    if (daily) {
      if (dailyComplete) {
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (leaderboardPanels.Count)) + extraHeight) / 2f, 0);
      } else {
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (successIndex)) + extraHeight) / 2f, 0);
      }
    } else {
      if (allTimeComplete) {
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (leaderboardPanels.Count)) + extraHeight) / 2f, 0);
      } else {
        panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((leaderHeight * (successIndex)) + extraHeight) / 2f, 0);
      }
    }
  }
  public void UpdateLeaderboard (bool daily) {
    //print("eaderboard updated");
    bool success = true;
    successIndex = 0;
    if (daily) {
      DailyLeaderboards = UserData.instance.DailyFriendsLeaderboard;
      TempDailyLeaderboards = DailyLeaderboards;
      for (int i = 0; i < DailyLeaderboards.Count; i++) {
        if ((UserData.instance.requestingById.Contains (DailyLeaderboards[i]["PlayFabID"].ToString ())) || (UserData.instance.requestedById.Contains (DailyLeaderboards[i]["PlayFabID"].ToString ()))) {
          TempDailyLeaderboards.Remove (DailyLeaderboards[i]);
        }
      }
      DailyLeaderboards = TempDailyLeaderboards;
      rankText.text = "RANK #" + (UserData.instance.dailyFriendLeaderboardPosition + 1);
      for (int i = 0; i < leaderboardPanels.Count; i++) {
        if (success == true) {
          try {
            DailyLeaderboards[i] = DailyLeaderboards[i];
          } catch {
            success = false;
            successIndex = i;
          }
        }
        leaderboardPanels[i].gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(leaderHeight / 2) - (leaderHeight * i), 0);
        if (success == true) {
          try {
            //Set Text
            leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = DailyLeaderboards[i]["DisplayName"].ToString ();
            if (DailyLeaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
              leaderboardPanels[i].gameOb.GetComponent<Image> ().color = youColor;
            } else {
              leaderboardPanels[i].gameOb.GetComponent<Image> ().color = backgroundColor;
            }
            //DailyLeaderboards
          } catch {
            //print ("Fail1");
            leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "Player";
          }
          leaderboardPanels[i].gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (DailyLeaderboards[i]["Position"]).ToString ("00");
          leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = DailyLeaderboards[i]["StatValue"].ToString ();
          foreach (Transform child in leaderboardPanels[i].gameOb.transform.Find ("Render").Find ("CarRender")) {
            if (child.name != DailyLeaderboards[i]["Car"].ToString ()) {
              child.gameObject.SetActive (false);
            } else {
              child.gameObject.SetActive (true);
            }
          }
          foreach (Transform child in leaderboardPanels[i].gameOb.transform.Find ("Render").Find ("TrailRender")) {
            if (child.name != DailyLeaderboards[i]["Trail"].ToString ()) {
              child.gameObject.SetActive (false);
            } else {
              child.gameObject.SetActive (true);
            }
          }
        } else {
          leaderboardPanels[i].gameOb.GetComponent<Image> ().color = clearColor;
        }
      }
      if (success) {
        dailyComplete = true;
      } else {
        dailyComplete = false;
      }
    } else {
      rankText.text = "RANK #" + (UserData.instance.friendLeaderboardPosition + 1);
      Leaderboards = UserData.instance.FriendsLeaderboard;
      TempLeaderboards = Leaderboards;
      for (int i = 0; i < Leaderboards.Count; i++) {
        if ((UserData.instance.requestingById.Contains (Leaderboards[i]["PlayFabID"].ToString ())) || (UserData.instance.requestedById.Contains (Leaderboards[i]["PlayFabID"].ToString ()))) {
          TempLeaderboards.Remove (Leaderboards[i]);
        }
      }
      Leaderboards = TempLeaderboards;
      for (int i = 0; i < leaderboardPanels.Count; i++) {
        if (success == true) {
          try {
            Leaderboards[i] = Leaderboards[i];
          } catch {
            success = false;
            successIndex = i;
          }
        }
        leaderboardPanels[i].gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(leaderHeight / 2) - (leaderHeight * i), 0);
        if (success == true) {
          try {
            //Set Text
            leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = Leaderboards[i]["DisplayName"].ToString ();
            if (Leaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
              leaderboardPanels[i].gameOb.GetComponent<Image> ().color = youColor;
            } else {
              leaderboardPanels[i].gameOb.GetComponent<Image> ().color = backgroundColor;
            }
            //Leaderboards
          } catch {
            //print ("Fail2");
            leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "";
          }
          leaderboardPanels[i].gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (Leaderboards[i]["Position"]).ToString ("00");
          leaderboardPanels[i].gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = Leaderboards[i]["StatValue"].ToString ();
          foreach (Transform child in leaderboardPanels[i].gameOb.transform.Find ("Render").Find ("CarRender")) {
            if (child.name != Leaderboards[i]["Car"].ToString ()) {
              child.gameObject.SetActive (false);
            } else {
              child.gameObject.SetActive (true);
            }
          }
          foreach (Transform child in leaderboardPanels[i].gameOb.transform.Find ("Render").Find ("TrailRender")) {
            if (child.name != Leaderboards[i]["Trail"].ToString ()) {
              child.gameObject.SetActive (false);
            } else {
              child.gameObject.SetActive (true);
            }
          }
        } else {
          leaderboardPanels[i].gameOb.GetComponent<Image> ().color = clearColor;
        }
      }
      if (success) {
        allTimeComplete = true;
      } else {
        allTimeComplete = false;
      }
    }
    if (success) {
      if ((leaderHeight * (leaderboardPanels.Count)) < leaderboardsWindow.rect.height) {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, leaderboardsWindow.rect.height);
      } else {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, (leaderHeight * (leaderboardPanels.Count)) + extraHeight);
      }
    } else {
      if ((leaderHeight * (successIndex)) < leaderboardsWindow.rect.height) {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, leaderboardsWindow.rect.height);
      } else {
        panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, (leaderHeight * (successIndex)) + extraHeight);
      }
    }
  }
  int SortByTime (Dictionary<string, object> l1, Dictionary<string, object> l2) {
    double p1 = (DateTime.UtcNow - Convert.ToDateTime (l1["LastLogin"])).TotalHours;
    double p2 = (DateTime.UtcNow - Convert.ToDateTime (l2["LastLogin"])).TotalHours;
    return p1.CompareTo (p2);
  }
  void SetFriendRequests () {
    scrollBackground = GameObject.FindWithTag ("FriendsScroll");
    index = 0;
    foreach (KeyValuePair<string, string> player in requestedBy) {
      FriendRequest req = new FriendRequest ();
      req.PlayfabId = player.Key;
      req.DisplayName = player.Value;
      req.gameObject = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/FriendRequest"));
      req.gameObject.GetComponent<FriendRequestPanel> ().PlayFabId = player.Key;
      //print (req.gameObject.name);
      height = req.gameObject.GetComponent<RectTransform> ().rect.height;
      width = req.gameObject.GetComponent<RectTransform> ().rect.width;
      req.gameObject.transform.SetParent (scrollBackground.transform, false);
      req.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * index), 0);
      req.DisplayNameText = req.gameObject.transform.Find ("Username").GetComponent<Text> ();
      req.DisplayNameText.text = req.DisplayName;
      friendRequestPanels.Add (req.gameObject);
      index++;
    }
    if (FriendsList.Count == 0) {
      if ((height * (index)) > ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * (index)) + extraHeight);
        scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * index) + extraHeight) / 2f, 0);
      } else {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight);
        scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) / 2f, 0);
      }
    } else {
      SetFriends ();
    }
  }
  public void UpdateFriendsList () {
    //Refresh the friends list on screen after updating the list from UserData.cs
    FriendsList = UserData.instance.FriendsList;
    requestedBy = UserData.instance.requestedBystring;
    index = 0;
    ResetFriendRequests ();
    /*
    		if (requestedBy.Count > 0) {
    			ResetFriendRequests ();
    		} else {
    			if (FriendsList.Count > 0) {
    				ResetFriends ();
    			}
    		}*/
  }
  void UpdateFriendsListPosition () {
    if ((height * (index)) > ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) {
      //scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * (index)) + extraHeight);
      scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * index) + extraHeight) / 2f, 0);
    } else {
      //scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight);
      scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) / 2f, 0);
    }
  }
  void ResetFriendRequests () {
    scrollBackground = GameObject.FindWithTag ("FriendsScroll");
    index = 0;
    for (int i = 0; i < friendRequestPanels.Count; i++) {
      friendRequestPanels[i].SetActive (false);
    }
    if (requestedBy.Count > 0) {
      foreach (KeyValuePair<string, string> player in requestedBy) {
        FriendRequest req = new FriendRequest ();
        try {
          req.gameObject = friendRequestPanels[index];
          req.gameObject.SetActive (true);
        } catch {
          req.gameObject = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/FriendRequest"));
        }
        req.PlayfabId = player.Key;
        req.DisplayName = player.Value;
        req.gameObject.GetComponent<FriendRequestPanel> ().PlayFabId = player.Key;
        //print (req.gameObject.name);
        height = req.gameObject.GetComponent<RectTransform> ().rect.height;
        width = req.gameObject.GetComponent<RectTransform> ().rect.width;
        req.gameObject.transform.SetParent (scrollBackground.transform, false);
        req.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * index), 0);
        req.DisplayNameText = req.gameObject.transform.Find ("Username").GetComponent<Text> ();
        req.DisplayNameText.text = req.DisplayName;
        index++;
      }
    }
    if (FriendsList.Count == 0) {
      if ((height * (index)) > ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * (index)) + extraHeight);
        //scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * index) + extraHeight) / 2f, 0);
      } else {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight);
        //scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) / 2f, 0);
      }
    } else {
      ResetFriends ();
    }
  }
  void SetFriends () {
    scrollBackground = GameObject.FindWithTag ("FriendsScroll");
    TempFriendsList = FriendsList;
    for (int i = 0; i < FriendsList.Count; i++) {
      if ((UserData.instance.requestingById.Contains (FriendsList[i]["PlayFabID"].ToString ())) || (UserData.instance.requestedById.Contains (FriendsList[i]["PlayFabID"].ToString ()))) {
        TempFriendsList.Remove (FriendsList[i]);
      }
    }
    FriendsList = TempFriendsList;
    FriendsList.Sort (SortByTime);
    FriendsList.Sort (SortByTime);
    friendsCount = FriendsList.Count;
    for (int i = 0; i < FriendsList.Count; i++) {
      if (requestedBy.ContainsKey (FriendsList[i]["PlayFabID"].ToString ()) == false) {
        FriendClass friend = new FriendClass ();
        friend.PlayfabId = FriendsList[i]["PlayFabID"].ToString ();
        friend.DisplayName = FriendsList[i]["DisplayName"].ToString ();
        friend.LastLogin = FriendsList[i]["LastLogin"].ToString ();
        friend.AvatarURL = FriendsList[i]["AvatarURL"].ToString ();
        friend.gameObject = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/FriendPanel"));
        friend.gameObject.transform.SetParent (scrollBackground.transform, false);
        friend.gameObject.GetComponent<FriendPanelController> ().PlayFabId = friend.PlayfabId;
        friend.gameObject.GetComponent<FriendPanelController> ().DisplayName = friend.DisplayName;
        height = friend.gameObject.GetComponent<RectTransform> ().rect.height;
        width = friend.gameObject.GetComponent<RectTransform> ().rect.width;
        friend.LastLoginText = friend.gameObject.transform.Find ("LastLogin").GetComponent<Text> ();
        friend.LastLoginText.text = MeasureTime (Convert.ToDateTime (FriendsList[i]["LastLogin"]));
        friend.DisplayNameText = friend.gameObject.transform.Find ("Username").GetComponent<Text> ();
        friend.DisplayNameText.text = friend.DisplayName;
        friend.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * index), 0);
        friend.avatarContainer = friend.gameObject.transform.Find ("Avatar");
        for (int x = 0; x < friend.avatarContainer.childCount; x++) {
          if (friend.avatarContainer.GetChild (x).gameObject.name == friend.AvatarURL) {
            friend.avatarContainer.GetChild (x).gameObject.SetActive (true);
          } else {
            friend.avatarContainer.GetChild (x).gameObject.SetActive (false);
          }
        }
        friendPanels.Add (friend.gameObject);
        index++;
      }
    }
    if ((height * (index)) > ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) {
      scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * (index)) + extraHeight);
      scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * index) + extraHeight) / 2f, 0);
    } else {
      scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight);
      scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) / 2f, 0);
    }
  }
  void ResetFriends () {
    //Reset the friends list
    //print ("Friends Reset");
    //print ("Friends Count: " + FriendsList.Count);
    scrollBackground = GameObject.FindWithTag ("FriendsScroll");
    if (FriendsList.Count > 0) {
      if (FriendsList.Count != friendsCount) {
        TempFriendsList = FriendsList;
        for (int i = 0; i < friendPanels.Count; i++) {
          friendPanels[i].SetActive (false);
        }
        for (int i = 0; i < FriendsList.Count; i++) {
          if ((UserData.instance.requestingById.Contains (FriendsList[i]["PlayFabID"].ToString ())) || (UserData.instance.requestedById.Contains (FriendsList[i]["PlayFabID"].ToString ()))) {
            TempFriendsList.Remove (FriendsList[i]);
          }
        }
        FriendsList = TempFriendsList;
        FriendsList.Sort (SortByTime);
        FriendsList.Sort (SortByTime);
        FriendsList.Sort (SortByTime);
        friendsCount = FriendsList.Count;
        for (int i = 0; i < FriendsList.Count; i++) {
          if (requestedBy.ContainsKey (FriendsList[i]["PlayFabID"].ToString ()) == false) {
            FriendClass friend = new FriendClass ();
            friend.PlayfabId = FriendsList[i]["PlayFabID"].ToString ();
            friend.DisplayName = FriendsList[i]["DisplayName"].ToString ();
            friend.LastLogin = FriendsList[i]["LastLogin"].ToString ();
            friend.AvatarURL = FriendsList[i]["AvatarURL"].ToString ();
            try {
              friend.gameObject = friendPanels[i];
              friend.gameObject.SetActive (true);
            } catch {
              friend.gameObject = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/FriendPanel"));
            }
            friend.gameObject.transform.SetParent (scrollBackground.transform, false);
            friend.gameObject.GetComponent<FriendPanelController> ().PlayFabId = friend.PlayfabId;
            friend.gameObject.GetComponent<FriendPanelController> ().DisplayName = friend.DisplayName;
            height = friend.gameObject.GetComponent<RectTransform> ().rect.height;
            width = friend.gameObject.GetComponent<RectTransform> ().rect.width;
            friend.LastLoginText = friend.gameObject.transform.Find ("LastLogin").GetComponent<Text> ();
            friend.LastLoginText.text = MeasureTime (Convert.ToDateTime (FriendsList[i]["LastLogin"]));
            friend.DisplayNameText = friend.gameObject.transform.Find ("Username").GetComponent<Text> ();
            friend.DisplayNameText.text = friend.DisplayName;
            friend.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * index), 0);
            friend.avatarContainer = friend.gameObject.transform.Find ("Avatar");
            for (int x = 0; x < friend.avatarContainer.childCount; x++) {
              if (friend.avatarContainer.GetChild (x).gameObject.name == friend.AvatarURL) {
                friend.avatarContainer.GetChild (x).gameObject.SetActive (true);
              } else {
                friend.avatarContainer.GetChild (x).gameObject.SetActive (false);
              }
            }
            index++;
          }
        }
      }
      if ((height * (index)) > ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight) {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * (index)) + extraHeight);
      } else {
        scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ViewContainer.GetComponent<RectTransform> ().rect.height + extraHeight);
      }
    }
  }
  public void AcceptFriendRequest (string id) {
    //print ("Requested by " + id);
    //print ("self " + UserData.instance.playfabId);
    PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
      FunctionName = "FriendRequestConfirmed",
        FunctionParameter = new Dictionary<string, string> {
          {
            "requester",
            id
          },
          {
            "requestee",
            UserData.instance.playfabId
          },
          {
            "Username",
            UserData.instance.displayName
          }
        },
        GeneratePlayStreamEvent = true
    }, (ExecuteCloudScriptResult res) => {
      //print ("Friend Request accepted");
      for (int k = 0; k < res.Logs.Count; k++) {
        //print (res.Logs[k].Message);
      }
      requestedBy = UserData.instance.requestedBystring;
      if (requestedBy.ContainsKey (id)) {
        requestedBy.Remove (id);
      }
      FriendsList = UserData.instance.FriendsList;
      ResetFriendRequests ();
      UpdateFriendsListPosition ();
    }, CallFailure);
  }
  public void RejectFriendRequest (string id) {
    PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
      FunctionName = "FriendRequestRejected",
        FunctionParameter = new Dictionary<string, string> {
          {
            "requester",
            id
          },
          {
            "requestee",
            UserData.instance.playfabId
          }
        },
        GeneratePlayStreamEvent = true
    }, (ExecuteCloudScriptResult res) => {
      for (int k = 0; k < res.Logs.Count; k++) {
        print (res.Logs[k].Message);
      }
      requestedBy = UserData.instance.requestedBystring;
      if (requestedBy.ContainsKey (id)) {
        requestedBy.Remove (id);
      }
      FriendsList = UserData.instance.FriendsList;
      TempFriendsList = FriendsList;
      for (int i = 0; i < FriendsList.Count; i++) {
        if (FriendsList[i]["PlayFabID"].ToString () == id) {
          TempFriendsList.Remove (FriendsList[i]);
        }
      }
      FriendsList = TempFriendsList;
      ResetFriendRequests ();
      UpdateFriendsListPosition ();
      //print ("Rejected");
    }, CallFailure);
  }
  string MeasureTime (DateTime time) {
    //print (time);
    TimeSpan span = DateTime.UtcNow - time;
    if (span.TotalHours < 1) {
      if (RoundTime (span.TotalMinutes) == 0) {
        return ("1M AGO");
      } else {
        return (RoundTime (span.TotalMinutes) + "M AGO");
      }
    } else if (span.TotalHours < 24) {
      if (RoundTime (span.TotalHours) == 0) {
        return ("1H AGO");
      } else {
        return (RoundTime (span.TotalHours) + "H AGO");
      }
    } else {
      if (RoundTime (span.TotalDays) == 0) {
        return ("1D AGO");
      } else {
        return (RoundTime (span.TotalDays) + "D AGO");
      }
    }
  }
  int RoundTime (Double span) {
    return (Mathf.FloorToInt ((float) span));
  }
  public void ToggleMode (bool mode) {
    //print ("Toggle Pressed");
    if (mode != friendsMode) {
      friendsMode = mode;
      if (mode == true) {
        //Leaderboard mode
        //print ("Leaderboards");
        NoPress.gameObject.SetActive (false);
        ErrorNoPress.gameObject.SetActive (false);
        RemoveNoPress.gameObject.SetActive (false);
        addFriendAnimator.Play ("Off");
        removeFriendAnimator.Play ("Off");
        StartCoroutine (ShowLeaderboards ());
      } else {
        //Players mode
        //print ("Players");
        StartCoroutine (ShowPlayers ());
      }
    }
  }
  IEnumerator ShowPlayers () {
    //print ("Show Players");
    leaderboards.SetTrigger ("Hide");
    leaderboardRank.SetTrigger ("Hide");
    leaderboardModeAnimator.SetTrigger ("Hide");
    yield return null;
    playerList.SetTrigger ("Show");
  }
  IEnumerator ShowLeaderboards () {
    //print ("Show Leaderboards");
    playerList.SetTrigger ("Hide");
    yield return null;
    leaderboards.SetTrigger ("Show");
    leaderboardRank.SetTrigger ("Show");
    leaderboardModeAnimator.SetTrigger ("Show");
  }
  public void AddFriendButtonClicked () {
    combinedScrollRect.horizontal = false;
    NoPress.gameObject.SetActive (true);
    addFriendAnimator.SetTrigger ("Show");
    friendUsernameInput.text = "";
    StartCoroutine (DelayedSelectInputField ());
  }
  IEnumerator DelayedSelectInputField () {
    yield return new WaitForSeconds (0.75f);
    friendUsernameInput.Select ();
    friendUsernameInput.ActivateInputField ();
  }
  public void ExitAddFriendClicked () {
    combinedScrollRect.horizontal = true;
    //friendUsernameInput.text = "";
    addFriendAnimator.SetTrigger ("Hide");
    NoPress.SetTrigger ("FadeOut");
  }
  public void ExitRemoveFriendClicked () {
    combinedScrollRect.horizontal = true;
    //friendUsernameInput.text = "";
    removeFriendAnimator.SetTrigger ("Hide");
    RemoveNoPress.SetTrigger ("FadeOut");
  }
  IEnumerator RequestSent () {
    //errorText.text = "SENT!";
    addFriendAnimator.SetTrigger ("Hide");
    //yield return new WaitForSeconds (0.2f);
    //errorShowing = true;
    //error.SetTrigger ("Show");
    //yield return new WaitForSeconds (0.6f);
    combinedScrollRect.horizontal = true;
    //errorShowing = false;
    //error.SetTrigger ("Hide");
    NoPress.SetTrigger ("FadeOut");
    yield return null;
  }
  public void BackPressed () {
    combinedScrollRect.horizontal = true;
    NoPress.gameObject.SetActive (false);
    addFriendAnimator.Play ("Off");
  }
  void ShowError (int errorCode) {
    if (errorShowing == false) {
      switch (errorCode) {
        case 0:
          //Already Linked
          errorText.text = "USERNAME NOT VALID";
          break;
        case 1:
          //Already friends
          errorText.text = "USER ALREADY FRIENDS";
          break;
        case 2:
          //User not found
          errorText.text = "USER NOT FOUND";
          break;
        case 3:
          //default error
          errorText.text = "API CALLBACK ERROR";
          break;
        case 4:
          //invalid credentials
          errorText.text = "INVALID CREDENTIALS";
          break;
        case 5:
          //invalid credentials
          errorText.text = "CANNOT ADD YOURSELF";
          break;
        case 6:
          //invalid credentials
          errorText.text = "REQUEST ALREADY SENT";
          break;
        case 100:
          //Success
          errorText.text = "SENT";
          break;
      }
      StartCoroutine (ErrorAnim ());
    }
  }
  IEnumerator ErrorAnim () {
    errorShowing = true;
    ErrorNoPress.gameObject.SetActive (true);
    error.SetTrigger ("Show");
    if (DataEntry.instance.hapticOn) {
      iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 6);
    }
    yield return new WaitForSeconds (1f);
    error.SetTrigger ("Hide");
    ErrorNoPress.SetTrigger ("FadeOut");
    yield return new WaitForSeconds (0.5f);
    errorShowing = false;
  }
  public void AddBack (string playfabId) {
    if (playfabId.ToLower () != UserData.instance.playfabId.ToLower ()) {
      //If not adding self
      PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
        FunctionName = "AddFriendBack",
          FunctionParameter = new Dictionary<string, string> {
            {
              "Input",
              playfabId
            },
            {
              "Username",
              UserData.instance.displayName
            }
          },
          GeneratePlayStreamEvent = true
      }, (ExecuteCloudScriptResult res) => {
        if (res.Logs.Count > 0) {
          if (res.Logs[0].Message == "PlayFab API request error") {
            if (res.Logs.Count > 1) {
              switch (res.Logs[1].Message) {
                case "1183":
                  //UsersAlreadyFriends
                  ShowError (1);
                  break;
                case "1001":
                  //Not found
                  ShowError (2);
                  break;
                case "1126":
                  //Invalid Credentials
                  ShowError (4);
                  break;
                default:
                  ShowError (3);
                  break;
              }
            } else {
              ShowError (3);
            }
          } else {
            //Friend request success
            StartCoroutine (RequestSent ());
          }
        } else {
          //Friend request success
          StartCoroutine (RequestSent ());
        }
        if (Debug.isDebugBuild) {
          for (int k = 0; k < res.Logs.Count; k++) {
            print (res.Logs[k].Message);
          }
        }
      }, (PlayFabError err) => {
        //print (err);
      });
    } else {
      //Cannot add yourself
      ShowError (5);
    }
  }
  public void EnterUsername () {
    //When "Send Friend Request" is pressed
    if (!friendUsernameInput.text.Contains (" ")) {
      //Make sure text field is not empty
      if ((friendUsernameInput.text.Length >= 5) && (friendUsernameInput.text.Length <= 16)) {
        //Confirm length of username is valid
        if ((friendUsernameInput.text.ToLower () != UserData.instance.playfabId.ToLower ()) && (friendUsernameInput.text.ToLower () != UserData.instance.displayName.ToLower ())) {
          //Make sure not sending friend request to self
          if (UserData.instance.requestingById.Contains (friendUsernameInput.text.ToUpper ()) == false) {
            //Confirm that haven't sent friend request to user yet
            if (friendUsernameInput.text.Length == 16) {
              //Input is Playfab ID
              //Adding user to friends list from playfab ID
              PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "AddFriendsFromPlayFabID",
                  FunctionParameter = new Dictionary<string, string> {
                    {
                      "Input",
                      friendUsernameInput.text
                    }
                  },
                  GeneratePlayStreamEvent = true
              }, (ExecuteCloudScriptResult res) => {
                //If friend was successfully added on own list, add self on other user's friend list
                AddBack (friendUsernameInput.text);
                if (Debug.isDebugBuild) {
                  for (int k = 0; k < res.Logs.Count; k++) {
                    print (res.Logs[k].Message);
                  }
                }
              }, CallFailure);
            } else {
              //Input is Username
              //Adding user to friends list from username
              PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "AddFriendsFromUsername",
                  FunctionParameter = new Dictionary<string, string> {
                    {
                      "Input",
                      friendUsernameInput.text
                    }
                  },
                  GeneratePlayStreamEvent = true
              }, (ExecuteCloudScriptResult res) => {
                //If friend was successfully added on own list, get playfab id of friend
                PlayFabClientAPI.GetAccountInfo (new GetAccountInfoRequest { TitleDisplayName = friendUsernameInput.text }, (GetAccountInfoResult ress) => {
                    if (UserData.instance.requestingById.Contains (ress.AccountInfo.PlayFabId.ToUpper ()) == false) {
                      //If haven't sent user a friend request yet, add self to other user's friendlist
                      AddBack (ress.AccountInfo.PlayFabId);
                    } else {
                      ShowError (6);
                    }
                    //print (ress.AccountInfo.PlayFabId);
                  },
                  (PlayFabError err) => {
                    //print (err);
                    ShowError (3);
                  });
                if (res.Logs.Count > 0) {
                  if (res.Logs[0].Message == "PlayFab API request error") {
                    switch (res.Logs[1].Message) {
                      case "1183":
                        //UsersAlreadyFriends
                        ShowError (1);
                        break;
                      case "1001":
                        //Not found
                        ShowError (2);
                        break;
                      case "1126":
                        //Invalid Credentials
                        ShowError (4);
                        break;
                      default:
                        ShowError (3);
                        break;
                    }
                  }
                }
                if (Debug.isDebugBuild) {
                  for (int k = 0; k < res.Logs.Count; k++) {
                    //print (res.Logs[k].Message);
                  }
                }
              }, CallFailure);
            }
          } else {
            ShowError (6);
          }
        } else {
          ShowError (5);
        }
      } else {
        ShowError (0);
      }
    } else {
      ShowError (0);
    }
  }
  public void ShowRemoveFriendsModal (string id, string displayName) {
    removeFriendName.text = "REMOVE '" + displayName + "' FROM FRIENDS?";
    RemoveNoPress.gameObject.SetActive (true);
    removeFriendAnimator.SetTrigger ("Show");
    combinedScrollRect.horizontal = false;
    idToRemove = id;
    //print ("Removing " + displayName);
  }
  public void RemoveFriend () {
    string id = idToRemove;
    RemoveNoPress.SetTrigger ("FadeOut");
    removeFriendAnimator.SetTrigger ("Hide");
    combinedScrollRect.horizontal = true;
    PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
      FunctionName = "RemoveFriends",
        FunctionParameter = new Dictionary<string, string> {
          {
            "requester",
            id
          },
          {
            "requestee",
            UserData.instance.playfabId
          }
        },
        GeneratePlayStreamEvent = true
    }, (ExecuteCloudScriptResult res) => {
      for (int k = 0; k < res.Logs.Count; k++) {
        print (res.Logs[k].Message);
      }
      FriendsList = UserData.instance.FriendsList;
      TempFriendsList = FriendsList;
      for (int i = 0; i < FriendsList.Count; i++) {
        if (FriendsList[i]["PlayFabID"].ToString () == id) {
          TempFriendsList.Remove (FriendsList[i]);
        }
      }
      FriendsList = TempFriendsList;
      ResetFriendRequests ();
      /* if (requestedBy.Count > 0) {
      	ResetFriendRequests ();
      } else {
      	if (FriendsList.Count > 0) {
      		ResetFriends ();
      	}
      }*/
      UpdateFriendsListPosition ();
    }, CallFailure);
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
        print (error);
      }
    }
  }
  public class FriendRequest {
    public string PlayfabId;
    public string DisplayName;
    public GameObject gameObject;
    public Text DisplayNameText;
  }
  public class FriendClass {
    public string PlayfabId;
    public string DisplayName;
    public string LastLogin;
    public string AvatarURL;
    public GameObject gameObject;
    public Text DisplayNameText;
    public Text LastLoginText;
    public Transform avatarContainer;
  }
}