using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CarScoreController : MonoBehaviour {
    public int score;
    public int oldScore;
    public static CarScoreController instance;
    public Text scoreText;
    // Use this for initialization
    void Start () {
        instance = this;
    }
    // Update is called once per frame
    void Update () {
        if (Mathf.FloorToInt (CarVariables.instance.distance) != oldScore) {
            score = Mathf.FloorToInt (CarVariables.instance.distance);
            scoreText.text = score.ToString ();
            oldScore = score;
        }
    }
}