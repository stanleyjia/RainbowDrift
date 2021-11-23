using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CanvasTransitionController : MonoBehaviour {
	bool dragging = false;
	public RectTransform center;
	public ScrollRect scrollRect;
	public GameObject[] screens = new GameObject[3];
	public RectTransform content;
	public float[] screenDistanceToCenter = new float[3];
	public Animator leaderboardAnimator;
	public Animator IAPAnimator;
	int indexOfClosest = 0;
	float positionToGoTowards;
	float positionToGoTowards1;
	public float[] positioning;
	public RectTransform panelContainer;
	float speedToSnap = 0.22f;
	bool calculating = true;
	private float lastXPosition;
	float velocity;
	int index = 1;
	float distance = 0;
	RectTransform canvas;
	bool buttonPressed = false;
	public Animator infoAnimator;
	public Animator challengesAnimator;
	public Animator friendsAnimator;
	public Animator userAnimator;
	public Animator moreInfo;
	public Animator settingsAnimator;
	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("CombinedSceneCavas").GetComponent<RectTransform> ();
		positioning = new float[3] {
			canvas.rect.width, 0, -canvas.rect.width
		};
		speedToSnap = 0.22f;
		UpdateDistances ();
		content.localPosition = new Vector3 (positioning[UserData.instance.combinedIndex], content.localPosition.y, content.localPosition.z);
		scrollRect.decelerationRate = 5f;
	}
	// Update is called once per frame
	void Update () {
		UpdateDistances ();
		if (buttonPressed == false) {
#if (UNITY_IOS|| UNITY_ANDROID) && !UNITY_EDITOR
			//	print (" notEditor");
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				//print (panelContainer.anchoredPosition.x);
				if (dragging == false) {
					//Started dragging
					//calculating = true;
					//StartCoroutine (CalculateVelocity ());
					dragging = true;
					indexOfClosest = System.Array.IndexOf (screenDistanceToCenter, Mathf.Min (screenDistanceToCenter));
					//print ("start pos: " + Input.mousePosition.x / Screen.width);
				}
			} else {
				if (dragging) {
					//Stopped dragging
					//print ("stopped dragging");
					dragging = false;
					//					makeSwooshSound = false;
					//		scrollRect.inertia = true;
					StartCoroutine (SnapToScreen ());
				}
			}
#endif
#if UNITY_EDITOR
			//			print ("Editor");
			if (Input.GetMouseButton (0)) {
				//print (panelContainer.anchoredPosition.x);
				if (dragging == false) {
					//Started dragging
					//calculating = true;
					//StartCoroutine (CalculateVelocity ());
					dragging = true;
					indexOfClosest = System.Array.IndexOf (screenDistanceToCenter, Mathf.Min (screenDistanceToCenter));
					//print ("start pos: " + Input.mousePosition.x / Screen.width);
				}
			} else {
				if (dragging) {
					//Stopped dragging
					//print ("stopped dragging");
					dragging = false;
					//	scrollRect.inertia = true;
					StartCoroutine (SnapToScreen ());
				}
			}
#endif
		}
		if (dragging == false) { }
	}
	void UpdateDistances () {
		for (int i = 0; i < screens.Length; i++) {
			screenDistanceToCenter[i] = Mathf.Abs (screens[i].GetComponent<RectTransform> ().position.x - center.position.x);
		}
		//indexOfClosest = System.Array.IndexOf (screenDistanceToCenter, Mathf.Min (screenDistanceToCenter));
		//print (indexOfClosest);
	}
	IEnumerator CalculateVelocity () {
		while (calculating) {
			yield return null;
			velocity = (panelContainer.anchoredPosition.x - lastXPosition) / Time.unscaledDeltaTime;
			//Negative velocity goes right
			//Positive velocity goes left
			//time = Mathf.Clamp ((Screen.width / velocity), 0.2f, 0.8f);
			lastXPosition = panelContainer.anchoredPosition.x;
		}
	}
	IEnumerator SnapToScreen () {
		//yield return new WaitForSecondsRealtime (waitBeforeSnap);
		lastXPosition = panelContainer.anchoredPosition.x;
		yield return null;
		velocity = (panelContainer.anchoredPosition.x - lastXPosition) / Time.unscaledDeltaTime;
		//print (panelContainer.anchoredPosition.x);
		if (Mathf.Abs (velocity) > 200f) {
			if (velocity < 0f) {
				//Going right, swiping left
				//print ("going right");
				if (Mathf.Abs (panelContainer.anchoredPosition.x) < 1080) {
					if (indexOfClosest + 1 <= 2) {
						index = indexOfClosest + 1;
					}
				}
			} else {
				//Going left, swiping right
				//				print ("going left");
				if (Mathf.Abs (panelContainer.anchoredPosition.x) < 1080) {
					if (indexOfClosest - 1 >= 0) {
						index = indexOfClosest - 1;
					}
				}
			}
		} else {
			index = System.Array.IndexOf (screenDistanceToCenter, Mathf.Min (screenDistanceToCenter));
		}
		//print (index);
		positionToGoTowards = positioning[index];
		distance = Mathf.Abs (positionToGoTowards - content.localPosition.x);
		speedToSnap = 0.22f * (Mathf.Abs (distance / velocity));
		speedToSnap = Mathf.Clamp (speedToSnap, 0.15f, 0.3f);
		for (float i = 0; i < 1; i += Time.unscaledDeltaTime / speedToSnap) {
			if (buttonPressed == false) {
				if (dragging == false) {
					//	calculating = false;
					yield return null;
					content.localPosition = Vector3.Lerp (content.localPosition, new Vector3 (positionToGoTowards, content.localPosition.y, content.localPosition.z), i);
					scrollRect.inertia = false;
					if (Mathf.Abs (content.localPosition.x - positionToGoTowards) < 50) {
						if (distance > 300) {
							//PlayAudio.PlaySound ("swoosh");
							//	iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 1);
							distance = 0;
						}
					}
				} else {
					//	scrollRect.inertia = true;
					break;
				}
			} else {
				//	scrollRect.inertia = true;
				break;
			}
		}
		if (dragging == false) {
			//scrollRect.inertia = false;
			content.localPosition = new Vector3 (positionToGoTowards, content.localPosition.y, content.localPosition.z);
			/*if (distance > 300) {
				PlayAudio.PlaySound ("swoosh");
			}*/
		}
		//yield return new WaitForSecondsRealtime (0.05f);
		yield return null;
		scrollRect.inertia = true;
		ResetPanels (index);
		//ResetPanels ();
	}
	public void ShowLeaderboards () {
		leaderboardAnimator.SetTrigger ("showLeaderboards");
	}
	public void HideLeaderboards () {
		leaderboardAnimator.SetTrigger ("hideLeaderboards");
	}
	public void ShowUser () {
		userAnimator.SetTrigger ("showInfo");
	}
	public void HideUser () {
		userAnimator.SetTrigger ("hideInfo");
	}
	public void ShowIAP () {
		IAPAnimator.SetTrigger ("showIAP");
	}
	public void HideIAP () {
		IAPAnimator.SetTrigger ("hideIAP");
	}
	public void ViewMoreInfo () {
		moreInfo.SetTrigger ("showLeaderboards");
	}
	public void HideMoreInfo () {
		moreInfo.SetTrigger ("hideLeaderboards");
	}
	public void ShowStats () {
		infoAnimator.SetTrigger ("showInfo");
	}
	public void HideStats () {
		infoAnimator.SetTrigger ("hideInfo");
	}
	public void ShowChallenges () {
		challengesAnimator.SetTrigger ("showChallenges");
	}
	public void HideChallenges () {
		challengesAnimator.SetTrigger ("hideChallenges");
	}
	public void ShowFriends () {
		friendsAnimator.SetTrigger ("showInfo");
	}
	public void HideFriends () {
		friendsAnimator.SetTrigger ("hideInfo");
	}
	public void ShowScreen (int ind) {
		StartCoroutine (TransitionTo (ind));
		buttonPressed = true;
	}
	public void ShowSettings () {
		settingsAnimator.SetTrigger ("showInfo");
	}
	public void HideSettings () {
		settingsAnimator.SetTrigger ("hideInfo");
	}
	void ResetPanels (int ToScreen) {
		switch (ToScreen) {
			case 0:
				//To User Info
				leaderboardAnimator.Play ("LeaderboardsIdle");
				IAPAnimator.Play ("idle");
				break;
			case 1:
				//To Start
				infoAnimator.Play ("infoIdle");
				userAnimator.Play ("infoIdle");
				moreInfo.Play ("LeaderboardsIdle");
				challengesAnimator.Play ("challengesIdle");
				friendsAnimator.Play ("infoIdle");
				settingsAnimator.Play ("infoIdle");
				IAPAnimator.Play ("idle");
				break;
			case 2:
				//To Store
				leaderboardAnimator.Play ("LeaderboardsIdle");
				settingsAnimator.Play ("infoIdle");
				friendsAnimator.Play ("infoIdle");
				infoAnimator.Play ("infoIdle");
				userAnimator.Play ("infoIdle");
				moreInfo.Play ("LeaderboardsIdle");
				challengesAnimator.Play ("challengesIdle");
				break;
			default:
				break;
		}
	}
	IEnumerator TransitionTo (int ind) {
		while (scrollRect.inertia == false) {
			yield return null;
		}
		positionToGoTowards1 = positioning[ind];
		distance = Mathf.Abs (positionToGoTowards - content.localPosition.x);
		//print (distance);
		scrollRect.inertia = false;
		for (float i = 0; i < 1; i += Time.unscaledDeltaTime / 0.15f) {
			yield return null;
			//	print (content.localPosition);
			//print (positionToGoTowards);
			content.localPosition = Vector3.Lerp (content.localPosition, new Vector3 (positionToGoTowards1, content.localPosition.y, content.localPosition.z), i);
		}
		content.localPosition = new Vector3 (positionToGoTowards1, content.localPosition.y, content.localPosition.z);
		buttonPressed = false;
		yield return new WaitForSecondsRealtime (0.15f);
		scrollRect.inertia = true;
		ResetPanels (index);
		//ResetPanels ();
	}
}