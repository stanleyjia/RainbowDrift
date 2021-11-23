using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOrbCount : MonoBehaviour
{
    public OrbGeneratorController ogc;
    Text text;

    bool oneTime = true;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CarVariables.instance.gameOn == false)
        {
            if (oneTime == true)
            {
                StartCoroutine(FadeAway());
                oneTime = false;
            }
        }
        else
        {
            text.text = OrbGeneratorController.instance.orbCount.ToString();
        }
    }
    IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(0.4f);

        for (float i = text.color.a; i >= 0; i -= Time.deltaTime / 0.6f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
}
