using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RenderTextureResizer : MonoBehaviour {
	public RectTransform playObject;
	void Start () {
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (playObject.rect.width, playObject.rect.width);
	}
}