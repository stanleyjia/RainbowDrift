using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIController : MonoBehaviour {
    public Transform car;
    public TutorialCar carController;
    public Pathmaker pathmaker;
    List<Vector3> points = new List<Vector3> ();
    public static AIController instance;
    public bool AIControllerOn = false;
    Rigidbody rb;
    float pointDistance;
    int index;
    Vector3 norm;
    float maxSpeed;
    public Vector3 idealVelocity;
    public Vector3 angleVelocity;
    float currentAngle;
    float clampedAngle;
    bool firstTime = true;
    Vector3 trackDirection;
    int closePointIndex;
    Vector3 farPoint;
    // Use this for initialization
    void Start () {
        instance = this;
        maxSpeed = carController.maxSpeed;
        rb = car.GetComponent<Rigidbody> ();
        rb.velocity = new Vector3 (0, 10, rb.velocity.z);
    }
    // Update is called once per frame
    void Update () {
        points = pathmaker.totalPoints;
    }
    private void FixedUpdate () {
        if (points.Count > 1) {
            trackDirection = TrackDirection ();
            if (firstTime) {
                idealVelocity = trackDirection * maxSpeed;
                idealVelocity = new Vector3 (idealVelocity.x, idealVelocity.y, rb.velocity.z);
                rb.velocity = idealVelocity;
                firstTime = false;
                TutorialController.instance.start = true;
                AIControllerOn = true;
            }
            if ((CarVariables.instance.gameOn == true) && (TutorialController.instance.start)) {
                if (AIControllerOn == true) {
                    idealVelocity = trackDirection * maxSpeed;
                    idealVelocity = new Vector3 (idealVelocity.x, idealVelocity.y, rb.velocity.z);
                    if (rb.velocity.magnitude > 3f) {
                        car.position = Vector3.Lerp (car.position, points[closePointIndex], 0.1f);
                        idealVelocity = (Vector3.Normalize (farPoint - car.position) * 1f) + trackDirection;
                        idealVelocity = Vector3.Normalize (idealVelocity) * maxSpeed;
                        idealVelocity = new Vector3 (idealVelocity.x, idealVelocity.y, rb.velocity.z);
                        if (rb.velocity.magnitude < maxSpeed) {
                            //rb.AddForce (transform.up * 5f);
                        }
                        //rb.velocity = idealVelocity;
                        // print (car.transform.up);
                        currentAngle = Vector2.SignedAngle (angleVelocity.normalized, car.transform.up);
                        //print (currentAngle);
                        //clampedAngle = Mathf.Clamp (currentAngle, -10f, 10f);
                        //print (rb.rotation.eulerAngles);
                        //print ("Changing by " + currentAngle);
                        // rb.rotation = Quaternion.Euler (rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, Mathf.LerpAngle (rb.rotation.eulerAngles.z, rb.rotation.eulerAngles.z - (currentAngle * 1.5f), 0.9f));
                        if (rb.angularVelocity.magnitude < 0.8f) {
                            rb.AddTorque (0, 0, -currentAngle * 0.1f);
                        } else {
                            rb.AddTorque (0, 0, -currentAngle / 20f);
                        }
                        if (Mathf.Abs (currentAngle) < 0.2f) {
                            if (rb.angularVelocity.magnitude < 3f) {
                                rb.angularVelocity = new Vector3 (rb.angularVelocity.x / 2f, rb.angularVelocity.y / 2f, rb.angularVelocity.z / 2f);
                            }
                        }
                        //print (rb.angularVelocity.magnitude);
                        //print (rb.rotation.eulerAngles);
                        //rb.rotation = Quaternion.Euler (0, 0, currentAngle);
                    } else {
                        rb.velocity = new Vector3 (0, 10, rb.velocity.z);
                    }
                }
            }
        }
    }
    public Vector3 TrackDirection (int ahead = 40) {
        closePointIndex = FindClosestPoint (points, car.position);
        farPoint = points[closePointIndex + ahead];
        norm = Vector3.Normalize (farPoint - points[closePointIndex]);
        angleVelocity = points[closePointIndex + 50] - points[closePointIndex + 30];
        return norm;
    }
    int FindClosestPoint (List<Vector3> pos, Vector3 po) {
        pointDistance = 2f;
        index = 0;
        for (int i = 0; i < pos.Count - 1; i++) {
            if (Vector3.Distance (pos[i], po) < pointDistance) {
                pointDistance = Vector3.Distance (pos[i], po);
                index = i;
            }
        }
        return (index);
    }
    int FindClosestPointFromFar (List<Vector3> pos, Vector3 po) {
        pointDistance = 200f;
        index = 0;
        for (int i = 0; i < pos.Count - 1; i++) {
            if (Vector3.Distance (pos[i], po) < pointDistance) {
                pointDistance = Vector3.Distance (pos[i], po);
                index = i;
            }
        }
        return (index);
    }
}