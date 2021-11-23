using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardController : MonoBehaviour {
	float height;
	float width;
	public List<Panel> panels = new List<Panel> ();
	public List<Dictionary<string, object>> Leaderboards;
	public List<Dictionary<string, object>> DailyLeaderboards;
	GameObject panelContainer;
	public RectTransform leaderboardsWindow;
	public bool leaderboardMode = true;
	public GameObject actualLeaderboards;
	public GameObject jackpot;
	public Animator jackpotHeader;
	public Animator leaderboardHeader;
	public Animator leaderboardPanel;
	public Animator leaderboardRank;
	public Animator jackpotPanel;
	public Animator leaderboardScroll;
	public Animator aroundLeaderboardScroll;
	public Animator leaderboardModeAnimator;
	Texture2D carTexture;
	Texture2D trailTexture;
	float extraHeight = 40f;
	public static LeaderboardController instance;
	public Text rankText;
	Color youColor = new Color (0, 0, 0, 0.3f);
	Color clearColor = new Color (0, 0, 0, 0.0f);
	Color backgroundColor = new Color (0, 0, 0, 0.18f);
	bool dailyComplete;
	bool allTimeComplete;
	int successIndex = 0;
	// Use this for initialization
	//	void Start () { }
	void Start () {
		instance = this;
		rankText.text = "RANK #1000";
		//print (UserData.instance.DailyLeaderboards.Count);
		//jackpot.SetActive (false);
		SetDailyLeaderboardPanel ();
		//SetAroundLeaderboardPanel ();
		leaderboardHeader.Play ("On");
		leaderboardModeAnimator.Play ("On");
		leaderboardRank.Play ("On");
		leaderboardPanel.Play ("On");
		//	aroundLeaderboardScroll.Play ("Off");
		leaderboardScroll.Play ("On");
	}
	// Update is called once //per frame
	Texture2D GetCarTexture (string id) {
		try {
			if (Resources.Load<Texture2D> ("CarRender/" + id)) {
				return (Resources.Load<Texture2D> ("CarRender/" + id));
			} else {
				return (Resources.Load<Texture2D> ("CarRender/C0001"));
			}
		} catch {
			return (Resources.Load<Texture2D> ("CarRender/C0001"));
		}
	}
	Texture2D GetTrailTexture (string id) {
		try {
			if (Resources.Load<Texture2D> ("TrailRender/" + id)) {
				return (Resources.Load<Texture2D> ("TrailRender/" + id));
			} else {
				return (Resources.Load<Texture2D> ("TrailRender/T0001"));
			}
		} catch {
			return (Resources.Load<Texture2D> ("TrailRender/T0001"));
		}
	}
	public void UpdateLeaderboard (bool daily) {
		bool success = true;
		successIndex = 0;
		if (daily) {
			DailyLeaderboards = UserData.instance.DailyLeaderboards;
			rankText.text = "RANK #" + (UserData.instance.dailyLeaderboardPosition + 1);
			for (int i = 0; i < panels.Count; i++) {
				if (success == true) {
					try {
						DailyLeaderboards[i] = DailyLeaderboards[i];
					} catch {
						success = false;
						successIndex = i;
					}
				}
				panels[i].gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * i), 0);
				if (success == true) {
					try {
						//Set Text
						panels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = DailyLeaderboards[i]["DisplayName"].ToString ();
						if (DailyLeaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
							panels[i].gameOb.GetComponent<Image> ().color = youColor;
						} else {
							panels[i].gameOb.GetComponent<Image> ().color = backgroundColor;
						}
						//DailyLeaderboards
					} catch {
						//print ("Fail1");
						panels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "Player";
					}
					panels[i].gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (DailyLeaderboards[i]["Position"]).ToString ("00");
					panels[i].gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = DailyLeaderboards[i]["StatValue"].ToString ();
					foreach (Transform child in panels[i].gameOb.transform.Find ("Render").Find ("CarRender")) {
						if (child.name != DailyLeaderboards[i]["Car"].ToString ()) {
							child.gameObject.SetActive (false);
						} else {
							child.gameObject.SetActive (true);
						}
					}
					foreach (Transform child in panels[i].gameOb.transform.Find ("Render").Find ("TrailRender")) {
						if (child.name != DailyLeaderboards[i]["Trail"].ToString ()) {
							child.gameObject.SetActive (false);
						} else {
							child.gameObject.SetActive (true);
						}
					}
				} else {
					panels[i].gameOb.GetComponent<Image> ().color = clearColor;
				}
			}
			if (success) {
				dailyComplete = true;
			} else {
				dailyComplete = false;
			}
		} else {
			rankText.text = "RANK #" + (UserData.instance.leaderboardPosition + 1);
			Leaderboards = UserData.instance.Leaderboards;
			for (int i = 0; i < panels.Count; i++) {
				if (success == true) {
					try {
						Leaderboards[i] = Leaderboards[i];
					} catch {
						success = false;
						successIndex = i;
					}
				}
				panels[i].gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * i), 0);
				if (success == true) {
					try {
						//Set Text
						panels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = Leaderboards[i]["DisplayName"].ToString ();
						if (Leaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
							panels[i].gameOb.GetComponent<Image> ().color = youColor;
						} else {
							panels[i].gameOb.GetComponent<Image> ().color = backgroundColor;
						}
						//Leaderboards
					} catch {
						//print ("Fail2");
						panels[i].gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "";
					}
					panels[i].gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (Leaderboards[i]["Position"]).ToString ("00");
					panels[i].gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = Leaderboards[i]["StatValue"].ToString ();
					foreach (Transform child in panels[i].gameOb.transform.Find ("Render").Find ("CarRender")) {
						if (child.name != Leaderboards[i]["Car"].ToString ()) {
							child.gameObject.SetActive (false);
						} else {
							child.gameObject.SetActive (true);
						}
					}
					foreach (Transform child in panels[i].gameOb.transform.Find ("Render").Find ("TrailRender")) {
						if (child.name != Leaderboards[i]["Trail"].ToString ()) {
							child.gameObject.SetActive (false);
						} else {
							child.gameObject.SetActive (true);
						}
					}
				} else {
					panels[i].gameOb.GetComponent<Image> ().color = clearColor;
				}
			}
			if (success) {
				allTimeComplete = true;
			} else {
				allTimeComplete = false;
			}
		}
		if (success) {
			if ((height * (panels.Count)) < leaderboardsWindow.rect.height) {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, leaderboardsWindow.rect.height);
			} else {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, (height * (panels.Count)) + extraHeight);
			}
		} else {
			if ((height * (successIndex)) < leaderboardsWindow.rect.height) {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, leaderboardsWindow.rect.height);
			} else {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (panelContainer.GetComponent<RectTransform> ().sizeDelta.x, (height * (successIndex)) + extraHeight);
			}
		}
		//panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (panels.Count)) + extraHeight) / 2f, 0);
	}
	void SetDailyLeaderboardPanel () {
		bool success = true;
		rankText.text = "RANK #" + (UserData.instance.dailyLeaderboardPosition + 1);
		panels.Clear ();
		panelContainer = GameObject.FindWithTag ("leaderboardsPanelContainer");
		DailyLeaderboards = UserData.instance.DailyLeaderboards;
		for (int i = 0; i < 25; i++) {
			//print (i);
			Panel panel = new Panel ();
			panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/LeaderboardPanel"));
			panel.gameOb.transform.SetParent (panelContainer.transform, false);
			//panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count), 0);
			height = panel.gameOb.GetComponent<RectTransform> ().rect.height;
			width = panel.gameOb.GetComponent<RectTransform> ().rect.width;
			panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count), 0);
			if (success == true) {
				try {
					DailyLeaderboards[i] = DailyLeaderboards[i];
					success = true;
				} catch {
					success = false;
					successIndex = i;
				}
			}
			if (success == true) {
				try {
					//Set Text
					panel.gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = DailyLeaderboards[i]["DisplayName"].ToString ();
					if (DailyLeaderboards[i]["DisplayName"].ToString () == UserData.instance.displayName) {
						panel.gameOb.GetComponent<Image> ().color = youColor;
					} else {
						panel.gameOb.GetComponent<Image> ().color = backgroundColor;
					}
					//DailyLeaderboards
				} catch {
					panel.gameOb.transform.Find ("Player").Find ("Username").GetComponent<Text> ().text = "Player";
				}
				panel.gameOb.transform.Find ("Place").GetComponent<Text> ().text = System.Convert.ToInt32 (DailyLeaderboards[i]["Position"]).ToString ("00");
				panel.gameOb.transform.Find ("Player").Find ("Score").GetComponent<Text> ().text = DailyLeaderboards[i]["StatValue"].ToString ();
				//Set Image
				foreach (Transform child in panel.gameOb.transform.Find ("Render").Find ("CarRender")) {
					//print (child.name);
					if (child.name != DailyLeaderboards[i]["Car"].ToString ()) {
						child.gameObject.SetActive (false);
					} else {
						child.gameObject.SetActive (true);
					}
				}
				foreach (Transform child in panel.gameOb.transform.Find ("Render").Find ("TrailRender")) {
					if (child.name != DailyLeaderboards[i]["Trail"].ToString ()) {
						child.gameObject.SetActive (false);
					} else {
						child.gameObject.SetActive (true);
					}
				}
			} else {
				panel.gameOb.GetComponent<Image> ().color = clearColor;
			}
			panels.Add (panel);
		}
		if (success) {
			dailyComplete = true;
			if ((height * (panels.Count)) < leaderboardsWindow.rect.height) {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, leaderboardsWindow.rect.height);
			} else {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, (height * (panels.Count)) + extraHeight);
			}
			panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (panels.Count)) + extraHeight) / 2f, 0);
		} else {
			dailyComplete = false;
			if ((height * (successIndex)) < leaderboardsWindow.rect.height) {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, leaderboardsWindow.rect.height);
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (panels.Count)) + extraHeight) / 2f, 0);
			} else {
				panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, (height * (successIndex)) + extraHeight);
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (successIndex)) + extraHeight) / 2f, 0);
			}
		}
	}
	public void ToggleMode (bool mode) {
		if (mode != leaderboardMode) {
			leaderboardMode = mode;
			if (mode == true) {
				//Leaderboard mode
				StartCoroutine (ShowLeaderboardHeader ());
			} else {
				//Jackpot mode
				StartCoroutine (ShowJackpotHeader ());
			}
		}
	}
	public void ResetLeaderboardPos (bool daily) {
		if (daily) {
			if (dailyComplete) {
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (panels.Count)) + extraHeight) / 2f, 0);
			} else {
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (successIndex)) + extraHeight) / 2f, 0);
			}
		} else {
			if (allTimeComplete) {
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (panels.Count)) + extraHeight) / 2f, 0);
			} else {
				panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * (successIndex)) + extraHeight) / 2f, 0);
			}
		}
	}
	IEnumerator ShowJackpotHeader () {
		leaderboardHeader.SetTrigger ("Hide");
		leaderboardPanel.SetTrigger ("Hide");
		leaderboardRank.SetTrigger ("Hide");
		leaderboardModeAnimator.SetTrigger ("Hide");
		//yield return new WaitForSeconds (0.2f);
		yield return null;
		jackpotHeader.SetTrigger ("Show");
		jackpotPanel.SetTrigger ("Show");
	}
	IEnumerator ShowLeaderboardHeader () {
		jackpotHeader.SetTrigger ("Hide");
		jackpotPanel.SetTrigger ("Hide");
		yield return null;
		leaderboardRank.SetTrigger ("Show");
		leaderboardHeader.SetTrigger ("Show");
		leaderboardPanel.SetTrigger ("Show");
		leaderboardModeAnimator.SetTrigger ("Show");
	}
}