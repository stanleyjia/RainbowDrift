using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PointerScript : MonoBehaviour {
    float moveAmount;
    int direction = 0;
    float yValue;
    float zValue;
    float startValue;
    float endValue;
    //0 is going to Right;
    RectTransform rect;
    // Use this for initialization
    void Start () {
    }
    // Update is called once per frame
    void Update () {
        //print(Time.unscaledDeltaTime);
    }
    void Awake () {
        rect = GetComponent<RectTransform> ();
        yValue = rect.localPosition.y;
        zValue = rect.localPosition.z;
        moveAmount = (Screen.width / 2f) * 0.5f;
        //print(Screen.width);
        StartCoroutine (Move ());
    }
    IEnumerator Move () {
        //`//print("started");
        while (true) {
            if (direction == 0) {
                endValue = moveAmount;
            } else if (direction == 1) {
                endValue = 0;
            } else if (direction == 2) {
                endValue = -moveAmount;
            } else if (direction == 3) {
                endValue = 0;
            }
            startValue = rect.localPosition.x;
            for (float i = 0f; i <= 1f; i += Time.unscaledDeltaTime / 0.5f) {
                rect.localPosition = new Vector3 (Mathf.Lerp (startValue, endValue, i), yValue, zValue);
                yield return null;
            }
            rect.localPosition = new Vector3 (endValue, yValue, zValue);
            ////print(rect.position.x);
            direction++;
            if (direction == 4) {
                direction = 0;
            }
        }
    }
}