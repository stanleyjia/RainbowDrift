using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeVolume : MonoBehaviour {
    public static ChangeVolume instance;
    GameObject[] audioBGM;
    GameObject[] audioSFX;
    // Use this for initialization
    void Start () {
        audioBGM = GameObject.FindGameObjectsWithTag ("BGM");
        audioSFX = GameObject.FindGameObjectsWithTag ("SFX");
        instance = this;
        if (DataEntry.instance.sfxMuted) {
            MuteSFX ();
        }
        if (DataEntry.instance.bgmMuted) {
            MuteBGM ();
        }
        //Because no more volume slider
        ChangeThisVolume (DataEntry.instance.volume);
        //print ("Volume: " + DataEntry.instance.volume);
    }
    public void ChangeThisVolume (float value) {
        AudioListener.volume = value;
        DataEntry.instance.UpdateVolume (value);
        DataEntry.instance.Save ();
        //print ("New Volume: " + DataEntry.instance.volume);
    }
    public void MuteBGM () {
        for (int i = 0; i < audioBGM.Length; i++) {
            audioBGM[i].GetComponent<AudioSource> ().volume = 0f;
        }
    }
    public void UnmuteBGM () {
        for (int i = 0; i < audioBGM.Length; i++) {
            audioBGM[i].GetComponent<AudioSource> ().volume = PlayAudio.soundVolume[audioBGM[i].name];
        }
    }
    public void MuteSFX () {
        for (int i = 0; i < audioSFX.Length; i++) {
            audioSFX[i].GetComponent<AudioSource> ().volume = 0f;
        }
    }
    public void UnmuteSFX () {
        for (int i = 0; i < audioSFX.Length; i++) {
            audioSFX[i].GetComponent<AudioSource> ().volume = PlayAudio.soundVolume[audioSFX[i].name];
        }
    }
    public void ToggleBGM () {
        if (DataEntry.instance.bgmMuted) {
            DataEntry.instance.bgmMuted = false;
            UnmuteBGM ();
        } else {
            DataEntry.instance.bgmMuted = true;
            MuteBGM ();
        }
        DataEntry.instance.Save ();
    }
    public void ToggleSFX () {
        if (DataEntry.instance.sfxMuted) {
            DataEntry.instance.sfxMuted = false;
            UnmuteSFX ();
        } else {
            DataEntry.instance.sfxMuted = true;
            MuteSFX ();
        }
        DataEntry.instance.Save ();
    }
}