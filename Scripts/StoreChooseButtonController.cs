using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreChooseButtonController : MonoBehaviour {
    public static StoreChooseButtonController instance;
    StoreController store;
    public GameObject UICoin;
    public GameObject UIOrb;
    Vector3 leftPosition = new Vector3 (-120, 0, 0);
    Vector3 farLeftPosition = new Vector3 (-160, 0, 0);
    Vector3 centerPosition = new Vector3 (0, 0, 0);
    List<string> ownedCars;
    List<string> ownedTrails;
    Image image;
    void Start () {
        instance = this;
        image = GetComponentInParent<Image> ();
        store = GameObject.FindWithTag ("StoreController").GetComponent<StoreController> ();
        ownedCars = DataEntry.instance.carsOwned;
        ownedTrails = DataEntry.instance.trailsOwned;
        UIOrb.SetActive (false);
        if (store.storeMode) {
            //cars
            UpdateButton (store.storeMode, ownedCars.Contains (GameController.GetCarNameFromIndex (StoreController.instance.currentIndex)),
                GameController.GetCarNameFromIndex (StoreController.instance.currentIndex) == DataEntry.instance.carUsing);
            //print (GameController.GetCarNameFromIndex (StoreController.instance.currentIndex) == DataEntry.instance.carUsing);
            // //print (DataEntry.instance.carUsing);
        } else {
            //trails
            UpdateButton (store.storeMode, ownedTrails.Contains (GameController.GetTrailNameFromIndex (StoreController.instance.currentIndex)), GameController.GetTrailNameFromIndex (StoreController.instance.currentIndex) == DataEntry.instance.trailUsing);
        }
    }
    public void UpdateButton (bool carMode, bool owned, bool selected) {
        if (carMode == true) {
            //Store in car mode
            UIOrb.SetActive (false);
            image.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 400, 0);
            if (owned) {
                GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
                gameObject.transform.localPosition = centerPosition;
                // gameObject.GetComponent<Text> ().font = Resources.Load<Font> ("Fonts/Raleway/Raleway-Medium");
                UICoin.SetActive (false);
                if (selected) {
                    gameObject.GetComponentInChildren<Text> ().text = "SELECTED";
                    image.color = new Color (0.85f, 0.85f, 0.85f, 1f);
                } else {
                    gameObject.GetComponentInChildren<Text> ().text = "SELECT";
                    image.color = new Color (1f, 1f, 1f, 1f);
                }
            } else {
                image.color = new Color (1f, 1f, 1f, 1f);
                gameObject.GetComponent<Text> ().alignment = TextAnchor.MiddleRight;
                //  gameObject.GetComponent<Text> ().font = Resources.Load<Font> ("Fonts/roboto/Roboto-Regular");
                gameObject.transform.localPosition = leftPosition;
                UICoin.SetActive (true);
                gameObject.GetComponentInChildren<Text> ().text = GameController.GetStats (StoreController.instance.currentCarID) ["Price"].ToString ();
            }
        } else {
            UICoin.SetActive (false);
            image.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 400, 0);
            ////print (ownedTrails.Count);
            if (owned) {
                // //print ("select trail");
                GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
                gameObject.transform.localPosition = centerPosition;
                //  gameObject.GetComponent<Text> ().font = Resources.Load<Font> ("Fonts/Raleway/Raleway-Medium");
                UIOrb.SetActive (false);
                if (selected) {
                    gameObject.GetComponentInChildren<Text> ().text = "SELECTED";
                    image.color = new Color (0.85f, 0.85f, 0.85f, 1f);
                } else {
                    gameObject.GetComponentInChildren<Text> ().text = "SELECT";
                    image.color = new Color (1f, 1f, 1f, 1f);
                }
            } else {
                //print ("buy trail");
                image.color = new Color (1f, 1f, 1f, 1f);
                gameObject.GetComponent<Text> ().alignment = TextAnchor.MiddleRight;
                //gameObject.GetComponent<Text> ().font = Resources.Load<Font> ("Fonts/roboto/Roboto-Regular");
                gameObject.transform.localPosition = farLeftPosition;
                UIOrb.SetActive (true);
                gameObject.GetComponentInChildren<Text> ().text = GameController.GetTrail (StoreController.instance.currentTrailID) ["Price"].ToString ();
            }
        }
    }
    void Update () {
        if ((store.pageChange == true) || (store.buttonPressed == true) || (store.toggled == true)) {
            ////print ("button updated");
            if (store.storeMode) {
                //cars
                UpdateButton (store.storeMode, DataEntry.instance.carsOwned.Contains (GameController.GetCarNameFromIndex (StoreController.instance.currentIndex)), GameController.GetCarNameFromIndex (StoreController.instance.currentIndex) == DataEntry.instance.carUsing);
            } else {
                //trails
                UpdateButton (store.storeMode, DataEntry.instance.trailsOwned.Contains (GameController.GetTrailNameFromIndex (StoreController.instance.currentIndex)), GameController.GetTrailNameFromIndex (StoreController.instance.currentIndex) == DataEntry.instance.trailUsing);
            }
            store.pageChange = false;
            store.buttonPressed = false;
            store.toggled = false;
        }
    }
}