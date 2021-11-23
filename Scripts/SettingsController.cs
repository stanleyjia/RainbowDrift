using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsController : MonoBehaviour {
    public static SettingsController instance;
    public Text hapticButtonText;
    bool hapticOn;
    void Start () {
        instance = this;
        //print (DataEntry.instance.hapticOn);
        InitializeSettings ();
    }
    // Update is called once per frame
    void Update () { }
    public void InitializeSettings () {
        hapticOn = DataEntry.instance.hapticOn;
        if (hapticOn) {
            hapticButtonText.text = "HAPTIC FEEDBACK: ON";
        } else {
            hapticButtonText.text = "HAPTIC FEEDBACK: OFF";
        }
    }
    public void ToggleHaptic () {
        if (hapticOn) {
            hapticOn = false;
            hapticButtonText.text = "HAPTIC FEEDBACK: OFF";
            DataEntry.instance.UpdateHaptic (false);
        } else {
            hapticOn = true;
            hapticButtonText.text = "HAPTIC FEEDBACK: ON";
            DataEntry.instance.UpdateHaptic (true);
        }
        DataEntry.instance.Save ();
        //print (DataEntry.instance.hapticOn);
    }
}