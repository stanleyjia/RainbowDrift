using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarDriftController : MonoBehaviour {
	// Use this for initialization
	float driftAmount = 0.4f;
	float zeroOpacity = 0f;
	float fullOpacity = 0.6f;
	public bool drifting = false;
	public Skidmarks skidmarks;
	GameObject car;
	Transform rightWheel;
	Transform leftWheel;
	int leftSkid;
	int rightSkid;
	bool drifted = false;
	float timeAfterDrift = 0.4f;
	float skidOpacity = 0.6f;
	float distanceToTrack = 1.6f;
	float leftDistance;
	float rightDistance;
	bool leftGenerate;
	bool rightGenerate;
	float distanceToLeft;
	float distanceToRight;
	bool once = true;
	public static CarDriftController instance;
	int driftCount = 0;
	bool chal5;
	bool chal13;
	bool chal20;
	bool interruptedFadeOut = false;
	bool interruptedFadeIn = false;
	bool tutorial = false;
	void Awake () {
		instance = this;
	}
	void Start () {
		car = GetActiveCar ();
		rightWheel = car.transform.Find ("RightWheel");
		leftWheel = car.transform.Find ("LeftWheel");
		skidOpacity = zeroOpacity;
		chal5 = ChallengesController.ChallengeDone (5);
		chal13 = ChallengesController.ChallengeDone (13);
		chal20 = ChallengesController.ChallengeDone (20);
		if (SceneManager.GetActiveScene ().name == "TutorialScene") {
			tutorial = true;
			driftAmount = 0.3f;
		}
		//print (car.name);
	}
	// Update is called once per frame
	void Update () {
		if (CarVariables.instance.gameOn == true) {
			if (once == false) {
				once = true;
			}
			if (Mathf.Abs (CarVariables.instance.leftToRight) > driftAmount) {
				if (drifting == false) {
					//	iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 0);
					driftCount++;
					CheckChallenges ();
					drifting = true;
					StartCoroutine (SkidmarksFadeIn ());
				}
				drifted = true;
				//skidOpacity = fullOpacity;
				GenerateTrackIfOnTrack ();
				PlayAudio.PlayDrift ();
			} else {
				if (drifted) {
					drifted = false;
					GenerateTrackIfOnTrack ();
					StartCoroutine (StillDrifting ());
				}
			}
		} else {
			if (once) {
				PlayAudio.FadeOut ("drift");
				once = false;
			}
		}
	}
	void GenerateTrackIfOnTrack () {
		CheckForDistance (Pathmaker.instance.totalPoints, leftWheel.position, rightWheel.position);
		if (leftGenerate) {
			leftSkid = skidmarks.AddSkidMark (leftWheel.position, Vector3.back, skidOpacity, leftSkid);
		} else {
			leftSkid = skidmarks.AddSkidMark (leftWheel.position, Vector3.back, 0f, leftSkid);
		}
		if (rightGenerate) {
			rightSkid = skidmarks.AddSkidMark (rightWheel.position, Vector3.back, skidOpacity, rightSkid);
		} else {
			rightSkid = skidmarks.AddSkidMark (rightWheel.position, Vector3.back, 0f, rightSkid);
		}
	}
	void CheckChallenges () {
		if (chal5) {
			if (driftCount >= 5f) {
				ChallengesController.CompleteChallenge (5);
				chal5 = false;
			}
		}
		if (chal13) {
			if (driftCount >= 25f) {
				ChallengesController.CompleteChallenge (13);
				chal13 = false;
			}
		}
		if (chal20) {
			if (driftCount >= 100f) {
				ChallengesController.CompleteChallenge (20);
				chal20 = false;
			}
		}
	}
	IEnumerator SkidmarksFadeOut () {
		//print ("start fading out");
		interruptedFadeOut = false;
		float startOpacity = skidOpacity;
		float endOpacity = zeroOpacity;
		for (float i = 0; i < 1f; i += Time.deltaTime * 2.5f) {
			if (drifting == false) {
				skidOpacity = Mathf.Lerp (startOpacity, endOpacity, i);
				GenerateTrackIfOnTrack ();
				yield return null;
			} else {
				//	print ("exit fading out");
				interruptedFadeOut = true;
				break;
			}
		}
		if (!interruptedFadeOut) {
			skidOpacity = zeroOpacity;
		}
	}
	IEnumerator SkidmarksFadeIn () {
		//	print ("Start Fade in ");
		interruptedFadeIn = false;
		float startOpacity = skidOpacity;
		float endOpacity = fullOpacity;
		for (float i = 0; i < 1f; i += Time.deltaTime * 4f) {
			if (drifting == true) {
				skidOpacity = Mathf.Lerp (startOpacity, endOpacity, i);
				yield return null;
			} else {
				//	print ("exit fade in ");
				interruptedFadeIn = true;
				break;
			}
		}
		if (interruptedFadeIn) {
			skidOpacity = fullOpacity;
		}
	}
	IEnumerator StillDrifting () {
		bool drifted = false;
		for (float i = 0; i < 1f; i += Time.deltaTime / timeAfterDrift) {
			GenerateTrackIfOnTrack ();
			if (Mathf.Abs (CarVariables.instance.leftToRight) > driftAmount) {
				drifting = true;
				drifted = true;
				break;
			}
			yield return null;
		}
		if (drifted == false) {
			drifting = false;
			GenerateTrackIfOnTrack ();
			StartCoroutine (SkidmarksFadeOut ());
			PlayAudio.StopSound ("drift");
		}
	}
	GameObject GetActiveCar () {
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).gameObject.activeSelf == true) {
				return (transform.GetChild (i).gameObject);
			}
		}
		if (Debug.isDebugBuild) {
			//print ("no active car found");
		}
		return null;
	}
	void CheckForDistance (List<Vector3> points, Vector3 leftWheelPosition, Vector3 rightWheelPosition) {
		leftDistance = 5f;
		rightDistance = 5f;
		for (int i = 0; i < points.Count - 1; i = i + 2) {
			distanceToLeft = Vector3.Distance (points[i], leftWheelPosition);
			distanceToRight = Vector3.Distance (points[i], rightWheelPosition);
			if (distanceToLeft < leftDistance) {
				leftDistance = distanceToLeft;
			}
			if (distanceToRight < rightDistance) {
				rightDistance = distanceToRight;
			}
		}
		if (leftDistance < distanceToTrack) {
			leftGenerate = true;
		} else {
			//print ("left is out");
			leftGenerate = false;
		}
		if (rightDistance < distanceToTrack) {
			rightGenerate = true;
		} else {
			//	print ("right is out");
			rightGenerate = false;
		}
	}
}