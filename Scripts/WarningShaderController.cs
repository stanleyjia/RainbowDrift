using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningShaderController : MonoBehaviour
{
    UnityEngine.UI.Image image;
    void Start()
    {

        image = GetComponent<UnityEngine.UI.Image>();
        image.color = new Color(1, 0, 0, 0f);
    }


    void Update()
    {
        if (PlayerController.instance.callWarning == true)
        {
            image.color = new Color(1, 0, 0, 0.05f);
        }
        else
        {
            image.color = new Color(1, 0, 0, 0.0f);
        }

    }



}
