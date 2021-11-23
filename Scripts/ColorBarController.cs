using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#pragma warning disable 0414
public class ColorBarController : MonoBehaviour {
    Image image;
    bool firstTime = true;
    // public List<Color> colors;
    float orbCount;
    float listLength;
    float percentage;
    Color lastcolor;
    Color endcolor;
    Color color;
    int index;
    int distanceIndex = 1;
    float distanceForChange = 50f;
    float zero = -700f;
    float currentPosition;
    float nextPosition = 0f;
    public float constant1 = 1.2f;
    public bool rainbowMode = false;
    float time;
    float timer;
    int numOfRainbows = 0;
    float hue = 0f;
    float startingHue = 0f;
    float saturation;
    float value;
    Color lastMeshColor;
    float distanceForRainbow = 125f;
    public int numberOfColors = 3;
    float seed;
    public Material trackTop;
    public Material trackSide;
    public Material background;
    float generatedSaturated = 0.5f;
    float generatedValue = 0.7f;
    bool chal2;
    bool gameOn;
    bool colorChanging = false;
    Color brightGold = new Color (1, 0.7649f, 0.3870f, 1);
    Color dullGold = new Color (0.95f, 0.729f, 0.36f, 1f);
    Color brightPink = new Color (0.9122f, 0.3550f, 0.4979f, 1);
    Color dullPink = new Color (0.8773f, 0.3931f, 0.5272f, 1f);
    Color brightBlue = new Color (0.4003f, 0.7295f, 0.92f, 1f);
    Color dullBlue = new Color (0.3921f, 0.6801f, 0.8784f, 1f);
    Color brightGreen = new Color (0.3670f, 0.9611f, 0.3875f, 1f);
    Color dullGreen = new Color (0.3921f, 0.8784f, 0.4078f, 1f);
    public int colorMode = 0;
    //0 is none, 1 is gold, two is red, 3 is blue
    List<float> rainbowLengths = new List<float> {
        5f,
        6f,
        7f,
        8f,
        9f,
        10f,
        11f
    };
    public static ColorBarController instance;
    //Counter for how far traveled until next rainbow mode
    private float internalDistance;
    //Counter for how far traveled until next track color change
    private float internalDistanceForColor;
    int firstCheckpoint = 2000;
    int secondCheckpoint = 2500;
    int thirdCheckpoint = 3000;
    void Start () {
        chal2 = ChallengesController.ChallengeDone (2);
        // rainbowLengths.Add(5f, 10f, 15f, 20f, 25f, 30f);
        distanceIndex = 1;
        numberOfColors = GameColorController.instance.trackTopColors.Count;
        instance = this;
        currentPosition = zero;
        //  trackTop = GameObject.FindWithTag ("Track").GetComponent<MeshRenderer> ().materials[0];
        //trackSide = GameObject.FindWithTag ("Track").GetComponent<MeshRenderer> ().materials[1];
        //background = GameObject.FindWithTag ("Background").GetComponent<SpriteRenderer> ().material;
        image = GetComponent<Image> ();
        seed = Random.Range (0f, 1f);
        /* for (int i = 0; i < numberOfColors; i++) {
             value = seed + (i / ((float) numberOfColors));
             if (value >= 1f) {
                 value -= 1f;
             }
             color = Color.HSVToRGB (value, generatedSaturated, generatedValue);
             colors.Add (color);
         }*/
        listLength = (float) numberOfColors;
        transform.localPosition = new Vector3 (zero, 0, 0);
        trackSide.color = GameColorController.instance.trackSideColors[0];
        trackTop.color = GameColorController.instance.trackTopColors[0];
        background.color = GameColorController.instance.backgroundColors[0];
        //image.color = GameColorController.instance.backgroundColors[0];
        colorMode = 0;
        image.color = GetNormalColor (colorMode);
        // index++;
    }
    /*IEnumerator UpdateColorList () {
        seed = Random.Range (0f, 1f);
        colors.Clear ();
        for (int i = 0; i < numberOfColors; i++) {
            value = seed + (i / ((float) numberOfColors * 3));
            if (value >= 1f) {
                value -= 1f;
            }
            color = Color.HSVToRGB (value, generatedSaturated, generatedValue);
            colors.Add (color);
            yield return null;
        }
    }*/
    void Update () {
        if (CarVariables.instance.gameOn == false) {
            //Game over
            if (firstTime == true) {
                if (rainbowMode) {
                    PlayAudio.StopRainbowMode ();
                }
                StartCoroutine (FadeImage ());
                firstTime = false;
            }
        } else {
            //Game is playing
            if (Time.frameCount % 10 == 4) {
                if (CarVariables.instance.distance < firstCheckpoint) {
                    colorMode = 0;
                } else if ((CarVariables.instance.distance > firstCheckpoint) && (CarVariables.instance.distance <= secondCheckpoint)) {
                    colorMode = 1;
                } else if ((CarVariables.instance.distance > secondCheckpoint) && (CarVariables.instance.distance <= thirdCheckpoint)) {
                    colorMode = 2;
                } else if ((CarVariables.instance.distance > thirdCheckpoint)) {
                    colorMode = 3;
                }
                if (chal2) {
                    if (numOfRainbows > 0) {
                        ChallengesController.CompleteChallenge (2);
                        chal2 = false;
                    }
                }
            }
            if (rainbowMode == false) {
                //Move the bar according to distance drifted
                internalDistanceForColor += CarVariables.instance.distanceTraveled;
                if (CarDriftController.instance.drifting) {
                    internalDistance += CarVariables.instance.distanceTraveled;
                    percentage = internalDistance / distanceForRainbow;
                    nextPosition = zero - (percentage * zero);
                    transform.localPosition = new Vector3 (nextPosition, 0, 0);
                }
                if (internalDistanceForColor > distanceForChange) {
                    internalDistanceForColor = 0;
                    StartCoroutine (ChangeColor ());
                }
                if (transform.localPosition.x > -2f) {
                    if (numOfRainbows <= rainbowLengths.Count - 1) {
                        time = rainbowLengths[numOfRainbows];
                    } else {
                        time = rainbowLengths[rainbowLengths.Count - 1];
                    }
                    numOfRainbows++;
                    rainbowMode = true;
                    PlayAudio.PlayRainbowMode ();
                    //StartCoroutine (UpdateColorList ());
                    StartCoroutine (RainbowMove ());
                    StartCoroutine (RainbowColors ());
                }
            } else {
                internalDistance = 0;
            }
        }
    }
    IEnumerator FadeImage () {
        yield return new WaitForSeconds (0.2f);
        for (float i = image.color.a; i >= 0; i -= Time.deltaTime / 2f) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }
        image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
    }
    IEnumerator ChangeColor () {
        if (colorChanging == false) {
            colorChanging = true;
            //lastcolor = image.color;
            lastMeshColor = trackTop.color;
            Color lastSideColor = trackSide.color;
            Color lastBackgroundColor = background.color;
            index++;
            index = index % numberOfColors;
            endcolor = GameColorController.instance.trackTopColors[index];
            for (float i = 0; i <= 1; i += Time.deltaTime * constant1) {
                trackTop.color = Color.Lerp (lastMeshColor, GameColorController.instance.trackTopColors[index], i);
                trackSide.color = Color.Lerp (lastSideColor, GameColorController.instance.trackSideColors[index], i);
                background.color = Color.Lerp (lastBackgroundColor, GameColorController.instance.backgroundColors[index], i);
                //image.color = new Color (Mathf.Lerp (lastcolor.r, endcolor.r, i), Mathf.Lerp (lastcolor.g, endcolor.g, i), Mathf.Lerp (lastcolor.b, endcolor.b, i), image.color.a);
                yield return null;
            }
            trackTop.color = GameColorController.instance.trackTopColors[index];
            trackSide.color = GameColorController.instance.trackSideColors[index];
            background.color = GameColorController.instance.backgroundColors[index];
            //image.color = endcolor;
            colorChanging = false;
        }
    }
    IEnumerator RainbowMove () {
        for (float i = 0; i <= 1; i += Time.deltaTime / time) {
            transform.localPosition = new Vector3 (Mathf.Lerp (0, -600, i), 0, 0);
            yield return null;
        }
        transform.localPosition = new Vector3 (-600, 0, 0);
        rainbowMode = false;
        if (CarVariables.instance.gameOn == true) {
            internalDistanceForColor = 0;
            StartCoroutine (ChangeColor ());
        }
    }
    Color GetRainbowColor (int mode) {
        switch (mode) {
            case 0:
                return brightGold;
            case 1:
                return brightPink;
            case 2:
                return brightBlue;
            case 3:
                return brightGreen;
            default:
                return brightGold;
        }
    }
    Color GetNormalColor (int mode) {
        switch (mode) {
            case 0:
                return dullGold;
            case 1:
                return dullPink;
            case 2:
                return dullBlue;
            case 3:
                return dullGreen;
            default:
                return dullGold;
        }
    }
    IEnumerator RainbowColors () {
        timer = 0;
        lastcolor = image.color;
        image.color = GetRainbowColor (colorMode);
        while (timer < time) {
            /* lastcolor = image.color;
            Color.RGBToHSV (image.color, out startingHue, out saturation, out value);
            for (float i = 0; i <= 1; i += Time.deltaTime / 5f) {
                hue = startingHue + i;
                if (hue > 1) {
                    hue = hue - 1;
                }
                endcolor = Color.HSVToRGB (hue, 0.3f, 0.8f);
                endcolor.a = image.color.a;
                image.color = endcolor;
                yield return null;
                timer += Time.deltaTime;
                if (timer >= time) {
                    break;
                }
            }*/
            timer += Time.deltaTime;
            yield return null;
            if (timer >= time) {
                break;
            }
        }
        image.color = GetNormalColor (colorMode);
        PlayAudio.StopRainbowMode ();
    }
}