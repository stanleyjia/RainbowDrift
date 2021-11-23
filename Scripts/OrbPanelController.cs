using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OrbPanelController : MonoBehaviour {
    public static OrbPanelController instance;
    //  Image image;
    public GameObject Header;
    public Transform OrbLabels;
    public GameObject UIOrbs;
    public GameObject OrbCount;
    public GameObject Congrats;
    public GameObject Collected;
    public float fadeInTime = 0.2f;
    float fadeOutTime = 0.4f;
    List<GameObject> objects = new List<GameObject> ();
    void Start () {
        //image = GetComponent<Image>();
        OrbLabels = transform.Find ("OrbLabels");
        UIOrbs = OrbLabels.Find ("UIOrbs").gameObject;
        OrbCount = OrbLabels.Find ("OrbCount").gameObject;
        Congrats = transform.Find ("Header").Find ("Congrats").gameObject;
        Collected = transform.Find ("Collected").gameObject;
        Header = transform.Find ("Header").gameObject;
        objects.Add (gameObject);
        objects.Add (Header);
        objects.Add (UIOrbs);
        objects.Add (OrbCount);
        objects.Add (Congrats);
        objects.Add (Collected);
        instance = this;
        /*for (int i = 0; i < objects.Count; i++) {
            SetZeroAlpha (objects[i]);
        }*/
    }
    void SetZeroAlpha (GameObject gameOb) {
        if (gameOb.GetComponent<Text> () != null) {
            gameOb.GetComponent<Text> ().color = new Color (gameOb.GetComponent<Text> ().color.r, gameOb.GetComponent<Text> ().color.g, gameOb.GetComponent<Text> ().color.b, 0);
        } else if (gameOb.GetComponent<Image> () != null) {
            gameOb.GetComponent<Image> ().color = new Color (gameOb.GetComponent<Image> ().color.r, gameOb.GetComponent<Image> ().color.g, gameOb.GetComponent<Image> ().color.b, 0);
        }
    }
    public void Activate () {
        OrbCount.GetComponent<Text> ().text = OrbGeneratorController.instance.orbCount.ToString ();
        //Wait until orb animation starts
        // StartCoroutine(SetReady());
        /*for (int i = 0; i < objects.Count; i++) {
            StartCoroutine (FadeIn (objects[i], 1f, fadeInTime));
        }*/
    }
    IEnumerator SetInactive () {
        yield return new WaitForSeconds (fadeOutTime);
        //show challenges
        GameOverController.readyToTransition1 = true;
        yield return new WaitForSeconds (0.1f);
        //deactivate this gameobject so can click challenges
        gameObject.SetActive (false);
    }
    public void Deactivate () {
        StartCoroutine (SetInactive ());
        for (int i = 0; i < objects.Count; i++) {
            StartCoroutine (FadeIn (objects[i], 0f, fadeOutTime));
        }
    }
    void Update () {
        /* if (ready)
         {
             OrbAnimate();
             ready = false;
         }*/
    }
    /* void OrbAnimate()
     {
         PlayerController.instance.updateOrb = true;
         ChallengesController.instance.orbAnimation = true;
     }*/
    IEnumerator FadeIn (GameObject obj, float alpha, float time) {
        if (obj.GetComponent<Text> () != null) {
            Text img = obj.GetComponent<Text> ();
            float startAlpha = img.color.a;
            for (float i = 0; i <= 1; i += Time.deltaTime / time) {
                img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, alpha, i));
                yield return null;
            }
            img.color = new Color (img.color.r, img.color.g, img.color.b, alpha);
        } else if (obj.GetComponent<Image> () != null) {
            Image img = obj.GetComponent<Image> ();
            float startAlpha = img.color.a;
            // yield return new WaitForSeconds(1f);
            for (float i = 0; i <= 1; i += Time.deltaTime / time) {
                img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (startAlpha, alpha, i));
                yield return null;
            }
            img.color = new Color (img.color.r, img.color.g, img.color.b, alpha);
        }
    }
}