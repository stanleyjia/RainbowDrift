using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayfabStatisticsController : MonoBehaviour {
	// Use this for initialization
	public static PlayfabStatisticsController instance;
	public Dictionary<string, int> statistics = new Dictionary<string, int> ();
	bool firstTime = true;
	public int dailyHighScore = 0;
	void Start () {
		DontDestroyOnLoad (this);
		instance = this;
	}
	// Update is called once per frame
	void Update () {
		if (firstTime == true) {
			if (PlayFabLogin.instance.loggedIn) {
				firstTime = false;
			}
		}
	}
	public void SetStatistics () {
		PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
			FunctionName = "SetStatistics",
				FunctionParameter = statistics
		}, (ExecuteCloudScriptResult res) => {
			if (Debug.isDebugBuild) {
				for (int k = 0; k < res.Logs.Count; k++) {
					print (res.Logs[k].Message);
				}
			}
		}, CallFailure);
	}
	public void UpdateStats () {
		statistics = DataEntry.instance.userStatistics;
		try {
			statistics["HighScoresToday"] = DataEntry.instance.userStatistics["HighScoresToday"];
		} catch {
			statistics.Add ("HighScoresToday", 0);
		}
		try {
			statistics["ActualHighScoresToday"] = DataEntry.instance.userStatistics["ActualHighScoresToday"];
		} catch {
			statistics.Add ("ActualHighScoresToday", 0);
		}
		statistics["PlayersReferred"] = UserData.instance.referrals.Count;
	}
	void CallFailure (PlayFabError error) {
		if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
			//no connection
			ConnectionController.instance.noConnectionDetected ();
		} else {
			if (Debug.isDebugBuild) {
				print (error);
			}
		}
	}
}