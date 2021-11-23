using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarVariables : MonoBehaviour {
    public bool gameOn = false;
    public float distance;
    public float distanceDrifted;
    //Distance Traveled in 1 frame
    public float distanceTraveled;
    public static CarVariables instance;
    public float leftToRight;
    // Use this for initialization
    void Awake () {
        instance = this;
    }
}