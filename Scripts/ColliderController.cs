using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private int interval = 3;
    float distance;
    Pathmaker pathmaker;
    // Use this for initialization
    void Start()
    {
        pathmaker = GameObject.FindWithTag("Pathmaker").GetComponent<Pathmaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % interval == 1)
        {
            if (pathmaker.removeObjects == true)
            {
                CheckForDistance(pathmaker.points, transform.position);
            }
        };
    }


    void CheckForDistance(List<Vector3> points, Vector3 position)
    {
        distance = 10f;
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (Vector3.Distance(points[i], position) < distance)
            {
                distance = Vector3.Distance(points[i], position);
            }
        }
        if (distance > 2f)
        {
            gameObject.SetActive(false);
        }
    }
}
