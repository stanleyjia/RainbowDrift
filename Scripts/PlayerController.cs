using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
    Transform mainCamera;
    public MeshController mc;
    float speed;
    float acceleration = 11f;
    float maxSpeed = 10f;
    private Rigidbody rb;
    // public float leftToRight;
    float elapsed;
    float elapsed1;
    float elapsed2;
    float steeringSensitivity = 8f;
    float drift = 0.9999f;
    float turnAmount;
    float turnSpeed = 50f;
    public Pathmaker pathmaker;
    Vector3 lastPosition;
    public CoinGeneratorController cg;
    public OrbGeneratorController ogc;
    public bool updateCoin = false;
    public bool updateOrb = false;
    public bool start;
    float startCountdown = 1f;
    float tf;
    float pointDistance;
    Vector2 forwardVelocity;
    Quaternion carRotation;
    Vector2 rightVelocity;
    public bool Godmode;
    List<Vector3> lastPassedPoints = new List<Vector3> ();
    public Vector3 closestPoint;
    int index;
    public bool callWarning;
    Texture2D texture;
    bool oneTime3 = true;
    Quaternion quaternion;
    Quaternion angleAxis;
    float angle;
    public DragHandler joystick;
    Vector3 velVector;
    public static PlayerController instance;
    void Start () {
        instance = this;
        speed = 0;
        start = false;
        lastPosition = transform.position;
        CarVariables.instance.gameOn = true;
        DataEntry.instance.oneTime = true;
        System.GC.Collect ();
        mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
    }
    void Update () {
        if (start == false) {
            elapsed += Time.deltaTime;
            if (elapsed >= 0.7f) {
                startCountdown = startCountdown - 1;
                if (startCountdown <= 0) {
                    start = true;
                }
                elapsed = elapsed % 0.7f;
            }
        }
        if (start == true) {
            if (CarVariables.instance.gameOn == true) {
                CarVariables.instance.distanceTraveled = Vector3.Distance (lastPosition, transform.position);
                CarVariables.instance.distance += CarVariables.instance.distanceTraveled;
                if (CarDriftController.instance.drifting) {
                    CarVariables.instance.distanceDrifted += CarVariables.instance.distanceTraveled;
                }
                lastPosition = transform.position;
                if (Godmode == false) {
                    if (transform.position.z > -0.15f) {
                        CarVariables.instance.gameOn = false;
                    }
                }
            }
        }
        if ((transform.position.z > 100f) && (oneTime3 == true)) {
            rb.useGravity = false;
            oneTime3 = false;
            // iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 6);
            //print (rb.velocity);
            StartCoroutine (SlowDownCar ());
            // rb.velocity = new Vector3 (0, 0, 0);
        }
        if (CarVariables.instance.gameOn == true) {
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
                            if (Godmode == false) {
                                CarVariables.instance.gameOn = false;
                            }
                        }
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
    private void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            PauseScene.instance.pauseScene (true);
        }
    }
    IEnumerator SlowDownCar () {
        Vector3 startVel = rb.velocity;
        for (float i = 0; i < 1; i += Time.deltaTime / 2f) {
            rb.velocity = Vector3.Lerp (startVel, Vector3.zero, i);
            yield return null;
        }
        rb.velocity = Vector3.zero;
    }
    void FixedUpdate () {
        rb = GetComponent<Rigidbody> ();
        carRotation = mainCamera.transform.rotation;
        carRotation.eulerAngles = new Vector3 (0, 0, carRotation.eulerAngles.z);
        speed = rb.velocity.magnitude;
        elapsed1 += Time.deltaTime;
        if (start == true) {
            if (CarVariables.instance.gameOn == true) {
                if (speed < maxSpeed) {
                    rb.AddForce (transform.up * acceleration);
                }
                tf = steeringSensitivity;
                turnAmount = joystick.turn * tf;
                CarVariables.instance.leftToRight = (Vector2.Dot (rb.velocity, transform.right) / maxSpeed);
                turnAmount = Mathf.Clamp (turnAmount, -turnSpeed, turnSpeed);
                //print (turnAmount);
                rb.rotation = Quaternion.Lerp (rb.rotation, carRotation * Quaternion.AngleAxis (17 * turnAmount, Vector3.forward), 0.15f);
                forwardVelocity = transform.up * Vector2.Dot (rb.velocity, transform.up);
                rightVelocity = transform.right * Vector2.Dot (rb.velocity, transform.right);
                velVector = forwardVelocity + rightVelocity * drift;
                velVector.z = rb.velocity.z;
                //rb.velocity = velVector;
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
}