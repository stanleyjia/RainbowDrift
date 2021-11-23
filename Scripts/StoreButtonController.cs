using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StoreButtonController : MonoBehaviour {
    UnityEngine.UI.Image image;
    Color storeOne = new Color (1, 1, 1, 0.6f);
    Color storeTwo = new Color (1, 1, 1, 0.9f);
    float float1 = 0.6f;
    float float2 = 0.9f;
    int goingUp = 0;
    float time = 1;
    void Start () {
        image = GetComponent<UnityEngine.UI.Image> ();
        storeOne = new Color (image.color.r, image.color.g, image.color.b, float1);
        storeTwo = new Color (image.color.r, image.color.g, image.color.b, float2);
        if (SceneManager.GetActiveScene ().name == "CombinedScene") {
            // image.color = storeTwo;
            StartCoroutine (Blink (storeOne, storeTwo));
        }
    }
    IEnumerator Blink (Color first, Color second) {
        while (true) {
            if (goingUp % 2 == 0) {
                if (time < 1) {
                    time += Time.deltaTime * 2f;
                    image.color = Color.Lerp (first, second, time);
                    yield return null;
                } else {
                    goingUp++;
                    time = 0;
                    yield return new WaitForSeconds (1f);
                }
            } else {
                if (time < 1) {
                    time += Time.deltaTime * 2f;
                    image.color = Color.Lerp (second, first, time);
                    yield return null;
                } else {
                    goingUp++;
                    time = 0;
                    yield return null;
                }
            }
        }
    }
}