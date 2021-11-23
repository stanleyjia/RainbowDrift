using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSceneOrbController : MonoBehaviour {
    public GameObject orb;
    Text text;
    bool oneTime = true;
    int currentValue;
    public static GameSceneOrbController instance;
    public float speed;
    int nextValue;
    bool updating = false;
    void Start () {
        text = GetComponent<Text> ();
        orb.SetActive (false);
        text.color = new Color (text.color.r, text.color.g, text.color.b, 0);
        instance = this;
        InvokeRepeating ("CheckValue", 0, 1f);
    }
    void Update () {
        if ((CarVariables.instance.gameOn == false) && (oneTime == true)) {
            instance.text.text = DataEntry.instance.orbCount.ToString ();
            currentValue = DataEntry.instance.orbCount;
            oneTime = false;
        }
    }
    public static void UpdateValue () {
        instance.nextValue = instance.currentValue + OrbGeneratorController.instance.orbCount;;
        instance.speed = ((FlyingOrbController.instance.num + 1) * FlyingOrbController.instance.inBetweenTime);
        MySingleton.Instance.StartCoroutine (instance.NumberLerp (instance.currentValue, instance.nextValue));
        instance.updating = false;
    }
    IEnumerator NumberLerp (int firstNum, int secondNum) {
        for (float i = 0; i < 1; i += Time.deltaTime / speed) {
            instance.text.text = (Mathf.FloorToInt (Mathf.Lerp (firstNum, secondNum, i)) + 1).ToString ();
            yield return null;
        }
        instance.text.text = secondNum.ToString ();
        currentValue = secondNum;
        yield return new WaitForSeconds (1.0f);
        //instance.updating = true;
    }
    public void GameOver () {
        instance.text.text = DataEntry.instance.orbCount.ToString ();
        currentValue = DataEntry.instance.orbCount;
        StartCoroutine (FadeIn ());
    }
    IEnumerator FadeIn (float alpha = 1) {
        //  yield return new WaitForSeconds(1f);
        orb.SetActive (true);
        for (float i = 0; i <= alpha; i += Time.deltaTime / 1.5f) {
            // set color with i as alpha
            instance.text.color = new Color (instance.text.color.r, instance.text.color.g, instance.text.color.b, i);
            yield return null;
        }
        instance.text.color = new Color (instance.text.color.r, instance.text.color.g, instance.text.color.b, 1);
    }
    void CheckValue () {
        if (updating == true) {
            if (DataEntry.instance.orbCount.ToString () != text.text) {
                gameObject.GetComponent<Text> ().text = DataEntry.instance.orbCount.ToString ();
            }
        }
    }
}