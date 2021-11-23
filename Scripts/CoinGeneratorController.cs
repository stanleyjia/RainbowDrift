using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinGeneratorController : MonoBehaviour {
    public Pathmaker pathmaker;
    List<Vector3> points = new List<Vector3> ();
    List<GameObject> coins = new List<GameObject> ();
    public List<Vector3> coinPositions = new List<Vector3> ();
    public int coinCount;
    int amount;
    float spacing = 2.5f;
    float distance;
    float orbDistance;
    Vector3 lastCoin;
    Vector3 point;
    float norm;
    int index;
    int count;
    float random;
    Vector3 vect;
    Vector3 direction;
    Vector3 perpVector;
    float pointDistance;
    int indexs;
    int value;
    int[] randomList = new int[2];
    int minAmount = 4;
    public bool noneLost = true;
    public static CoinGeneratorController instance;
    public List<Vector3> orbPoints = new List<Vector3> ();
    Vector3 lastOrb = new Vector3 (0, 0, 0);
    Vector3 scale = new Vector3 (1f, 1f, 1f);
    Vector3 zero = new Vector3 (0, 0, 0);
    bool coinGenerate = false;
    void Start () {
        instance = this;
        coinCount = 0;
        points = pathmaker.totalPoints;
        amount = Random.Range (5, 8);
        distance = Random.Range (10, 20);
        norm = Random.Range (-0.700f, 0.700f);
        point = new Vector3 (0, 0, 0);
        for (int i = 0; i < 120; i++) {
            GameObject coin = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/USD"));
            coin.SetActive (false);
            coins.Add (coin);
            coin.transform.parent = this.transform;
            coin.transform.localScale = scale;
            coin.transform.eulerAngles = zero;
        }
        NewDistance ();
    }
    void NewDistance () {
        orbDistance = Random.Range (20 - (pathmaker.checkpoint / 1.5f), 30 - (pathmaker.checkpoint / 1.5f));
        orbDistance = Mathf.Clamp (orbDistance, 7f, 40);
    }
    void Update () {
        if (pathmaker.generate3 == true) {
            points = pathmaker.totalPoints;
            index = FindClosestPoint (points, point);
            for (int i = 0; i < points.Count - 1; i = i + 3) {
                if (i > index) {
                    if (coinGenerate == false) {
                        if (Vector3.Distance (points[i], lastOrb) > orbDistance) {
                            if (Vector3.Distance (points[i], lastCoin) > spacing * 2.5f) {
                                random = Random.Range (0.0f, 1.0f);
                                if (random > 0.5f) {
                                    norm += amount * 0.2f;
                                } else {
                                    norm -= amount * 0.2f;
                                }
                                norm = Mathf.Clamp (norm, -0.7f, 0.7f);
                                vect = points[i + 1] - points[i];
                                direction = vect.normalized;
                                perpVector = new Vector3 (-direction.y, direction.x, 0.0f);
                                //((Vector3.Distance(points[i], lastOrb)));
                                lastOrb = points[i];
                                orbPoints.Add (points[i] + (perpVector * norm));
                                NewDistance ();
                            }
                        }
                        if (Vector3.Distance (points[i], lastCoin) > distance) {
                            //Starts adding coins
                            //Makes sure not only single coin spawned
                            if (Vector3.Distance (points[points.Count - 1], points[i]) > minAmount * spacing) {
                                count = 0;
                                coinGenerate = true;
                                amount = Randomize (pathmaker.checkpoint) [0];
                                distance = Randomize (pathmaker.checkpoint) [1];
                                norm = Random.Range (-0.700f, 0.700f);
                            }
                        }
                    } else {
                        if (Vector3.Distance (points[i], lastOrb) > spacing * 2.5f) {
                            if (Vector3.Distance (points[i], lastCoin) > spacing) {
                                if (count < amount) {
                                    random = Random.Range (0.0f, 1.0f);
                                    if (random > 0.5f) {
                                        norm += amount * 0.03f;
                                    } else {
                                        norm -= amount * 0.03f;
                                    }
                                    norm = Mathf.Clamp (norm, -0.7f, 0.7f);
                                    count++;
                                    GenerateCoin (points[i], points[i + 1], norm);
                                    lastCoin = points[i];
                                    point = points[i];
                                    index = i;
                                } else {
                                    coinGenerate = false;
                                    //  break;
                                }
                            }
                        }
                    }
                    pathmaker.generate3 = false;
                    pathmaker.generate4 = true;
                }
            }
        }
    }
    int[] Randomize (int difficulty) {
        //Amount
        value = Random.Range (minAmount + difficulty / 2, 6 + difficulty / 2);
        value = Mathf.Clamp (value, 3, 12);
        randomList[0] = (Mathf.RoundToInt (value));
        //Distance
        value = Random.Range (13 - difficulty / 3, 16 - difficulty / 3);
        value = Mathf.Clamp (value, 7, 16);
        randomList[1] = (Mathf.RoundToInt (value));
        return randomList;
    }
    void GenerateCoin (Vector3 position, Vector3 nextPosition, float nor) {
        vect = nextPosition - position;
        direction = vect.normalized;
        perpVector = new Vector3 (-direction.y, direction.x, 0.0f);
        for (int i = 0; i < coins.Count; i++) {
            if (coins[i].activeInHierarchy == false) {
                coins[i].SetActive (true);
                coins[i].transform.position = position + (perpVector * nor);
                coins[i].transform.localScale = scale;
                coinPositions.Add (coins[i].transform.position);
                break;
            }
            if (i == coins.Count - 1) {
                MakeMoreCoins ();
            }
        }
    }
    int FindClosestPoint (List<Vector3> pos, Vector3 po) {
        pointDistance = 2f;
        indexs = 0;
        for (int i = 0; i < pos.Count - 1; i++) {
            if (Vector3.Distance (pos[i], po) < pointDistance) {
                pointDistance = Vector3.Distance (pos[i], po);
                indexs = i;
            }
        }
        return (indexs);
    }
    void MakeMoreCoins () {
        for (int i = 0; i < 5; i++) {
            GameObject coin = (GameObject) Instantiate (Resources.Load ("Prefab/Objects/USD"));
            coin.SetActive (false);
            coin.transform.parent = this.transform;
            coin.transform.localScale = scale;
            //coin.transform.rotation = Quaternion.LookRotation(Vector3.forward, vect);
            coins.Add (coin);
        }
    }
}