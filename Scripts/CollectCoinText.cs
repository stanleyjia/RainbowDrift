using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollectCoinText : MonoBehaviour {
	public static CollectCoinText instance;
	Text text;
	Camera cam;
	RectTransform rect;
	Vector3 shiftedPosition;
	Animator anim;
	GameObject textObject;
	int labelNum = 0;
	bool combinedRunning;
	bool animateInCombined = false;
	List<GameObject> coinCollectTextList = new List<GameObject> ();
	Vector3 currentScale;
	float screenScale;
	int countOfList = 5;
	// Use this for initialization
	void Start () {
		instance = this;
		cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		for (int i = 0; i < countOfList; i++) {
			GameObject collectObject = (GameObject) Instantiate (Resources.Load ("Prefab/Animation/CoinCollectObject"));
			collectObject.transform.SetParent (this.transform);
			collectObject.GetComponent<RectTransform> ().localScale = Vector3.one;
			collectObject.transform.Find ("CoinCollectText").GetComponent<Text> ().color = Color.clear;
			coinCollectTextList.Add (collectObject);
		}
		screenScale = Screen.width / 1080f;
		transform.localScale = new Vector3 (screenScale, screenScale, 1f);
		//print (screenScale);
	}
	// Update is called once per frame
	void Update () {
		//print ("Current state: " + anim.GetCurrentAnimatorClipInfo (0) [0].clip.name);
	}
	public void CollectCoin (Vector3 position) {
		//		print ("First: " + position);
		//print ("After: " + cam.WorldToScreenPoint (position));
		if (ColorBarController.instance.rainbowMode) {
			labelNum = labelNum + ((ColorBarController.instance.colorMode + 2));
		} else {
			labelNum = labelNum + ((ColorBarController.instance.colorMode + 1));
		}
		if (combinedRunning == false) {
			for (int i = 0; i < coinCollectTextList.Count; i++) {
				if (coinCollectTextList[i].transform.Find ("CoinCollectText").gameObject.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0) [0].clip.name == "idle") {
					StartCoroutine (CombinedCollect (position, coinCollectTextList[i]));
					//					print ("started");
					combinedRunning = true;
					break;
				}
			}
		}
		animateInCombined = true;
	}
	IEnumerator CombinedCollect (Vector3 pos, GameObject obj) {
		text = obj.transform.Find ("CoinCollectText").gameObject.GetComponent<Text> ();
		anim = obj.transform.Find ("CoinCollectText").gameObject.GetComponent<Animator> ();
		//	print (pos);
		shiftedPosition = cam.WorldToScreenPoint (pos);
		//print ("Shifted " + shiftedPosition);
		//shiftedPosition = new Vector3 (shiftedPosition.x, shiftedPosition.y, shiftedPosition.z);
		obj.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (shiftedPosition.x, shiftedPosition.y);
		//obj.GetComponent<RectTransform> ().localScale = new Vector3 (Screen.width / 1080, Screen.height / 1920);
		//currentScale = obj.transform.Find ("CoinCollectText").GetComponent<RectTransform> ().localScale;
		//print (currentScale.x);
		//obj.transform.Find ("CoinCollectText").GetComponent<RectTransform> ().localScale = new Vector3 (currentScale.x * (Screen.width / 1080), currentScale.y * Screen.height / 1920, 1f);
		//print (obj.GetComponent<RectTransform> ().anchoredPosition);
		text.color = Color.white;
		anim.SetTrigger ("ShowNumber");
		for (float i = 0; i < 1f; i += Time.unscaledDeltaTime / 0.4f) {
			if (animateInCombined) {
				i = 0;
				text.text = "+" + labelNum;
				animateInCombined = false;
			}
			yield return null;
		}
		anim.SetTrigger ("FadeOut");
		combinedRunning = false;
		labelNum = 0;
	}
	Vector3 ResizeToScreenWidth (Vector3 pos) {
		return (new Vector3 (pos.x / 1080f * Screen.width, pos.y, pos.z));
	}
}