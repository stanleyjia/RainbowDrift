using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlyingOrbController : MonoBehaviour {
    List<GameObject> orbs = new List<GameObject> ();
    Vector3 startPosition;
    Vector3 endPosition;
    public GameObject UIOrbCount;
    public GameObject container;
    public GameObject CenterOrbCount;
    Vector3 startScale;
    Vector3 endScale;
    int count = 15;
    public int num;
    public float speed = 4f;
    public float inBetweenTime = 0.2f;
    public static FlyingOrbController instance;
    bool firstTime = true;
    // Use this for initialization
    void Start () {
        instance = this;
        inBetweenTime = 0.2f;
        endPosition = UIOrbCount.GetComponent<RectTransform> ().position;
        for (int i = 0; i < count; i++) {
            GameObject orb = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/FlyingOrb"));
            orb.SetActive (false);
            orb.transform.SetParent (container.transform);
            orbs.Add (orb);
        }
    }
    // Update is called once per frame
    void Update () {
        if (ChallengesController.instance.orbAnimation == true) {
            firstTime = true;
            startScale = CenterOrbCount.GetComponent<RectTransform> ().localScale;
            endScale = UIOrbCount.GetComponent<RectTransform> ().localScale;
            startPosition = CenterOrbCount.GetComponent<RectTransform> ().position;
            num = OrbGeneratorController.instance.orbCount;
            if (OrbGeneratorController.instance.orbCount <= count) {
                num = OrbGeneratorController.instance.orbCount;
                StartCoroutine (Generate (num));
            } else {
                num = count;
                StartCoroutine (Generate (num));
            }
            ChallengesController.instance.orbAnimation = false;
        }
    }
    IEnumerator Generate (int amount) {
        endPosition = UIOrbCount.GetComponent<RectTransform> ().position;
        for (int i = 0; i < amount; i++) {
            PlayAudio.PlaySound ("orbPickup");
            orbs[i].transform.position = startPosition;
            orbs[i].SetActive (true);
            orbs[i].transform.localScale = startScale;
            StartCoroutine (FlyTo (orbs[i]));
            yield return new WaitForSeconds (inBetweenTime);
        }
    }
    IEnumerator FlyTo (GameObject orb) {
        for (float i = 0; i <= 1; i += Time.deltaTime / speed) {
            orb.transform.position = Vector3.Lerp (startPosition, endPosition, i);
            orb.transform.localScale = Vector3.Lerp (startScale, endScale, i);
            yield return null;
        }
        orb.gameObject.SetActive (false);
        if (firstTime == true) {
            firstTime = false;
            GameSceneOrbController.UpdateValue ();
        }
        if (orbs.IndexOf (orb) == num - 1) {
            yield return new WaitForSeconds (0.3f);
            GameOverController.ready1 = true;
        }
    }
}