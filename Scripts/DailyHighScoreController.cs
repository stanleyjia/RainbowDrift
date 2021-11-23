using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
public class DailyHighScoreController : MonoBehaviour {
	// Use this for initialization
	public static DailyHighScoreController instance;
	List<string> tagsToDelete = new List<string> ();
	List<string> tagsToAdd = new List<string> ();
	void Start () {
		instance = this;
	}
	// Update is called once per frame
	void Update () { }
	public void UpdateCarTrailTags (string car, string trail) {
		//print (car);
		//print ("Updating trail: " + trail);
		tagsToDelete.Clear ();
		tagsToAdd.Clear ();
		if (UserData.instance.tags.Count >= 2) {
			for (int i = 0; i < UserData.instance.tags.Count; i++) {
				//print (UserData.instance.tags[i]);
				//print (UserData.instance.tags[i][0]);
				if (UserData.instance.tags[i][0] == 'C') {
					if (UserData.instance.tags[i] != car) {
						tagsToAdd.Add (car);
						tagsToDelete.Add (UserData.instance.tags[i]);
					}
				} else if (UserData.instance.tags[i][0] == 'T') {
					if (UserData.instance.tags[i] != trail) {
						tagsToAdd.Add (trail);
						tagsToDelete.Add (UserData.instance.tags[i]);
					}
				}
			}
		} else if (UserData.instance.tags.Count == 1) {
			bool trailPresent = false;
			bool carPresent = false;
			for (int i = 0; i < UserData.instance.tags.Count; i++) {
				if (UserData.instance.tags[i][0] == 'C') {
					carPresent = true;
				} else if (UserData.instance.tags[i][0] == 'T') {
					trailPresent = true;
				}
			}
			if (trailPresent) {
				//Add car tag
				tagsToAdd.Add (car);
			}
			if (carPresent) {
				//Add trail tag
				tagsToAdd.Add (trail);
			}
			if ((trailPresent == false) && (carPresent == false)) {
				tagsToAdd.Add (car);
				tagsToAdd.Add (trail);
			}
		} else if (UserData.instance.tags.Count == 0) {
			tagsToAdd.Add (car);
			tagsToAdd.Add (trail);
		}
		if (tagsToAdd.Count > 0) {
			StartCoroutine (AddTags ());
		}
		if (tagsToDelete.Count > 0) {
			StartCoroutine (DeleteTags ());
		}
	}
	IEnumerator DeleteTags () {
		//print ("Deleting " + tagsToDelete.Count + " tags");
		for (int i = 0; i < tagsToDelete.Count; i++) {
			PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
				FunctionName = "RemovePlayerTag",
					FunctionParameter = new Dictionary<string, string> {
						{
							"tagName",
							tagsToDelete[i]
						}
					}
			}, (ExecuteCloudScriptResult res) => {
				for (int k = 0; k < res.Logs.Count; k++) {
					print (res.Logs[k].Message);
				}
			}, CallFailure);
			tagsToDelete.RemoveAt (i);
			yield return new WaitForSeconds (1f);
		}
		StartCoroutine (AddTags ());
	}
	IEnumerator AddTags () {
		//print ("Adding " + tagsToAdd.Count + " tags");
		//print (tagsToAdd.Count);
		for (int i = 0; i < tagsToAdd.Count; i++) {
			PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
				FunctionName = "AddPlayerTag",
					FunctionParameter = new Dictionary<string, string> {
						{
							"tagName",
							tagsToAdd[i]
						}
					}
			}, (ExecuteCloudScriptResult res) => {
				if (Debug.isDebugBuild) {
					for (int k = 0; k < res.Logs.Count; k++) {
						print (res.Logs[k].Message);
					}
				}
			}, CallFailure);
			tagsToAdd.RemoveAt (i);
			yield return new WaitForSeconds (1f);
		}
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