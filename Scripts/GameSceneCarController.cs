using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameSceneCarController : MonoBehaviour {
    StoreController storeController;
    // Use this for initialization
    void Awake () {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild (i).gameObject.name == DataEntry.instance.carUsing) {
                transform.GetChild (i).gameObject.SetActive (true);
            } else {
                transform.GetChild (i).gameObject.SetActive (false);
            }
        }
    }
    // Update is called once per frame
    void Update () { }
}