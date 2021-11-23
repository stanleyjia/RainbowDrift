using System.Collections;
using UnityEngine;
#pragma warning disable 0219
#pragma warning disable 0414
public class CarModelRotator : MonoBehaviour {
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool oneTime = true;
    public bool rotating = true;
    void Start () {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
        StartCoroutine (Rotate ());
    }
    public void RotateCar () {
        StartCoroutine (Rotate ());
    }
    void Update () {
        /* if (Input.GetMouseButton (0)) {
            broken = true;
            oneTime2 = true;
            if (oneTime == true) {
                oneTime = false;
                _mouseReference = Input.mousePosition;
            }
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            // apply rotation
            _rotation.z = (_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            // rotate
            transform.Rotate (_rotation);
            // store mouse
            _mouseReference = Input.mousePosition;
        } else {
            oneTime = true;
            if (oneTime2 == true) {
                broken = false;
                StartCoroutine (Rotate ());
                oneTime2 = false;
            }
        }*/
    }
    void OnMouseDown () {
        // store mouse
        _mouseReference = Input.mousePosition;
    }
    IEnumerator Rotate () {
        rotating = true;
        //transform.eulerAngles = zero;
        while (rotating) {
            transform.Rotate (0, 0, -1f, Space.Self);
            yield return new WaitForFixedUpdate ();
        }
    }
}