using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FriendRequestPanel : MonoBehaviour {
	public string PlayFabId;
	public void AcceptFriendRequest () {
		FriendsController.instance.AcceptFriendRequest (PlayFabId);
	}
	public void RejectFriendRequest () {
		FriendsController.instance.RejectFriendRequest (PlayFabId);
	}
}