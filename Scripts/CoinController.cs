using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinController : MonoBehaviour {
    Pathmaker pathmaker;
    CoinGeneratorController controller;
    float distance;
    private int interval = 3;
    private int interval1 = 10;
    int amount = 1;
    bool inRange = false;
    bool outAlready = false;
    GameObject player;
    //float multiplier;
    void Start () {
        player = GameObject.FindWithTag ("Player");
        pathmaker = GameObject.FindWithTag ("Pathmaker").GetComponent<Pathmaker> ();
        controller = GameObject.FindWithTag ("CoinGenerator").GetComponent<CoinGeneratorController> ();
    }
    void Update () {
        if (Time.frameCount % interval == 1) {
            if (pathmaker.removeObjects == true) {
                CheckForDistance (pathmaker.points, transform.position);
            }
        };
        if (Time.frameCount % interval == 0) {
            CheckForRange ();
        }
    }
    void CheckForRange () {
        if (outAlready == false) {
            if (CoinGeneratorController.instance.noneLost == true) {
                if (inRange == false) {
                    if (Vector3.Distance (transform.position, player.transform.position) < 4f) {
                        inRange = true;
                    }
                } else {
                    if (Vector3.Distance (transform.position, player.transform.position) > 4f) {
                        outAlready = true;
                        CoinGeneratorController.instance.noneLost = false;
                    }
                }
            }
        }
    }
    void CheckForDistance (List<Vector3> points, Vector3 position) {
        distance = 15f;
        for (int i = 0; i < points.Count - 1; i++) {
            if (Vector3.Distance (points[i], position) < distance) {
                distance = Vector3.Distance (points[i], position);
            }
        }
        if (distance > 10f) {
            if (controller.coinPositions.Contains (transform.position) == true) {
                CoinGeneratorController.instance.noneLost = false;
                controller.coinPositions.Remove (transform.position);
            }
            transform.parent.gameObject.SetActive (false);
        }
    }
    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            outAlready = true;
            PlayAudio.PlaySound ("collectCoin");
            if (DataEntry.instance.hapticOn) {
                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 1);
            }
            if (ColorBarController.instance.rainbowMode == true) {
                CoinGeneratorController.instance.coinCount = CoinGeneratorController.instance.coinCount + ((ColorBarController.instance.colorMode + 2));
            } else {
                CoinGeneratorController.instance.coinCount = CoinGeneratorController.instance.coinCount + (ColorBarController.instance.colorMode + 1);
            }
            if (controller.coinPositions.Contains (transform.position) == true) {
                controller.coinPositions.Remove (transform.position);
            }
            transform.parent.gameObject.SetActive (false);
        }
    }
}