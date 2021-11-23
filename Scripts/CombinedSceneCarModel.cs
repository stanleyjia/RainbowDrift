using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombinedSceneCarModel : MonoBehaviour {
    void Start () {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild (i).gameObject.name == DataEntry.instance.carUsing) {
                transform.GetChild (i).gameObject.SetActive (true);
            } else {
                transform.GetChild (i).gameObject.SetActive (false);
            }
        }
    }
    // Update is called once per frame
}