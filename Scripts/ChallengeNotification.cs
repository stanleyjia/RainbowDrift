using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChallengeNotification : MonoBehaviour {
    public Animator shine;
    bool activated = false;
    // Use this for initialization
    void Start () {
    }
    // Update is called once per frame
    void Update () {
        if (ChallengesController.instance.challengeNotif == true) {
            if (activated == false) {
                StartCoroutine (SlideInOut ());
            }
            ChallengesController.instance.challengeNotif = false;
        }
    }
    IEnumerator SlideInOut () {
        activated = true;
        GetComponent<Animator> ().SetTrigger ("Show");
        shine.SetTrigger ("Go");
        yield return new WaitForSeconds (1.25f);
        GetComponent<Animator> ().SetTrigger ("Hide");
        activated = false;
    }
}