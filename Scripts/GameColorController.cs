using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameColorController : MonoBehaviour {
	public static GameColorController instance;
	List<Vector3> trackTopHSV = new List<Vector3> () {
		new Vector3 (197, 30, 74),
			new Vector3 (242, 30, 74),
			new Vector3 (280, 30, 74),
			new Vector3 (335, 30, 74),
			new Vector3 (0, 30, 74),
			new Vector3 (40, 30, 74),
			new Vector3 (80, 30, 74),
			new Vector3 (120, 30, 74),
			new Vector3 (160, 30, 74),
	};
	List<Vector3> trackSideHSV = new List<Vector3> () {
		new Vector3 (197, 30, 60),
			new Vector3 (242, 30, 60),
			new Vector3 (280, 30, 60),
			new Vector3 (335, 30, 60),
			new Vector3 (0, 30, 60),
			new Vector3 (40, 30, 60),
			new Vector3 (80, 30, 60),
			new Vector3 (120, 30, 60),
			new Vector3 (160, 30, 60),
	};
	List<Vector3> backgroundHSV = new List<Vector3> () {
		new Vector3 (197, 17, 74),
			new Vector3 (242, 17, 74),
			new Vector3 (280, 17, 74),
			new Vector3 (335, 17, 74),
			new Vector3 (0, 17, 74),
			new Vector3 (40, 17, 74),
			new Vector3 (80, 17, 74),
			new Vector3 (120, 17, 74),
			new Vector3 (160, 17, 74),
	};
	public List<Color> trackTopColors = new List<Color> { };
	public List<Color> trackSideColors = new List<Color> { };
	public List<Color> backgroundColors = new List<Color> { };
	// Use this for initialization
	void Awake () {
		instance = this;
		trackTopColors.Clear ();
		trackSideColors.Clear ();
		backgroundColors.Clear ();
		for (int i = 0; i < trackTopHSV.Count; i++) {
			trackTopColors.Add (HSVToRGB (trackTopHSV[i]));
			trackSideColors.Add (HSVToRGB (trackSideHSV[i]));
			backgroundColors.Add (HSVToRGB (backgroundHSV[i]));
		}
	}
	// Update is called once per frame
	void Update () { }
	Color HSVToRGB (Vector3 hsv) {
		return (Color.HSVToRGB (hsv.x / 360f, hsv.y / 100f, hsv.z / 100f));
	}
}