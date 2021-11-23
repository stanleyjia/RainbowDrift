using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BorderFadeController : MonoBehaviour {
    UnityEngine.UI.Image image;
    bool firstTime = true;
    bool gameOn;
    // Use this for initialization
    void Start () {
        image = GetComponent<UnityEngine.UI.Image> ();
    }
    // Update is called once per frame
    void Update () {
        if ((CarVariables.instance.gameOn == false) && (firstTime == true)) {
            StartCoroutine (FadeImage ());
            firstTime = false;
        }
    }
    IEnumerator FadeImage () {
        yield return new WaitForSeconds (0.2f);
        for (float i = image.color.a; i >= 0; i -= Time.deltaTime / 2f) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }
        image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
    }
}