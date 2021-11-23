using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeTextBlink : MonoBehaviour
{
    Text text;
    Color first = new Color(1, 1, 1, 0.3f);
    Color second = new Color(1, 1, 1, 1f);
    int goingUp = 0;
    float time = 1;

    void OnEnable()
    {
        text = GetComponent<Text>();
        text.color = second;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)

        {
            if (goingUp % 2 == 0)
            {

                if (time < 1)
                {
                    time += Time.deltaTime * 2.5f;
                    text.color = Color.Lerp(first, second, time);
                    yield return null;
                }
                else
                {
                    goingUp++;
                    time = 0;
                    text.color = second;
                    yield return new WaitForSeconds(0.25f);
                }
            }
            else
            {

                if (time < 1)
                {
                    time += Time.deltaTime * 2.5f;
                    text.color = Color.Lerp(second, first, time);
                    yield return null;
                }
                else
                {
                    goingUp++;
                    time = 0;
                    text.color = first;
                    yield return null;
                }
            }
        }

    }
}






