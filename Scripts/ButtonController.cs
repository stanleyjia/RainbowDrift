using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    UnityEngine.UI.Image image;
    public Color first = new Color(1, 1, 1, 0.7f);
    public Color second = new Color(1, 1, 1, 1f);
    int goingUp = 0;
    float time = 1;

    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        image.color = second;
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
                    image.color = Color.Lerp(first, second, time);
                    yield return null;
                }
                else
                {
                    goingUp++;
                    time = 0;
                    yield return new WaitForSeconds(0.8f);
                }
            }
            else
            {

                if (time < 1)
                {
                    time += Time.deltaTime * 2.5f;
                    image.color = Color.Lerp(second, first, time);
                    yield return null;
                }
                else
                {
                    goingUp++;
                    time = 0;
                    yield return null;
                }
            }
        }
    }
}






