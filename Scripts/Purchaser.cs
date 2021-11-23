using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
//using UnityEngine.Purchasing;
// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
/*
public class Purchaser : MonoBehaviour, IStoreListener {
	private static IStoreController m_StoreController; // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
	// Product identifiers for all products capable of being purchased:
	// "convenience" general identifiers for use with Purchasing, and their store-specific identifier
	// counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers
	// also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
	// General product identifiers for the consumable, non-consumable, and subscription products.
	// Use these handles in the code to reference which product to purchase. Also use these values
	// when defining the Product Identifiers on the store. Except, for illustration purposes, the
	// kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
	// specific mapping to Unity Purchasing's AddProduct, below.
	public static string kProductIDConsumable = "A_COINS";
	public static string kProductIDNonConsumable = "nonconsumable";
	public static string kProductIDSubscription = "subscription";
	// Apple App Store-specific product identifier for the subscription product.
	//private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
	// Google Play Store-specific product identifier subscription product.
	//private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
	int purchasePrice;
	void Start () {
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null) {
			// Begin to configure our connection to Purchasing
			//print ("m_StoreController is null");
			InitializePurchasing ();
		}
	}
	public void InitializePurchasing () {
		// If we have already connected to Purchasing ...
		//print ("Initializing");
		if (IsInitialized ()) {
			//print ("Already initialized");
			// ... we are done here.
			return;
		}
		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
		builder.AddProduct ("A_COINS", ProductType.Consumable, new IDs () { { "MP.MoosePark.Drift.A_COINS", AppleAppStore.Name }, });
		builder.AddProduct ("B_COINS", ProductType.Consumable, new IDs () { { "MP.MoosePark.Drift.B_COINS", AppleAppStore.Name } });
		//print ("Initialize started");
		UnityPurchasing.Initialize (this, builder);
	}
	private bool IsInitialized () {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}
	public void BuyConsumable () {
		// Buy the consumable product using its general identifier. Expect a response either
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID (kProductIDConsumable);
	}
	public void BuyNonConsumable () {
		// Buy the non-consumable product using its general identifier. Expect a response either
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID (kProductIDNonConsumable);
	}
	public void BuySubscription () {
		// Buy the subscription product using its the general identifier. Expect a response either
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		// Notice how we use the general product identifier in spite of this ID being mapped to
		// custom store-specific identifiers above.
		BuyProductID (kProductIDSubscription);
	}
	public void BuyProductID (string productId) {
		// If Purchasing has been initialized ...
		if (IsInitialized ()) {
			// ... look up the Product reference with the general product identifier and the Purchasing
			// system's products collection.
			Product product = m_StoreController.products.WithID (productId);
			// If the look up found a product for this device's store and that product is ready to be sold ...
			if (product != null && product.availableToPurchase) {
				if (Debug.isDebugBuild) {
					Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
				}
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed
				// asynchronously.
				m_StoreController.InitiatePurchase (product);
			}
			// Otherwise ...
			else {
				// ... report the product look-up failure situation
				if (Debug.isDebugBuild) {
					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
		}
		// Otherwise ...
		else {
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or
			// retrying initiailization.
			if (Debug.isDebugBuild) {
				Debug.Log ("BuyProductID FAIL. Not initialized.");
			}
		}
	}
	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases () {
		// If Purchasing has not yet been set up ...
		if (!IsInitialized ()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			if (Debug.isDebugBuild) {
				Debug.Log ("RestorePurchases FAIL. Not initialized.");
			}
			return;
		}
		// If we are running on an Apple device ...
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			// ... begin restoring purchases
			Debug.Log ("RestorePurchases started ...");
			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions ((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then
				// no purchases are available to be restored.
				if (Debug.isDebugBuild) {
					Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				}
			});
		}
		// Otherwise ...
		else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			if (Debug.isDebugBuild) {
				//Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			}
		}
	}
	//
	// --- IStoreListener
	//
	public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		if (Debug.isDebugBuild) {
			//Debug.Log ("OnInitialized: PASS");
		}
		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}
	public void OnInitializeFailed (InitializationFailureReason error) {
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		if (Debug.isDebugBuild) {
			//Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
		}
	}
	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args) {
		//args.purchasedProduct.definition.id
		//args.purchasedProduct.p
		if (Debug.isDebugBuild) {
			print (args.purchasedProduct.receipt);
		}
		for (int i = 0; i < UserData.instance.StoreItems.Count; i++) {
			if (UserData.instance.StoreItems[i]["ItemId"].ToString () == args.purchasedProduct.definition.id) {
				purchasePrice = (int) (UserData.instance.StoreItems[i]["VirtualCurrencyPrices"] as Dictionary<string, uint>) ["RM"];
				break;
			}
		}
		//as Dictionary<string, uint>)["RM"]
		var request = new ValidateIOSReceiptRequest {
			CurrencyCode = "RM",
				PurchasePrice = purchasePrice,
				ReceiptData = args.purchasedProduct.receipt
		};
		PlayFabClientAPI.ValidateIOSReceipt (request, (ValidateIOSReceiptResult res) => {
				PlayAudio.PlaySound ("buy");
				PlayFabClientAPI.PurchaseItem (new PurchaseItemRequest {
					ItemId = args.purchasedProduct.definition.id,
						Price = purchasePrice,
						VirtualCurrency = "RM",
				}, (PurchaseItemResult res1) => {
					if (Debug.isDebugBuild) {
						print ("Coins added");
					}
				}, CallFailure);
			},
			CallFailure);
		// A consumable product has been purchased by this user.
		if (String.Equals (args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal)) {
			if (Debug.isDebugBuild) {
				//Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
				// The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
				//	ScoreManager.score += 100;
				if (Debug.isDebugBuild) {
					print ("Coins added");
				}
			}
		}
		// Or ... a non-consumable product has been purchased by this user.
		else if (String.Equals (args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal)) {
			if (Debug.isDebugBuild) {
				//Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			}
			// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
		}
		// Or ... a subscription product has been purchased by this user.
		else if (String.Equals (args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal)) {
			if (Debug.isDebugBuild) {
				//Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			}
			// TODO: The subscription item has been successfully purchased, grant this to the player.
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else {
			if (Debug.isDebugBuild) {
				//Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
			}
		}
		// Return a flag indicating whether this product has completely been received, or if the application needs
		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still
		// saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}
	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason) {
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing
		// this reason with the user to guide their troubleshooting actions.
		if (Debug.isDebugBuild) {
			//Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		}
	}
	void CallFailure (PlayFabError error) {
		if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
			//no connection
			ConnectionController.instance.noConnectionDetected ();
		} else if (error.Error == PlayFabErrorCode.InvalidReceipt) {
			PlayAudio.PlaySound ("error");
			iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 5);
			IAPController.instance.ShowWarning ();
		} else {
			if (Debug.isDebugBuild) {
				print (error);
			}
		}
	}
}*/