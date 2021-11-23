using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombinedSceneOrbCount : MonoBehaviour {
    Text text;
    void Start () {
        text = gameObject.GetComponent<Text> ();
        text.text = DataEntry.instance.orbCount.ToString ();
        InvokeRepeating ("CheckValue", 0, 1f);
    }
    void Update () { }
    void CheckValue () {
        if (DataEntry.instance.orbCount.ToString () != gameObject.GetComponent<Text> ().text) {
            text.text = DataEntry.instance.orbCount.ToString ();
        }
    }
}