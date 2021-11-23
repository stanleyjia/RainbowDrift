using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbRotateController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    void OnEnable()
    {
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(0, 0, 5f, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
