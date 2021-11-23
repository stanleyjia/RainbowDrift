using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardModeController : MonoBehaviour {
	public static LeaderboardModeController instance;
	public int mode = 0;
	//0 is daily, 1 is all-time
	Color unfocused = new Color (0.736f, 0.8404f, 0.853f, 1f);
	Color focus = new Color (0.923f, 0.6512f, 0.2584f, 1f);
	public Text dailyText;
	public Text allTimeText;
	// Use this for initialization
	void Start () {
		instance = this;
		mode = 0;
		dailyText.color = focus;
		allTimeText.color = unfocused;
	}
	// Update is called once per frame
	void Update () { }
	public void SetAllTime () {
		if (mode == 0) {
			mode = 1;
			dailyText.color = unfocused;
			allTimeText.color = focus;
			LeaderboardController.instance.UpdateLeaderboard (false);
			LeaderboardController.instance.ResetLeaderboardPos (false);
		}
	}
	public void SetDaily () {
		if (mode == 1) {
			mode = 0;
			dailyText.color = focus;
			allTimeText.color = unfocused;
			LeaderboardController.instance.UpdateLeaderboard (true);
			LeaderboardController.instance.ResetLeaderboardPos (true);
		}
	}
}