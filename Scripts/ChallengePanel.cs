using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChallengePanel : MonoBehaviour {
    float currentAlpha;
    Image image;
    //under header
    public Text level;
    public Text chalName;
    public Text desc;
    public Image checkbox;
    public Image checkmark;
    public Text coinReward;
    public Image coinRewardImage;
    public static Vector3 scale = new Vector3 (1.3f, 1.3f, 1.3f);
    float fadeOutTime = 0.4f;
    void OnAwake () {
        image = GetComponent<Image> ();
        scale = coinRewardImage.GetComponent<RectTransform> ().localScale;
    }
    public void CompleteChallenge (GameObject obj) {
        //   ChallengesCompletedController.instance.ChallengesShine ();
        int displayIndex = obj.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().displayIndex;
        if (ChallengesController.instance.lastFinished == displayIndex) {
            ChallengesController.instance.lastFinished++;
            int index = obj.transform.Find ("ChallengeID").GetComponent<ChallengeNumber> ().challengeIndex;
            PlayAudio.PlaySound ("click");
            PlayAudio.PlaySound ("buy");
            if (DataEntry.instance.hapticOn) {
                iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 2);
            }
            ChallengesController.AddCheck (index);
        } else {
            //put error sound here
        }
    }
    public void FadeOut () {
        if (gameObject.activeSelf == true) {
            transform.Find ("Checkbox").Find ("Checkmark").Find ("UIParticle").gameObject.SetActive (false);
            StartCoroutine (FadeChildText (level));
            StartCoroutine (FadeChildText (chalName));
            StartCoroutine (FadeChildText (desc));
            StartCoroutine (FadeChildText (coinReward));
            StartCoroutine (FadeChildImage (checkbox));
            StartCoroutine (FadeChildImage (checkmark));
            StartCoroutine (FadeChildImage (coinRewardImage));
        }
    }
    IEnumerator FadeOutFunction () {
        image = GetComponent<Image> ();
        currentAlpha = image.color.a;
        // //print (currentAlpha);
        for (float i = 0; i <= 1f; i += Time.deltaTime / fadeOutTime) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.Lerp (currentAlpha, 0, i));
            yield return null;
        }
        //  yield return new WaitForSeconds (fadeOutTime);
        image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
        if (ChallengesCompletedController.instance.disappear == false) {
            ChallengesCompletedController.instance.disappear = true;
        }
    }
    IEnumerator FadeChildText (Text txt) {
        currentAlpha = txt.color.a;
        for (float i = 0; i <= 1f; i += Time.deltaTime / fadeOutTime) {
            txt.color = new Color (txt.color.r, txt.color.g, txt.color.b, Mathf.Lerp (currentAlpha, 0, i));
            yield return null;
        }
        txt.color = new Color (txt.color.r, txt.color.g, txt.color.b, 0);
    }
    IEnumerator FadeChildImage (Image img) {
        //print (img.name);
        currentAlpha = img.color.a;
        for (float i = 0; i <= 1f; i += Time.deltaTime / fadeOutTime) {
            img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Lerp (currentAlpha, 0, i));
            yield return null;
        }
        img.color = new Color (img.color.r, img.color.g, img.color.b, 0);
    }
}