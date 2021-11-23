using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour {
    public Slider volumeSlider;
    public Button volumeButton;
    //On Start Screen
    public Button volumeButton2;
    public Button bgm1;
    public Button bgm2;
    public Texture2D volumeOn;
    public Texture2D volumeOff;
    public Texture2D musicOn;
    public Texture2D musicOff;
    Sprite onSprite;
    Sprite offSprite;
    Sprite BGMOnSprite;
    Sprite BGMOffSprite;
    public static VolumeController instance;
    bool chal9;
    // Use this for initialization
    void Start () {
        instance = this;
        // SFX Mute Off
        onSprite = Sprite.Create (volumeOn, new Rect (0, 0, volumeOn.width, volumeOn.height), new Vector2 (0.50f, 0.5f), 100, 1, SpriteMeshType.FullRect);
        //SFX Mute On
        offSprite = Sprite.Create (volumeOff, new Rect (0, 0, volumeOff.width, volumeOff.height), new Vector2 (0.50f, 0.5f), 100, 1, SpriteMeshType.FullRect);
        volumeSlider.value = DataEntry.instance.volume;
        chal9 = ChallengesController.ChallengeDone (9);
        // SFX Mute Off
        BGMOnSprite = Sprite.Create (musicOn, new Rect (0, 0, musicOn.width, musicOn.height), new Vector2 (0.50f, 0.5f), 100, 1, SpriteMeshType.FullRect);
        //SFX Mute On
        BGMOffSprite = Sprite.Create (musicOff, new Rect (0, 0, musicOff.width, musicOff.height), new Vector2 (0.50f, 0.5f), 100, 1, SpriteMeshType.FullRect);
        SetSFXVolumePic (DataEntry.instance.sfxMuted);
        SetBGMVolumePic (DataEntry.instance.bgmMuted);
    }
    // Update is called once per frame
    void Update () { }
    public void SetSFXVolumePic (bool muted) {
        if (muted == true) {
            //Muted
            volumeButton.GetComponent<Image> ().sprite = offSprite;
            volumeButton2.GetComponent<Image> ().sprite = offSprite;
        } else {
            //Unmuted
            volumeButton.GetComponent<Image> ().sprite = onSprite;
            volumeButton2.GetComponent<Image> ().sprite = onSprite;
        }
    }
    public void SetBGMVolumePic (bool muted) {
        if (muted == true) {
            //Muted
            //           //print ("Muted");
            bgm1.GetComponent<Image> ().sprite = BGMOffSprite;
            bgm2.GetComponent<Image> ().sprite = BGMOffSprite;
        } else {
            //Unmuted
            //print ("Unmuted");
            bgm1.GetComponent<Image> ().sprite = BGMOnSprite;
            bgm2.GetComponent<Image> ().sprite = BGMOnSprite;
        }
    }
    public void ToggleSFXMute () {
        ChangeVolume.instance.ToggleSFX ();
        SetSFXVolumePic (DataEntry.instance.sfxMuted);
    }
    public void ToggleBGMMute () {
        ChangeVolume.instance.ToggleBGM ();
        SetBGMVolumePic (DataEntry.instance.bgmMuted);
    }
    public void VolumeSliderChange () {
        ChangeVolume.instance.ChangeThisVolume (volumeSlider.value);
        if (chal9) {
            ChallengesController.CompleteChallenge (9);
            chal9 = false;
        }
    }
}