using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
public class IAPController : MonoBehaviour {
	float height;
	float width;
	List<Panel> panels = new List<Panel> ();
	public List<Dictionary<string, object>> Sales;
	Dictionary<string, int> prices;
	GameObject panelContainer;
	GameObject scrollBackground;
	public Animator purchaseWarning;
	public static IAPController instance;
	// Use this for initialization
	void Start () {
		instance = this;
		SetStore ();
	}
	// Update is called once per frame
	void Update () { }
	void SetStore () {
		panels.Clear ();
		panelContainer = GameObject.FindWithTag ("IAPPanelContainer");
		scrollBackground = GameObject.FindWithTag ("IAPScroll");
		Sales = SetSalesList (UserData.instance.StoreItems);
		//Sales = UserData.instance.StoreItems;
		for (int i = 0; i < Sales.Count; i++) {
			Panel panel = new Panel ();
			panel.gameOb = (GameObject) Instantiate (Resources.Load ("Prefab/Panels/IAPPanel"));
			panel.gameOb.GetComponent<IAPPanelController> ().id = Sales[i]["ItemId"].ToString ();
			panel.gameOb.transform.SetParent (panelContainer.transform, false);
			height = panel.gameOb.GetComponent<RectTransform> ().rect.height;
			width = panel.gameOb.GetComponent<RectTransform> ().rect.width;
			panel.gameOb.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height / 2) - (height * panels.Count), 0);
			panel.gameOb.transform.Find ("IAPName").GetComponent<Text> ().text = Sales[i]["DisplayName"].ToString ();
			//	pics[i].sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			panel.gameOb.transform.Find ("BuyButton").Find ("Text").GetComponent<Text> ().text = "$" + ((Sales[i]["VirtualCurrencyPrices"] as Dictionary<string, uint>) ["RM"] / 100f).ToString ("F2");
			//print ((Sales[i]["Bundle"] as PlayFab.ClientModels.CatalogItemBundleInfo).BundledVirtualCurrencies["CN"]);
			//print ((Sales[i]["Tags"] as List<string>) [0]);
			Texture2D texture;
			if ((Sales[i]["Tags"] as List<string>).Contains ("coins")) {
				panel.gameOb.transform.Find ("Rewards").Find ("Count").GetComponent<Text> ().text = (Sales[i]["Bundle"] as PlayFab.ClientModels.CatalogItemBundleInfo).BundledVirtualCurrencies["CN"].ToString ();
				panel.gameOb.transform.Find ("Rewards").Find ("Images").Find ("UIOrbs").gameObject.SetActive (false);
				try {
					texture = Resources.Load ("IAP/Coins/" + Sales[i]["ItemId"]) as Texture2D;
					panel.gameOb.transform.Find ("Image").GetComponent<Image> ().sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
					panel.gameOb.transform.Find ("Image").GetComponent<Image> ().preserveAspect = true;
				} catch { }
			} else if ((Sales[i]["Tags"] as List<string>).Contains ("orbs")) {
				panel.gameOb.transform.Find ("Rewards").Find ("Count").GetComponent<Text> ().text = (Sales[i]["Bundle"] as PlayFab.ClientModels.CatalogItemBundleInfo).BundledVirtualCurrencies["OB"].ToString ();
				panel.gameOb.transform.Find ("Rewards").Find ("Images").Find ("UICoins").gameObject.SetActive (false);
				try {
					texture = Resources.Load ("IAP/Orbs/" + Sales[i]["ItemId"]) as Texture2D;
					panel.gameOb.transform.Find ("Image").GetComponent<Image> ().sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
					panel.gameOb.transform.Find ("Image").GetComponent<Image> ().preserveAspect = true;
				} catch { }
			}
			panels.Add (panel);
		}
		if ((height * panels.Count) < Screen.height - scrollBackground.GetComponent<RectTransform> ().offsetMax.y - scrollBackground.GetComponent<RectTransform> ().offsetMin.y) {
			panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, scrollBackground.GetComponent<RectTransform> ().rect.height);
		} else {
			panelContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, (height * panels.Count));
			//print (-(height * panels.Count));
		}
		panelContainer.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -(height * panels.Count) / 2f, 0);
	}
	List<Dictionary<string, object>> SetSalesList (List<Dictionary<string, object>> list) {
		List<Dictionary<string, object>> returnList = new List<Dictionary<string, object>> ();
		for (int i = 0; i < list.Count; i++) {
			if (list[i]["ItemClass"].ToString () == "IAP") {
				returnList.Add (list[i]);
			}
		}
		returnList = returnList.OrderBy (go => go["ItemId"]).ToList ();
		return returnList;
	}
	static int SortById (Dictionary<string, object> p1, Dictionary<string, object> p2) {
		return p1["ItemId"].ToString ().CompareTo (p2["ItemId"].ToString ());
	}
	public void ShowWarning () {
		StartCoroutine (Warning ());
	}
	IEnumerator Warning () {
		purchaseWarning.SetTrigger ("Show");
		yield return new WaitForSeconds (0.5f);
		yield return new WaitForSeconds (0.5f);
		purchaseWarning.SetTrigger ("Hide");
	}
}