using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DataEntry : MonoBehaviour {
    public bool oneTime = true;
    public int highScore;
    public int orbCount;
    public int balance;
    public float volume;
    public bool sfxMuted;
    public bool bgmMuted;
    public bool hapticOn;
    //PRODUCTION
    public int totalCoins;
    public string FBTokenString;
    public Dictionary<string, int> userStatistics = new Dictionary<string, int> ();
    public List<string> carsOwned;
    public List<string> trailsOwned;
    public bool firstTime;
    public List<int> challengesCompleted;
    public List<int> challengesToFinish;
    public string carUsing;
    public string trailUsing;
    bool gameOn;
    public bool FBLinked = false;
    public string FBName = "";
    public string AvatarUrl = "";
    public bool firstTimeOnDevice;
    public int AvatarInd = 0;
    public bool UsernameSet = false;
    public bool loggedIn = false;
    public string displayName = "";
    public int dailyHighScore = 0;
    public int actualDailyHighScore = 0;
    public static DataEntry instance;
    PlayerData data;
    bool setFailed = false;
    void Awake () {
        instance = this;
        DontDestroyOnLoad (this);
        //Reloads Saved Data
        carsOwned.Add ("C0002");
        trailsOwned.Add ("T0001");
        carUsing = "C0002";
        trailUsing = "T0001";
        firstTime = true;
        balance = 0;
        totalCoins = 0;
        orbCount = 0;
        dailyHighScore = 0;
        actualDailyHighScore = 0;
        firstTimeOnDevice = true;
        volume = 1f;
        //SANDBOX
        try {
            SetFromFile ();
        } catch {
            setFailed = true;
        }
    }
    void Start () {
        //SetFromFile ();
        if (setFailed) {
            if (Debug.isDebugBuild) {
                //print ("Set failed");
            }
            Save ();
            setFailed = false;
        }
    }
    public void SetFromFile () {
        data = SaveData.LoadData ();
        firstTimeOnDevice = data.firstTimeOnDevice;
        AvatarInd = data.AvatarInd;
        AvatarUrl = data.AvatarUrl;
        carUsing = data.carUsing;
        trailUsing = data.trailUsing;
        carsOwned = data.ownedCars;
        trailsOwned = data.ownedTrails;
        sfxMuted = data.sfxMuted;
        bgmMuted = data.bgmMuted;
        hapticOn = data.hapticOn;
        volume = data.volume;
        dailyHighScore = data.dailyHighScore;
        actualDailyHighScore = data.actualDailyHighScore;
        FBTokenString = data.FBTokenString;
        firstTime = data.firstTime;
        challengesCompleted = data.challengesCompleted;
        challengesToFinish = data.challengesToFinish;
        highScore = data.stats[0];
        orbCount = data.stats[1];
        totalCoins = data.stats[2];
        balance = data.stats[3];
        userStatistics = data.userStatistics;
        displayName = data.displayName;
        UsernameSet = data.UsernameSet;
    }
    public void UpdateUserStatistics (Dictionary<string, int> dict) {
        userStatistics = dict;
    }
    public void UpdateVolume (float val) {
        volume = val;
    }
    public void UpdateHaptic (bool haptic) {
        hapticOn = haptic;
        //print ("haptic is now " + haptic);
    }
    public void UpdateSFXMute (bool thismuted) {
        sfxMuted = thismuted;
    }
    public void UpdateBGMMute (bool thismuted) {
        bgmMuted = thismuted;
    }
    public void UpdateHighScore (int score) {
        highScore = score;
    }
    public void UpdateDailyHighScore (int score) {
        dailyHighScore = score;
    }
    public void UpdateActualDailyHighScore (int score) {
        actualDailyHighScore = score;
    }
    public void UpdateFBToken (string token) {
        FBTokenString = token;
    }
    public void UpdateCoins (int coins) {
        totalCoins = coins;
    }
    public void UpdateOrbs (int orbs) {
        orbCount = orbs;
    }
    public void UpdateBalance (int bal) {
        balance = bal;
    }
    public void UpdateCarsOwned (List<string> list) {
        carsOwned = list;
    }
    public void UpdateTrailsOwned (List<string> list) {
        trailsOwned = list;
    }
    public void UpdateChallengesToFinish (List<int> chal) {
        challengesToFinish = chal;
    }
    public void UpdateChallengesCompleted (List<int> chal) {
        challengesCompleted = chal;
    }
    public void UpdateCarUsing (string ind) {
        carUsing = ind;
    }
    public void UpdateTrailUsing (string ind) {
        trailUsing = ind;
    }
    public void UpdateFacebookLinked (bool linked, string name = "") {
        FBLinked = linked;
        FBName = name;
    }
    public void UpdateUsernameSet (bool boolean) {
        UsernameSet = boolean;
    }
    public void UpdateDisplayName (string name) {
        displayName = name;
    }
    public void BuyCar (string id) {
        totalCoins = StoreController.instance.newCoinCount;
        carsOwned.Add (id);
        StoreCoinCountController.UpdateValue ();
    }
    public void BuyTrail (string id) {
        orbCount = StoreController.instance.newOrbCount;
        trailsOwned.Add (id);
        StoreOrbCount.UpdateValue ();
    }
    public void UpdateAvatarUrlInd (string url, int ind) {
        AvatarUrl = url;
        AvatarInd = ind;
    }
    public void Save () {
        SaveData.Save (this);
    }
}