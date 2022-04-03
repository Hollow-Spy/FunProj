using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    IEnumerator Scalecoroutine;
    Text text;
    [SerializeField] GameObject clickSFX, HoverSFX;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        
    }

    public void UnHighlightText()
    {
        text.color = Color.white;
        text.transform.localScale = new Vector3(1, 1, 1);
        StopCoroutine(Scalecoroutine);
    }
    public void Clicked()
    {

        Instantiate(clickSFX, transform.position, Quaternion.identity);
        Scalecoroutine = scalenumerator();
        StartCoroutine(Scalecoroutine);
    }

    public void HighLightText()
    {
        Instantiate(HoverSFX, transform.position, Quaternion.identity);
        text.color = Color.yellow;

        Scalecoroutine = scalenumerator();
        StartCoroutine(Scalecoroutine);

    }
    IEnumerator scalenumerator()
    {
        yield return null;
        while (text.transform.localScale.x > .8f)
        {
            yield return null;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, new Vector3(.7f, .7f, .7f), 15 * Time.deltaTime);
        }

        while (text.transform.localScale.x < 1.2f)
        {
            yield return null;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, new Vector3(1.3f, 1.3f, 1.3f), 35 * Time.deltaTime);
        }

    }


}
