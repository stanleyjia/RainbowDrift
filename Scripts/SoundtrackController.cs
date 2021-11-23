using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundtrackController : MonoBehaviour {
	// Use this for initialization
	string lastScene;
	void Start () {
		DontDestroyOnLoad (this);
		lastScene = SceneManager.GetActiveScene ().name;
		SceneManager.activeSceneChanged += ActiveSceneChanged;
	}
	// Update is called once per frame
	void ActiveSceneChanged (Scene last, Scene next) {
		AudioListener.pause = false;
		lastScene = GameController.instance.previousScene;
		switch (next.name) {
			case "CombinedScene":
				switch (lastScene) {
					case "Persistent":
						PlayAudio.PlaySound ("startRev");
						PlayAudio.PlaySound ("menu");
						break;
					case "GameScene":
						if (PlayAudio.IsPlaying ("rainbow")) {
							PlayAudio.StopSound ("rainbow");
						}
						//PlayAudio.StopRainbowMode ();
						PlayAudio.StopSound ("drift");
						PlayAudio.PlaySound ("startRev");
						if (!PlayAudio.pause) {
							PlayAudio.FadeInto ("play", "menu");
						} else {
							PlayAudio.SetVolumes ();
							PlayAudio.StopSound ("play");
							PlayAudio.PlaySound ("menu");
							PlayAudio.pause = false;
						}
						break;
					default:
						break;
				}
				break;
			case "GameScene":
				switch (lastScene) {
					case "CombinedScene":
						PlayAudio.PlaySound ("startRev");
						PlayAudio.FadeInto ("menu", "play");
						break;
					case "GameScene":
						if (PlayAudio.IsPlaying ("rainbow")) {
							//PlayAudio.FadeOut ("rainbow");
							PlayAudio.StopRainbowMode ();
						}
						PlayAudio.StopSound ("drift");
						PlayAudio.PlaySound ("startRev");
						break;
					default:
						break;
				}
				break;
		}
	}
}