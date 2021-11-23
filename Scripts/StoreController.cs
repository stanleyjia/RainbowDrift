using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
public class StoreController : MonoBehaviour {
    public GameObject carRender;
    public GameObject currentCarRender;
    public Text carName;
    public Text carDescription;
    public static StoreController instance;
    public bool buyingCar = false;
    public bool buyingTrail = false;
    public bool trailSelected = false;
    public int boughtCarIndex = 0;
    public int currentIndex = 0;
    public int boughtTrailIndex = 0;
    public string carUsing;
    public string trailUsing;
    public bool pageChange = false;
    public bool buttonPressed;
    public int newCoinCount;
    public int newOrbCount;
    List<string> carsOwned;
    List<string> trailsOwned;
    public Animator coinWarning;
    public bool toggled = false;
    //public GameObject statsPanel;
    public GameObject carNameDesc;
    public GameObject trailRender;
    public string currentCarID;
    public string currentTrailID;
    public bool storeMode = true;
    bool chal3;
    bool chal12;
    bool chal21;
    bool chal22;
    bool chal28;
    Text text;
    void Awake () {
        //BACK
        instance = this;
        //sparkle = GameObject.FindWithTag("UIParticle").GetComponent<ParticleSystem>();
        carsOwned = DataEntry.instance.carsOwned;
        currentCarID = DataEntry.instance.carUsing;
        currentIndex = GameController.GetCarIndexFromID (currentCarID);
        SetInfo (currentCarID);
        //  sparkle.Stop()
        text = coinWarning.gameObject.GetComponentInChildren<Text> ();
        trailRender.SetActive (false);
        chal3 = ChallengesController.ChallengeDone (3);
        chal12 = ChallengesController.ChallengeDone (12);
        chal21 = ChallengesController.ChallengeDone (21);
        chal22 = ChallengesController.ChallengeDone (22);
    }
    void SetInfo (string index) {
        if (GameController.GetStats (index) ["Name"] != null) {
            carName.text = GameController.GetStats (index) ["Name"].ToString ().ToUpper ();
            carDescription.text = GameController.GetStats (index) ["Description"].ToString ().ToUpper ();
            for (int i = 0; i < carRender.transform.childCount; i++) {
                if (carRender.transform.GetChild (i).gameObject.name == GameController.GetStats (index) ["ID"].ToString ()) {
                    carRender.transform.GetChild (i).gameObject.SetActive (true);
                    if (currentCarRender) {
                        carRender.transform.GetChild (i).transform.rotation = currentCarRender.transform.rotation;
                        carRender.transform.GetChild (i).GetComponent<CarModelRotator> ().RotateCar ();
                    }
                    currentCarRender = carRender.transform.GetChild (i).gameObject;
                } else {
                    carRender.transform.GetChild (i).GetComponent<CarModelRotator> ().rotating = false;
                    carRender.transform.GetChild (i).gameObject.SetActive (false);
                }
            }
        }
    }
    void SetTrailInfo (string index) {
        if (GameController.GetTrail (index) ["Name"] != null) {
            carName.text = GameController.GetTrail (index) ["Name"].ToString ().ToUpper ();
            carDescription.text = GameController.GetTrail (index) ["Description"].ToString ().ToUpper ();
        }
        int children = trailRender.transform.childCount;
        for (int i = 0; i < children; ++i) {
            if (trailRender.transform.GetChild (i).name == GameController.GetTrail (index) ["Render"].ToString ()) {
                trailRender.transform.GetChild (i).gameObject.SetActive (true);
            } else {
                trailRender.transform.GetChild (i).gameObject.SetActive (false);
            }
        }
    }
    public void RightButton () {
        //sparkle.Stop();
        if (storeMode == true) {
            if (currentIndex < GameController.GetCarCount () - 1) {
                currentIndex += 1;
            } else {
                currentIndex = (currentIndex + 1) % (GameController.GetCarCount ());
            }
            currentCarID = GameController.GetCarNameFromIndex (currentIndex);
            SetInfo (currentCarID);
            pageChange = true;
        } else {
            if (currentIndex < GameController.GetTrailCount () - 1) {
                currentIndex += 1;
            } else {
                currentIndex = (currentIndex + 1) % (GameController.GetTrailCount ());
            }
            currentTrailID = GameController.GetTrailNameFromIndex (currentIndex);
            SetTrailInfo (currentTrailID);
            pageChange = true;
        }
    }
    public void LeftButton () {
        // sparkle.Stop();
        if (storeMode == true) {
            if (currentIndex > 0) {
                currentIndex -= 1;
            } else {
                currentIndex = GameController.GetCarCount () - 1 + currentIndex;
            }
            currentCarID = GameController.GetCarNameFromIndex (currentIndex);
            SetInfo (currentCarID);
            pageChange = true;
        } else {
            if (currentIndex > 0) {
                currentIndex -= 1;
            } else {
                currentIndex = GameController.GetTrailCount () - 1 + currentIndex;
            }
            currentTrailID = GameController.GetTrailNameFromIndex (currentIndex);
            SetTrailInfo (currentTrailID);
            pageChange = true;
        }
    }
    public void ToggleMode (bool mode) {
        toggled = true;
        //sparkle.Stop();
        if (mode != storeMode) {
            storeMode = mode;
            if (mode == true) {
                //Cars
                StoreModeToggle.instance.GoToLeft ();
                currentIndex = GameController.GetCarIndexFromID (DataEntry.instance.carUsing);
                carRender.SetActive (true);
                // statsPanel.SetActive (true);
                carNameDesc.SetActive (true);
                trailRender.SetActive (false);
                currentCarID = DataEntry.instance.carUsing;
                SetInfo (currentCarID);
            } else {
                //Trails
                StoreModeToggle.instance.GoToRight ();
                currentIndex = GameController.GetTrailIndexFromID (DataEntry.instance.trailUsing);
                carRender.SetActive (false);
                // statsPanel.SetActive (false);
                trailRender.SetActive (true);
                currentTrailID = DataEntry.instance.trailUsing;
                SetTrailInfo (currentTrailID);
            }
        }
    }
    public void NewCar () {
        if (storeMode) {
            currentCarID = GameController.GetCarNameFromIndex (currentIndex);
            carsOwned = DataEntry.instance.carsOwned;
            if (carsOwned.Contains (GameController.GetCarNameFromIndex (StoreController.instance.currentIndex))) {
                if (currentCarID != DataEntry.instance.carUsing) {
                    //Choosing it
                    if (DataEntry.instance.hapticOn) {
                        iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
                    }
                    PlayAudio.PlaySound ("click");
                    PlayAudio.PlaySound ("carSelect");
                    carUsing = GameController.GetCarNameFromIndex (currentIndex);
                    PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                            Data = new Dictionary<string, string> {
                                {
                                    "CurrentCar",
                                    carUsing
                                }
                            }
                        },
                        (UpdateUserDataResult res) => { UserData.instance.UpdateInfo (); },
                        CallFailure
                    );
                    DataEntry.instance.UpdateCarUsing (carUsing);
                    DataEntry.instance.Save ();
                }
            } else {
                PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (), (GetUserInventoryResult result) => {
                    //Attempting to buy current car
                    if (result.VirtualCurrency["CN"] >= (int) GameController.GetStats (currentCarID) ["Price"]) {
                        PlayAudio.PlaySound ("click");
                        bool carAlreadyBought = false;
                        for (int i = 0; i < result.Inventory.Count; i++) {
                            if (result.Inventory[i].ItemId == (string) GameController.GetStats (currentCarID) ["ID"]) {
                                //Car
                                carAlreadyBought = true;
                            }
                        }
                        if (carAlreadyBought == false) {
                            PlayfabStatisticsController.instance.UpdateStats ();
                            PlayfabStatisticsController.instance.statistics["CarsOwned"] += 1;
                            PlayfabStatisticsController.instance.SetStatistics ();
                            if (DataEntry.instance.hapticOn) {
                                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
                            }
                            if (chal3) {
                                ChallengesController.CompleteChallenge (3);
                                chal3 = false;
                            }
                            if (chal12) {
                                if (PlayfabStatisticsController.instance.statistics["CarsOwned"] >= 3) {
                                    ChallengesController.CompleteChallenge (12);
                                    chal12 = false;
                                }
                            }
                            if (chal22) {
                                if (PlayfabStatisticsController.instance.statistics["CarsOwned"] >= 4) {
                                    ChallengesController.CompleteChallenge (22);
                                    chal22 = false;
                                }
                            }
                            if (chal28) {
                                if (PlayfabStatisticsController.instance.statistics["CarsOwned"] >= 5) {
                                    ChallengesController.CompleteChallenge (28);
                                    chal28 = false;
                                }
                            }
                            StoreChooseButtonController.instance.UpdateButton (storeMode, true, false);
                            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                                ItemId = (string) GameController.GetStats (currentCarID) ["ID"],
                                    Price = (int) GameController.GetStats (currentCarID) ["Price"],
                                    VirtualCurrency = "CN",
                            }, (PurchaseItemResult res) => {
                                DataEntry.instance.BuyCar ((string) GameController.GetStats (currentCarID) ["ID"]);
                                UserData.instance.UpdateInfo ();
                            }, CallFailure);
                            newCoinCount = result.VirtualCurrency["CN"] - (int) GameController.GetStats (currentCarID) ["Price"];
                        } else {
                            if (Debug.isDebugBuild) {
                                //print ("already have car");
                            }
                            carAlreadyBought = true;
                        }
                        boughtCarIndex = currentIndex;
                        PlayAudio.PlaySound ("buy");
                    } else {
                        text.text = "NOT ENOUGH COINS!";
                        PlayAudio.PlaySound ("error");
                        if (DataEntry.instance.hapticOn) {
                            iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 5);
                        }
                        StartCoroutine (Warning ());
                    }
                }, CallFailure);
            }
        } else {
            currentTrailID = GameController.GetTrailNameFromIndex (currentIndex);
            trailsOwned = DataEntry.instance.trailsOwned;
            if (trailsOwned.Contains (currentTrailID)) {
                PlayAudio.PlaySound ("click");
                if (DataEntry.instance.hapticOn) {
                    iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
                }
                if (currentTrailID != DataEntry.instance.trailUsing) {
                    trailUsing = currentTrailID;
                    PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
                            Data = new Dictionary<string, string> {
                                {
                                    "CurrentTrail",
                                    trailUsing
                                }
                            }
                        },
                        (UpdateUserDataResult res) => { UserData.instance.UpdateInfo (); },
                        CallFailure
                    );
                    DataEntry.instance.UpdateTrailUsing (trailUsing);
                    DataEntry.instance.Save ();
                    trailSelected = true;
                    PlayAudio.PlaySound ("orbPickup");
                }
            } else {
                PlayFabClientAPI.GetUserInventory (new GetUserInventoryRequest (), (GetUserInventoryResult result) => {
                    if (result.VirtualCurrency["OB"] >= (int) GameController.GetTrail (currentTrailID) ["Price"]) {
                        bool trailAlreadyBought = false;
                        for (int i = 0; i < result.Inventory.Count; i++) {
                            if (result.Inventory[i].ItemId == currentTrailID) {
                                //Car
                                trailAlreadyBought = true;
                            }
                        }
                        if (trailAlreadyBought == false) {
                            if (DataEntry.instance.hapticOn) {
                                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
                            }
                            PlayAudio.PlaySound ("click");
                            StoreChooseButtonController.instance.UpdateButton (storeMode, true, false);
                            PlayfabStatisticsController.instance.UpdateStats ();
                            PlayfabStatisticsController.instance.statistics["TrailsOwned"] += 1;
                            PlayfabStatisticsController.instance.SetStatistics ();
                            if (chal21) {
                                if (PlayfabStatisticsController.instance.statistics["TrailsOwned"] >= 1) {
                                    ChallengesController.CompleteChallenge (21);
                                    chal21 = false;
                                }
                            }
                            PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
                                ItemId = currentTrailID,
                                    Price = (int) GameController.GetTrail (currentTrailID) ["Price"],
                                    VirtualCurrency = "OB",
                            }, (PurchaseItemResult res) => {
                                DataEntry.instance.BuyTrail (currentTrailID);
                                UserData.instance.UpdateInfo ();
                            }, CallFailure);
                            newOrbCount = DataEntry.instance.orbCount - (int) GameController.GetTrail (currentTrailID) ["Price"];
                            buyingTrail = true;
                            boughtTrailIndex = currentIndex;
                            StoreOrbCount.UpdateValue ();
                            PlayAudio.PlaySound ("buy");
                        } else {
                            if (Debug.isDebugBuild) {
                                //print ("already have car");
                            }
                        }
                    } else {
                        PlayAudio.PlaySound ("error");
                        text.text = "NOT ENOUGH ORBS!";
                        if (DataEntry.instance.hapticOn) {
                            iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 5);
                        }
                        StartCoroutine (Warning ());
                    }
                }, CallFailure);
            }
        }
        buttonPressed = true;
    }
    IEnumerator Warning () {
        coinWarning.SetTrigger ("Show");
        yield return new WaitForSeconds (0.5f);
        yield return new WaitForSeconds (0.5f);
        coinWarning.SetTrigger ("Hide");
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
}