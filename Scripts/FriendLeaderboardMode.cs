using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FriendLeaderboardMode : MonoBehaviour {
	public static FriendLeaderboardMode instance;
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
		//print ("Set all time");
		if (mode == 0) {
			//print ("Set all time2");
			mode = 1;
			dailyText.color = unfocused;
			allTimeText.color = focus;
			FriendsController.instance.UpdateLeaderboard (false);
			FriendsController.instance.ResetLeaderboardPos (false);
		}
	}
	public void SetDaily () {
		//print ("Set daily");
		if (mode == 1) {
			//print ("Set daily2");
			mode = 0;
			dailyText.color = focus;
			allTimeText.color = unfocused;
			FriendsController.instance.UpdateLeaderboard (true);
			FriendsController.instance.ResetLeaderboardPos (true);
		}
	}
}