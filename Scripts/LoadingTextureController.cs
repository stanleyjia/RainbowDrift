using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingTextureController : MonoBehaviour {
	Image image;
	Texture2D iPhone;
	//Texture2D iPhone55;
	// Use this for initialization
	void Start () {
		//image = GetComponent<Image> ();
		//print (Screen.height);
		switch (Screen.height) {
			case 2688:
				//iphoneXS Max
				//print (2688);
				SetActive ("iphoneX");
				break;
			case 2436:
				//iphoneX + iphoneXs
				//print (2436);
				SetActive ("iphoneX");
				break;
			case 1792:
				//iphoneXR
				//print (1792);
				SetActive ("iphoneX");
				break;
			case 1920:
				//iphone plus
				//print (1920);
				SetActive ("iphone5.5");
				break;
			case 1334:
				//iphone 6
				SetActive ("iphone4.7");
				break;
			case 1136:
				//iphone 5
				SetActive ("iphone4");
				break;
			case 960:
				//iphone 4
				SetActive ("iphone3.5");
				break;
			default:
				//print ("Default");
				SetActive ("iphoneX");
				//SetActive ("mobile");\
				break;
		}
	}
	void SetActive (string id) {
		foreach (Transform child in transform) {
			//print (child.name);
			if (child.name != id) {
				child.gameObject.SetActive (false);
			} else {
				child.gameObject.SetActive (true);
			}
		}
	}
	// Update is called once per frame
	void Update () { }
}