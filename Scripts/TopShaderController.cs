using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TopShaderController : MonoBehaviour {
    public static TopShaderController instance;
    Image image;
    // private bool firstTime = true;
    void Start () {
        instance = this;
        image = GetComponent<Image> ();
        image.color = new Color (image.color.r, image.color.g, image.color.b, 0.95f);
    }
}