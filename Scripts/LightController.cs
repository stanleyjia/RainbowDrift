using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightController : MonoBehaviour {
    GameObject theCar;
    Vector3 position;
    float MaximumDistance = 5.1f;
    float mult;
    Vector3 Diff;
    public static LightController instance;
    // Use this for initialization
    void Start () {
        instance = this;
        theCar = GameObject.FindWithTag ("Player");
        transform.position = new Vector3 (theCar.transform.position.x, theCar.transform.position.y - 4f, -6f);
    }
    // Update is called once per frame
    void Update () {
        Diff = theCar.transform.position - new Vector3 (transform.position.x, transform.position.y, theCar.transform.position.z);
        if (Diff.magnitude > MaximumDistance) {
            mult = MaximumDistance / Diff.magnitude;
            transform.position += Diff;
            transform.position -= Diff * mult;
        }
        // transform.position = new Vector3 (theCar.transform.position.x, theCar.transform.position.y, )
    }
    public void UpdatePosition () {
        Diff = theCar.transform.position - new Vector3 (transform.position.x, transform.position.y, theCar.transform.position.z);
        if (Diff.magnitude > MaximumDistance) {
            mult = MaximumDistance / Diff.magnitude;
            transform.position += Diff;
            transform.position -= Diff * mult;
        }
    }
}