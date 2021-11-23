using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinModelController : MonoBehaviour
{
    Transform model;
    Vector3 zero = new Vector3(0, 0, 0);
    void OnEnable()
    {

        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        transform.eulerAngles = zero;
        while (true)
        {
            transform.Rotate(0, 0, 5, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
