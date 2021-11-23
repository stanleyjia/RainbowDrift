using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeshController : MonoBehaviour {
    public Pathmaker pathmaker;
    public float width;
    public bool debug = false;
    public Material trackTop;
    public Material trackSides;
    public Material[] mats;
    public List<Vector3> pointas = new List<Vector3> ();
    public List<Vector3> pointbs = new List<Vector3> ();
    List<Vector3> pointas2 = new List<Vector3> ();
    List<Vector3> pointbs2 = new List<Vector3> ();
    //Faster to search thru
    Vector3 newPointa;
    Vector3 newPointb;
    public bool CR_running = false;
    public bool CR_running2 = false;
    bool firstTime = true;
    Vector3 pointa;
    Vector3 pointb;
    Vector3 pointc;
    Vector3 pointd;
    public int amount = 100;
    Vector3 lastActualA;
    Vector3 lastActualB;
    Renderer rend;
    public bool aGenerate = false;
    public bool bGenerate = false;
    public bool removePoints = false;
    Mesh ms;
    Mesh colliderMs;
    MeshFilter mf;
    float distance;
    Vector2 zeroVector = new Vector2 (0, 0);
    Vector2 oneVector = new Vector2 (1, 1);
    public float trackHeight = 1f;
    MeshCollider meshC;
    Vector3 lastPosition, position, direction, perpVector;
    Vector2 bottomRight = new Vector2 (1, 0);
    void Start () {
        mats = new Material[] {
            trackTop,
            trackSides
        };
        mf = GetComponent<MeshFilter> ();
        mf.mesh.subMeshCount = 2;
        meshC = gameObject.AddComponent<MeshCollider> () as MeshCollider;
        meshC.material = Resources.Load ("Materials/TrackPhysics") as PhysicMaterial;
        rend = GetComponent<Renderer> ();
        rend.sortingLayerName = "Track";
        rend.sortingOrder = 4;
        //rend.material = material;
        rend.materials = mats;
        if (pathmaker.generate2 == true) {
            mf.mesh = ExtrudeAlongPath (pathmaker.totalPoints, width, true, 0);
            //print (rend.materials.);
            rend.materials = mats;
            colliderMs = ExtrudeAlongPath (pathmaker.colliderPoints, width, true, 1);
            meshC.sharedMesh = colliderMs;
            pointas = pointas2;
            pointbs = pointbs2;
            bGenerate = true;
            aGenerate = true;
            pathmaker.generate2 = false;
        }
    }
    void Update () {
        if (pathmaker.generate2 == true) {
            if (firstTime == true) {
                pointas2.Clear ();
                pointbs2.Clear ();
                ms = ExtrudeAlongPath (pathmaker.totalPoints, width, false, 0);
                colliderMs = ExtrudeAlongPath (pathmaker.colliderPoints, width, false, 1);
                //meshC.material = Resources.Load("Materials/TrackPhysics") as PhysicMaterial;
                firstTime = false;
            }
            if (CR_running == false) {
                pointas = pointas2;
                pointbs = pointbs2;
                mf.mesh = ms;
                rend.materials = mats;
                pathmaker.generate2 = false;
                firstTime = true;
                bGenerate = true;
                aGenerate = true;
                removePoints = true;
            }
            if (CR_running2 == false) {
                meshC.sharedMesh = colliderMs;
            }
        }
    }
    public Mesh ExtrudeAlongPath (List<Vector3> points, float width, bool start, int id) {
        Mesh m = new Mesh ();
        m.subMeshCount = 2;
        if (start == true) {
            List<Vector3> verts = new List<Vector3> ();
            List<Vector3> norms = new List<Vector3> ();
            List<Vector2> uv = new List<Vector2> ();
            List<int> tris = new List<int> ();
            List<int> tris2 = new List<int> ();
            /* verts.Clear();
             norms.Clear();
             uv.Clear();*/
            for (int i = 1; i < points.Count; i++) {
                lastPosition = points[i - 1];
                position = points[i];
                direction = (position - lastPosition).normalized;
                perpVector = new Vector3 (-direction.y, direction.x, 0.0f);
                newPointa = position + perpVector * width;
                newPointb = position - perpVector * width;
                pointa = newPointa;
                pointb = newPointb;
                pointc = new Vector3 (pointa.x, pointa.y, trackHeight);
                pointd = new Vector3 (pointb.x, pointb.y, trackHeight);
                // pointas2.Add(pointa);
                //pointbs2.Add(pointb);
                lastActualA = pointa;
                lastActualB = pointb;
                verts.Add (transform.InverseTransformPoint (pointc));
                norms.Add (Vector3.forward);
                verts.Add (transform.InverseTransformPoint (pointc));
                norms.Add (perpVector);
                verts.Add (transform.InverseTransformPoint (pointa));
                norms.Add (perpVector);
                verts.Add (transform.InverseTransformPoint (pointa));
                norms.Add (Vector3.back);
                verts.Add (transform.InverseTransformPoint (pointb));
                norms.Add (Vector3.back);
                verts.Add (transform.InverseTransformPoint (pointb));
                norms.Add (-perpVector);
                verts.Add (transform.InverseTransformPoint (pointd));
                norms.Add (-perpVector);
                verts.Add (transform.InverseTransformPoint (pointd));
                norms.Add (Vector3.forward);
                /*uv.Add (zeroVector);
                uv.Add (oneVector);
                uv.Add (zeroVector);
                uv.Add (oneVector);
                uv.Add (zeroVector);
                uv.Add (oneVector);
                uv.Add (zeroVector);
                uv.Add (oneVector);*/
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
                uv.Add (bottomRight);
            }
            m.SetVertices (verts);
            m.SetNormals (norms);
            m.uv = uv.ToArray ();
            tris.Clear ();
            for (int w = 0; w < verts.Count - 16; w++) {
                if (w % 8 == 0) {
                    tris.Add (w);
                    tris.Add (w + 15);
                    tris.Add (w + 8);
                } else if ((w + 1) % 8 == 0) {
                    tris.Add (w);
                    tris.Add (w + 8);
                    tris.Add (w - 7);
                } else {
                    if ((w + 1) % 2 == 0) {
                        //odd
                        if (w % 8 == 1 || w % 8 == 5) {
                            tris2.Add (w);
                            tris2.Add (w + 8);
                            tris2.Add (w + 1);
                        } else {
                            tris.Add (w);
                            tris.Add (w + 8);
                            tris.Add (w + 1);
                        }
                    } else {
                        //even
                        if (w % 8 == 2 || w % 8 == 6) {
                            tris2.Add (w);
                            tris2.Add (w + 7);
                            tris2.Add (w + 8);
                        } else {
                            tris.Add (w);
                            tris.Add (w + 7);
                            tris.Add (w + 8);
                        }
                    }
                }
            }
            m.SetTriangles (tris.ToArray (), 0);
            m.SetTriangles (tris2.ToArray (), 1);
            m.name = "pathMesh";
            m.RecalculateBounds ();
            return m;
        } else {
            StartCoroutine (MakeMesh (points, width, m, id));
            //m.RecalculateNormals();
            m.RecalculateBounds ();
            return m;
        }
    }
    IEnumerator MakeMesh (List<Vector3> points, float wid, Mesh m, int id) {
        List<Vector3> verts = new List<Vector3> ();
        List<Vector3> norms = new List<Vector3> ();
        List<Vector2> uv = new List<Vector2> ();
        List<int> tris = new List<int> ();
        List<int> tris2 = new List<int> ();
        if (id == 0) {
            CR_running = true;
        }
        if (id == 1) {
            CR_running2 = true;
        }
        for (int i = 1; i < points.Count; i++) {
            lastPosition = points[i - 1];
            position = points[i];
            direction = (position - lastPosition).normalized;
            perpVector = new Vector3 (-direction.y, direction.x, 0.0f);
            newPointa = position + perpVector * wid;
            newPointb = position - perpVector * wid;
            distance = Vector3.Distance (newPointa, lastActualA);
            if (distance < 0.3f) {
                pointa = lastActualA;
            } else {
                pointa = newPointa;
            }
            distance = Vector3.Distance (newPointb, lastActualB);
            if (distance < 0.3f) {
                pointb = lastActualB;
            } else {
                pointb = newPointb;
            }
            //pointas2.Add(pointa);
            //pointbs2.Add(pointb);
            lastActualA = pointa;
            lastActualB = pointb;
            pointc = new Vector3 (pointa.x, pointa.y, trackHeight);
            pointd = new Vector3 (pointb.x, pointb.y, trackHeight);
            //remove half points to have soft edge
            verts.Add (transform.InverseTransformPoint (pointc));
            norms.Add (Vector3.forward);
            verts.Add (transform.InverseTransformPoint (pointc));
            norms.Add (perpVector);
            verts.Add (transform.InverseTransformPoint (pointa));
            norms.Add (perpVector);
            verts.Add (transform.InverseTransformPoint (pointa));
            norms.Add (Vector3.back);
            verts.Add (transform.InverseTransformPoint (pointb));
            norms.Add (Vector3.back);
            verts.Add (transform.InverseTransformPoint (pointb));
            norms.Add (-perpVector);
            verts.Add (transform.InverseTransformPoint (pointd));
            norms.Add (-perpVector);
            verts.Add (transform.InverseTransformPoint (pointd));
            norms.Add (Vector3.forward);
            uv.Add (zeroVector);
            uv.Add (oneVector);
            uv.Add (zeroVector);
            uv.Add (oneVector);
            uv.Add (zeroVector);
            uv.Add (oneVector);
            uv.Add (zeroVector);
            uv.Add (oneVector);
            if (i % amount == 0) {
                yield return null;
            }
        }
        m.SetVertices (verts);
        m.SetNormals (norms);
        m.uv = uv.ToArray ();
        tris.Clear ();
        for (int w = 0; w < verts.Count - 16; w++) {
            if (w % 8 == 0) {
                tris.Add (w);
                tris.Add (w + 15);
                tris.Add (w + 8);
            } else if ((w + 1) % 8 == 0) {
                tris.Add (w);
                tris.Add (w + 8);
                tris.Add (w - 7);
            } else {
                if ((w + 1) % 2 == 0) {
                    //odd
                    if (w % 8 == 1 || w % 8 == 5) {
                        tris2.Add (w);
                        tris2.Add (w + 8);
                        tris2.Add (w + 1);
                    } else {
                        tris.Add (w);
                        tris.Add (w + 8);
                        tris.Add (w + 1);
                    }
                } else {
                    //even
                    if (w % 8 == 2 || w % 8 == 6) {
                        tris2.Add (w);
                        tris2.Add (w + 7);
                        tris2.Add (w + 8);
                    } else {
                        tris.Add (w);
                        tris.Add (w + 7);
                        tris.Add (w + 8);
                    }
                }
            }
        }
        m.SetTriangles (tris.ToArray (), 0);
        m.SetTriangles (tris2.ToArray (), 1);
        m.name = "pathMesh";
        //Coroutine is done now
        if (id == 0) {
            CR_running = false;
        }
        if (id == 1) {
            CR_running2 = false;
        }
    }
    //Debug Mesh Vertices
    void OnDrawGizmos () {
        if (debug) {
            /*  Gizmos.color = Color.green;
              for (int i = 0; i < pointas.Count - 1; i++)
              {
                  Gizmos.DrawSphere(pointas[i], 0.2f);
              }

              Gizmos.color = Color.red;
              for (int j = 0; j < pointbs.Count - 1; j++)
              {
                  Gizmos.DrawSphere(pointbs[j], 0.2f);
              }*/
            /* Gizmos.color = Color.blue;
             for (int s = 0; s < pathmaker.totalPoints.Count - 1; s++)
             {
                 Gizmos.DrawSphere(pathmaker.totalPoints[s], 0.2f);
             }*/
            /*for (int m = 1; m < verts.Count; m++)
            {
                Debug.DrawLine(transform.TransformPoint(verts[m - 1]), transform.TransformPoint(verts[m]), Color.cyan);
                //  Gizmos.DrawWireSphere(verts[m], 0.3f);
            }*/
        }
    }
}