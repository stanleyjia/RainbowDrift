using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCarAnimationController : MonoBehaviour

{
    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.Play("Constant", 0, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        animator.Play("Constant", 0, 0.5f);
    }
}
