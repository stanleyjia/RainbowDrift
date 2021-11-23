using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatisticsController : MonoBehaviour {
	float height;
	float headerHeight;
	float extraHeight = 40f;
	List<Panel> panels = new List<Panel> ();
	public Dictionary<string, int> Statistics;
	GameObject panelContainer;
	GameObject scrollBackground;
	List<string> statsNotShown = new List<string> {
		//	"HighScoresToday"
	};
	Dictionary<string, string> statsLabelText = new Dictionary<string, string> {
		{
			"DistanceTraveled",
			"DISTANCE TRAVELED"
		},
		{
			"DistanceDrifted",
			"DISTANCE DRIFTED"
		},
		{
			"CarsOwned",
			"CARS OWNED"
		},
		{
			"PlayersReferred",
			"PLAYERS REFERRED"
		},
		{
			"TrailsOwned",
			"TRAILS OWNED"
		},
		{
			"CoinsCollected",
			"COINS COLLECTED"
		},
		{
			"OrbsCollected",
			"ORBS COLLECTED"
		},
		{
			"ChallengesCompleted",
			"CHALLENGES COMPLETED"
		},
		{
			"GamesPlayed",
			"GAMES PLAYED"
		},
		{
			"HighScoresToday",
			"HIGH SCORE"
		}
	};
	Dictionary<string, string> statsUnits = new Dictionary<string, string> {
		{
			"DistanceTraveled",
			"m"
		},
		{
			"DistanceDrifted",
			"m"
		},
		{
			"CarsOwned",
			""
		},
		{
			"PlayersReferred",
			""
		},
		{
			"TrailsOwned",
			""
		},
		{
			"CoinsCollected",
			""
		},
		{
			"OrbsCollected",
			""
		},
		{
			"ChallengesCompleted",
			""
		},
		{
			"GamesPlayed",
			""
		},
		{
			"HighScoresToday",
			""
		}
	};
	Dictionary<string, string> statsUnitsSingular = new Dictionary<string, string> {
		{
			"DistanceTraveled",
			"m"
		},
		{
			"DistanceDrifted",
			"m"
		},
		{
			"CarsOwned",
			""
		},
		{
			"PlayersReferred",
			""
		},
		{
			"TrailsOwned",
			""
		},
		{
			"CoinsCollected",
			""
		},
		{
			"OrbsCollected",
			""
		},
		{
			"ChallengesCompleted",
			""
		},
		{
			"GamesPlayed",
			""
		},
		{
			"HighScoresToday",
			""
		}
	};
	Dictionary<string, int> statsIndex = new Dictionary<string, int> {
		{
			"HighScoresToday",
			0
		},
		{
			"DistanceTraveled",
			1
		},
		{
			"DistanceDrifted",
			2
		},
		{
			"CarsOwned",
			5
		},
		{
			"PlayersReferred",
			3
		},
		{
			"TrailsOwned",
			6
		},
		{
			"CoinsCollected",
			7
		},
		{
			"OrbsCollected",
			8
		},
		{
			"ChallengesCompleted",
			9
		},
		{
			"GamesPlayed",
			4
		}
	};
	// Use this for initialization
	void Awake () {
		SetStatisticsPanel ();
	}
	// Update is called once per frame
	void Update () { }
	void SetStatisticsPanel () {
		panels.Clear ();
		panelContainer = GameObject.FindWithTag ("StatsPanel");
		scrollBackground = GameObject.FindWithTag ("StatsScroll");
		Statistics = DataEntry.instance.userStatistics;
		Panel npanel = new Panel ();
		npanel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/StatsHeaderPanel"));
		npanel.gameOb.transform.SetParent (panelContainer.transform, false);
		headerHeight = npanel.gameOb.GetComponent<RectTransform> ().rect.height;
		npanel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -headerHeight / 2, 0);
		//panels.Add (npanel);
		string[] keys = new string[statsIndex.Count];
		foreach (KeyValuePair<string, int> statistic in Statistics) {
			int ind;
			try {
				ind = statsIndex[statistic.Key];
			} catch {
				ind = 0;
			}
			//			print (statistic.Key);
			//print (statsIndex[statistic.Key]);
			try {
				keys[statsIndex[statistic.Key]] = statistic.Key;
			} catch {
				//print ("Failed");
				//keys.Insert(statistic.Key);
			}
		}
		//foreach (KeyValuePair<string, int> statistic in Statistics) {
		for (int i = 0; i < keys.Length; i++) {
			if (statsNotShown.Contains (keys[i]) == false) {
				Panel panel = new Panel ();
				panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/StatsPanel"));
				panel.gameOb.transform.SetParent (panelContainer.transform, false);
				//panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count), 0);
				height = panel.gameOb.GetComponent<RectTransform> ().rect.height;
				panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * (panels.Count)) - headerHeight, 0);
				try {
					panel.gameOb.transform.Find ("Name").GetComponent<Text> ().text = statsLabelText[keys[i]];
				} catch {
					panel.gameOb.transform.Find ("Name").GetComponent<Text> ().text = keys[i];
					print ("Stats not found");
				}
				//print (statsUnits[statistic.Key]);
				try {
					if (Statistics[keys[i]] == 1) {
						panel.gameOb.transform.Find ("Value").GetComponent<Text> ().text = Statistics[keys[i]].ToString () + " " + statsUnitsSingular[keys[i]];
					} else {
						panel.gameOb.transform.Find ("Value").GetComponent<Text> ().text = Statistics[keys[i]].ToString () + " " + statsUnits[keys[i]];
					}
				} catch {
					try {
						panel.gameOb.transform.Find ("Value").GetComponent<Text> ().text = Statistics[keys[i]].ToString ();
					} catch {
						panel.gameOb.transform.Find ("Value").GetComponent<Text> ().text = "0";
					}
				}
				panels.Add (panel);
			}
		}
		if (((height * panels.Count) + headerHeight + extraHeight) < Screen.height + scrollBackground.GetComponent<RectTransform> ().offsetMax.y - scrollBackground.GetComponent<RectTransform> ().offsetMin.y) {
			panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, scrollBackground.GetComponent<RectTransform> ().rect.height + extraHeight);
		} else {
			panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, (height * panels.Count) + headerHeight + extraHeight);
		}
		panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * panels.Count) + headerHeight + extraHeight) / 2f, 0);
	}
}