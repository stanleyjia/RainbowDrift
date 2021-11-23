using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverOrb : MonoBehaviour
{

    public OrbGeneratorController ogc;
    Text text;
    bool oneTime = true;
    // Use this for initialization
    void Start()
    {

        text = GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CarVariables.instance.gameOn == false)
        {
            if (oneTime == true)
            {
                text.text = OrbGeneratorController.instance.orbCount.ToString();
                StartCoroutine(FadeIn());
                oneTime = false;
            }
        }
    }

    IEnumerator FadeIn(float alpha = 1)
    {
        yield return new WaitForSeconds(1f);

        for (float i = 0; i <= alpha; i += Time.deltaTime / 0.6f)
        {
            // set color with i as alpha
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }
}
