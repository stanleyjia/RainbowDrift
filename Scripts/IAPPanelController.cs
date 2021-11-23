using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IAPPanelController : MonoBehaviour {
	public string id;
	AudioPathway pa;
	//Purchaser purchaser;
	// Use this for initialization
	void Start () {
		//purchaser = GameObject.FindWithTag ("Purchaser").GetComponent<Purchaser> ();
	}
	// Update is called once per frame
	void Update () { }
	public void BuyIAP () {
		PlayAudio.PlaySound ("click");
		if (DataEntry.instance.hapticOn) {
			iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 4);
		}
		//purchaser = GameObject.FindWithTag ("Purchaser").GetComponent<Purchaser> ();
		//print (id);
		//purchaser.BuyProductID (id);
	}
}