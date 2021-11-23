using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InfoPanelController : MonoBehaviour {
    public GameObject userPanel;
    public GameObject settingsPanel;
    public GameObject challengesPanel;
    public bool challengesReady = false;
    public static InfoPanelController instance;
    List<GameObject> panels = new List<GameObject> ();
    float savedChallengePosition;
    float savedUserPosition;
    // Use this for initialization
    void Awake () {
        challengesPanel.SetActive (true);
    }
    void Start () {
        instance = this;
        panels.Add (userPanel);
        panels.Add (settingsPanel);
        panels.Add (challengesPanel);
        savedChallengePosition = challengesPanel.GetComponent<RectTransform> ().localPosition.y;
        savedUserPosition = userPanel.GetComponent<RectTransform> ().localPosition.y;
        userPanel.GetComponent<RectTransform> ().localPosition = new Vector3 (userPanel.GetComponent<RectTransform> ().localPosition.x, 10000, userPanel.GetComponent<RectTransform> ().localPosition.z);
        challengesPanel.GetComponent<RectTransform> ().localPosition = new Vector3 (challengesPanel.GetComponent<RectTransform> ().localPosition.x, savedChallengePosition, challengesPanel.GetComponent<RectTransform> ().localPosition.z);
        userPanel.GetComponent<RectTransform> ().localPosition = new Vector3 (userPanel.GetComponent<RectTransform> ().localPosition.x, savedUserPosition, userPanel.GetComponent<RectTransform> ().localPosition.z);
    }
}