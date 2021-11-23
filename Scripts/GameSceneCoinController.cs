using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSceneCoinController : MonoBehaviour {
    public GameObject coin;
    Text text;
    bool oneTime = true;
    int currentValue;
    public static GameSceneCoinController instance;
    public float speed;
    int nextValue;
    public bool updating = false;
    void Start () {
        instance = this;
        text = GetComponent<Text> ();
        coin.SetActive (false);
        text.color = new Color (text.color.r, text.color.g, text.color.b, 0);
        instance = this;
        // InvokeRepeating ("CheckValue", 0, 1f);
    }
    public static void UpdateValue (int amount) {
        instance.text = instance.GetComponent<Text> ();
        instance.speed = ((FlyingCoinsGenerator.instance.num + 1) * FlyingCoinsGenerator.instance.inBetweenTime);
        instance.updating = false;
        MySingleton.Instance.StartCoroutine (instance.NumberLerp (instance.currentValue, instance.currentValue + amount));
    }
    public void GameOver () {
        currentValue = DataEntry.instance.totalCoins;
        instance.text.text = currentValue.ToString ();
        StartCoroutine (FadeIn ());
    }
    void Update () {
        if ((CarVariables.instance.gameOn == false) && (oneTime == true)) {
            currentValue = DataEntry.instance.totalCoins;
            instance.text.text = currentValue.ToString ();
            StartCoroutine (FadeIn ());
            oneTime = false;
        }
    }
    IEnumerator NumberLerp (int firstNum, int secondNum) {
        // //print(secondNum);
        for (float i = 0; i < 1; i += Time.deltaTime / speed) {
            if (instance.text) {
                instance.text.text = (Mathf.FloorToInt (Mathf.Lerp (firstNum, secondNum, i)) + 1).ToString ();
                //print(Mathf.FloorToInt(Mathf.Lerp(firstNum, secondNum, i)) + 1);
                yield return null;
            } else {
                break;
            }
        }
        if (instance.text) {
            instance.text.text = secondNum.ToString ();
        }
        currentValue = secondNum;
        yield return null;
        updating = true;
        // //print("updating turned back on");
    }
    IEnumerator FadeIn (float alpha = 1) {
        coin.SetActive (true);
        for (float i = 0; i <= alpha; i += Time.deltaTime / 1.5f) {
            // set color with i as alpha
            instance.text.color = new Color (instance.text.color.r, instance.text.color.g, instance.text.color.b, i);
            yield return null;
        }
        instance.text.color = new Color (instance.text.color.r, instance.text.color.g, instance.text.color.b, 1);
    }
}