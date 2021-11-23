using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinParticleController : MonoBehaviour {
    bool lastRainbowMode = false;
    int lastColorMode = 0;
    ParticleSystem sparkles;
    ParticleSystem glow;
    ParticleSystem glow1;
    ParticleSystem glow2;
    ParticleSystem glow3;
    Dictionary<int, ParticleSystem> dict = new Dictionary<int, ParticleSystem> ();
    // Use this for initialization
    void Start () {
        glow = transform.Find ("Glow").GetComponent<ParticleSystem> ();
        glow1 = transform.Find ("Glow1").GetComponent<ParticleSystem> ();
        glow2 = transform.Find ("Glow2").GetComponent<ParticleSystem> ();
        glow3 = transform.Find ("Glow3").GetComponent<ParticleSystem> ();
        sparkles = transform.Find ("Sparkle").GetComponent<ParticleSystem> ();
        dict.Add (1, glow);
        dict.Add (2, glow1);
        dict.Add (3, glow2);
        dict.Add (4, glow3);
        glow1.Stop ();
        glow2.Stop ();
        glow3.Stop ();
        glow.Stop ();
        sparkles.Stop ();
        lastRainbowMode = ColorBarController.instance.rainbowMode;
        lastColorMode = ColorBarController.instance.colorMode;
    }
    // Update is called once per frame
    void PlayRainbow () {
        //print ("----");
        //print (ColorBarController.instance.colorMode + 1);
        foreach (KeyValuePair<int, ParticleSystem> pair in dict) {
            if (pair.Key == (ColorBarController.instance.colorMode + 1)) {
                if (pair.Value.isPlaying == false) {
                    pair.Value.Play ();
                    //print (pair.Key + " played");
                }
            } else {
                if (pair.Value.isPlaying) {
                    pair.Value.Pause ();
                    pair.Value.Stop (true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //print (pair.Key + " stopped");
                    //print ("After" + pair.Value.isPlaying);
                }
            }
            //print (pair.Key + "  " + pair.Value.isPlaying);
        }
        //print (ColorBarController.instance.colorMode);
        //print ("Yellow glow Plauin: " + glow.isPlaying);
    }
    void StopRainbow () {
        foreach (KeyValuePair<int, ParticleSystem> pair in dict) {
            if (pair.Key == (ColorBarController.instance.colorMode)) {
                if (pair.Value.isPlaying == false) {
                    pair.Value.Play ();
                }
            } else {
                if (pair.Value.isPlaying) {
                    pair.Value.Pause ();
                    pair.Value.Stop (true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }
    void Update () {
        if (lastColorMode != ColorBarController.instance.colorMode) {
            if (ColorBarController.instance.rainbowMode == true) {
                PlayRainbow ();
                if (sparkles.isPlaying == false) {
                    sparkles.Play ();
                }
            } else {
                if (sparkles.isPlaying == true) {
                    sparkles.Stop ();
                }
                StopRainbow ();
            }
            lastRainbowMode = ColorBarController.instance.rainbowMode;
            lastColorMode = ColorBarController.instance.colorMode;
        }
        if (lastRainbowMode != ColorBarController.instance.rainbowMode) {
            if (ColorBarController.instance.rainbowMode == true) {
                PlayRainbow ();
                if (sparkles.isPlaying == false) {
                    sparkles.Play ();
                }
            } else {
                if (sparkles.isPlaying == true) {
                    sparkles.Stop ();
                }
                StopRainbow ();
            }
            lastColorMode = ColorBarController.instance.colorMode;
            lastRainbowMode = ColorBarController.instance.rainbowMode;
        }
    }
}