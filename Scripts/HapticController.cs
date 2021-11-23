using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HapticController : MonoBehaviour {
	//0 Selection Change
	//1 Light Impact
	//2 Medium Impact
	//3 Heavy Impact
	//4 Notification Success
	//5 Notification Warning
	//6 Notification Failure
	public void Trigger (int id) {
		if (DataEntry.instance.hapticOn) {
			iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) id);
		}
	}
}