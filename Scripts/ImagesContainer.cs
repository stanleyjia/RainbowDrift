using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ImagesContainer : MonoBehaviour {
	public Dictionary<string, string> avatarUrls = new Dictionary<string, string> ();
	public Dictionary<string, Texture2D> avatarTextures = new Dictionary<string, Texture2D> ();
	public static ImagesContainer instance;
	int texturesCount = 0;
	bool texturesUpdating = false;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		instance = this;
		avatarUrls.Add ("fox", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/fox.png");
		avatarUrls.Add ("panda", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/panda.png");
		avatarUrls.Add ("walrus", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/walrus.png");
		avatarUrls.Add ("penguin", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/penguin.png");
		avatarUrls.Add ("tiger", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/tiger.png");
		avatarUrls.Add ("pig", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/pig.png");
		avatarUrls.Add ("cobra", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/cobra.png");
		avatarUrls.Add ("rhino", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/rhino.png");
	}
	// Update is called once per frame
	void Update () {
		if (texturesUpdating) {
			if (texturesCount == avatarUrls.Count) {
				print (avatarTextures["fox"]);
				texturesCount = 0;
				texturesUpdating = false;
			}
		}
	}
	public string GetName (int ind) {
		switch (ind) {
			case 0:
				return "fox";
			case 1:
				return "panda";
			case 2:
				return "walrus";
			case 3:
				return "penguin";
			case 4:
				return "tiger";
			case 5:
				return "pig";
			case 6:
				return "cobra";
			case 7:
				return "rhino";
			default:
				return "fox";
		}
	}
	IEnumerator WWWPic (string url) {
		////print(url);
		UnityWebRequest www = UnityWebRequestTexture.GetTexture (url);
		yield return www.SendWebRequest ();
		if (www.isNetworkError || www.isHttpError) {
			//Debug.log (www.error);
		} else {
			//	outputTexture = ((DownloadHandlerTexture) www.downloadHandler).texture as Texture2D;
			//texturesCount++;
			yield return ((DownloadHandlerTexture) www.downloadHandler).texture as Texture2D;
		}
	}
}