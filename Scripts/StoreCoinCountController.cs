using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreCoinCountController : MonoBehaviour {
    static Text text;
    static StoreCoinCountController instance;
    void Start () {
        instance = this;
        text = gameObject.GetComponent<Text> ();
        text.text = DataEntry.instance.totalCoins.ToString ();
        InvokeRepeating ("CheckValue", 0, 1f);
    }
    public static void UpdateValue () {
        instance.CheckValue ();
    }
    void CheckValue () {
        if (DataEntry.instance.totalCoins.ToString () != text.text) {
            if (text) {
                text.text = DataEntry.instance.totalCoins.ToString ();
            }
        }
    }
}