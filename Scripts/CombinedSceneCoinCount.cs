using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombinedSceneCoinCount : MonoBehaviour {
    Text text;
    void Start () {
        text = gameObject.GetComponent<Text> ();
        text.text = DataEntry.instance.totalCoins.ToString ();
        CheckValue ();
        InvokeRepeating ("CheckValue", 0, 1f);
    }
    void Update () { }
    void CheckValue () {
        if (DataEntry.instance.totalCoins.ToString () != text.text) {
            text.text = DataEntry.instance.totalCoins.ToString ();
        }
    }
}