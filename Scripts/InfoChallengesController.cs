using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoChallengesController : MonoBehaviour {
	GameObject scrollBackground;
	float levelHeight;
	List<Panel> panels = new List<Panel> ();
	List<Panel> levelPanels = new List<Panel> ();
	float height;
	float pWidth;
	int lastLevel = 0;
	float extraHeight = 40f;
	// Use this for initialization
	void Awake () {
		SetChallengesPanel ();
	}
	// Update is called once per frame
	void Update () { }
	void SetChallengesPanel () {
		panels.Clear ();
		levelPanels.Clear ();
		scrollBackground = GameObject.FindWithTag ("PanelContainer");
		for (int i = 0; i < 32; i++) {
			try {
				if ((int) ChallengesController.GetChallenge (i) ["Level"] == lastLevel) {
					Panel panel = new Panel ();
					panel.challenge = i;
					panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/ChallengesPanel"));
					panel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().challengeIndex = i;
					panel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().displayIndex = i;
					height = panel.gameOb.GetComponent<RectTransform> ().rect.height;
					pWidth = panel.gameOb.GetComponent<RectTransform> ().rect.width;
					panel.gameOb.transform.SetParent (scrollBackground.transform, false);
					panel.chalDesc = panel.gameOb.transform.Find ("ChallengeDesc").GetComponent<Text> ();
					panel.chalDesc.text = ChallengesController.GetChallenge (i) ["Description"].ToString ().ToUpper ();
					panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count) - (levelHeight * levelPanels.Count), 0);
					panel.chalName = panel.gameOb.transform.Find ("ChallengeName").GetComponent<Text> ();
					panel.chalLevel = panel.gameOb.transform.Find ("ChallengeLevel").GetComponent<Text> ();
					panel.chalName.text = ChallengesController.GetChallenge (i) ["Name"].ToString ().ToUpper ();
					panel.chalLevel.text = "LEVEL " + ChallengesController.GetChallenge (i) ["Level"].ToString ().ToUpper ();
					panel.coinReward = panel.gameOb.transform.Find ("CoinReward").GetComponentInChildren<Text> ();
					panel.coinReward.text = ChallengesController.GetChallenge (i) ["coinReward"].ToString ();
					panel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").gameObject.SetActive (false);
					panel.gameOb.transform.Find ("Checkbox").GetComponent<Button> ().interactable = false;
					if (DataEntry.instance.challengesCompleted.Contains (i) == true) {
						panel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").gameObject.SetActive (true);
					}
					panels.Add (panel);
				} else {
					lastLevel = (int) ChallengesController.GetChallenge (i) ["Level"];
					Panel panel = new Panel ();
					panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/LevelPanel"));
					levelHeight = panel.gameOb.GetComponent<RectTransform> ().rect.height;
					panel.gameOb.transform.SetParent (scrollBackground.transform, false);
					panel.chalLevel = panel.gameOb.transform.Find ("Text").GetComponent<Text> ();
					panel.chalLevel.text = ("LEVEL " + ChallengesController.GetChallenge (i) ["Level"].ToString ());
					panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(levelHeight / 2) - (height * panels.Count) - (levelHeight * levelPanels.Count), 0);
					levelPanels.Add (panel);
					Panel npanel = new Panel ();
					npanel.challenge = i;
					npanel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/ChallengesPanel"));
					npanel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().challengeIndex = i;
					npanel.gameOb.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().displayIndex = i;
					height = npanel.gameOb.GetComponent<RectTransform> ().rect.height;
					pWidth = panel.gameOb.GetComponent<RectTransform> ().rect.width;
					npanel.gameOb.transform.SetParent (scrollBackground.transform, false);
					npanel.chalDesc = npanel.gameOb.transform.Find ("ChallengeDesc").GetComponent<Text> ();
					npanel.chalDesc.text = ChallengesController.GetChallenge (i) ["Description"].ToString ().ToUpper ();
					npanel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count) - (levelHeight * levelPanels.Count), 0);
					npanel.chalName = npanel.gameOb.transform.Find ("ChallengeName").GetComponent<Text> ();
					npanel.chalLevel = npanel.gameOb.transform.Find ("ChallengeLevel").GetComponent<Text> ();
					npanel.chalName.text = ChallengesController.GetChallenge (i) ["Name"].ToString ().ToUpper ();
					npanel.chalLevel.text = "LEVEL " + ChallengesController.GetChallenge (i) ["Level"].ToString ();
					npanel.coinReward = npanel.gameOb.transform.Find ("CoinReward").GetComponentInChildren<Text> ();
					npanel.coinReward.text = ChallengesController.GetChallenge (i) ["coinReward"].ToString ();
					npanel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").gameObject.SetActive (false);
					npanel.gameOb.transform.Find ("Checkbox").GetComponent<Button> ().interactable = false;
					if (DataEntry.instance.challengesCompleted.Contains (i) == true) {
						npanel.gameOb.transform.Find ("Checkbox").Find ("Checkmark").gameObject.SetActive (true);
					}
					panels.Add (npanel);
				}
			} catch { }
		}
		scrollBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (pWidth, (height * panels.Count) + (levelHeight * levelPanels.Count) + extraHeight);
		scrollBackground.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -((height * panels.Count) + (levelHeight * levelPanels.Count) + extraHeight) / 2f, 0);
	}
}