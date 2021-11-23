using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarAnimationController : MonoBehaviour {
    Animator animator;
    float leftToRight;
    float animLeftToRight;
    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator> ();
        //  leftToRight = 0f;
    }
    // Update is called once per frame
    void Update () {
        leftToRight = CarVariables.instance.leftToRight / 2f;
        animLeftToRight = (leftToRight + 0.5f);
        animator.Play ("Constant", 0, animLeftToRight);
    }
}