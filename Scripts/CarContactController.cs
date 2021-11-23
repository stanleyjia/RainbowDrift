using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarContactController : MonoBehaviour {
    ParticleController pc;
    Collider col;
    void Start () {
        pc = transform.Find ("Particles").GetComponent<ParticleController> ();
        col = GetComponent<Collider> ();
    }
    void OnTriggerEnter (Collider other) {
        if (other.tag == "Coin") {
            CollectCoinText.instance.CollectCoin (col.ClosestPoint (other.transform.position));
            //print (transform.TransformPoint (col.ClosestPoint (other.transform.position)));
            ////print (col.ClosestPoint (other.transform.position));
            // CollectCoinText.instance.CollectCoin (transform.InverseTransformPoint (col.ClosestPoint (other.transform.position)));
            pc.PlayCoinHit (transform.InverseTransformPoint (col.ClosestPoint (other.transform.position)));
            //pc.PlayOrbHit(transform.InverseTransformPoint(col.ClosestPoint(other.transform.position)));
        }
        if (other.tag == "Orb") {
            pc.PlayOrbHit (transform.InverseTransformPoint (col.ClosestPoint (other.transform.position)));
        }
    }
}