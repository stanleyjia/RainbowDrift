using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
public static class SaveData {
    public static void Save (DataEntry dataEntry) {
        BinaryFormatter bf = new BinaryFormatter ();
        FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Create);
        PlayerData data = new PlayerData (dataEntry);
        ////Debug.log ("DDD " + dataEntry.bgmMuted);
        bf.Serialize (stream, data);
        stream.Close ();
    }
    public static PlayerData LoadData () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            return (data);
        }
        //Debug.log ("File does not exist");
        return null;
    }
    /*
    public static int[] LoadStats () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            // //Debug.log(data.stats.Length);
            return (data.stats);
        }
        //Debug.log ("File does not exist");
        return new int[4];
    }
    public static Dictionary<string, int> LoadUserStatistics () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            // //Debug.log(data.stats.Length);
            return (data.userStatistics);
        }
        //Debug.log ("File does not exist");
        return new Dictionary<string, int> ();
    }
    public static List<string> LoadCars () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            return (data.ownedCars);
        }
        //Debug.log ("File does not exist");
        return new List<string> ();
    }
    public static List<string> LoadTrails () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            return (data.ownedTrails);
        }
        //Debug.log ("File does not exist");
        return new List<string> ();
    }
    public static List<int> LoadChallengesCompleted () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            return (data.challengesCompleted);
        }
        //Debug.log ("File does not exist");
        return new List<int> ();
    }
    public static List<int> LoadChallengesToFinish () {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            return (data.challengesToFinish);
        }
        //Debug.log ("File does not exist");
        return new List<int> ();
    }
    public static bool LoadBoolean (string name) {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            try {
                return ((bool) data.GetType ().GetField (name).GetValue (data));
            } catch {
                //Debug.log ("Couldn't find variable");
                return false;
            }
        }
        //Debug.log ("File does not exist");
        return false;
    }
    public static int LoadInteger (string name) {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            try {
                return ((int) data.GetType ().GetField (name).GetValue (data));
            } catch {
                //Debug.log ("Couldn't find variable: " + name);
                return 0;
            }
        }
        //Debug.log ("File does not exist");
        return 0;
    }
    public static float LoadFloat (string name) {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            try {
                return ((float) data.GetType ().GetField (name).GetValue (data));
            } catch {
                //Debug.log ("Couldn't find variable");
                return 0f;
            }
        }
        //Debug.log ("File does not exist");
        return 0;
    }
    public static string LoadString (string name) {
        if (File.Exists (Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = new FileStream (Application.persistentDataPath + "/player.save", FileMode.Open);
            PlayerData data = bf.Deserialize (stream) as PlayerData;
            stream.Close ();
            try {
                return ((string) data.GetType ().GetField (name).GetValue (data));
            } catch {
                //Debug.log ("Couldn't find variable");
                return "";
            }
        }
        //Debug.log ("File does not exist");
        return "";
    }
}*/
}
[Serializable]
public class PlayerData {
    public string FBTokenString;
    public float volume;
    public bool sfxMuted;
    public bool bgmMuted;
    public bool hapticOn;
    public int[] stats;
    public List<string> ownedCars;
    public List<string> ownedTrails;
    public List<int> challengesCompleted;
    public List<int> challengesToFinish;
    public Dictionary<string, string> userPassEmail;
    public bool firstTime;
    public string carUsing;
    public string trailUsing;
    public bool FBLinked;
    public string FBName;
    public string AvatarUrl;
    public int AvatarInd;
    public bool UsernameSet;
    public bool loggedIn;
    public string displayName;
    public int dailyHighScore;
    public int actualDailyHighScore;
    public bool firstTimeOnDevice;
    public Dictionary<string, int> userStatistics;
    public PlayerData (DataEntry dataEntry) {
        FBTokenString = dataEntry.FBTokenString;
        sfxMuted = dataEntry.sfxMuted;
        bgmMuted = dataEntry.bgmMuted;
        hapticOn = dataEntry.hapticOn;
        volume = dataEntry.volume;
        firstTime = dataEntry.firstTime;
        ownedCars = dataEntry.carsOwned;
        ownedTrails = dataEntry.trailsOwned;
        challengesCompleted = dataEntry.challengesCompleted;
        challengesToFinish = dataEntry.challengesToFinish;
        carUsing = dataEntry.carUsing;
        trailUsing = dataEntry.trailUsing;
        FBLinked = dataEntry.FBLinked;
        FBName = dataEntry.FBName;
        stats = new int[4];
        stats[0] = dataEntry.highScore;
        stats[1] = dataEntry.orbCount;
        stats[2] = dataEntry.totalCoins;
        stats[3] = dataEntry.balance;
        AvatarUrl = dataEntry.AvatarUrl;
        AvatarInd = dataEntry.AvatarInd;
        UsernameSet = dataEntry.UsernameSet;
        displayName = dataEntry.displayName;
        dailyHighScore = dataEntry.dailyHighScore;
        actualDailyHighScore = dataEntry.actualDailyHighScore;
        userStatistics = dataEntry.userStatistics;
        firstTimeOnDevice = dataEntry.firstTimeOnDevice;
    }
}