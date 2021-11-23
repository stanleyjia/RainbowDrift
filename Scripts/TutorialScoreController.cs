using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialScoreController : MonoBehaviour {
	// Text text;
	public static TutorialScoreController instance;
	public Text inGameCoins;
	public Text inGameScore;
	void Start () {
		instance = this;
		inGameCoins.color = new Color (inGameCoins.color.r, inGameCoins.color.g, inGameCoins.color.b, 1f);
		inGameScore.color = new Color (inGameScore.color.r, inGameScore.color.g, inGameScore.color.b, 1);
	}
	void Update () {
		if (CarVariables.instance.gameOn) {
			inGameCoins.text = CoinGeneratorController.instance.coinCount.ToString ();
		}
	}
}