using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialCar : MonoBehaviour {
    public MeshController mc;
    float speed;
    public float maxSpeed = 9;
    private Rigidbody rb;
    Quaternion carRotation;
    float steeringSensitivity = 7f;
    float drift = 0.999f;
    float turnAmount;
    public float turnSpeed = 9f;
    public Pathmaker pathmaker;
    Vector3 lastPosition;
    public CoinGeneratorController cg;
    public bool updateCoin = false;
    public bool updateOrb = false;
    float tf;
    float pointDistance;
    Vector2 forwardVelocity;
    Vector2 rightVelocity;
    public bool Godmode;
    List<Vector3> lastPassedPoints = new List<Vector3> ();
    Vector3 closestPoint;
    int index;
    public bool callWarning;
    bool oneTime3 = true;
    public DragHandler joystick;
    Vector3 velVector;
    public static TutorialCar instance;
    Transform mainCamera;
    void Start () {
        instance = this;
        CarVariables.instance.leftToRight = 0;
        speed = 0;
        lastPosition = transform.position;
        CarVariables.instance.gameOn = true;
        DataEntry.instance.oneTime = true;
        System.GC.Collect ();
        mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
    }
    void Update () {
        if ((CarVariables.instance.gameOn == true) && (TutorialController.instance.start)) {
            CarVariables.instance.distanceTraveled = Vector3.Distance (lastPosition, transform.position);
            CarVariables.instance.distance += CarVariables.instance.distanceTraveled;
            lastPosition = transform.position;
            if (transform.position.z > -0.15f) {
                CarVariables.instance.gameOn = false;
                if (AIController.instance.AIControllerOn == true) {
                    TutorialController.instance.TryAgain ();
                } else {
                    TutorialController.instance.EndTutorial ();
                }
            }
        }
        if ((transform.position.z > 100f) && (oneTime3 == true)) {
            rb.useGravity = false;
            oneTime3 = false;
            StartCoroutine (SlowDownCar ());
        }
        if (TutorialController.instance.start == true) {
            closestPoint = FindClosestPoint (pathmaker.points, transform.position);
            if (lastPassedPoints.Contains (closestPoint) == true) {
                //Old territory
                if (lastPassedPoints.Count > 3) {
                    index = lastPassedPoints.IndexOf (closestPoint);
                    if (index <= 2) {
                        //Going back
                        callWarning = true;
                        if (index == 1) {
                            callWarning = true;
                            CarVariables.instance.gameOn = false;
                        } else {
                            callWarning = false;
                        }
                    }
                } else {
                    //New territory
                    callWarning = false;
                    lastPassedPoints.Add (closestPoint);
                    if (lastPassedPoints.Count > 4) {
                        lastPassedPoints.RemoveAt (0);
                    }
                }
            }
        }
    }
    void FixedUpdate () {
        rb = GetComponent<Rigidbody> ();
        speed = rb.velocity.magnitude;
        if (TutorialController.instance.start == true) {
            if (CarVariables.instance.gameOn == true) {
                CarVariables.instance.leftToRight = (Vector2.Dot (rb.velocity, transform.right) / maxSpeed);
                if (AIController.instance.AIControllerOn == false) {
                    //rb.drag = 0.5f;
                    if (speed < maxSpeed) {
                        rb.AddForce (transform.up * 11f);
                    }
                    carRotation = mainCamera.transform.rotation;
                    carRotation.eulerAngles = new Vector3 (0, 0, carRotation.eulerAngles.z);
                    tf = steeringSensitivity;
                    turnAmount = joystick.turn * tf;
                    turnAmount = Mathf.Clamp (turnAmount, -turnSpeed, turnSpeed);
                    //print (joystick.turn);
                    rb.rotation = Quaternion.Lerp (rb.rotation, carRotation * Quaternion.AngleAxis (17 * turnAmount, Vector3.forward), 0.15f);
                    forwardVelocity = transform.up * Vector2.Dot (rb.velocity, transform.up);
                    rightVelocity = transform.right * Vector2.Dot (rb.velocity, transform.right);
                    velVector = forwardVelocity + rightVelocity * drift;
                    velVector.z = rb.velocity.z;
                }
            }
        }
    }
    Vector3 FindClosestPoint (List<Vector3> points, Vector3 point) {
        pointDistance = 6f;
        for (int i = 0; i < points.Count - 1; i++) {
            if (Vector3.Distance (points[i], point) < pointDistance) {
                pointDistance = Vector3.Distance (points[i], point);
                closestPoint = points[i];
            }
        }
        return (closestPoint);
    }
    IEnumerator SlowDownCar () {
        Vector3 startVel = rb.velocity;
        for (float i = 0; i < 1; i += Time.deltaTime / 2f) {
            rb.velocity = Vector3.Lerp (startVel, Vector3.zero, i);
            yield return null;
        }
        rb.velocity = Vector3.zero;
    }
}