using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreOrbCount : MonoBehaviour {
    static Text text;
    static StoreOrbCount instance;
    void Start () {
        instance = this;
        text = gameObject.GetComponent<Text> ();
        text.text = DataEntry.instance.orbCount.ToString ();
        InvokeRepeating ("CheckValue", 0, 1f);
    }
    public static void UpdateValue () {
        instance.CheckValue ();
    }
    void CheckValue () {
        if (DataEntry.instance.orbCount.ToString () != text.text) {
            gameObject.GetComponent<Text> ().text = DataEntry.instance.orbCount.ToString ();
        }
    }
}