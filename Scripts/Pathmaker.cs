using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pathmaker : MonoBehaviour {
    public MeshController mc;
    public List<Vector3> points = new List<Vector3> ();
    public List<Vector3> totalPoints = new List<Vector3> ();
    //List with less points
    public List<Vector3> colliderPoints = new List<Vector3> ();
    HashSet<Vector3> totColliderPoints = new HashSet<Vector3> ();
    //Faster to check for points
    HashSet<Vector3> totPoints = new HashSet<Vector3> ();
    Vector3 lastGeneratedPoint;
    public Vector3 checkpointLastPoint;
    float maxY;
    float minY;
    public bool generate1 = true;
    public bool generate2 = true;
    public bool generate3 = true;
    public bool generate4 = true;
    public bool generate5 = true;
    Rigidbody player;
    public Vector3 checkpointLocation = new Vector3 (100f, 100f, 100f);
    bool checkpointReached;
    bool checkpointReached1;
    bool checkpointReached2;
    public int checkpoint;
    float angle;
    float DistanceToNextPoint;
    float randomAngle;
    float finalAngle;
    float logValue = 0;
    Vector3 lastPoint;
    Vector3 threePointsAgo;
    //Vector3 twoPointsAgo;
    Vector3 lastDirection;
    Vector3 vect;
    Vector3 newPoint;
    int loops;
    int colliderLoops;
    float t;
    Vector3 newPos;
    Vector3 a, b, c, d;
    Vector3 pos;
    float pointDistance;
    int indexs;
    int index;
    bool CR_Running;
    float resolution = 0.025f;
    float colliderResolution = 0.5f;
    bool oneTime = true;
    public bool debug;
    public bool removeObjects;
    public bool removeOrbs;
    WaitForSeconds wait = new WaitForSeconds (3);
    int chunksInOneCheckpoint = 20;
    int checkpointIndex = 19;
    int indexOf;
    bool tutorial = false;
    int checkpointStartDelete = 1;
    public static Pathmaker instance;
    int startingCheckpoint = 1;
    void Awake () {
        instance = this;
        if (SceneManager.GetActiveScene ().name == "TutorialScene") {
            tutorial = true;
            checkpointStartDelete = 2;
        }
        player = GameObject.FindWithTag ("Player").GetComponent<Rigidbody> ();
        checkpoint = startingCheckpoint;
        Gizmos.color = Color.white;
        maxY = 0f;
        loops = Mathf.FloorToInt (1f / resolution);
        colliderLoops = Mathf.FloorToInt (1f / colliderResolution);
        points.Add (new Vector3 (0f, -30f, 0));
        points.Add (new Vector3 (0f, -25f, 0));
        points.Add (new Vector3 (0f, -20f, 0));
        points.Add (new Vector3 (0f, -15f, 0));
        points.Add (new Vector3 (0f, -10f, 0));
        points.Add (new Vector3 (0f, -5f, 0));
        points.Add (new Vector3 (0f, -0f, 0));
        points.Add (new Vector3 (0f, 5f, 0));
        points.Add (new Vector3 (0f, 10f, 0));
        checkpointLastPoint = new Vector3 (0f, 10f, 0);
        GenerateTenPoints ();
        generate1 = true;
        generate2 = true;
        generate3 = true;
        generate5 = true;
        for (int i = 3; i <= points.Count - 1; i++) {
            DrawCurve (points[i - 3], points[i - 2], points[i - 1], points[i]);
            DrawColliderPoints (points[i - 3], points[i - 2], points[i - 1], points[i]);
        }
        lastGeneratedPoint = points[points.Count - 1];
    }
    void Update () {
        if (checkpointReached2 == true) {
            if (mc.removePoints == true) {
                checkpointReached2 = false;
                mc.removePoints = false;
                if (checkpoint > checkpointStartDelete) {
                    for (int i = points.Count - (chunksInOneCheckpoint * 3); i >= 0; i--) {
                        points.Remove (points[i]);
                    }
                    //print ("POINTS REMOVED");
                    StartCoroutine (RemoveCurve (points[0], points[1], points[2], points[3]));
                }
            }
        }
        if (Vector3.Distance (player.transform.position, checkpointLocation) < 5f) {
            if (checkpointReached == false) {
                checkpointReached = true;
                checkpointReached1 = true;
                checkpointReached2 = true;
            }
        }
        if (checkpointReached1 == true) {
            //True until coroutine is over
            if (oneTime == true) {
                GenerateTenPoints (checkpoint);
                oneTime = false;
                index = FindClosestPoint (points, lastGeneratedPoint);
                StartCoroutine (DrawOverTime (index, points));
            }
            if (CR_Running == false) {
                lastGeneratedPoint = points[points.Count - 1];
                generate1 = true;
                generate2 = true;
                generate3 = true;
                generate5 = true;
                oneTime = true;
                checkpointReached = false;
                checkpointReached1 = false;
            }
        }
        if (debug == true) {
            for (int w = 1; w < points.Count - 1; w++) {
                Debug.DrawLine (points[w - 1], points[w], Color.red);
            }
            for (int n = 1; n < totalPoints.Count - 1; n++) {
                Debug.DrawLine (totalPoints[n - 1], totalPoints[n], Color.green);
            }
        }
    }
    IEnumerator DrawOverTime (int ind, List<Vector3> po) {
        CR_Running = true;
        if (ind >= 3) {
            for (int i = ind; i <= po.Count - 1; i++) {
                DrawCurve (po[i - 3], po[i - 2], po[i - 1], po[i]);
                DrawColliderPoints (po[i - 3], po[i - 2], po[i - 1], po[i]);
                if (i % 5 == 0) {
                    yield return null;
                }
            }
        } else {
            for (int i = 3; i <= po.Count - 1; i++) {
                DrawCurve (po[i - 3], po[i - 2], po[i - 1], po[i]);
                DrawColliderPoints (po[i - 3], po[i - 2], po[i - 1], po[i]);
                if (i % 5 == 0) {
                    yield return null;
                }
            }
        }
        CR_Running = false;
    }
    float c1 = 1.5f;
    float AngleGenerator (int difficulty = 1) {
        float a = 48 * Mathf.Pow (c1, difficulty);
        float b = Mathf.Pow (c1, difficulty) + 10;
        return (a / b);
    }
    void GeneratePoint (int difficulty = 1, float angle1 = 15) {
        if (tutorial) {
            angle = 15f;
            randomAngle = Random.Range (-angle, angle);
            DistanceToNextPoint = 12f;
        } else {
            angle = 7f + angle1;
            randomAngle = Random.Range (-angle, angle);
            randomAngle = Mathf.Sign (randomAngle) * Mathf.Clamp (Mathf.Abs (randomAngle), 10f + Mathf.Clamp (difficulty * 2f, 0, 29), 60);
            //debug for trail
            //randomAngle = 0;
            DistanceToNextPoint = Random.Range (12f - (difficulty / 8f), 13f);
            DistanceToNextPoint = Mathf.Clamp (DistanceToNextPoint, 8f, 12f);
        }
        //change for difficulty
        lastPoint = points[points.Count - 1];
        threePointsAgo = points[points.Count - 3];
        lastDirection = (lastPoint - threePointsAgo).normalized;
        vect = Quaternion.Euler (0, 0, randomAngle) * lastDirection * DistanceToNextPoint;
        newPoint = lastPoint + vect;
        finalAngle = GetAngle (lastDirection, vect);
        //print (finalAngle);
        if (newPoint.y > maxY) {
            maxY = newPoint.y;
            minY = maxY - 15;
            points.Add (newPoint);
        } else if (newPoint.y < minY) {
            // print ("Too far low");
            if (Mathf.Sign (vect.y) < 0) {
                vect.y = -vect.y;
                finalAngle = GetAngle (lastDirection, vect);
                if (Mathf.Abs (finalAngle) > angle) {
                    AdjustVect ();
                    vect = new Vector2 (DistanceToNextPoint * 2f, vect.y);
                    if (Mathf.Sign (vect.y) < 0) {
                        vect.y = -vect.y;
                        finalAngle = GetAngle (lastDirection, vect);
                        if (Mathf.Abs (finalAngle) > angle) {
                            AdjustVect ();
                        }
                    }
                }
            }
            points.Add (newPoint);
        } else {
            //print ("In between");
            //going down
            if (Mathf.Sign (lastPoint.x) < 0) {
                //print ("Left Half");
                //left half
                if (Mathf.Sign (vect.x) > 0) {
                    //print ("Going Right");
                    //Going Right
                    vect.x = -vect.x;
                    finalAngle = GetAngle (lastDirection, vect);
                    if (Mathf.Abs (finalAngle) > angle) {
                        AdjustVect ();
                        vect = new Vector2 (vect.x, DistanceToNextPoint * 2f);
                        if (Mathf.Sign (vect.x) > 0) {
                            vect.x = -vect.x;
                            finalAngle = GetAngle (lastDirection, vect);
                            if (Mathf.Abs (finalAngle) > angle) {
                                AdjustVect ();
                            }
                        }
                        //print ("Multiplied by two");
                    }
                } else {
                    //left half going left
                    //print ("Going Left");
                    vect.y = -vect.y;
                    finalAngle = GetAngle (lastDirection, vect);
                    if (Mathf.Abs (finalAngle) > angle) {
                        AdjustVect ();
                        vect = new Vector2 (DistanceToNextPoint * 2f, vect.y);
                    }
                }
            } else {
                //print ("Right Half");
                //right half
                if (Mathf.Sign (vect.x) < 0) {
                    //print ("Going Left");
                    //Going Left
                    vect.x = -vect.x;
                    finalAngle = GetAngle (lastDirection, vect);
                    if (Mathf.Abs (finalAngle) > angle) {
                        AdjustVect ();
                        vect = new Vector2 (vect.x, DistanceToNextPoint * 2f);
                        if (Mathf.Sign (vect.x) < 0) {
                            vect.x = -vect.x;
                            finalAngle = GetAngle (lastDirection, vect);
                            if (Mathf.Abs (finalAngle) > angle) {
                                AdjustVect ();
                            }
                        }
                    }
                } else {
                    //right side going right
                    //print ("Going Right");
                    vect.y = -vect.y;
                    finalAngle = GetAngle (lastDirection, vect);
                    if (Mathf.Abs (finalAngle) > angle) {
                        AdjustVect ();
                        vect = new Vector2 (DistanceToNextPoint * 2f, vect.y);
                    }
                }
            }
            newPoint = lastPoint + vect;
            points.Add (newPoint);
        }
        //print ("Point generated with angle: " + GetAngle (lastDirection, vect));
    }
    void AdjustVect () {
        finalAngle = GetAngle (lastDirection, vect);
        while (Mathf.Abs (finalAngle) > angle) {
            if (finalAngle > 0) {
                vect = Quaternion.Euler (0, 0, 5) * vect;
            } else {
                vect = Quaternion.Euler (0, 0, -5) * vect;
            }
            finalAngle = GetAngle (lastDirection, vect);
        }
        //print ("Vector Adjusted");
    }
    void GenerateTenPoints (int difficulty = 1) {
        float angle = AngleGenerator (difficulty);
        for (int i = 0; i < chunksInOneCheckpoint; i++) {
            GeneratePoint (difficulty, angle);
            if (i == 0) {
                checkpointLastPoint = points[points.Count - 1];
            }
        }
        checkpointLocation = points[points.Count - checkpointIndex];
        checkpoint = checkpoint + 1;
        //print (checkpoint);
    }
    IEnumerator RemoveCurve (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        newPos = GetCatmullRomPosition (1f, p0, p1, p2, p3);
        indexOf = totalPoints.IndexOf (newPos);
        if (indexOf > 0) {
            for (int i = indexOf; i >= 0; i--) {
                totalPoints.RemoveAt (i);
                if (i % 100 == 0) {
                    yield return null;
                }
            }
        }
        StartCoroutine (RemoveObjects ());
        removeOrbs = true;
    }
    IEnumerator RemoveCollider (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        newPos = GetCatmullRomPosition (1f, p0, p1, p2, p3);
        indexOf = colliderPoints.IndexOf (newPos);
        if (indexOf > 0) {
            for (int i = indexOf; i >= 0; i--) {
                colliderPoints.RemoveAt (i);
                if (i % 100 == 0) {
                    yield return null;
                }
            }
        }
        StartCoroutine (RemoveObjects ());
        removeOrbs = true;
    }
    IEnumerator RemoveObjects () {
        removeObjects = true;
        yield return wait;
        removeObjects = false;
    }
    void DrawCurve (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        for (int i = 1; i <= loops; i++) {
            t = i * resolution;
            //Find the coordinate between the end points with a Catmull-Rom spline
            newPos = GetCatmullRomPosition (t, p0, p1, p2, p3);
            if (totPoints.Add (newPos) == true) {
                totPoints.Add (newPos);
                totalPoints.Add (newPos);
            }
        }
    }
    void DrawColliderPoints (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        for (int i = 1; i <= colliderLoops; i++) {
            t = i * colliderResolution;
            //Find the coordinate between the end points with a Catmull-Rom spline
            newPos = GetCatmullRomPosition (t, p0, p1, p2, p3);
            if (totColliderPoints.Add (newPos) == true) {
                totColliderPoints.Add (newPos);
                colliderPoints.Add (newPos);
            }
        }
    }
    Vector3 GetCatmullRomPosition (float ta, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        a = 2f * p1;
        b = p2 - p0;
        c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        d = -p0 + 3f * p1 - 3f * p2 + p3;
        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        pos = 0.5f * (a + (b * ta) + (c * ta * ta) + (d * ta * ta * ta));
        return pos;
    }
    int FindClosestPoint (List<Vector3> points, Vector3 point) {
        pointDistance = 2f;
        indexs = 0;
        for (int i = 0; i < points.Count - 1; i++) {
            if (Vector3.Distance (points[i], point) < pointDistance) {
                pointDistance = Vector3.Distance (points[i], point);
                indexs = i;
            }
        }
        return (indexs);
    }
    float GetAngle (Vector3 ld, Vector3 vect) {
        return Vector3.Angle (vect, ld) * Mathf.Sign (Vector3.Dot (new Vector3 (ld.y, -ld.x, 0), vect));
    }
    void OnDrawGizmos () {
        if (debug) {
            Gizmos.color = Color.green;
            for (int i = 0; i < points.Count - 1; i++) {
                Gizmos.DrawSphere (points[i], 0.2f);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < totalPoints.Count - 1; i++) {
                Gizmos.DrawSphere (totalPoints[i], 0.2f);
            }
        }
        /* Gizmos.color = Color.green;
         for (int i = 0; i < colliderPoints.Count - 1; i++)
         {
             Gizmos.DrawSphere(colliderPoints[i], 0.1f);
         }
         Gizmos.color = Color.red;
         for (int i = 0; i < points.Count - 1; i++)
         {
             Gizmos.DrawSphere(points[i], 0.2f);
         }*/
    }
}