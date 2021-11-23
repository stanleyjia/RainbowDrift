using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayAudio : MonoBehaviour {
    public AudioSource clickInstance;
    public AudioSource menuSongInstance;
    public AudioSource collectCoinInstance;
    public AudioSource orbPickupInstance;
    public AudioSource buyInstance;
    public AudioSource errorInstance;
    public AudioSource carSelectInstance;
    public AudioSource startRevInstance;
    public AudioSource gameSongInstance;
    public AudioSource rainbowSongInstance;
    public AudioSource driftInstance;
    public AudioSource swooshInstance;
    public AudioSource countdownInstance;
    public AudioSource lastCountdownInstance;
    public AudioSource congratsInstance;
    public static AudioSource Congrats { get { return instance.congratsInstance; } }
    public static AudioSource Click { get { return instance.clickInstance; } }
    public static AudioSource Error { get { return instance.errorInstance; } }
    public static AudioSource CarSelect { get { return instance.carSelectInstance; } }
    public static AudioSource CollectCoin { get { return instance.collectCoinInstance; } }
    public static AudioSource OrbPickup { get { return instance.orbPickupInstance; } }
    public static AudioSource Buy { get { return instance.buyInstance; } }
    public static AudioSource StartRev { get { return instance.startRevInstance; } }
    public static AudioSource GameSong { get { return instance.gameSongInstance; } }
    public static AudioSource RainbowSong { get { return instance.rainbowSongInstance; } }
    public static AudioSource MenuSong { get { return instance.menuSongInstance; } }
    public static AudioSource Drift { get { return instance.driftInstance; } }
    public static AudioSource Swoosh { get { return instance.swooshInstance; } }
    public static AudioSource Countdown { get { return instance.countdownInstance; } }
    public static AudioSource LastCountdown { get { return instance.lastCountdownInstance; } }
    public static PlayAudio instance;
    float volume;
    static float gameSongVolume = 1f;
    static float menuSongVolume = 1f;
    static float clickVolume = 1f;
    static float startRevVolume = 1f;
    static float rainbowSongVolume = 0.5f;
    static float collectCoinVolume = 1f;
    static float orbPickupVolume = 1f;
    static float driftVolume = 0.8f;
    static float swooshVolume = 1f;
    static float countdownVolume = 1f;
    static float lastCountdownVolume = 1f;
    static float buyVolume = 1f;
    static float errorVolume = 1f;
    static float carSelectVolume = 1f;
    static float congratsVolume = 1f;
    public static bool pause = false;
    //public static bool sfxMuted;
    //public static bool bgmMuted;
    public static Dictionary<string, float> soundVolume = new Dictionary<string, float> { { "play", gameSongVolume },
        { "menu", menuSongVolume },
        { "rainbow", rainbowSongVolume },
        { "click", clickVolume },
        { "startRev", startRevVolume },
        { "collectCoin", collectCoinVolume },
        { "orbPickup", orbPickupVolume },
        { "drift", driftVolume },
        { "swoosh", swooshVolume },
        { "countdown", countdownVolume },
        { "lastCountdown", lastCountdownVolume },
        { "buy", buyVolume },
        { "error", errorVolume },
        { "carSelect", carSelectVolume },
        { "congrats", congratsVolume }
    };
    //public AudioSource tires;
    void Start () {
        DontDestroyOnLoad (this);
        instance = this;
        Countdown.ignoreListenerPause = true;
        LastCountdown.ignoreListenerPause = true;
        Click.ignoreListenerPause = true;
        SetVolumes ();
    }
    static AudioSource GetSound (string name) {
        //print (name);
        switch (name) {
            case "click":
                return (Click);
            case "collectCoin":
                return (CollectCoin);
            case "orbPickup":
                return (OrbPickup);
            case "buy":
                return (Buy);
            case "startRev":
                return (StartRev);
            case "play":
                return (GameSong);
            case "rainbow":
                return (RainbowSong);
            case "menu":
                return (MenuSong);
            case "error":
                return (Error);
            case "drift":
                return (Drift);
            case "swoosh":
                return (Swoosh);
            case "countdown":
                return (Countdown);
            case "lastCountdown":
                return (LastCountdown);
            case "carSelect":
                return (CarSelect);
            case "congrats":
                return (Congrats);
            default:
                return null;
        }
    }
    public static void PlaySound (string sound) {
        //GetSound (sound).volume = soundVolume[sound];
        //print ("Play Sound: " + sound);
        StopAndPlay (GetSound (sound));
    }
    public static void StopSound (string sound) {
        //print ("Stop Sound: " + sound);
        Stop (GetSound (sound));
    }
    public static void SetVolumes () {
        //print ("Volume Set");
        foreach (KeyValuePair<string, float> pair in soundVolume) {
            if (GetSound (pair.Key).gameObject.tag == "SFX") {
                if (!DataEntry.instance.sfxMuted) {
                    GetSound (pair.Key).volume = pair.Value;
                } else {
                    GetSound (pair.Key).volume = 0;
                }
            } else if (GetSound (pair.Key).gameObject.tag == "BGM") {
                if (!DataEntry.instance.bgmMuted) {
                    GetSound (pair.Key).volume = pair.Value;
                } else {
                    GetSound (pair.Key).volume = 0;
                }
            }
        }
    }
    public static void FadeInto (string sound1, string sound2) {
        //print ("Fade from " + sound1 + " to " + sound2);
        instance.StartCoroutine (instance.FadeVolume (GetSound (sound1), GetSound (sound1).volume, 0f));
        //GetSound (sound1).volume = 0;
        GetSound (sound2).volume = 0;
        GetSound (sound2).Play ();
        if (GetSound (sound2).gameObject.tag == "SFX") {
            if (!DataEntry.instance.sfxMuted) {
                instance.StartCoroutine (instance.FadeVolume (GetSound (sound2), 0f, soundVolume[sound2]));
            }
        } else if (GetSound (sound2).gameObject.tag == "BGM") {
            if (!DataEntry.instance.bgmMuted) {
                instance.StartCoroutine (instance.FadeVolume (GetSound (sound2), 0f, soundVolume[sound2]));
            }
        }
    }
    public static void PlayRainbowMode () {
        //print ("Rainbow Mode Play");
        if (!DataEntry.instance.bgmMuted) {
            instance.StartCoroutine (instance.FadeVolume (GameSong, GameSong.volume, 0.0f, true));
            RainbowSong.Play ();
            instance.StartCoroutine (instance.FadeVolume (RainbowSong, 0, rainbowSongVolume));
        }
    }
    public static void StopRainbowMode () {
        ////print ("Rainbow Mode Stop");
        if (!DataEntry.instance.bgmMuted) {
            instance.StartCoroutine (instance.FadeVolume (RainbowSong, RainbowSong.volume, 0f));
            GameSong.UnPause ();
            instance.StartCoroutine (instance.FadeVolume (GameSong, 0, gameSongVolume));
        }
    }
    private static void Stop (AudioSource source) {
        source.Stop ();
    }
    public static bool IsPlaying (string sound) {
        return (GetSound (sound).isPlaying);
    }
    private static void StopAndPlay (AudioSource source) {
        if (source.isPlaying == false) {
            source.Play ();
        } else {
            source.Stop ();
            source.Play ();
        }
    }
    public static void PlayDrift () {
        if (Drift.isPlaying == false) {
            PlaySound ("drift");
        }
    }
    public static void FadeOut (string name) {
        instance.StartCoroutine (instance.FadeVolume (GetSound (name), GetSound (name).volume, 0, false));
    }
    IEnumerator FadeVolume (AudioSource source, float vol, float endVol, bool pause = false) {
        yield return null;
        for (float i = 0; i <= 1; i += Time.unscaledDeltaTime) {
            source.volume = Mathf.Lerp (vol, endVol, i);
            yield return null;
        }
        source.volume = endVol;
        if (source.volume == 0) {
            if (pause) {
                source.Pause ();
            } else {
                source.Stop ();
            }
        }
    }
}