using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour {
    public static GameController instance;
    public string previousScene;
    public static string currentCar;
    public static string currentTrail;
    bool isLoggedIn;
    private static Dictionary<string, object> cars = new Dictionary<string, object> ();
    private static Dictionary<string, object> C0001 = new Dictionary<string, object> { { "Acceleration", 7.5f },
        { "Name", "THE BEAST" },
        { "Description", "SPEED OVER EVERYTHING" },
        { "ID", "C0001" },
        { "Price", 40000 }
    };
    private static Dictionary<string, object> C0002 = new Dictionary<string, object> { { "Acceleration", 8f },
        { "Name", "ROAD RAGE" },
        { "Description", "MEAN DRIVING MACHINE" },
        { "ID", "C0002" },
        { "Price", 0 }
    };
    private static Dictionary<string, object> C0003 = new Dictionary<string, object> { { "Acceleration", 7.75f },
        { "Name", "THUNDER" },
        { "Description", "FAST LIKE A FLASH" },
        { "ID", "C0003" },
        { "Price", 5000 }
    };
    private static Dictionary<string, object> C0004 = new Dictionary<string, object> { { "Acceleration", 8f },
        { "Name", "ROADRUNNER" },
        { "Description", "EAT MY DUST!" },
        { "ID", "C0004" },
        { "Price", 10000 }
    };
    private static Dictionary<string, object> C0005 = new Dictionary<string, object> { { "Acceleration", 7.75f },
        { "Name", "BIG BOY" },
        { "Description", "MOVE OUT THE WAY..." },
        { "ID", "C0005" },
        { "Price", 20000 }
    };
    private static Dictionary<string, object> C0006 = new Dictionary<string, object> { { "Acceleration", 7.75f },
        { "Name", "HOT ROD" },
        { "Description", "THE QUINTESSENTIAL MUSCLE CAR" },
        { "ID", "C0006" },
        { "Price", 80000 }
    };
    private static Dictionary<string, object> C0007 = new Dictionary<string, object> { { "Acceleration", 7.75f },
        { "Name", "SCHOOL BUS" },
        { "Description", "TAKE EM TO SCHOOL" },
        { "ID", "C0007" },
        { "Price", 160000 }
    };
    private static Dictionary<string, int> carIndex = new Dictionary<string, int> {
        //
        { "C0002", 0 },
        { "C0003", 1 },
        { "C0004", 2 },
        { "C0005", 3 },
        { "C0001", 4 },
        { "C0006", 5 },
        { "C0007", 6 }
    };
    private static Dictionary<int, string> indexCar = new Dictionary<int, string> {
        //
        { 0, "C0002" },
        { 1, "C0003" },
        { 2, "C0004" },
        { 3, "C0005" },
        { 4, "C0001" },
        { 5, "C0006" },
        { 6, "C0007" }
    };
    private static Dictionary<string, int> trailIndex = new Dictionary<string, int> { { "T0001", 0 },
        { "T0002", 1 },
        { "T0003", 2 }
    };
    private static Dictionary<int, string> indexTrail = new Dictionary<int, string> { { 0, "T0001" },
        { 1, "T0002" },
        { 2, "T0003" }
    };
    private static Dictionary<string, object> trails = new Dictionary<string, object> ();
    /*private static Dictionary<string, object> InkTrail = new Dictionary<string, object> {
        {"Name", "Ink"},
        {"Description", "Your standard ink-based trail"},
        {"Price", 0},
        {"Render", "InkTrail"}
    };*/
    private static Dictionary<string, object> T0001 = new Dictionary<string, object> { { "Name", "Sparkles" },
        { "Description", "'Ooh, sparkly'" },
        { "Price", 0 },
        { "Render", "T0001" }
    };
    private static Dictionary<string, object> T0002 = new Dictionary<string, object> { { "Name", "Smoke" },
        { "Description", "Global Warming" },
        { "Price", 10 },
        { "Render", "T0002" }
    };
    private static Dictionary<string, object> T0003 = new Dictionary<string, object> { { "Name", "Bubbles" },
        { "Description", "How many can you pop?" },
        { "Price", 20 },
        { "Render", "T0003" }
    };
    void Start () {
        DontDestroyOnLoad (this);
        if (cars.Count == 0) {
            cars.Add ("C0001", C0001);
            cars.Add ("C0002", C0002);
            cars.Add ("C0003", C0003);
            cars.Add ("C0004", C0004);
            cars.Add ("C0005", C0005);
            cars.Add ("C0006", C0006);
            cars.Add ("C0007", C0007);
        }
        instance = this;
        //trails.Add(0, InkTrail);
        if (trails.Count == 0) {
            trails.Add ("T0001", T0001);
            trails.Add ("T0002", T0002);
            trails.Add ("T0003", T0003);
        }
        currentTrail = DataEntry.instance.trailUsing;
        currentCar = DataEntry.instance.carUsing;
        //SceneManager.LoadSceneAsync("GameScene");
    }
    public static int GetCarIndexFromID (string str) {
        return carIndex[str];
    }
    public static int GetTrailIndexFromID (string str) {
        return trailIndex[str];
    }
    public static Dictionary<string, object> GetTrail (string index) {
        if (trails[index] != null) {
            return (trails[index] as Dictionary<string, object>);
        }
        return new Dictionary<string, object> ();
    }
    public static Dictionary<string, object> GetStats (string index) {
        if (cars[index] != null) {
            return (cars[index] as Dictionary<string, object>);
        }
        return new Dictionary<string, object> ();
    }
    public static string GetCurrentCar () {
        return currentCar;
    }
    public static string GetCarNameFromIndex (int index) {
        return indexCar[index].ToString ();
    }
    public static string GetTrailNameFromIndex (int index) {
        return indexTrail[index].ToString ();
    }
    public static int GetTrailCount () {
        return trails.Count;
    }
    public static int GetCarCount () {
        return cars.Count;
    }
    public static Dictionary<string, Dictionary<string, float>> GetHighAndLow () {
        Dictionary<string, Dictionary<string, float>> highLow = new Dictionary<string, Dictionary<string, float>> () {
            {
            "Acceleration",
            new Dictionary<string, float> () { { "high", 12.5f }, { "low", 6 }
            }
            }, {
            "Top Speed",
            new Dictionary<string, float> () { { "high", 13.5f }, { "low", 6 }
            }
            }, {
            "Handling",
            new Dictionary<string, float> () { { "high", 280f }, { "low", 235 }
            }
            }, {
            "Traction",
            new Dictionary<string, float> () { { "high", 0.999f }, { "low", 0.985f }
            }
            }, {
            "Coolness",
            new Dictionary<string, float> () { { "high", 10f }, { "low", 0.6f }
            }
            }
        };
        return (highLow);
    }
}