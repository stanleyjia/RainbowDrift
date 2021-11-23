using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
public class FillMissingController : MonoBehaviour {
	public static FillMissingController instance;
	Dictionary<string, System.Action> data = new Dictionary<string, System.Action> ();
	public List<string> MissingUserInventory = new List<string> ();
	public List<string> MissingReadOnly = new List<string> ();
	public List<string> MissingUserData = new List<string> ();
	public Dictionary<string, string> UserDataDefaultValues = new Dictionary<string, string> {
		{
			"ChallengesCompleted",
			"#"
		},
		{
			"ChallengesFinished",
			"#"
		},
		{
			"HighScore",
			"0"
		},
		{
			"CurrentCar",
			"C0002"
		},
		{
			"CurrentTrail",
			"T0001"
		},
		{
			"AvatarUrl",
			"https://res.cloudinary.com/moosepark/image/upload/v1534403658/fox.png"
		},
		{
			"AvatarInd",
			"0"
		},
		{
			"UsernameSet",
			"False"
		},
	};
	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad (this);
		data.Add ("UserInventory", SetUserInventory);
		data.Add ("UserReadOnlyData", SetUserReadOnlyData);
		data.Add ("UserData", SetUserData);
		data.Add ("AccountInfo", SetAccountInfo);
		data.Add ("PlayerTags", SetPlayerTags);
		data.Add ("SetStatistics", SetStatistics);
	}
	private void SetUserInventory () { }
	private void SetUserReadOnlyData () {
		Dictionary<string, List<string>> data = new Dictionary<string, List<string>> { { "Referrals", new List<string> () }
		};
		PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
			FunctionName = "CreateReadOnlyData",
				FunctionParameter = new Dictionary<string, Dictionary<string, List<string>>> {
					{
						"Data",
						data
					}
				}
		}, (ExecuteCloudScriptResult res) => { }, (PlayFabError error) => {
			if (Debug.isDebugBuild) {
				error.GenerateErrorReport ();
			}
		});
	}
	private void SetUserData () {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		for (int i = 0; i < MissingUserData.Count; i++) {
			data.Add (MissingUserData[i], UserDataDefaultValues[MissingUserData[i]]);
		}
		PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest {
			Data = data
		}, (UpdateUserDataResult result) => {
			if (Debug.isDebugBuild) {
				print ("Succeeded setting " + data.Count + " User Data Values");
			}
			MissingUserData.Clear ();
		}, (PlayFabError error) => {
			if (Debug.isDebugBuild) {
				print (error);
			}
		});
	}
	private void SetAccountInfo () { }
	private void SetPlayerTags () { }
	private void SetStatistics () { }
	public void SetData (string id) {
		data[id] ();
	}
}