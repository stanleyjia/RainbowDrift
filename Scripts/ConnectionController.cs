using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ConnectionController : MonoBehaviour {
	public static ConnectionController instance;
	public GameObject noConnectionWarning;
	float timeToWait = 0.5f;
	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad (this);
		noConnectionWarning.SetActive (false);
		//StartCoroutine (CheckForConnection ());
	}
	// Update is called once per frame
	void Update () { }
	public void noConnectionDetected () {
		//GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
		//SceneManager.LoadSceneAsync ("Persistent");
		noConnectionWarning.SetActive (true);
		Time.timeScale = 0;
		StartCoroutine (CheckForConnection ());
	}
	IEnumerator CheckForConnection () {
		while (Application.internetReachability == NetworkReachability.NotReachable) {
			//print ("checked");
			yield return StartCoroutine (CoroutineUtil.WaitForRealSeconds (timeToWait));
			if (timeToWait < 5f) {
				timeToWait = timeToWait * 1.2f;
			}
		}
		noConnectionWarning.SetActive (false);
		Time.timeScale = 1;
		if (SceneManager.GetActiveScene ().name == "Persistent") {
			ResetGame ();
		}
	}
	void ResetGame () {
		foreach (GameObject o in Object.FindObjectsOfType<GameObject> ()) {
			Destroy (o);
		}
		SceneManager.LoadSceneAsync ("Persistent");
	}
}