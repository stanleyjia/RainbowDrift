using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbGeneratorController : MonoBehaviour {
    public Pathmaker pathmaker;
    public MeshController meshController;
    public CoinGeneratorController coinController;
    public static OrbGeneratorController instance;
    float distance;
    List<GameObject> orbs = new List<GameObject> ();
    float pointDistance;
    int indexs;
    public int orbCount = 0;
    public int rainbowCycles;
    //For Trail change
    public bool hitOrb = false;
    //For color ring
    public bool hitOrb2 = false;
    int lastOrbCount;
    public bool noneLost = true;
    Vector3 scale = new Vector3 (0.3f, 0.3f, 0.3f);
    Vector3 zero = new Vector3 (0, 0, 0);
    //Out of 1 hundred
    private float chanceOfOrb = 2f;
    void Start () {
        instance = this;
        orbCount = 0;
        for (int i = 0; i <= 15; i++) {
            GameObject orb;
            orb = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/ORB"));
            orbs.Add (orb);
            orb.SetActive (false);
            orb.transform.parent = this.transform;
        }
        distance = Random.Range (5, 10);
    }
    void Update () {
        if (pathmaker.generate4 == true) {
            //  //print (coinController.orbPoints.Count);
            if (pathmaker.checkpoint > 5) {
                if (ShouldGenerateOrb ()) {
                    GenerateOrb (coinController.orbPoints[RandomIndex (coinController.orbPoints.Count - 1)]);
                }
            }
            coinController.orbPoints.Clear ();
            pathmaker.generate4 = false;
        }
    }
    void NewDistance () {
        distance = Random.Range (15 - pathmaker.checkpoint / 1f, 20 - pathmaker.checkpoint / 1f);
        distance = Mathf.Clamp (distance, 6.5f, 20);
    }
    int RandomIndex (int limit) {
        return (Random.Range (0, limit));
    }
    bool ShouldGenerateOrb () {
        if (Random.Range (0, 100) < chanceOfOrb) {
            return true;
        } else {
            return false;
        }
    }
    void GenerateOrb (Vector3 position) {
        for (int i = 0; i < orbs.Count - 1; i++) {
            if (orbs[i].gameObject.activeInHierarchy == false) {
                orbs[i].SetActive (true);
                orbs[i].transform.position = new Vector3 (position.x, position.y, 0);
                orbs[i].transform.eulerAngles = zero;
                orbs[i].transform.localScale = scale;
                break;
            }
            if (i == orbs.Count - 1) {
                MakeMoreOrbs ();
            }
        }
    }
    int FindClosestPoint (List<Vector3> pois, Vector3 poi) {
        pointDistance = 1.4f;
        indexs = 0;
        for (int i = 0; i < pois.Count - 1; i++) {
            if (Vector3.Distance (pois[i], poi) < pointDistance) {
                pointDistance = Vector3.Distance (pois[i], poi);
                indexs = i;
            }
        }
        return (indexs);
    }
    void MakeMoreOrbs () {
        for (int i = 0; i <= 5; i++) {
            GameObject orb;
            orb = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/ORB"));
            orbs.Add (orb);
            orb.SetActive (false);
            orb.transform.parent = this.transform;
        }
    }
}