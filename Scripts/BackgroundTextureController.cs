using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTextureController : MonoBehaviour
{
    CameraController cam;
    Renderer rend;
    public float size;
    Vector3 start = new Vector3(0, 0, 0);
    Vector3 pos;
    public Vector3 position;
    private int interval = 3;
    public float distance = 50f;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        rend = GetComponent<Renderer>();
        rend.sortingLayerName = "Background";
        size = rend.bounds.size.x;
    }

    private void OnEnable()
    {

    }


    void Update()
    {
        pos = cam.transform.position;
        if (Time.frameCount % interval == 2)
        {
            CheckForDistance(pos);
        }

    }
    void CheckForDistance(Vector3 camPos)
    {
        if (Vector3.Distance(camPos, transform.position) > distance)
        {
            gameObject.transform.position = start;
            gameObject.SetActive(false);
        }
    }
}
