using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbController : MonoBehaviour {
    Pathmaker pathmaker;
    OrbGeneratorController controller;
    float distance;
    private int interval = 3;
    void Start () {
        pathmaker = GameObject.FindWithTag ("Pathmaker").GetComponent<Pathmaker> ();
        controller = GameObject.FindWithTag ("OrbGenerator").GetComponent<OrbGeneratorController> ();
        transform.up = Vector3.up;
    }
    void Update () {
        if (Time.frameCount % interval == 0) {
            if (pathmaker.removeOrbs == true) {
                CheckForDistance (pathmaker.points, transform.position);
                pathmaker.removeOrbs = false;
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
            OrbGeneratorController.instance.noneLost = false;
            transform.parent.gameObject.SetActive (false);
        }
    }
    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            PlayAudio.PlaySound ("orbPickup");
            if (DataEntry.instance.hapticOn) {
                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 1);
            }
            OrbGeneratorController.instance.orbCount += 1;
            controller.hitOrb = true;
            controller.hitOrb2 = true;
            transform.parent.gameObject.SetActive (false);
        }
    }
}