using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProfileTextBlink : MonoBehaviour {
    Image image;
    Color infoOne = new Color (0, 0, 0, 0.15f);
    Color infoTwo = new Color (0, 0, 0, 0.3f);
    int goingUp = 0;
    float time = 1;
    void Start () {
        image = GetComponent<UnityEngine.UI.Image> ();
        image.color = infoTwo;
        StartCoroutine (Blink (infoOne, infoTwo));
    }
    IEnumerator Blink (Color first, Color second) {
        while (true)
        {
            if (goingUp % 2 == 0) {
                if (time < 1f) {
                    time += Time.deltaTime * 1f;
                    image.color = Color.Lerp (first, second, time);
                    yield return null;
                } else {
                    goingUp++;
                    time = 0;
                    yield return new WaitForSeconds (0.4f);
                }
            } else {
                if (time < 1) {
                    time += Time.deltaTime * 1f;
                    image.color = Color.Lerp (second, first, time);
                    yield return null;
                } else {
                    goingUp++;
                    time = 0;
                    yield return new WaitForSeconds (0.4f);
                }
            }
        }
    }
}