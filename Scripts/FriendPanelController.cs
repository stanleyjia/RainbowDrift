using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FriendPanelController : MonoBehaviour {
	public string PlayFabId;
	public string DisplayName;
	// Use this for initialization
	public void RemoveFriend () {
		FriendsController.instance.ShowRemoveFriendsModal (PlayFabId, DisplayName);
	}
}