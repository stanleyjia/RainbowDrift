using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPS : MonoBehaviour
{
    ParticleSystem ps;
    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void PlayPS()
    {
        StartCoroutine(Sparkle());

    }


    IEnumerator Sparkle()
    {
        ps.Play();
        yield return new WaitForSeconds(2);
        ps.Stop();

    }
}
