using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlyingCoinsGenerator : MonoBehaviour {
    List<GameObject> coins = new List<GameObject> ();
    Vector3 startPosition;
    Vector3 endPosition;
    public GameObject UICoinCount;
    public GameObject container;
    public GameObject CenterCoinCount;
    Vector3 startScale;
    Vector3 endScale;
    int count = 15;
    public int num;
    bool type = false;
    public float speed = 0.5f;
    //false is challenges, true is player
    public float inBetweenTime = 0.06f;
    public static FlyingCoinsGenerator instance;
    int endAmount;
    bool firstTime = true;
    void Start () {
        instance = this;
        endPosition = UICoinCount.GetComponent<RectTransform> ().position;
        for (int i = 0; i < count; i++) {
            GameObject coin = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/FlyingCoin"));
            //coin.enabled = false;
            coin.SetActive (false);
            coin.transform.localScale = new Vector3 (1f, 1f, 1f);
            coin.transform.SetParent (container.transform);
            coins.Add (coin);
        }
    }
    // Update is called once per frame
    public void GenerateChallengeReward (int amount) {
        firstTime = true;
        //Coins from challenge
        //Type = false;
        type = false;
        num = 10;
        endAmount = amount;
        StartCoroutine (Generate ());
        startScale = ChallengePanel.scale;
        endScale = UICoinCount.GetComponent<RectTransform> ().localScale;
    }
    public void GenerateNormalReward () {
        firstTime = true;
        //Coins from playing
        //Type = true
        type = true;
        startPosition = CenterCoinCount.GetComponent<RectTransform> ().position;
        startScale = CenterCoinCount.GetComponent<RectTransform> ().localScale;
        endScale = UICoinCount.GetComponent<RectTransform> ().localScale;
        endAmount = CoinGeneratorController.instance.coinCount;
        if (CoinGeneratorController.instance.coinCount >= count) {
            num = count;
            StartCoroutine (Generate ());
        } else {
            num = CoinGeneratorController.instance.coinCount;
            StartCoroutine (Generate ());
        }
    }
    IEnumerator Generate () {
        if (type == false) {
            inBetweenTime = 0.06f;
        } else {
            inBetweenTime = 0.08f;
        }
        endPosition = UICoinCount.GetComponent<RectTransform> ().position;
        for (int i = 0; i < num; i++) {
            if (type == false) {
                startPosition = ChallengesController.instance.coinStartPosition;
            }
            PlayAudio.PlaySound ("collectCoin");
            coins[i].transform.position = startPosition;
            coins[i].SetActive (true);
            coins[i].transform.localScale = startScale;
            StartCoroutine (FlyTo (coins[i]));
            yield return new WaitForSeconds (inBetweenTime);
        }
        ChallengesController.instance.updateCoinPos = false;
        ChallengesController.instance.readyToMove = true;
    }
    IEnumerator FlyTo (GameObject coin) {
        for (float i = 0; i <= 1; i += Time.deltaTime / speed) {
            coin.transform.position = Vector3.Lerp (startPosition, endPosition, i);
            coin.transform.localScale = Vector3.Lerp (startScale, endScale, i);
            yield return null;
        }
        if (ChallengesController.instance.challengesFinished) {
            //print (type);
            if (type == false) {
                if (coins.IndexOf (coin) == num - 1) {
                    ////print ("in here");
                    //print (GameOverController.ready2);
                    //print (GameOverController.firstTime3);
                    if ((GameOverController.ready2 == false) && (GameOverController.firstTime3)) {
                        GameOverController.ready2 = true;
                        //print ("ready2 is true");
                        GameOverController.firstTime3 = false;
                        ChallengesController.instance.challengesFinished = false;
                    }
                }
            }
        }
        coin.gameObject.SetActive (false);
        if (firstTime == true) {
            GameSceneCoinController.UpdateValue (endAmount);
            firstTime = false;
        }
    }
}