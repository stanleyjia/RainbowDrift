using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailRenderColor : MonoBehaviour
{
    Color[] colors;
    Color color;
    Color start;
    Color end;
    Image image;
    int lastColor;
    int counter;
    int timesToDo = 100;
    void Awake()
    {
        image = GetComponent<Image>();
        colors = new Color[24];
        colors[0] = new Color(0.8382353f, 0f, 0.147924f, 1f);
        colors[1] = new Color(0.8676471f, 0f, 0.3062283f, 1f);
        colors[2] = new Color(0.8161765f, 0f, 0.6247972f, 1f);
        colors[3] = new Color(0.7572511f, 0f, 0.8014706f, 1f);
        colors[4] = new Color(0.7550146f, 0.1235943f, 0.9338235f, 1f);
        colors[5] = new Color(0.5162632f, 0f, 0.8529412f, 1f);
        colors[6] = new Color(0.3500507f, 0f, 0.8602941f, 1f);
        colors[7] = new Color(0.2041522f, 0.2041522f, 0.8676471f, 1f);
        colors[8] = new Color(0.1911765f, 0.4310345f, 1f, 1f);
        colors[9] = new Color(0.2058823f, 0.5704362f, 0.875f, 1f);
        colors[10] = new Color(0f, 0.6111054f, 0.8602941f, 1f);
        colors[11] = new Color(0.1658737f, 0.7805304f, 0.8676471f, 1f);
        colors[12] = new Color(0f, 0.875f, 0.8749996f, 1f);
        colors[13] = new Color(0.1729023f, 0.9044118f, 0.7833343f, 1f);
        colors[14] = new Color(0f, 0.8602941f, 0.5043104f, 1f);
        colors[15] = new Color(0.2634083f, 0.8529412f, 0.2634083f, 1f);
        colors[16] = new Color(0.6990873f, 0.8970588f, 0f, 1f);
        colors[17] = new Color(1f, 1f, 0f, 1f);
        colors[18] = new Color(1f, 0.862919f, 0.2426471f, 1f);
        colors[19] = new Color(1f, 0.7273971f, 0.1838235f, 1f);
        colors[20] = new Color(1f, 0.62875f, 0.1911765f, 1f);
        colors[21] = new Color(0.9044118f, 0.4418409f, 0.0864511f, 1f);
        colors[22] = new Color(0.9632353f, 0.3004982f, 0.0566609f, 1f);
        colors[23] = new Color(0.9044118f, 0.08772793f, 0f, 1f);
        /*if (gameObject.name == "InkTrail")
        {

        }*/
        StartCoroutine(ShiftColors(true));
    }

    void OnEnable()
    {
        StartCoroutine(ShiftColors(false));
    }

    IEnumerator ShiftColors(bool firstTime = false)
    {
        if (firstTime)
        {
            while (counter < timesToDo)
            {
                for (int j = 0; j < colors.Length; j++)
                {
                    lastColor = j;
                    if (j - 1 >= 0)
                    {
                        start = colors[j - 1];
                    }
                    else
                    {
                        start = colors[colors.Length - 1];
                    }
                    end = colors[j];

                    for (float i = 0; i <= 1; i += Time.deltaTime * 2f)
                    {

                        color = Color.Lerp(start, end, i);
                        image.material.SetColor("_Color", color);
                        yield return null;
                    }

                }
                counter++;
            }
        }
        else
        {
            while (counter < timesToDo)
            {
                if (lastColor == colors.Length - 1)
                {
                    lastColor = -1;
                }
                for (int j = lastColor + 1; j < colors.Length; j++)
                {
                    lastColor = j;
                    if (j - 1 >= 0)
                    {
                        start = colors[j - 1];
                    }
                    else
                    {
                        start = colors[colors.Length - 1];
                    }
                    end = colors[j];

                    for (float i = 0; i <= 1; i += Time.deltaTime * 2f)
                    {

                        color = Color.Lerp(start, end, i);
                        image.material.SetColor("_Color", color);
                        yield return null;
                    }

                }
                counter++;
            }
        }
    }
}
