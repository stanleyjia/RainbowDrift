using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ParticleController : MonoBehaviour {
    ParticleSystem current;
    ParticleSystem currentRainbow;
    Transform coinHit;
    Transform orbHit;
    bool started = false;
    bool oneTime = true;
    bool lastRainbowMode;
    string trailUsing;
    // Use this for initialization
    void Start () {
        coinHit = transform.Find ("CoinHit").transform;
        orbHit = transform.Find ("OrbHit").transform;
        trailUsing = (DataEntry.instance.trailUsing);
        if (SceneManager.GetActiveScene ().name == "TutorialScene") {
            //           //print ("TUtorial");
            current = transform.Find ("Trails").Find ("T0001").Find ("Trail").GetComponent<ParticleSystem> ();
            currentRainbow = transform.Find ("Trails").Find ("T0001").Find ("Rainbow").GetComponent<ParticleSystem> ();
            foreach (Transform child in transform.Find ("Trails")) {
                //  //print (child.name);
                if (child.name != "T0001") {
                    child.gameObject.SetActive (false);
                }
            }
        } else {
            current = transform.Find ("Trails").Find (trailUsing).Find ("Trail").GetComponent<ParticleSystem> ();
            currentRainbow = transform.Find ("Trails").Find (trailUsing).Find ("Rainbow").GetComponent<ParticleSystem> ();
            foreach (Transform child in transform.Find ("Trails")) {
                if (child.name != trailUsing) {
                    child.gameObject.SetActive (false);
                } else {
                    child.gameObject.SetActive (true);
                }
            }
        }
        //current.Play ();
        currentRainbow.Stop ();
        current.Stop ();
        StartCoroutine (DelayedPlay (current));
    }
    IEnumerator DelayedPlay (ParticleSystem ps) {
        yield return new WaitForSeconds (0.5f);
        lastRainbowMode = ColorBarController.instance.rainbowMode;
        started = true;
        ps.Play ();
    }
    public void PlayCoinHit (Vector3 pos) {
        coinHit = transform.Find ("CoinHit");
        coinHit.localPosition = pos;
        for (int i = 0; i < coinHit.childCount; i++) {
            coinHit.GetChild (i).GetComponent<ParticleSystem> ().Play ();
        }
    }
    public void PlayOrbHit (Vector3 pos) {
        orbHit = transform.Find ("OrbHit");
        orbHit.localPosition = pos;
        for (int i = 0; i < orbHit.childCount; i++) {
            orbHit.GetChild (i).GetComponent<ParticleSystem> ().Play ();
        }
    }
    // Update is called once per frame
    void Update () {
        if (CarVariables.instance.gameOn == false) {
            if (oneTime == true) {
                //ps.Stop();
                StartCoroutine (DelayedStop (current));
                StartCoroutine (DelayedStop (currentRainbow));
                oneTime = false;
            }
        } else {
            if (started) {
                if (ColorBarController.instance.rainbowMode != lastRainbowMode) {
                    if (ColorBarController.instance.rainbowMode == true) {
                        current.Stop ();
                        currentRainbow.Play ();
                    } else {
                        //print ("Play normal");
                        current.Play ();
                        currentRainbow.Stop ();
                    }
                    lastRainbowMode = ColorBarController.instance.rainbowMode;
                }
            }
        }
    }
    IEnumerator DelayedStop (ParticleSystem ps) {
        yield return new WaitForSeconds (1f);
        ps.Stop ();
    }
}